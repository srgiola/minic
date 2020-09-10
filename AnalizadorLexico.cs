using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace minic
{
	class AnalizadorLexico
	{
		string fileName { get; set; }
		string pathFile { get; set; }
		string pathDirectory { get; set; }
		List<string> Reservadas { get; set; }
		List<string> Operadores { get; set; }
		List<Token> Tokens { get; set; }
		List<string> outputLines { get; set; }
		//Jerarquia de la ER
		//1)String						(""((\w)|(\s)|(\p{P})|(\p{S}))*"")
		//2)Comentarios de linea		(//((\w)|(\s)|(\p{P})|(\p{S}))+)
		//3)Numeros
		//	3.1)Numeros Exponenciales	([0-9]+[.][0-9]*(E|e)([+]|[-])?[0-9]+)
		//	3.2)Numeros Hexadecimales	(0(x|X)([0-9]|[a-fA-F])+)
		//	3.3)Numeros Flotantes		([0-9]+[.][0-9]*)
		//	3.4)Numeros enteros			([0-9]+)
		//4)Error string incompleto		(""((\w)|(\s)|(\p{P})|(\p{S}))+)					
		//5)Identificadores/Reservadas	([a-zA-Z]((\w)|[_])*))
		//6)Operadores
		//	6.1)De agrupación Juntos	[()]|[{}]|[\[\]] --> Se quito de la ER, la solución esta en Analizar()
		//	6.2)De comparación			(<=|>=|==|!=|&&|[||])
		//	6.3)Matematicos				Se reconosen como error 8) y luego se identifican como Operador en getTypeToken(string)
		//7)Error */					([*][/])
		//8)Caracteres de Error			((\p{P}){1}|(\p{S}){1})
		Regex ER = new Regex(@"(""((\w)|(\s)|(\p{P})|(\p{S}))*"")|(//((\w)|(\s)|(\p{P})|(\p{S}))+)|(([0-9]+[.][0-9]*(E|e)([+]|[-])?[0-9]+)|(0(x|X)([0-9]|[a-fA-F])+)|([0-9]+[.][0-9]*)|([0-9]+))|(""((\w)|(\s)|(\p{P})|(\p{S}))+)|([a-zA-Z](([\w]|[_])*))|(<=|>=|==|!=|&&|[||]|([*][/]))|((\p{P}){1}|(\p{S}){1})");

		//Constructor
		public AnalizadorLexico(string fileName, string pathFile, string pathDirectory)
		{
			this.fileName = fileName;
			this.pathFile = pathFile;
			this.pathDirectory = pathDirectory;
			Reservadas = new List<string>();
			Operadores = new List<string>();
			outputLines = new List<string>();
			Tokens = new List<Token>();

			string[] _Reserverdas = new string[]{"void", "int", "double", "bool", "string", "class", "const", "interface", "null",
				"this", "for", "while", "foreach", "if", "else", "return", "break", "New", "NewArray", "Console", "WriteLine",
				"Print"};
			string[] _Operadores = new string[] { "+", "-", "*", "/", "%", "<", ">", "<=", ">=", "=", "==", "!=",
				"&&", "||", "!", ";", ",", ".", "{", "}", "{}", "[", "]", "[]", "(", ")", "()", "." };
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
								numLinea++;
								if (String.IsNullOrEmpty(linea)) //EOF
								{
									SetOutputLines("un comentario", numLinea, 1);
									break;
								}
							}
						}
						else
						{
							Match match = ER.Match(linea);
							string caracter61 = "";
							while (match.Success)
							{
								string matchRgx = linea.Substring(match.Index, match.Length);
								if (matchRgx == "(" || matchRgx == "{" || matchRgx == "[") //Para corregir el error de no aceptar (), [] y {}
								{
									string tmp = linea.Replace("\t", "");
									if (tmp.Length <= 1) { }
									else if (linea.Length > 1 && (match.Index + 1) >= linea.Length) { }
									else if (matchRgx == "(" && linea.Substring(match.Index + 1, 1) == ")")
									{
										caracter61 = matchRgx;
										goto nextMatch;
									}
									else if (matchRgx == "{" && linea.Substring(match.Index + 1, 1) == "}")
									{
										caracter61 = matchRgx;
										goto nextMatch;
									}
									else if (matchRgx == "[" && linea.Substring(match.Index + 1, 1) == "]")
									{
										caracter61 = matchRgx;
										goto nextMatch;
									}
								}
								else if (caracter61.Length > 0)
								{
									matchRgx = caracter61 + matchRgx;
								}

								//string tmpComillas = matchRgx.Replace(" ", "");
								//if (tmpComillas.Length == 3 && tmpComillas[1] == '"' && tmpComillas[2] == '"') //Entra cuando viene un operador seguido por un par de comillas 
								//{
								//	SetOutputLines(tmpComillas[0].ToString(), numLinea, match.Index, (match.Index + 1), ("'" + tmpComillas[0].ToString() + "'"));
								//	matchRgx = "\"\"";
								//}
								//else if (tmpComillas.Length == 2 && tmpComillas[0] == '"' && tmpComillas[1] == '"') //Si viene un identificador/numero seguido por un par de comillas
								//	matchRgx = tmpComillas;

								Token Token;

								string TypeToken = getTypeToken(matchRgx); //Type es la Jerarquia de la ER
								if (TypeToken == "2.1" || TypeToken == "2.3")
								{
									SetOutputLines(matchRgx, numLinea, match.Index, (match.Index + match.Length), ("T_DoubleConstant (value = " + matchRgx + ")"));
									if (TypeToken == "2.1")
										Token = new Token("Constante", matchRgx, numLinea, "E");
									else
										Token = new Token("Constante", matchRgx, numLinea, "F");
									Tokens.Add(Token);
								}
								else if (TypeToken == "2.2" || TypeToken == "2.4")
								{
									SetOutputLines(matchRgx, numLinea, match.Index, (match.Index + match.Length), ("T_IntConstant (value = " + matchRgx + ")"));
									if (TypeToken == "2.2")
										Token = new Token("Constante", matchRgx, numLinea, "H");
									else
										Token = new Token("Constante", matchRgx, numLinea, "D");
									Tokens.Add(Token);
								}
								else if (TypeToken == "3")
									SetOutputLines("una cadena", numLinea, 3);
								else if (TypeToken == "4")
								{
									SetOutputLines(matchRgx, numLinea, match.Index, (match.Index + match.Length), ("T_StringConstant (value = " + matchRgx + ")"));
									Token = new Token("Constante", matchRgx, numLinea, "S");
									Tokens.Add(Token);
								}
								else if (TypeToken == "5")
								{
									if (matchRgx == "true" || matchRgx == "false")
									{
										SetOutputLines(matchRgx, numLinea, match.Index, (match.Index + match.Length), "T_BoolConstant (value = " + matchRgx + ")");
										Token = new Token("Constante", matchRgx, numLinea, "B");
									}
									else
									{
										int tmpCol = 0; //Temporal para llevar el control de las columnas eliminadas en el trunqueo
														//while (matchRgx.Length > 31) //Controla los errores de truncado reduciendo la cadena hasta tener menor o igual a logitud a 31
														//{
														//	string errorTruncado = matchRgx.Substring(0,31);
														//	tmpCol += 31;
														//	SetOutputLines(errorTruncado, numLinea, 5);
														//	matchRgx = matchRgx.Substring(31, matchRgx.Length - 31);
														//}

										if (Reservadas.Contains(matchRgx)) //Busca las palabras reservadas
										{
											SetOutputLines(matchRgx, numLinea, (match.Index + tmpCol), (match.Index + match.Length), ("T_" + matchRgx[0].ToString().ToUpper() + matchRgx.Substring(1, matchRgx.Length - 1)));
											if (matchRgx == "null")
												Token = new Token("Contante", matchRgx, numLinea, "N");
											else
												Token = new Token("PR", matchRgx, numLinea);
											Tokens.Add(Token);
										}
										else //Si no es reservada es un identificador
										{
											if (matchRgx.Length > 31)
											{
												SetOutputLines(matchRgx.Substring(0, 31), numLinea, match.Index, (match.Index + 31), ("T_Identifier"));
												SetOutputLines("", numLinea, 5);
												Token = new Token("Identificador", matchRgx.Substring(0, 31), numLinea);
												Tokens.Add(Token);
											}
											else
											{
												SetOutputLines(matchRgx, numLinea, (match.Index + tmpCol), (match.Index + match.Length), ("T_Identifier"));
												Token = new Token("Identificador", matchRgx, numLinea);
												Tokens.Add(Token);
											}
										}
									}
								}
								else if (TypeToken == "6")
								{
									if (caracter61.Length > 0)
									{
										SetOutputLines(matchRgx, numLinea, (match.Index - 1), ((match.Index) + match.Length), ("'" + matchRgx + "'"));
										caracter61 = "";
										Token = new Token("Operador", matchRgx, numLinea);
										Tokens.Add(Token);
									}
									else
									{
										SetOutputLines(matchRgx, numLinea, match.Index, (match.Index + match.Length), ("'" + matchRgx + "'"));
										Token = new Token("Operador", matchRgx, numLinea);
										Tokens.Add(Token);
									}
								}
								else if (matchRgx == "*/") //Este seria el TypeToken = "7"
									SetOutputLines("", numLinea, 4);
								else if (TypeToken == "8")
									SetOutputLines(matchRgx, numLinea, 2);

								nextMatch:
								match = match.NextMatch();
							}
						}
					}
				}
			}
			//Archivo de Salida del Analizador Lexico
			//using (StreamWriter writer = new StreamWriter(pathDirectory + fileName + ".out"))
			//{
			//	foreach (string linea in outputLines)
			//		writer.WriteLine(linea);
			//}
		}
		private void SetOutputLines(string cadena, int numlinea, int colI, int colF, string T)
		{
			string output = cadena + "		" + "linea " + numlinea + " columnas " + (colI + 1) + "-" + (colF + 1) + " es " + T;
			//outputLines.Add(output);
			//Console.WriteLine(output);
		}
		private void SetOutputLines(string cadena, int numlinea, int ErrorType)
		{
			//ErrorType 1 = EOF | 2 = Caracter invalido | 3 = String incompleto | 4 = */ | 5 = Error de truncado
			string output = "";
			if (ErrorType == 1)
				output = "*** EOF en " + cadena + ", linea " + numlinea + " ***";
			else if (ErrorType == 2)
				output = "*** Error linea " + numlinea + " *** Caracter invalido: '" + cadena + "'";
			else if (ErrorType == 3)
				output = "*** EOF en " + cadena + ", linea " + numlinea + " ***";
			else if (ErrorType == 4)
				output = "*** '*/' fuera de comentario en linea " + numlinea + " ***";
			else if (ErrorType == 5)
				output = "*** Error de truncado, linea " + numlinea + " *** Identificador ***";

			//outputLines.Add(output); //Esto añade los errores al archivo txt
			Console.WriteLine(output);
		}
		private string getTypeToken(string matchRgx) //El numero de retorno representa la jerarquia de la ER
		{
			Regex rgx1 = new Regex(@"(//((\w)|(\s)|(\p{P})|(\p{S}))+)");
			Regex rgx21 = new Regex(@"^([0-9]+[.][0-9]*(E|e)([+]|[-])?[0-9]+)$");
			Regex rgx22 = new Regex(@"^(0(x|X)([0-9]|[a-fA-F])+)$");
			Regex rgx23 = new Regex(@"^([0-9]+[.][0-9]*)$");
			Regex rgx24 = new Regex(@"^([0-9]+)$");
			Regex rgx3 = new Regex(@"(""((\w)|(\s)|(\p{P})|(\p{S}))+)");
			Regex rgx4 = new Regex(@"(""((\w)|(\s)|(\p{P})|(\p{S}))+"")");
			Regex rgx5 = new Regex(@"([a-zA-Z]([\w]|[_])*)");
			Regex rgx6 = new Regex(@"(<=|>=|==|!=|&&|[||])");


			if (rgx4.IsMatch(matchRgx))
				return "4";
			else if (rgx3.IsMatch(matchRgx))
				return "3";
			else if (rgx1.IsMatch(matchRgx))
				return "";
			else if (rgx21.IsMatch(matchRgx))
				return "2.1";
			else if (rgx22.IsMatch(matchRgx))
				return "2.2";
			else if (rgx23.IsMatch(matchRgx))
				return "2.3";
			else if (rgx24.IsMatch(matchRgx))
				return "2.4";
			else if (matchRgx == "\"\"")
				return "4";
			else if (rgx5.IsMatch(matchRgx))
				return "5";
			else if (rgx6.IsMatch(matchRgx) || Operadores.Contains(matchRgx))
				return "6";
			else
				return "8";
		}
		public List<Token> getTokens()
		{ return Tokens; }
	}
}
