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
	}
}
