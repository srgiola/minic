using System;
using System.Collections.Generic;
using System.Text;

namespace minic
{
	class AnalizadorSintactico
	{
		// Todas la funciones que comiensen con un _ son primas en la gramatica
		List<Token> Tokens { get; set; }
		public AnalizadorSintactico(List<Token> _Tokens)
		{
			Tokens = _Tokens;
			Program_();
		}
		bool Program_() //Program
		{
			if (!Decl())
				return false;
			else if (Tokens.Count > 0)
				return _Program();
			else
				return false;
		}
		bool Decl()
		{
			if (VariableDecl())
				return true;
			else
				return false;
		}
		bool _Program() // Program'
		{
			if (!Decl())
				return false;
			if (Tokens.Count > 0)
				return _Program();
			if (Tokens.Count == 0)
				return true;
			else
				return false;
		}
		bool VariableDecl()
		{
			if (Variable())
				return true;
			else
				return false;
		}
		//FunctionDecl
		bool Variable()
		{
			if (Type())
			{
				if (Tokens.Count == 0)
					return true;
				if (Tokens[0].type == "Identificador")
				{
					Tokens.RemoveAt(0);
					return true;
				}
				else
				{
					Console.WriteLine("Se esperaba un Identificador [" + Tokens[0].content + "] linea: " + Tokens[0].numLinea);

					return false; 
				}
			}
			else
				return false;
		}
		bool Type()
		{
			if (Tokens[0].content == "int" || Tokens[0].content == "double" || Tokens[0].content == "bool" || Tokens[0].content == "string")
			{
				Tokens.RemoveAt(0);
				return true;
			}
			if (Tokens[0].type == "Identificador")
			{
				Tokens.RemoveAt(0);
				if (_Type())
					return true;
				else
					return false;
			}
			else
				return false;
		}
		bool _Type()
		{
			if (Tokens.Count == 0)
				return true;
			if (Tokens[0].content == "[]")
			{
				Tokens.RemoveAt(0);
				return _Type();
			}
			else
				return false;
		}
	}
}
