using System;
using System.Collections.Generic;
using System.Text;

namespace minic
{
	class Token
	{
		public string type { get; set; } // Operador, PR, Constante, Identificador
		public string content { get; set; } // +, void, 5, hola

		/*
		 * Hexadecimal - H
		 * Exoponencial - E
		 * Entero - I
		 * Decimal - D
		 * Cadena - S
		 * Bool - B
		 * Flotantes - F
		 * Nula - N
		 */
		public string typeConst { get; set; }
		public int numLinea { get; set; }
		public Token(string _type, string _content, int _numLinea, string _typeConst = "")
		{
			type = _type;
			content = _content;
			typeConst = _typeConst;
			numLinea = _numLinea;
		}

		public ObjetoTS crearTS(Token Tk) //Crea aquellos token que llevan esas PR
		{
			ObjetoTS OTS = new ObjetoTS();
			string[] t = new string[] { "int", "double", "bool", "string" };
			List<string> tipos = new List<string>(t);
			if (Tk.type == "PR" && tipos.Contains(Tk.content))
			{
				OTS.tipo = Tk.content;
				OTS.caso = 0;
				return OTS;
			}
			else if (Tk.type == "PR" && Tk.content == "void")
			{
				OTS.tipo = Tk.content;
				OTS.caso = 1;
				return OTS;
			}
			else if (Tk.type == "PR" && Tk.content == "class")
			{
				OTS.tipo = Tk.content;
				OTS.caso = 2;
				return OTS;
			}
			else if (Tk.type == "PR" && Tk.content == "interface")
			{
				OTS.tipo = Tk.content;
				OTS.caso = 3;
				return OTS;
			}
			else
				return null;
		}

		public string identOTS(Token TK)
		{
			if (TK.type == "Identificador" || TK.content == "[]")
				return TK.content;
			else
				return null;
		}

	}
}