using System;
using System.Collections.Generic;
using System.Text;

namespace minic
{
	class AnalizadorSintactico
	{
		List<Token> Tokens { get; set; }
		public AnalizadorSintactico(List<Token> _Tokens)
		{
			Tokens = _Tokens;
		}
	}
}
