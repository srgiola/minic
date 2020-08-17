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
						// (\w) --> Cualquier tipo de palabra
						// (\s) --> Caracteres de escape
						//Regex.Match busca solo las coincidencias con la ER
						//Jerarquia de la ER
						//1)Comentarios de linea	  //((\w)|(\s)|(\p{P})|(\p{S}))+
						//2)Numeros
						//	2.1)Numeros Exponenciales ([0-9]+[.][0-9]*(E|e)[+]?[0-9]+){1,31}
						//	2.2)Numeros Hexadecimales (0(x|X)([0-9]|[a-fA-F])+){1,31}
						//	2.3)Numeros Flotantes	  ([0-9]+[.][0-9]+){1,31}
						//	2.4)Numeros enteros		  ([0-9]+){1,31}
						//3)String					  (""((\w)|(\s)|(\p{P})|(\p{S}))+""){1,31}
						//4)Identificadores			  [a-zA-Z]{1}((\w)|[_])+){0,31}
						//5)Operadores
						//	5.1)Operadores L1		  [()]|[{}]|[\[\]] --> Haberiguar por que no funciona esto
						//	5.2)Operadores L2		  ([+]|[-]|[*]|[/]|[%]|<=|>=|<|>|==|!=|=|&&|[||]|!|;|,|[.]|[[]|[]]|[(]|[)]|{|})
						Regex ER = new Regex(@"(//((\w)|(\s)|(\p{P})|(\p{S}))+)|(([0-9]+[.][0-9]*(E|e)[+]?[0-9]+){1,31}|(0(x|X)([0-9]|[a-fA-F])+){1,31}|([0-9]+[.][0-9]+){1,31}|([0-9]+){1,31})|(""((\w)|(\s)|(\p{P})|(\p{S}))+""){1,31}|([a-zA-Z]{1}(([\w]|[_])+){0,30})|(({.})|(\(.\)))|([+]|[-]|[*]|[/]|[%]|<=|>=|<|>|==|!=|=|&&|[||]|!|;|,|[.]|[[]|[]]|[(]|[)]|{|})");
						var match = ER.Match(linea);
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
