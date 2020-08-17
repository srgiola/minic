using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace minic
{
	class AnalizadorLexico
	{
		string fileName { get; set; }
		string pathFile { get; set; }
		List<string> Reservadas { get; set; }
		List<string> Operadores { get; set; }
		
		//Constructor
		public AnalizadorLexico(string fileName, string pathFile)
		{
			this.fileName = fileName;
			this.pathFile = pathFile;
			Reservadas = new List<string>();
			Operadores = new List<string>();
			Inicializar();
		}
		private void Inicializar()
		{
			string[] _Reserverdas = new string []{"void", "int", "double", "bool", "string", "class", "const", 
				"interface", "null", "this", "for", "while", "foreach", "if", "else", "return", "break", "New", 
				"NewArray", "Console", "WriteLine" };
			string[] _Operadores = new string[] { "+", "-", "*", "/", "%", "<", ">", "<=", ">=", "=", "==", "!=",
				"&&", "||", "!", ";", ",", ".", "{", "}", "{}", "[", "]", "[]", "(", ")", "()" };
			Reservadas.AddRange(_Reserverdas);
			Operadores.AddRange(_Operadores);
		}
		public void Analizar()
		{
			using (StreamReader reader = new StreamReader(pathFile))
			{
				while (reader.Peek() > -1)
				{
					string linea = reader.ReadLine();
					//Asi evita las lineas en blanco
					if (!String.IsNullOrEmpty(linea))
					{
						//Regex.Match busca solo las coincidencias con la ER
						Regex BuscadorComentarios = new Regex(@"//((\w)|(\s)|[a-zA-Z]|[0-9])+");
						Regex BuscadorIdentificadores = new Regex(@"([_]|[a-zA-Z]){1}([a-zA-Z]|[_]|[0-9])*");
						var match = BuscadorComentarios.Match(linea);
						//var match = BuscadorIdentificadores.Match(linea);
						while (match.Success)
						{
							Console.WriteLine(linea.Substring(match.Index, match.Length));
							match = match.NextMatch();
							Console.ReadKey();
						}
					}
				}
			}
		}
	}
}
