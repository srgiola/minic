using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace minic
{
    class SLR
    {
        List<Token> Tokens = new List<Token>();
        List<Producción> Producciones= new List<Producción>();
        Stack<string> PilaEstados = new Stack<string>();
        Stack<Token> PilaTokens = new Stack<Token>();
        Dictionary<int, Dictionary<string, string>> Estados = new Dictionary<int, Dictionary<string, string>>();
        //TKey = Numero estado
        //TValue = Dicionario con  TKey = Simbolo/Goto, TValue = acción

        //DEBIDO A QUE EL SIMBOLO '$' PUEDE VENIR INGRESADO COMO UN TOKEN STRING SE HA CAMBIADO EL SIMBOLO FINAL A ESTA COMBINACIÓN '$/#'
        //ASI MISMO 'ACC' O 'ACEPTAR' A SERA UTILIZADO LA COMBINACIÓN '/ACC'

        public SLR(List<Token> Tokens)
        {
            //No tocar.
            this.Tokens = Tokens;
            Token t_dollar = new Token("PR", "$/#", 0);
            this.Tokens.Add(t_dollar);

            string[] Simbolos0 = new string[] { "Program" };
            Producción P0 = new Producción(0, 1, "Program'", Simbolos0);
            Producciones.Add(P0);

            string[] Simbolos1 = new string[] { "Decl", "Decl'" };
            Producción P1 = new Producción(1, 2, "Program", Simbolos1);
            Producciones.Add(P1);

            string[] Simbolos2 = new string[] { "Decl", "Decl'" };
            Producción P2 = new Producción(2, 2, "Decl'", Simbolos2);
            Producciones.Add(P2);

            string[] Simbolos3 = new string[] { "" };
            Producción P3 = new Producción(3, 0, "Decl'", Simbolos3);
            Producciones.Add(P3);

            string[] Simbolos4 = new string[] { "Type", "ident", ";" };
            Producción P4 = new Producción(4, 3, "Decl", Simbolos4);
            Producciones.Add(P4);

            string[] Simbolos5 = new string[] { "FunctionDecl" };
            Producción P5 = new Producción(5, 1, "Decl", Simbolos5);
            Producciones.Add(P5);

            string[] Simbolos6 = new string[] { "const", "ConstType", "ident", ";" };
            Producción P6 = new Producción(6, 4, "Decl", Simbolos6);
            Producciones.Add(P6);

            string[] Simbolos7 = new string[] { "class", "ident", "ident'", "id", "{", "Field'", "}" };
            Producción P7 = new Producción(7, 7, "Decl", Simbolos7);
            Producciones.Add(P7);

            string[] Simbolos8 = new string[] { "interface", "ident", "{", "Prototype'", "}" };
            Producción P8 = new Producción(8, 5, "Decl", Simbolos8);
            Producciones.Add(P8);

            string[] Simbolos9 = new string[] { "int" };
            Producción P9 = new Producción(9, 1, "ConstType", Simbolos9);
            Producciones.Add(P9);

            string[] Simbolos10 = new string[] { "double" };
            Producción P10 = new Producción(10, 1, "ConstType", Simbolos10);
            Producciones.Add(P10);

            string[] Simbolos11 = new string[] { "bool" };
            Producción P11 = new Producción(11, 1, "ConstType", Simbolos11);
            Producciones.Add(P11);

            string[] Simbolos12 = new string[] { "string" };
            Producción P12 = new Producción(12, 1, "ConstType", Simbolos12);
            Producciones.Add(P12);

            string[] Simbolos13 = new string[] { "int" };
            Producción P13 = new Producción(13, 1, "Type", Simbolos13);
            Producciones.Add(P13);

            string[] Simbolos14 = new string[] { "double" };
            Producción P14 = new Producción(14, 1, "Type", Simbolos14);
            Producciones.Add(P14);

            string[] Simbolos15 = new string[] { "bool" };
            Producción P15 = new Producción(15, 1, "Type", Simbolos15);
            Producciones.Add(P15);

            string[] Simbolos16 = new string[] { "string" };
            Producción P16 = new Producción(16, 1, "Type", Simbolos16);
            Producciones.Add(P16);

            string[] Simbolos17 = new string[] { "ident" };
            Producción P17 = new Producción(17, 1, "Type", Simbolos17);
            Producciones.Add(P17);

            string[] Simbolos18 = new string[] { "Type", "[", "]" };
            Producción P18 = new Producción(18, 3, "Type", Simbolos18);
            Producciones.Add(P18);

            string[] Simbolos19 = new string[] { "Type", "ident", "(", "Formals", ")", "StmtBlock" };
            Producción P19 = new Producción(19, 6, "FunctionDecl", Simbolos19);
            Producciones.Add(P19);

            string[] Simbolos20 = new string[] { "void", "ident", "(", "Formals", ")", "StmtBlock" };
            Producción P20 = new Producción(20, 6, "FunctionDecl", Simbolos20);
            Producciones.Add(P20);

            string[] Simbolos21 = new string[] { "Type", "ident", "," , "Formals" };
            Producción P21 = new Producción(21, 4, "Formals", Simbolos21);
            Producciones.Add(P21);

            string[] Simbolos22 = new string[] { "Type", "ident" };
            Producción P22 = new Producción(22, 2, "Formals", Simbolos22);
            Producciones.Add(P22);

            string[] Simbolos23 = new string[] { ":", "ident" };
            Producción P23 = new Producción(23, 2, "ident'", Simbolos23);
            Producciones.Add(P23);

            string[] Simbolos24 = new string[] { "" };
            Producción P24 = new Producción(24, 0, "ident'", Simbolos24);
            Producciones.Add(P24);

            string[] Simbolos25 = new string[] { ",", "ident", "id'" };
            Producción P25 = new Producción(25, 3, "id", Simbolos25);
            Producciones.Add(P25);

            string[] Simbolos26 = new string[] { "" };
            Producción P26 = new Producción(26, 0, "id", Simbolos26);
            Producciones.Add(P26);

            string[] Simbolos27 = new string[] { "ident", "id'" };
            Producción P27 = new Producción(27, 2, "id'", Simbolos27);
            Producciones.Add(P27);

            string[] Simbolos28 = new string[] { "" };
            Producción P28 = new Producción(28, 0, "id'", Simbolos28);
            Producciones.Add(P28);

            string[] Simbolos29 = new string[] { "Field" };
            Producción P29 = new Producción(29, 1, "Field'", Simbolos29);
            Producciones.Add(P29);

            string[] Simbolos30 = new string[] { "" };
            Producción P30 = new Producción(30, 0, "Field'", Simbolos30);
            Producciones.Add(P30);

            string[] Simbolos31 = new string[] { "Type", "ident", ";" };
            Producción P31 = new Producción(31, 3, "Field", Simbolos31);
            Producciones.Add(P31);

            string[] Simbolos32 = new string[] { "FunctionDecl" };
            Producción P32 = new Producción(32, 1, "Field", Simbolos32);
            Producciones.Add(P32);

            string[] Simbolos33 = new string[] { "const", "ConstType", "ident", ";" };
            Producción P33 = new Producción(33, 4, "Field", Simbolos33);
            Producciones.Add(P33);

            string[] Simbolos34 = new string[] { "Prototype" };
            Producción P34 = new Producción(34, 1, "Prototype'", Simbolos34);
            Producciones.Add(P34);

            string[] Simbolos35 = new string[] { "" };
            Producción P35 = new Producción(35, 0, "Prototype'", Simbolos35);
            Producciones.Add(P35);

            string[] Simbolos36 = new string[] { "Type", "ident", "(", "Formals", ")", ";" };
            Producción P36 = new Producción(36, 6, "Prototype", Simbolos36);
            Producciones.Add(P36);

            string[] Simbolos37 = new string[] { "void", "ident", "(", "Formals", ")", ";" };
            Producción P37 = new Producción(37, 6, "Prototype", Simbolos37);
            Producciones.Add(P37);

            string[] Simbolos38 = new string[] { "{", "VariableDecl'", "ConstDecl'", "Stmt'", "}" };
            Producción P38 = new Producción(38, 5, "StmtBlock", Simbolos38);
            Producciones.Add(P38);

            string[] Simbolos39 = new string[] { "Type", "ident", ";" };
            Producción P39 = new Producción(39, 3, "VariableDecl'", Simbolos39);
            Producciones.Add(P39);

            string[] Simbolos40 = new string[] { "" };
            Producción P40 = new Producción(40, 0, "VariableDecl'", Simbolos40);
            Producciones.Add(P40);

            string[] Simbolos41 = new string[] { "const", "ConstType", "ident", ";" };
            Producción P41 = new Producción(41, 4, "ConstDecl'", Simbolos41);
            Producciones.Add(P41);

            string[] Simbolos42 = new string[] { "" };
            Producción P42 = new Producción(42, 0, "ConstDecl'", Simbolos42);
            Producciones.Add(P42);

            string[] Simbolos43 = new string[] { "Stmt" };
            Producción P43 = new Producción(43, 1, "Stmt'", Simbolos43);
            Producciones.Add(P43);

            string[] Simbolos44 = new string[] { "" };
            Producción P44 = new Producción(44, 0, "Stmt'", Simbolos44);
            Producciones.Add(P44);

            string[] Simbolos45 = new string[] { "Expr'", ";" };
            Producción P45 = new Producción(45, 2, "Stmt", Simbolos45);
            Producciones.Add(P45);

            string[] Simbolos46 = new string[] { "Expr" };
            Producción P46 = new Producción(46, 1, "Expr'", Simbolos46);
            Producciones.Add(P46);

            string[] Simbolos47 = new string[] { "" };
            Producción P47 = new Producción(47, 0, "Expr'", Simbolos47);
            Producciones.Add(P47);

            string[] Simbolos48 = new string[] { "if", "(", "Expr", ")", "Stmt", "Else" };
            Producción P48 = new Producción(48, 6, "Stmt", Simbolos48);
            Producciones.Add(P48);

            string[] Simbolos49 = new string[] { "" };
            Producción P49 = new Producción(49, , "Stmt", Simbolos49);
            Producciones.Add(P49);

            string[] Simbolos50 = new string[] { "" };
            Producción P50 = new Producción(50, , "Stmt", Simbolos50);
            Producciones.Add(P50);

            string[] Simbolos51 = new string[] { "" };
            Producción P51 = new Producción(51, , "Stmt", Simbolos51);
            Producciones.Add(P51);

            string[] Simbolos52 = new string[] { "" };
            Producción P52 = new Producción(52, , "Stmt", Simbolos52);
            Producciones.Add(P52);

            string[] Simbolos53 = new string[] { "" };
            Producción P53 = new Producción(53, , "Stmt", Simbolos53);
            Producciones.Add(P53);

            string[] Simbolos54 = new string[] { "StmtBlock" };
            Producción P54 = new Producción(54, 1, "Stmt", Simbolos54);
            Producciones.Add(P54);

            string[] Simbolos55 = new string[] { "" };
            Producción P55 = new Producción(55, , "", Simbolos55);
            Producciones.Add(P55);

            string[] Simbolos56 = new string[] { "" };
            Producción P56 = new Producción(56, , "", Simbolos56);
            Producciones.Add(P56);

            string[] Simbolos57 = new string[] { "" };
            Producción P57 = new Producción(57, , "", Simbolos57);
            Producciones.Add(P57);

            string[] Simbolos58 = new string[] { "" };
            Producción P58 = new Producción(58, , "", Simbolos58);
            Producciones.Add(P58);

            string[] Simbolos59 = new string[] { "" };
            Producción P59 = new Producción(59, , "", Simbolos59);
            Producciones.Add(P59);

            string[] Simbolos60 = new string[] { "ExprA", "Expr'" };
            Producción P60 = new Producción(60, 2, "Expr", Simbolos60);
            Producciones.Add(P60);

            string[] Simbolos61 = new string[] { "" };
            Producción P61 = new Producción(61, , "", Simbolos61);
            Producciones.Add(P61);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            string[] Simbolos = new string[] { "" };
            Producción P = new Producción(, , "", Simbolos44);
            Producciones.Add(P);

            /* EJEMPLOS 
            string[] Simbolos0 = new string[] {"E"};
            Producción P0 = new Producción(0, 1, "E'", Simbolos0);
            Producciones.Add(P0);

            string[] Simbolos1 = new string[] { "E", "+", "T" };
            Producción P1 = new Producción(1, 3, "E", Simbolos1);
            Producciones.Add(P1);

            string[] Simbolos2 = new string[] { "T" };
            Producción P2 = new Producción(2, 1, "E", Simbolos2);
            Producciones.Add(P2);

            Dictionary<string, string> Estado0 = new Dictionary<string, string>();
            Estado0.Add("(", "d4");
            Estado0.Add("id", "d5");
            Estado0.Add("E", "1");
            Estado0.Add("T", "2");
            Estado0.Add("F", "3");
            Estados.Add(0, Estado0);

            Dictionary<string, string> Estado1 = new Dictionary<string, string>();
            Estado1.Add("+", "d6");
            Estado1.Add("$/#", "/ACC");
            Estados.Add(1, Estado1);

            Dictionary<string, string> Estado2 = new Dictionary<string, string>();
            Estado2.Add("+", "r2");
            Estado2.Add("*", "d7");
            Estado2.Add(")", "r2");
            Estado2.Add("$/#", "r2");
            Estados.Add(2, Estado2);

           */




            PilaEstados.Push("0");
        }

        public void Iniciar()
        {
            bool aceptar = false;
            int estado_Actual = 0;
            bool proviene_de_R = false;

            while (!aceptar)
            {
                Dictionary<string, string> AuxDicc;
                bool AuxBool = Estados.TryGetValue(estado_Actual, out AuxDicc);
                if (AuxBool)
                {
                    Token token_actual = Tokens[0];
                    string action = "";

                    AuxBool = AuxDicc.TryGetValue(StrToken(token_actual), out action);

                    if (proviene_de_R) //IR_A  -- Ya que siempre que se ejecuta un r# se realiza un Ir_A luego
                    {
                        Token token_Cima = PilaTokens.First();
                        string tmp_Estado_Actual;
                        bool tmpR = AuxDicc.TryGetValue(StrToken(token_Cima), out tmp_Estado_Actual);

                        if (tmpR)
                        { 
                            estado_Actual = int.Parse(tmp_Estado_Actual);
                            PilaEstados.Push(estado_Actual.ToString());
                            proviene_de_R = false;
                        }
                        else
                        {
                            Console.WriteLine("Error no controlado");
                            aceptar = true;
                            break;
                        }
                    }
                    else if (AuxBool)
                    {
                        Regex ER = new Regex(@"[0-9]+");
                        Match match = ER.Match(action);
                        string matchRgx = action.Substring(match.Index, match.Length);
                        switch (TipoAction(action))
                        {
                            case "R": //Reduccion
                                int ID_Producion = int.Parse(matchRgx);
                                Producción P = Producciones.Find(x => (x.ID == ID_Producion));
                                int ultimalinea = 0;

                                for (int i = 0; i < P.cantSimbolos; i++)
                                {
                                    PilaEstados.Pop();
                                    ultimalinea = PilaTokens.First().numLinea;
                                    PilaTokens.Pop();
                                }
                                PilaTokens.Push(CrearTokenEstado(P.productor, ultimalinea));
                                estado_Actual = int.Parse(PilaEstados.First());
                                proviene_de_R = true;
                                break;

                            case "D": //Desplazamiento
                                Token token_cargar = Tokens[0];
                                string estado_cargar = matchRgx;
                                PilaEstados.Push(estado_cargar);
                                PilaTokens.Push(token_cargar);
                                Consumir();
                                estado_Actual = int.Parse(estado_cargar);
                                break;

                            case "/ACC": // Aceptacion
                                Console.WriteLine("Codigo Aceptado");
                                aceptar = true;
                                break;

                            default:
                                Console.WriteLine("Error no controlado");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error, no se encontro transición");
                        aceptar = true;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Estado Inexistente");
                    aceptar = true;
                    break;
                }
            }
        }
        private string TipoAction(string action)
        {
            if (IsR(action))
                return "R"; //Reduccion
            else if (IsD(action))
                return "D"; //Desplazamiento
            else
            {
                if (action == "/ACC")
                    return "/ACC";
                else
                    return "N"; //Ir_A
            }
        }
        private bool IsR(string R)
        {
            if (R[0] == 'r')
                return true;
            else
                return false;
        }
        private bool IsD(string D)
        {
            if (D[0] == 's')
                return true;
            else
                return false;
            //return (D[0] == 's') ? true : false;
        }
        private void Consumir()
        {
            Tokens.RemoveAt(0);
        }

        private Token CrearTokenEstado(string productor, int numLinea)
        {
            Token token = new Token("Estado", productor, numLinea);
            return token;
        }
        private string StrToken(Token token)
        {
            if (token.type == "Constante")
            {
                switch (token.typeConst)
                {
                    case "H": // Hexadecimal
                        return "intConstant";
                    case "E": //Exponencial
                        return "doubleConstant";
                    case "I": //Entero
                        return "intConstant";
                    case "D": //Decimal
                        return "doubleConstant";
                    case "S": //Cadena
                        return "stringConstant";
                    case "B": //Bool
                        return "boolConstant";
                    case "F": //Flotantes
                        return "doubleConstant";
                    default:
                        return "null";
                }
            }
            else if (token.type == "Identificador")
                return "ident";
            else
                return token.content;
        }
    }
}
