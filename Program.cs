using System;
using System.IO;
using System.Collections.Generic;

namespace minic
{
	class Program
	{
		static void Main(string[] args)
		{
			//Falta generar EXE
			// y comentarios multilinea

			List<Token> Tokens;

			Console.WriteLine("Arrastre su archivo a la consola");
			string pathfile = Console.ReadLine();
			if (!File.Exists(pathfile))
				Console.WriteLine("El archivo no existe");
			else if (new FileInfo(pathfile).Length == 0)
				Console.WriteLine("El archivo se encuentra vacio");
			else
			{
				// [0] = file name     [1] = file extencion
				string[] fileParts = Path.GetFileName(pathfile).Split('.');
				string pathDirectory = pathfile.Replace(Path.GetFileName(pathfile), "");
				
				AnalizadorLexico ALexico = new AnalizadorLexico(fileParts[0], pathfile, pathDirectory);
				ALexico.Analizar();
				Tokens = ALexico.getTokens();

				//AnalizadorSintactico ASintactivo = new AnalizadorSintactico(Tokens);

				//SLR SLR = new SLR(Tokens);
				List<Token> Tokens2 = new List<Token>();
				Token t0 = new Token("PR", "id", 4);
				Token t1 = new Token("PR", "+", 4);
				Token t2 = new Token("PR", "id", 4);
				Token t3 = new Token("PR", "*", 4);
				Token t4 = new Token("PR", "id", 4);
				Tokens2.Add(t0);
				Tokens2.Add(t1);
				Tokens2.Add(t2);
				Tokens2.Add(t3);
				Tokens2.Add(t4);

				SLR SLR = new SLR(Tokens2);
				SLR.Iniciar();
			}
			Console.ReadKey();
		}
	}
}
