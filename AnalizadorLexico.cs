using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.CompilerServices;

namespace minic
{
	class AnalizadorLexico
	{
		string fileName { get; set; }
		string pathFile { get; set; }
		List<string> Reservadas { get; set; }
		List<string> Operadores { get; set; }
		List<string> Tokens { get; set; }
		List<string> outputLines { get; set; }
		//Jerarquia de la ER
		//1)Comentarios de linea		(//((\w)|(\s)|(\p{P})|(\p{S}))+)
		//2)Numeros
		//	2.1)Numeros Exponenciales	([0-9]+[.][0-9]*(E|e)[+]?[0-9]+)
		//	2.2)Numeros Hexadecimales	(0(x|X)([0-9]|[a-fA-F])+)
		//	2.3)Numeros Flotantes		([0-9]+[.][0-9]+)
		//	2.4)Numeros enteros			([0-9]+){1,31}
		//3)Error string incompleto		(""((\w)|(\s)|(\p{P})|(\p{S}))+)
		//4)String						(""((\w)|(\s)|(\p{P})|(\p{S}))+"")
		//5)Identificadores/Reservadas	([a-zA-Z]((\w)|[_])*))
		//6)Operadores
		//	6.1)Juntos					[()]|[{}]|[\[\]] --> Se quito de la ER, la solución esta en Analizar()
		//	6.2)Mas de un caracter		(<=|>=|==|!=|&&|[||])
		//	6.3)De un caracter			Se reconosen como error 8) y luego se identifican ahi
		//7)Error */					([*][/])
		//8)Caracteres de Error			((\p{P}){1}|(\p{S}){1})
		Regex ER = new Regex(@"(//((\w)|(\s)|(\p{P})|(\p{S}))+)|(([0-9]+[.][0-9]*(E|e)[+]?[0-9]+)|(0(x|X)([0-9]|[a-fA-F])+)|([0-9]+[.][0-9]+)|([0-9]+))|(""((\w)|(\s)|(\p{P})|(\p{S}))+)|(""((\w)|(\s)|(\p{P})|(\p{S}))+"")|([a-zA-Z](([\w]|[_])*))|(<=|>=|==|!=|&&|[||]|([*][/]))|((\p{P}){1}|(\p{S}){1})");


		//Constructor
		public AnalizadorLexico(string fileName, string pathFile)
		{
			this.fileName = fileName;
			this.pathFile = pathFile;
			Reservadas = new List<string>();
			Operadores = new List<string>();
			outputLines = new List<string>();

			string[] _Reserverdas = new string[]{"void", "int", "double", "bool", "string", "class", "const", "interface", "null",
				"this", "for", "while", "foreach", "if", "else", "return", "break", "New", "NewArray", "Console", "WriteLine" };
			string[] _Operadores = new string[] { "+", "-", "*", "/", "%", "<", ">", "<=", ">=", "=", "==", "!=",
				"&&", "||", "!", ";", ",", ".", "{", "}", "{}", "[", "]", "[]", "(", ")", "()" };
			Reservadas.AddRange(_Reserverdas);
			Operadores.AddRange(_Operadores);
		}
		public void Analizar()
		{
			int numLinea = 0;
			using (StreamReader reader = new StreamReader(pathFile))
			{
				while (reader.Peek() > -1)
				{
					string linea = reader.ReadLine();
					numLinea++;
					//Asi evita las lineas en blanco
					if (!String.IsNullOrEmpty(linea))
					{
						if (linea.Length > 1 && linea.Substring(0, 2) == "/*")
						{
							while (linea.Substring(linea.Length - 2, 2) != "*/") //Evita el contenido de un comentario /* */
							{
								linea = reader.ReadLine();
								if (String.IsNullOrEmpty(linea)) //EOF
								{
									SetOutputLines("un comentario", numLinea, 1);
									break;
								}
							}
						}
						else
						{
							var match = ER.Match(linea);
							string caracter61 = ""; //Si el match resulta en (, [, { se guarda algo
							while (match.Success)
							{

								string matchRgx = linea.Substring(match.Index, match.Length);
								string TypeToken = getTypeToken(matchRgx); //Type es la Jerarquia de la ER
								string TValue = ""; 
								if (TypeToken == "1") //Quitar esto antes de la entrega final
									SetOutputLines(matchRgx, numLinea, match.Index, (match.Index + match.Length), "Comentario");
								if (TypeToken == "2.1" || TypeToken == "2.3")
									SetOutputLines(matchRgx, numLinea, match.Index, (match.Index + match.Length), ("T_DoubleConstant (value = " + TValue + ")"));
								else if (TypeToken == "2.2" || TypeToken == "2.4")
									SetOutputLines(matchRgx, numLinea, match.Index, (match.Index + match.Length), ("T_IntConstant (value = " + TValue + ")"));
								else if (TypeToken == "3")
									SetOutputLines("", numLinea, 3);
								else if (TypeToken == "4")
									SetOutputLines(matchRgx, numLinea, match.Index, (match.Index + match.Length), ("T_String (value = " + matchRgx + ")"));
								else if (TypeToken == "5")
								{ 
									//Poner metodo para evaluar separas una Reservada de un Identificador
								}
								else if (TypeToken == "6")
									SetOutputLines(matchRgx, numLinea, match.Index, (match.Index + match.Length), ("'" + matchRgx + "'"));
								else if (matchRgx == "*/") //Este seria el TypeToken = "7"
									SetOutputLines("", numLinea, 4);
								else if (TypeToken == "8")
								{
									//Poner metodo para evaluar si es Operador o Error
								}
								match = match.NextMatch();
							}
						}
					}
				}
			}
		}
		private void SetOutputLines(string cadena, int numlinea, int colI, int colF, string T)
		{
			string output = cadena + "			" + "linea " + numlinea + " columnas " + colI + "-" + colF + " es " + T;
			
			outputLines.Add(output);
			Console.WriteLine(output);
		}
		private void SetOutputLines(string cadena, int numlinea, int ErrorType)
		{
			//ErrorType 1 = EOF | 2 = Caracter invalido | 3 = String incompleto | 4 = */
			string output = "";
			if (ErrorType == 1)
				output = "*** EOF en " + cadena + ", linea " + numlinea + " ***";
			else if (ErrorType == 2)
				output = "*** Error linea " + numlinea + " *** Caracter invalido: '" + cadena + "'";
			else if (ErrorType == 3)
				output = "*** Cadena sin terminar en linea " + numlinea + " ***";
			else if (ErrorType == 4)
				output = "*** '*/' fuera de un comentario en linea " + numlinea;

			outputLines.Add(output);
			Console.WriteLine(output);
		}
		private string getTypeToken(string matchRgx) //El numero de retorno representa la jerarquia de la ER
		{
			if (matchRgx.Length > 1 && matchRgx.Substring(0, 2) == "//")
				return "1";

			return "0"; //Si retorna 0 hubo un error
		}
	}
}
