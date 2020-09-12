Universidad Rafael Landívar
Compiladores sec. 01

Desarrolladores:
Sergio Lara [1044418]
Andres Gálvez [1024718]

Como utilizar el programa.
1) Ejecute el exe llamado minic.exe
2) Arrastre su archivo hacia la consola emergente
3) Si su archivo contiene errores este se los mostrara en pantalla
4) Ya hallan ocurrido errores o no se generara un archivo con la exetención .out como salida mostrando los tokens generados y su linea y columna.

Analizador Sintactico Descendente Recursivo

Para esta fase se ha utilizado la siguiente gramatica:
https://docs.google.com/document/d/10RhYVMYsLzrrJKRTNNxrR2Z8TuBcNV2Q24yhxDdTG9I/edit?usp=sharing

La presente fase del compilador se trabaja sobre la una clase que recibe como parametro una lista de Tokens en orden conforme fueron encontrados y analizados por el Analizador lexico. Este objeto Token esta conformado por su Type, que indica si es operador, palabra reservada, constante o identificador. content que es el contenido del token, numLinea que representa el numero de linea donde se encontraba este token; y typeConst que indica que tipo de constante es este token, cuando no es una constante este campo se encuentra como una cadena vacia

El analizador Sintactivo funciona ingresando a cada producción segun sea el orden en la gramatica y cuando en la gramatica encuentra un simbolo terminal este compara que el token en la posición [0] de la lista de Tokens sea el mismo ST y si este es procede a consumir el token y eliminarlo de la lista Tokens, y luego procede a almacenar este en una lista temporal de token (tmpTokens).

Se ha creado una función por cada producción de tipo string. Cada funcion es capaz de devolver cinco tipos de retroalimentación a si mismo para poder ejecutar el analisis correctamente, las posibles respuestas son:
* "work", significa que la actual producción ha resultado correcta
* "error", significa que se ha encontrado un error de lexico
* "error###", indica que se ha ingresado a un posible error no controlado
* "cero", significa que la lista Tokens se encuentra vacia.
* "epsilon", significa que la cadena es aceptable que no se encuentre

Cuando se encuentra que una producción ha retornado error, el analizador lexico ingresa a la siguiente producción correspondiente del mismo nivel y si algun token ha sido consumido durante el analisis anterior, se procede a integrar los tokens eliminados almacenados en tmpTokens de nuevo a la lista Tokens realizando asi un Backtraking hacia la ultima cadena de tokens correcta. Luego tmpTokens se vuelve a su estado inicial para se utilizada una proxima vez si es necesario.

Si el Analizador Sintactico sigue revisando producciones cuando ya no se encuentran tokens, este solo verifica que el resultado sea "epsilon" o si regresa a la producción Decl o Program este verifica que si aun existen tokens por lo que si ya no hay se para el analisis. 