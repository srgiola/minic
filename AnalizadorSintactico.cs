using System;
using System.Collections.Generic;
using System.Text;

namespace minic
{
	class AnalizadorSintactico
	{
		//Todas la funciones que comiencen con un _ son primas en la gramática.
		List<Token> Tokens { get; set; }
		string error { get; set; }
		int numError { get; set; }
		public AnalizadorSintactico(List<Token> _Tokens)
		{
			Tokens = _Tokens;
			Program_();
			error = "";
			numError = -1;
		}
		private void Consumir()
		{ Tokens.RemoveAt(0); }
		public void Program_()
		{
			while (Tokens.Count > 0)
			{
				Decl();
				_Program();
				Tokens.RemoveAll(x => x.numLinea == numError);
			}
		}
		private string _Program()
		{
			if (Tokens.Count > 0 && Decl() == "error")
			{
				return "epsilon";
			}
			else if (Tokens.Count > 0 && Decl() == "work")
				return _Program();
			else return "error"; //Si entra acá hay clavo.
		}
		private string Decl()
		{
			if (Tokens.Count > 0 && VariableDecl() == "error")
			{
				if (Tokens.Count > 0 && FunctionDecl() == "work")
					return "work";
				else
				{
					Console.WriteLine(error);
					return "error"; 
				}
			}
			else
				return "work";
		}
		private string VariableDecl()
		{
			if (Tokens.Count > 0 && Variable() == "work")
			{
				if (Tokens[0].content == ";")
				{
					Consumir();
					return "work";
				}
				else
				{
					error = "Error en la lienea " + Tokens[0].numLinea + "Se esperaba ';'";
					numError = Tokens[0].numLinea;
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
			if (Tokens.Count > 0 && Type() == "work")
			{
				if (Tokens.Count > 0 && Tokens[0].type == "Identificador")
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
			if (Tokens.Count > 0 && (Tokens[0].content == "int" || Tokens[0].content == "double" || Tokens[0].content == "bool" || Tokens[0].content == "string"))
			{
				Consumir();
				string r_Type = _Type();
				if (r_Type == "epsilon" || r_Type == "work")
					return "work";
				else
					return "error";
			}
			else if (Tokens.Count > 0 && Tokens[0].type == "Identificador")
			{
				return _Type();
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba un Tipo de variable o un Identificador";
				numError = Tokens[0].numLinea;
				return "error"; 
			}
		}
		private string _Type()
		{
			if (Tokens.Count > 0 && Tokens[0].content == "[]")
			{
				Consumir();
				if (Tokens.Count > 0 && _Type() == "epsilon")
					return "epsilon";
				else
					return "error###"; //Si entra aca hay algo malo
			}
			else
				return "epsilon";
		}
		private string FunctionDecl()
		{
			if (Tokens.Count > 0 && Type() == "work")
			{
				if (Tokens.Count > 0 && Tokens[0].type == "Identificador")
				{
					Consumir();
					if (Tokens.Count > 0 && Tokens[0].content == "(")
					{
						Consumir();
						string r_Formals = Formals();
						if (r_Formals == "work" || r_Formals == "epsilon")
						{
							if (Tokens.Count > 0 && Tokens[0].content == ")")
							{
								Consumir();
								_FunctionDecl(); // Falta programarse
							}
							else
							{
								error = "Error en linea " + Tokens[0].numLinea + " Se esperaba ')'";
								numError = Tokens[0].numLinea;
								return "error";
							}
						}
						else
							return "error###"; //Si se mete aca hay algo malo
					}
					else
					{
						error = "Error en linea " + Tokens[0].numLinea + " Se esperaba '('";
						numError = Tokens[0].numLinea;
						return "error";
					}
				}
				else
				{
					error = "Error en linea " + Tokens[0].numLinea + " Se esperaba un Identificador";
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			else
			{
				if (Tokens.Count > 0 && Tokens[0].content == "void")
				{
					Consumir();
					if (Tokens.Count > 0 && Tokens[0].type == "Identificador")
					{
						Consumir();
						if (Tokens.Count > 0 && Tokens[0].content == "(")
						{
							Consumir();
							string r_Formals = Formals();
							if (r_Formals == "work" || r_Formals == "epsilon")
							{
								if (Tokens.Count > 0 && Tokens[0].content == ")")
								{
									Consumir();
									_FunctionDecl(); //TERMINAR DE PROGRAMAR ESCENARIO AQUI
								}
								else
								{
									error = "Error en linea " + Tokens[0].numLinea + " Se esperaba ')'";
									numError = Tokens[0].numLinea;
									return "error";
								}
							}
							else
								return "error###"; //Si se mete aca algo esta malo
						}
						else
						{
							error = "Error en linea " + Tokens[0].numLinea + " Se esperaba '('";
							numError = Tokens[0].numLinea;
							return "error";
						}
					}
					else
					{
						error = "Error en linea " + Tokens[0].numLinea + " Se esperaba un Identificador";
						numError = Tokens[0].numLinea;
						return "error";
					}
				}
				else
				{ 
					error = "Error en la linea " + Tokens[0].numLinea + " Se esperada un Identificador o 'void'";
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			return "error###"; //Si se mete aca algo esta malo
		}
		private string _FunctionDecl()
		{
			if (Tokens.Count > 0 && Stmt() == "work")
			{
				if (Tokens.Count > 0 && _FunctionDecl() == "epsilon")
					return "epsilon";
				else
					return "error###"; // Si entra aca hay algo malo
			}
			else
				return "epsilon";
		}
		private string Formals()
		{
			if (Tokens.Count > 0 && Variable() == "work")
			{
				string r_Formals = _Formals();
				if (r_Formals == "work" || r_Formals == "epsilon")
				{
					if (Tokens.Count > 0 && Tokens[0].content == ",")
					{
						Consumir();
						return "work";
					}
					else
					{
						error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ','";
						numError = Tokens[0].numLinea;
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
			if (Tokens.Count > 0 && Variable() == "work")
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
			if (Tokens.Count > 0 && ForStmt() == "work")
				return "work";
			else if (Tokens.Count > 0 && PrintStmt() == "work")
				return "work";
			else if (Tokens.Count > 0 && Expr() == "work")
			{
				if (Tokens.Count > 0 && Tokens[0].content == ";")
				{
					Consumir();
					return "work";
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ';'";
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba 'for' | 'Print' | Expresión";
				numError = Tokens[0].numLinea;
				return "error";
			}
		}
		private string ForStmt()
		{
			if (Tokens.Count > 0 && Tokens[0].content == "for")
			{
				Consumir();
				if (Tokens.Count > 0 && Tokens[0].content == "(")
				{
					Consumir();
					string r_ForStmt = _ForStmt();
					if (r_ForStmt == "work" || r_ForStmt == "epsilon")
					{
						if (Tokens.Count > 0 && Tokens[0].content == ";")
						{
							Consumir();
							if (Tokens.Count > 0 && Expr() == "work")
							{
								if (Tokens.Count > 0 && Tokens[0].content == ";")
								{
									Consumir();
									string r2_ForStmt = _ForStmt();
									if (r2_ForStmt == "work" || r_ForStmt == "epsilon")
									{
										if (Tokens.Count > 0 && Tokens[0].content == ")")
										{
											Consumir();
											if (Tokens.Count > 0 && Stmt() == "work")
											{
												return "work";
											}
											else
											{
												error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba 'for' | 'Print' | Expresión";
												numError = Tokens[0].numLinea;
												return "error";
											}
										}
										else
										{
											error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ')'";
											numError = Tokens[0].numLinea;
											return "error";
										}
									}
									else
										return "error###"; //Si entra aca hay algo malo
								}
								else
								{
									error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ';'";
									numError = Tokens[0].numLinea;
									return "error";
								}
							}
							else
							{
								error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba una Expresión";
								numError = Tokens[0].numLinea;
								return "error";
							}
						}
						else
						{
							error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ';'";
							numError = Tokens[0].numLinea;
							return "error";
						}
					}
					else
						return "error###"; //Si entra aca hay clavo
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba '('";
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba 'for'";
				numError = Tokens[0].numLinea;
				return "error";
			}
		}
		private string _ForStmt()
		{
			if (Tokens.Count > 0 && Expr() == "work")
			{
				return "work";
			}
			else
				return "epsilon";
		}
		private string PrintStmt()
		{
			if (Tokens.Count > 0 && Tokens[0].content == "Print")
			{
				Consumir();
				if (Tokens.Count > 0 && Tokens[0].content == "(")
				{
					Consumir();
					if (Tokens.Count > 0 && Expr() == "work")
					{
						string r_PrintStmt = _PrintStmt();
						if (r_PrintStmt == "work" || r_PrintStmt == "epsilon")
						{
							if (Tokens.Count > 0 && Tokens[0].content == ",")
							{
								Consumir();
								if (Tokens.Count > 0 && Tokens[0].content == ")")
								{
									Consumir();
									if (Tokens.Count > 0 && Tokens[0].content == ";")
									{
										Consumir();
										return "work";
									}
									else
									{
										error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ';'";
										numError = Tokens[0].numLinea;
										return "error";
									}
								}
								else
								{
									error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ')'";
									numError = Tokens[0].numLinea;
									return "error";
								}
							}
							else
							{
								error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba ','";
								numError = Tokens[0].numLinea;
								return "error";
							}
						}
						else
							return "error###"; //Si entra aca hay clavo
					}
					else
					{
						error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba una Expresión";
						numError = Tokens[0].numLinea;
						return "error";
					}
				}
				else
				{
					error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba '('";
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			else
			{
				error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba 'Print'";
				numError = Tokens[0].numLinea;
				return "error";
			}
		}
		private string _PrintStmt()
		{
			if (Tokens.Count > 0 && Expr() == "work")
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
			if (Tokens.Count > 0 && ExprP() == "work")
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
				numError = Tokens[0].numLinea;
				return "error";
			}
		}
		private string _Expr()
		{
			if (Tokens.Count > 0 && Tokens[0].content == "||")
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
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprP()
		{
			if (Tokens.Count > 0 && ExprQ() == "work")
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
				numError = Tokens[0].numLinea;
				return "error";
			}
		}
		private string _ExprP()	
		{
			if (Tokens.Count > 0 && Tokens[0].content == "&&")
			{
				Consumir();
				if (Tokens.Count > 0 && ExprQ() == "work")
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
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprQ()
		{
			if (Tokens.Count > 0 && ExprR() == "work") //La siguiente
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
				numError = Tokens[0].numLinea;
				return "error";
			}
		}
		private string _ExprQ()
		{
			if (Tokens.Count > 0 && (Tokens[0].content == "==" || Tokens[0].content == "!="))
			{
				Consumir();
				if (Tokens.Count > 0 && ExprR() == "work") //Esta es la siguiente
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
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprR()
		{
			if (Tokens.Count > 0 && ExprS() == "work") //La siguiente
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
				numError = Tokens[0].numLinea;
				return "error";
			}
		}
		private string _ExprR()
		{
			if (Tokens.Count > 0 && (Tokens[0].content == "<" || Tokens[0].content == ">" || Tokens[0].content == "<=" || Tokens[0].content == ">="))
			{
				Consumir();
				if (Tokens.Count > 0 && ExprS() == "work") //Esta es la siguiente
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
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprS()
		{
			if (Tokens.Count > 0 && ExprT() == "work") //La siguiente
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
				numError = Tokens[0].numLinea;
				return "error";
			}
		}
		private string _ExprS()
		{
			if (Tokens.Count > 0 && (Tokens[0].content == "+" || Tokens[0].content == "-"))
			{
				Consumir();
				if (Tokens.Count > 0 && ExprT() == "work") //Esta es la siguiente
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
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprT()
		{
			if (Tokens.Count > 0 && ExprU() == "work") //La siguiente
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
				numError = Tokens[0].numLinea;
				return "error";
			}
		}
		private string _ExprT()
		{
			if (Tokens.Count > 0 && (Tokens[0].content == "*" || Tokens[0].content == "/" || Tokens[0].content == "%"))
			{
				Consumir();
				if (Tokens.Count > 0 && ExprU() == "work") //Esta es la siguiente
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
					numError = Tokens[0].numLinea;
					return "error";
				}
			}
			else
				return "epsilon";
		}
		private string ExprU()
		{
			if (Tokens.Count > 0 && Tokens[0].content == "(")
			{
				Consumir();
				if (Tokens.Count > 0 && Expr() == "work")
				{
					if (Tokens.Count > 0 && Tokens[0].content == ")")
					{
						Consumir();
						return "work";
					}
				}
			}
			if (Tokens.Count > 0 && Constant() == "work")
				return "work";
			if (Tokens.Count > 0 && Tokens[0].content == "this")
			{
				Consumir();
				return "work"; 
			}
			if (Tokens.Count > 0 && Tokens[0].content == "!")
			{
				Consumir();
				if (Tokens.Count > 0 && Expr() == "work")
					return "work";
			}
			if (Tokens.Count > 0 && Tokens[0].content == "New")
			{
				Consumir();
				if (Tokens.Count > 0 && Tokens[0].content == "(")
				{
					Consumir();
					if (Tokens.Count > 0 && Tokens[0].type == "Identificador")
					{
						Consumir();
						if (Tokens.Count > 0 && Tokens[0].content == ")")
						{
							Consumir();
							return "work";
						}
					}
				}
			}
			if (Tokens.Count > 0 && Tokens[0].content == "-")
			{
				Consumir();
				if (Tokens.Count > 0 && Expr() == "work")
				{
					return "work";
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
			error = "Error en la linea " + Tokens[0].numLinea + " Expresión incorrecta";
			numError = Tokens[0].numLinea;
			return "error";
		}
		private string _ExprU()
		{
			if (Tokens.Count > 0 && Tokens[0].content == "-")
			{
				Consumir();
				if (Tokens.Count > 0 && Expr() == "work")
					return "work";
			}
			return "epsilon";
		}
		private string LValue()
		{
			if (Tokens.Count > 0 && Tokens[0].type == "Identificador")
			{
				Consumir();
				return "work"; 
			}
			if (Tokens.Count > 0 && (Tokens[1].content == "." || Tokens[1].content == "[")) //Se aplica Lookahead
			{
				if (Tokens.Count > 0 && Expr() == "work")
				{
					if (_LValue() == "work")
						return "work";
				}
			}
			error = "Error en la linea " + Tokens[0].numLinea + " Se esperaba un identificador o una Expresión correcta";
			numError = Tokens[0].numLinea;
			return "error";
		}
		private string _LValue()
		{
			if (Tokens.Count > 0 && Tokens[0].content == ".")
			{
				Consumir();
				if (Tokens.Count > 0 && Tokens[0].type == "Identificador")
				{
					Consumir();
					return "work";
				}
			}
			if (Tokens.Count > 0 && Tokens[0].content == "[")
			{
				Consumir();
				if (Tokens.Count > 0 && Expr() == "work")
				{
					if (Tokens.Count > 0 && Tokens[0].content == "]")
					{
						Consumir();
						return "work";
					}
				}
			}
			error = "Error en la linea " + Tokens[0].numLinea + " Expresión incorrecta";
			numError = Tokens[0].numLinea;
			return "error";
		}
		private string Constant()
		{
			if (Tokens.Count > 0 && (Tokens[0].content == "null" || Tokens[0].type == "Constante"))
			{
				Consumir();
				return "work";
			}
			else
			{
				error = "Error en linea" + Tokens[0].numLinea + " Se esperaba una constante";
				numError = Tokens[0].numLinea;
				return "error";
			}
		}
	}
}