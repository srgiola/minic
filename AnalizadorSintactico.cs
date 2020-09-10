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
		bool FunctionDecl()
        {

        }
		bool _FuntionDecl()
        {

        }
		bool Formals()
        {

        }
		bool _Formals()
        {

        }
		bool Stmt()
        {

        }
		bool ForStmt()
        {

        }
		bool _ForStmt()
        {

        }
		bool PrintStmt()
        {

        }
		bool _PrintStmt()
        {

        }
		bool Expr()
        {

        }
		bool _Expr()
        {

        }
		bool ExprP()
        {

        }
		bool _ExprP()
        {

        }
		bool ExprQ()
        {

        }
		bool _ExprQ()
        {

        }
		bool ExprR()
        {

        }
		bool _ExprR()
        {

        }
		bool ExprS()
        {

        }
		bool _ExprS()
        {

        }
		bool ExprT()
        {

        }
		bool _ExprT()
        {

        }
		bool ExprU()
        {

        }
		bool LValue()
        {
			if (Tokens[0].type == "Identificador")
			{
				return true;
			}
			else if (Tokens[0].content == "(" || Tokens[0].content == "this" || Tokens[0].content == "!" || Tokens[0].content == "New(" || Tokens[0].content == "-")
			{
				return Expr();
			}
			else return false;
        }
		bool Constant()
        {
			if (Tokens.Count > 0)
			{
				if (Tokens[0].typeConst == "D" || Tokens[0].typeConst == "B" || Tokens[0].typeConst == "S" || Tokens[0].typeConst == "N")
				{
					return true;
				}
				else return false;
			}
			else return false;
        }
	}
}