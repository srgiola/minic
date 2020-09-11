using System;
using System.Collections.Generic;
using System.Text;

namespace minic
{
	class AnalizadorSintactico
	{
		// Todas la funciones que comiensen con un _ son primas en la gramatica
		List<Token> Tokens { get; set; }
		string error { get; set; }
		public AnalizadorSintactico(List<Token> _Tokens)
		{
			Tokens = _Tokens;
			Program_();
			error = "";
		}

		private void Consumir()
		{ Tokens.RemoveAt(0); }
		public void Program_()
		{
			Console.WriteLine(Decl());
			Console.WriteLine(_Program());
		}
		private string _Program()
		{
			if (Decl() == "error")
			{
				return "epsilon";
			}
			else
				return _Program();
		}
		private string Decl()
		{
			if (VariableDecl() == "error")
			{
				if (FunctionDecl() == "work")
					return "work";
				else
					return "error sintactico";
			}
			else
				return "work";
		}
		private string VariableDecl()
		{
			if (Variable() == "work")
			{
				if (Tokens[0].content == ";")
				{
					Consumir();
					return "work";
				}
				else
				{
					error = "Error en la lienea " + Tokens[0].numLinea + "Se esperaba ';'";
					return "error";
				}
			}
			else
			{
				return "error";
			}
		}
		private string Variable()
		{
			if (Type() == "work")
			{
				if (Tokens[0].type == "Identificador")
				{
					Consumir();
					return "work";
				}
				else
					return "error";
			}
			else
				return "error";
		}
		private string Type()
		{
			if (Tokens[0].content == "int" || Tokens[0].content == "double" || Tokens[0].content == "bool" || Tokens[0].content == "string")
			{
				Consumir();
				string r_Type = _Type();
				if (r_Type == "epsilon" || r_Type == "work")
					return "work";
				else
					return "error";
			}
			else if (Tokens[0].type == "Identificador")
			{
				return _Type();
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba un Tipo de variable o un Identificador";
				return "error"; 
			}
		}
		private string _Type()
		{
			if (Tokens[0].content == "[]")
			{
				Consumir();
				if (_Type() == "epsilon")
					return "epsilon";
				else
					return "error###"; //Si entra aca hay algo malo
			}
			else
				return "epsilon";
		}
		private string FunctionDecl()
		{
			if (Type() == "work")
			{
				if (Tokens[0].type == "Identificador")
				{
					Consumir();
					if (Tokens[0].content == "(")
					{
						Consumir();
						string r_Formals = Formals();
						if (r_Formals == "work" || r_Formals == "epsilon")
						{
							if (Tokens[0].content == ")")
							{
								Consumir();
								_FunctionDecl(); // Falta programarse
							}
							else
							{
								error = "Error en linea " + Tokens[0].numLinea + " Se esperaba ')'";
								return "error";
							}
						}
						else
							return "error###"; //Si se mete aca hay algo malo
					}
					else
					{
						error = "Error en linea " + Tokens[0].numLinea + " Se esperaba '('";
						return "error";
					}
				}
				else
				{
					error = "Error en linea " + Tokens[0].numLinea + " Se esperaba un Identificador";
					return "error";
				}
			}
			else
			{
				if (Tokens[0].content == "void")
				{
					Consumir();
					if (Tokens[0].type == "Identificador")
					{
						Consumir();
						if (Tokens[0].content == "(")
						{
							Consumir();
							string r_Formals = Formals();
							if (r_Formals == "work" || r_Formals == "epsilon")
							{
								if (Tokens[0].content == ")")
								{
									Consumir();
									_FunctionDecl(); //TERMINAR DE PROGRAMAR ESCENARIO AQUI
								}
								else
								{
									error = "Error en linea " + Tokens[0].numLinea + " Se esperaba ')'";
									return "error";
								}
							}
							else
								return "error###"; //Si se mete aca algo esta malo
						}
						else
						{
							error = "Error en linea " + Tokens[0].numLinea + " Se esperaba '('";
							return "error";
						}
					}
					else
					{
						error = "Error en linea " + Tokens[0].numLinea + " Se esperaba un Identificador";
						return "error";
					}
				}
				else
				{ 
					error = "Error en la linea " + Tokens[0].numLinea + " Se esperada un Identificador o 'void'";
					return "error";
				}
			}
			return "error###"; //Si se mete aca algo esta malo
		}
		private string _FunctionDecl()
		{
			if (Stmt() == "work")
			{
				if (_FunctionDecl() == "epsilon")
					return "epsilon";
				else
					return "error###"; // Si entra aca hay algo malo
			}
			else
				return "epsilon";
		}
		private string Formals()
		{
			if (Variable() == "work")
			{
				string r_Formals = _Formals();
				if (r_Formals == "work" || r_Formals == "epsilon")
				{
					if (Tokens[0].content == ",")
					{
						Consumir();
						return "work";
					}
					else
					{
						error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ','";
						return "error";
					}
				}
				else
					return "error###"; //Si entra aqui hay algo malo
			}
			else
				return "epsilon";
		}
		private string _Formals()
		{
			if (Variable() == "work")
			{
				string r_Formals = _Formals();
				if (r_Formals == "work" || r_Formals == "epsilon")
				{
					return "work";
				}
				else
					return "error###"; //Si entra aqui hay algo malo
			}
			else
				return "epsilon";
		}
		private string Stmt()
		{
			if (ForStmt() == "work")
				return "work";
			else if (PrintStmt() == "work")
				return "work";
			else if (Expr() == "work")
			{
				if (Tokens[0].content == ";")
				{
					Consumir();
					return "work";
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ';'";
					return "error";
				}
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba 'for' | 'Print' | Expresión";
				return "error";
			}
		}
		private string ForStmt()
		{
			if (Tokens[0].content == "for")
			{
				Consumir();
				if (Tokens[0].content == "(")
				{
					Consumir();
					string r_ForStmt = _ForStmt();
					if (r_ForStmt == "work" || r_ForStmt == "epsilon")
					{
						if (Tokens[0].content == ";")
						{
							Consumir();
							if (Expr() == "work")
							{
								if (Tokens[0].content == ";")
								{
									Consumir();
									string r2_ForStmt = _ForStmt();
									if (r2_ForStmt == "work" || r_ForStmt == "epsilon")
									{
										if (Tokens[0].content == ")")
										{
											Consumir();
											if (Stmt() == "work")
											{
												return "work";
											}
											else
											{
												error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba 'for' | 'Print' | Expresión";
												return "error";
											}
										}
										else
										{
											error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ')'";
											return "error";
										}
									}
									else
										return "error###"; //Si entra aca hay algo malo
								}
								else
								{
									error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ';'";
									return "error";
								}
							}
							else
							{
								error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba una Expresión";
								return "error";
							}
						}
						else
						{
							error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ';'";
							return "error";
						}
					}
					else
						return "error###"; //Si entra aca hay clavo
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba '('";
					return "error";
				}
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba 'for'";
				return "error";
			}
		}
		private string _ForStmt()
		{
			if (Expr() == "work")
			{
				return "work";
			}
			else
				return "epsilon";
		}
		private string PrintStmt()
		{
			if (Tokens[0].content == "Print")
			{
				Consumir();
				if (Tokens[0].content == "(")
				{
					Consumir();
					if (Expr() == "work")
					{
						string r_PrintStmt = _PrintStmt();
						if (r_PrintStmt == "work" || r_PrintStmt == "epsilon")
						{
							if (Tokens[0].content == ",")
							{
								Consumir();
								if (Tokens[0].content == ")")
								{
									Consumir();
									if (Tokens[0].content == ";")
									{
										return "work";
									}
									else
									{
										error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ';'";
										return "error";
									}
								}
								else
								{
									error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ')'";
									return "error";
								}
							}
							else
							{
								error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ','";
								return "error";
							}
						}
						else
							return "error###"; //Si entra aca hay clavo
					}
					else
					{
						error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba una Expresión";
						return "error";
					}
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba '('";
					return "error";
				}
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba 'Print'";
				return "error";
			}
		}
		private string _PrintStmt()
		{
			if (Expr() == "work")
			{
				string r_PrintStmt= _PrintStmt();
				if (r_PrintStmt == "work" || r_PrintStmt == "epsilon")
				{
					return "work";
				}
				else
					return "error###"; //Si entra aqui hay algo malo
			}
			else
				return "epsilon";
		}
		private string Expr()
		{
			if (ExprP() == "work")
			{
				string r_Expr = _Expr();
				if (r_Expr == "work" || r_Expr == "epsilon")
				{
					return "work";
				}
				else
					return "error###"; //Si entra hay clavo
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba esperaba una Expresión";
				return "error";
			}
		}
		private string _Expr()
		{
			if (Tokens[0].content == "||")
			{
				Consumir();
				if (ExprP() == "work")
				{
					string r_Expr = _Expr();
					if (r_Expr == "epsilon" || r_Expr == "work")
					{
						return "work";
					}
					else
						return "error###"; //Si entra hay clavo
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se espraba una Expresión";
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprP()
		{
			if (ExprQ() == "work")
			{
				string r_ExprP = _ExprP();
				if (r_ExprP == "work" || r_ExprP == "epsilon")
				{
					return "work";
				}
				else
					return "error###"; //Si entra hay clavo
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba esperaba una Expresión";
				return "error";
			}
		}
		private string _ExprP()	
		{
			if (Tokens[0].content == "&&")
			{
				Consumir();
				if (ExprQ() == "work")
				{
					string r_ExprP = _ExprP();
					if (r_ExprP == "epsilon" || r_ExprP == "work")
					{
						return "work";
					}
					else
						return "error###"; //Si entra hay clavo
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se espraba una Expresión";
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprQ()
		{
			if (ExprR() == "work") //La siguiente
			{
				string r_ExprQ = _ExprQ(); //La prima de esta
				if (r_ExprQ == "work" || r_ExprQ == "epsilon")
				{
					return "work";
				}
				else
					return "error###"; //Si entra hay clavo
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba esperaba una Expresión";
				return "error";
			}
		}
		private string _ExprQ()
		{
			if (Tokens[0].content == "==" || Tokens[0].content == "!=")
			{
				Consumir();
				if (ExprR() == "work") //Esta es la siguiente
				{
					string r_ExprQ = _ExprQ(); //Es la misma
					if (r_ExprQ == "epsilon" || r_ExprQ == "work")
					{
						return "work";
					}
					else
						return "error###"; //Si entra hay clavo
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se espraba una Expresión";
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprR()
		{
			if (ExprS() == "work") //La siguiente
			{
				string r_ExprR = _ExprR(); //La prima de esta
				if (r_ExprR == "work" || r_ExprR == "epsilon")
				{
					return "work";
				}
				else
					return "error###"; //Si entra hay clavo
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba esperaba una Expresión";
				return "error";
			}
		}
		private string _ExprR()
		{
			if (Tokens[0].content == "<" || Tokens[0].content == ">" || Tokens[0].content == "<=" || Tokens[0].content == ">=")
			{
				Consumir();
				if (ExprS() == "work") //Esta es la siguiente
				{
					string r_ExprR = _ExprR(); //Es la misma
					if (r_ExprR == "epsilon" || r_ExprR == "work")
					{
						return "work";
					}
					else
						return "error###"; //Si entra hay clavo
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se espraba una Expresión";
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprS()
		{
			if (ExprT() == "work") //La siguiente
			{
				string r_ExprS = _ExprS(); //La prima de esta
				if (r_ExprS == "work" || r_ExprS == "epsilon")
				{
					return "work";
				}
				else
					return "error###"; //Si entra hay clavo
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba esperaba una Expresión";
				return "error";
			}
		}
		private string _ExprS()
		{
			if (Tokens[0].content == "+" || Tokens[0].content == "-")
			{
				Consumir();
				if (ExprT() == "work") //Esta es la siguiente
				{
					string r_ExprS = _ExprS(); //Es la misma
					if (r_ExprS == "epsilon" || r_ExprS == "work")
					{
						return "work";
					}
					else
						return "error###"; //Si entra hay clavo
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se espraba una Expresión";
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprT()
		{
			if (ExprU() == "work") //La siguiente
			{
				string r_ExprT = _ExprT(); //La prima de esta
				if (r_ExprT == "work" || r_ExprT == "epsilon")
				{
					return "work";
				}
				else
					return "error###"; //Si entra hay clavo
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba esperaba una Expresión";
				return "error";
			}
		}
		private string _ExprT()
		{
			if (Tokens[0].content == "*" || Tokens[0].content == "/" || Tokens[0].content == "%")
			{
				Consumir();
				if (ExprU() == "work") //Esta es la siguiente
				{
					string r_ExprT= _ExprT(); //Es la misma
					if (r_ExprT == "epsilon" || r_ExprT == "work")
					{
						return "work";
					}
					else
						return "error###"; //Si entra hay clavo
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se espraba una Expresión";
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprU()
		{
			if (Tokens[0].content == "(")
			{
				Consumir();
				if (Expr() == "work")
				{
					if (Tokens[0].content == ")")
					{
						Consumir();
						return "work";
					}
				}
			}
			if (LValue() == "work")
			{
				string r_ExprU = _ExprU();
				if (r_ExprU == "work" || r_ExprU == "epsilon")
				{
					return "work";
				}
				else
					return "error###"; //Si entra hay error
			}
			if (Constant() == "work")
				return "work";
			if (Tokens[0].content == "this")
			{
				Consumir();
				return "work"; 
			}
			if (Tokens[0].content == "!")
			{
				Consumir();
				if (Expr() == "work")
					return "work";
			}
			if (Tokens[0].content == "New")
			{
				Consumir();
				if (Tokens[0].content == "(")
				{
					Consumir();
					if (Tokens[0].type == "Identificador")
					{
						Consumir();
						if (Tokens[0].content == ")")
						{
							Consumir();
							return "work";
						}
					}
				}
			}
			if (Tokens[0].content == "-")
			{
				Consumir();
				if (Expr() == "work")
				{
					return "work";
				}
			}
			error = "Error en la linea " + Tokens[0].numLinea + " Expresión incorrecta";
			return "error";
		}
		private string _ExprU()
		{
			if (Tokens[0].content == "-")
			{
				Consumir();
				if (Expr() == "work")
					return "work";
			}
			return "epsilon";
		}
		private string LValue()
		{
			if (Tokens[0].type == "Identificador")
			{
				Consumir();
				return "work"; 
			}
			if (Expr() == "work")
			{
				if (_LValue() == "work")
					return "work";
			}
			error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba un identificador o una Expresión correcta";
			return "error";
		}
		private string _LValue()
		{
			if (Tokens[0].content == ".")
			{
				Consumir();
				if (Tokens[0].type == "Identificador")
				{
					Consumir();
					return "work";
				}
			}
			if (Tokens[0].content == "[")
			{
				Consumir();
				if (Expr() == "work")
				{
					if (Tokens[0].content == "]")
					{
						Consumir();
						return "work";
					}
				}
			}
			error = "Error en la linea " + Tokens[0].numLinea + " Expresión incorrecta";
			return "error";
		}
		private string Constant()
		{
			if (Tokens[0].content == "null" || Tokens[0].type == "Constante")
			{
				return "work";
			}
			else
			{
				error = "Error en linea" + Tokens[0].numLinea + " Se esperaba una constante";
				return "error";
			}
		}
	}
}