using System;
using System.IO;

namespace minic
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Arrastre su archivo a la consola");
			string pathfile = Console.ReadLine();
			//string file = "C:\\Users\\srgio\\Desktop\\Ejemplo.frag";
			//string file = "C";
			if (!File.Exists(pathfile))
				Console.WriteLine("El archivo no existe");
			//Poner un if que no deje iniciar si el archivo esta vacio
			else
			{
				// [0] = file name     [1] = file extencion
				string[] fileParts = Path.GetFileName(pathfile).Split('.');
				if (!(fileParts[1] == "frag"))
					Console.WriteLine("El archivo debe tener una extención valida");
				else
				{
					AnalizadorLexico ALexico = new AnalizadorLexico(fileParts[0], pathfile);
					ALexico.Analizar();
				}
			}
			Console.ReadKey();
		}
	}
}
