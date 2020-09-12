<p align="right">
2020-08-27
</p>
<p align="left">
Universidad Rafael Landívar </br> Compiladores Sec. 01
</p>

<p align="left">
Sergio Lara [1044418] </br> Andres Gálvez [1024718]
</p>

<h1 align= "center"><b>Mini C#  👨‍💻</b></h1>

<p align="center">
Mini Compilador de C# </br> 
</p>

<h3 align = "left">Analizador Léxico</h3>
<img align="right" alt="GIF" src="https://i.pinimg.com/originals/e4/26/70/e426702edf874b181aced1e2fa5c6cde.gif" />
<p align="left">
Tomamos la lógica de ir evaluando línea por línea el archivo de entrada.  Para poder identificar el formato de cada línea y separar esta en los diferentes tipos de tokens o errores que puede producir, utilizamos Expresiones Regulares (ER) de la librería Regex de C#.
</br> </br>
Primero analizamos la línea y verificamos que no se encuentra vacía, si no lo está, pasa a ser una cadena de caracteres la cual evaluamos en nuestra ER; gracias a Regex, esta es capaz de separar y devolvernos fragmentos que encajan en la ER, pero al haber tantas variantes, solía confundir algunos tokens o errores por otros; por ejemplo, una cadena de texto (string), supongamos "hola ", cuando era evaluada, la ER nos devolvía <"hola>  y <"> como token o errores por separado, por lo que dentro de la ER formamos grupos que tienen una jerarquía, para que ciertas partes de la ER tengan una mayor prioridad de hacer "match" antes que otras.
</br> </br>
Jerarquia de nuestra ER y su definición:
</br> </br>
1) Strings (""((\w)|(\s)|(\p{P})|(\p{S}))+"") </br> </br>
2) Comentarios de una sola linea (//((\w)|(\s)|(\p{P})|(\p{S}))+) </br></br>
3) Números </br>
 3.1) Exponenciales ([0-9]+[.][0-9]*(E|e)[+]?[0-9]+) </br>
 3.2) Hexadecimales (0(x|X)([0-9]|[a-fA-F])+) </br>
 3.3) Flotantes ([0-9]+[.][0-9]*) </br>
 3.4) Enteros ([0-9]+) </br> </br>
4) Error string incompleto (""((\w)|(\s)|(\p{P})|(\p{S}))+) </br> </br>
5) Identificadores/Reservadas ([a-zA-Z]((\w)|[_])*)) </br> </br>
6) Operadores </br>
 6.1) De agrupación juntos [()]|[{}]|[\[\]] --> Se quito de la ER, la solución se describirá en Analizar() </br>
 6.2) De comparación (<=|>=|==|!=|&&|[||]) </br>
 6.3) Matemáticos (Son un solo caracter) --> Se reconocen como error 8) y luego se identifican como Operador en getTypeToken(string) </br> </br>
7) Error '*/'   ([*][/]) </br> </br>
8) Caracteres de error ((\p{P}){1}|(\p{S}){1})
</br> </br>
A pesar de que con Regex somos capaces de clasificar y dividr por sub-grupos, aun existen tres tokens/errores que no han sido identificados: los comentarios multilínea, si una cadena es identificador o palabra reservada y los operadores de agrupación cuando vienen uno tras otro. </br>
Como función principal utilizamos Analizar(), la cual recorre el archivo línea por línea y cuando encuentra un '/*' ignora la línea actual y las siguientes hasta encontrar un '*/', si no es encontrado estos dos caractes de esa manera cuando Analizar() llega al final del archivo, esta guarda la posición donde se encuentra el '/*' y emite un error EOF de un comentario.</br> </br>

Si la cadena no es un comentario multilínea, entonces analizamos la cadena entrante y utilizamos el método Regex.Match(string) para encontrar cada sub-cadena que cumple con la ER; si el "match" es un operador de agrupación de apertura '(', '[' o '{', guardamos en una variable temporal los datos de este caracter (index y caracter) para luego si el siguiente match es un operador de agrupación de cierre se concatenen y sean analizados como un único token. </br> </br>
Luego, en la cadena a analizar, necesitamos saber qué tipo de token genera esta, el tipo de token se obtiene con el método getTypeToken(string), el cual devuelve el número al que pertenece la cadena según la jeraquía indicada anteriormente. </br>

Si el resultado de getTypeToken(string) indica que es un número, se obtiene su valor y se procede a generar el token. </br>
Si el resultado es un Identificador/Reservada se analiza si la cadena es una palabra reservada o un identificador, para luego ser generado su respectivo token. </br>
Si no es ninguno de estos dos se manda a generar el token o error con el caracter que se esta analizando en ese momento.
</p>

<h3 align = "left">Analizador Sintactico Descendente Recursivo</h3>
<p align="left">
Para esta fase se ha utilizado la siguiente gramatica: </br>
https://docs.google.com/document/d/10RhYVMYsLzrrJKRTNNxrR2Z8TuBcNV2Q24yhxDdTG9I/edit?usp=sharing
</br></br>
La presente fase del compilador se trabaja sobre la una clase que recibe como parametro una lista de Tokens en orden conforme fueron encontrados y analizados por el Analizador lexico. Este objeto Token esta conformado por su Type, que indica si es operador, palabra reservada, constante o identificador. content que es el contenido del token, numLinea que representa el numero de linea donde se encontraba este token; y typeConst que indica que tipo de constante es este token, cuando no es una constante este campo se encuentra como una cadena vacia
</br></br>
El analizador Sintactivo funciona ingresando a cada producción segun sea el orden en la gramatica y cuando en la gramatica encuentra un simbolo terminal este compara que el token en la posición [0] de la lista de Tokens sea el mismo ST y si este es procede a consumir el token y eliminarlo de la lista Tokens, y luego procede a almacenar este en una lista temporal de token (tmpTokens).
</br></br>
Se ha creado una función por cada producción de tipo string. Cada funcion es capaz de devolver cinco tipos de retroalimentación a si mismo para poder ejecutar el analisis correctamente, las posibles respuestas son:
* "work", significa que la actual producción ha resultado correcta
* "error", significa que se ha encontrado un error de lexico
* "error###", indica que se ha ingresado a un posible error no controlado
* "cero", significa que la lista Tokens se encuentra vacia.
* "epsilon", significa que la cadena es aceptable que no se encuentre
</br></br>
Cuando se encuentra que una producción ha retornado error, el analizador lexico ingresa a la siguiente producción correspondiente del mismo nivel y si algun token ha sido consumido durante el analisis anterior, se procede a integrar los tokens eliminados almacenados en tmpTokens de nuevo a la lista Tokens realizando asi un Backtraking hacia la ultima cadena de tokens correcta. Luego tmpTokens se vuelve a su estado inicial para se utilizada una proxima vez si es necesario.
</br></br>
Si el Analizador Sintactico sigue revisando producciones cuando ya no se encuentran tokens, este solo verifica que el resultado sea "epsilon" o si regresa a la producción Decl o Program este verifica que si aun existen tokens por lo que si ya no hay se para el analisis. 
</p>