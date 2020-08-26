Universidad Rafael Landívar
Compiladores sec. 01

Desarrolladores:
Sergio Lara [1044418]
Andres Gálvez [1024718]

Analizador léxico

Este programa constituye la primera fase de un compilador. Se basa en un escáner para una pequeña parte del lenguaje de programación C#.  Este reconoce lexemas (tokens) en orden de lectura en un archivo de entrada.

El analizador léxico evalúa línea por línea el archivo de entrada. Para poder identificar el formato de cada línea y separar esta en los diferentes tipos de tokens o errores que puede producir, se utilizan expresiones regulares (ER) de la librería Regex de C#. Primero, analiza la línea y verifica que no se encuentra vacía; si no lo está, pasa a ser una cadena de caracteres la cual evalúa en la ER. Gracias a la librería Regex, el analizador es capaz de separar y devolver fragmentos que encajan en la ER.

Al haber tantas variantes, el analizador solía confundir algunos tokens o errores por otros; p. ej., una cadena de texto (string), supóngase: "hola ", cuando era evaluada, la ER devolvía <"hola> y <"> como token o errores por separado, por lo que dentro de la ER se formaron grupos que tienen una jerarquía, para que ciertas partes de la ER tengan una mayor prioridad de hacer "match" antes que otras. Gracias a este ajuste, el analizador es robusto y capaz de funcionar ante cualquier archivo que reciba, funcionando de la manera que se describe a continuación.

Jerarquia de la ER y su definición:

1) Strings (""((\w)|(\s)|(\p{P})|(\p{S}))+"")

2) Comentarios de una sola linea (//((\w)|(\s)|(\p{P})|(\p{S}))+)

3) Numeros
   3.1) Exponenciales ([0-9]+[.][0-9]*(E|e)[+]?[0-9]+)
   3.2) Hexadecimales (0(x|X)([0-9]|[a-fA-F])+)
   3.3) Flotantes ([0-9]+[.][0-9]*)
   3.4) Enteros ([0-9]+)

4) Error string incompleto (""((\w)|(\s)|(\p{P})|(\p{S}))+)

5) Identificadores/Reservadas ([a-zA-Z]((\w)|[_])*))

6) Operadores
   6.1) De agrupación juntos [()]|[{}]|[\[\]] --> Se quito de la ER, la solución se describirá en Analizar()
   6.2) De comparación (<=|>=|==|!=|&&|[||])
   6.3) Matemáticos (Son un solo caracter) --> Se reconocen como error 8) y luego se identifican como Operador en getTypeToken(string)

7) Error '*/' ([*][/])

8) Caracteres de error ((\p{P}){1}|(\p{S}){1})

Como función principal del algoritmo, se utiliza Analizar(), la cual recorre el archivo línea por línea. Cuando encuentra un '/*' ignora la línea actual y las siguientes hasta encontrar un '*/'. Si este último no es encontrado cuando se llega al final del archivo, el analizador guarda la posición donde se encuentra el '/*' y emite un error EOF de un comentario.

Si la cadena no es un comentario multilínea, entonces se analiza la cadena entrante y se utiliza el método Regex.Match(string) para encontrar cada sub-cadena que cumple con la ER; si el "match" es un operador de agrupación de apertura '(', '[' o '{', se guarda en una variable temporal los datos de este caracter (index y caracter) para que luego, si el siguiente match es un operador de agrupación de cierre, se concatenen y sean analizados como un único token.

Luego, en la cadena a analizar, el analizador necesita saber qué tipo de token es generado, este se obtiene con el método getTypeToken(string), el cual devuelve el número al que pertenece la cadena según la jeraquía indicada anteriormente.

Si el resultado de getTypeToken(string) indica que es un número, se obtiene su valor y se procede a generar el token. Si el resultado es un Identificador/Reservada se analiza si la cadena es una palabra reservada o un identificador, para luego ser generado su respectivo token. Si no es ninguno de estos dos se genera el token o error con el caracter que se esta analizando en ese momento.