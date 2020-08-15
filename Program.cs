using System;
using System.IO;

namespace minic
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Ingrese arrastre su archivo");
			string file = Console.ReadLine();
			//string file = "C:\\Users\\srgio\\Desktop\\Ejemplo.frag";
			//string file = "C";
			if (!File.Exists(file))
				Console.WriteLine("El archivo no existe");
			else
			{
				// [0] = file name     [1] = file extencion
				string[] fileParts = Path.GetFileName(file).Split('.');
				if (!(fileParts[1] == "frag"))
					Console.WriteLine("El archivo debe tener una extención valida");
			}
		}
	}
}
