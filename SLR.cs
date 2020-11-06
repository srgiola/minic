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
        int linea = 0;
        //TKey = Numero estado
        //TValue = Dicionario con  TKey = Simbolo/Goto, TValue = acción

        //DEBIDO A QUE EL SIMBOLO '$' PUEDE VENIR INGRESADO COMO UN TOKEN STRING SE HA CAMBIADO EL SIMBOLO FINAL A ESTA COMBINACIÓN '$/#'
        //ASÍ MISMO 'ACC' O 'ACEPTAR' A SERA UTILIZADO LA COMBINACIÓN '/ACC'

        public SLR(List<Token> Tokens)
        {
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

            string[] Simbolos34 = new string[] { "Prototype", "Prototype'" };
            Producción P34 = new Producción(34, 2, "Prototype'", Simbolos34);
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

            string[] Simbolos39 = new string[] { "Type", "ident", ";", "VariableDecl'" };
            Producción P39 = new Producción(39, 4, "VariableDecl'", Simbolos39);
            Producciones.Add(P39);

            string[] Simbolos40 = new string[] { "" };
            Producción P40 = new Producción(40, 0, "VariableDecl'", Simbolos40);
            Producciones.Add(P40);

            string[] Simbolos41 = new string[] { "const", "ConstType", "ident", ";", "ConstDecl'" };
            Producción P41 = new Producción(41, 5, "ConstDecl'", Simbolos41);
            Producciones.Add(P41);

            string[] Simbolos42 = new string[] { "" };
            Producción P42 = new Producción(42, 0, "ConstDecl'", Simbolos42);
            Producciones.Add(P42);

            string[] Simbolos43 = new string[] { "Stmt", "Stmt'" };
            Producción P43 = new Producción(43, 2, "Stmt'", Simbolos43);
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

            string[] Simbolos49 = new string[] { "while", "(", "Expr", ")", "Stmt" };
            Producción P49 = new Producción(49, 5, "Stmt", Simbolos49);
            Producciones.Add(P49);

            string[] Simbolos50 = new string[] { "for", "(", "Expr", ";", "Expr", ";", "Expr", ")", "Stmt"};
            Producción P50 = new Producción(50, 9, "Stmt", Simbolos50);
            Producciones.Add(P50);

            string[] Simbolos51 = new string[] { "break", ";" };
            Producción P51 = new Producción(51, 2, "Stmt", Simbolos51);
            Producciones.Add(P51);

            string[] Simbolos52 = new string[] { "return", "Expr", ";" };
            Producción P52 = new Producción(52, 3, "Stmt", Simbolos52);
            Producciones.Add(P52);

            string[] Simbolos53 = new string[] { "Console", ".", "WriteLine", "(", "Ex", ",", ")", ";" };
            Producción P53 = new Producción(53, 8, "Stmt", Simbolos53);
            Producciones.Add(P53);

            string[] Simbolos54 = new string[] { "StmtBlock" };
            Producción P54 = new Producción(54, 1, "Stmt", Simbolos54);
            Producciones.Add(P54);

            string[] Simbolos55 = new string[] { "else", "Stmt" };
            Producción P55 = new Producción(55, 2, "Else", Simbolos55);
            Producciones.Add(P55);

            string[] Simbolos56 = new string[] { "" };
            Producción P56 = new Producción(56, 0, "Else", Simbolos56);
            Producciones.Add(P56);

            string[] Simbolos57 = new string[] { "Expr", "Ex'" };
            Producción P57 = new Producción(57, 2, "Ex", Simbolos57);
            Producciones.Add(P57);

            string[] Simbolos58 = new string[] { "Expr", "Ex'" };
            Producción P58 = new Producción(58, 2, "Ex'", Simbolos58);
            Producciones.Add(P58);

            string[] Simbolos59 = new string[] { "" };
            Producción P59 = new Producción(59, 0, "Ex'", Simbolos59);
            Producciones.Add(P59);

            string[] Simbolos60 = new string[] { "ExprA", "Expr'" };
            Producción P60 = new Producción(60, 2, "Expr", Simbolos60);
            Producciones.Add(P60);

            string[] Simbolos61 = new string[] { "&&", "ExprA", "Expr'" };
            Producción P61 = new Producción(61, 3, "Expr'", Simbolos61);
            Producciones.Add(P61);

            string[] Simbolos62 = new string[] { "" };
            Producción P62 = new Producción(62, 0, "Expr'", Simbolos62);
            Producciones.Add(P62);

            string[] Simbolos63 = new string[] { "ExprB", "ExprA'" };
            Producción P63 = new Producción(63, 2, "ExprA", Simbolos63);
            Producciones.Add(P63);

            string[] Simbolos64 = new string[] { "==", "ExprB", "ExprA'" };
            Producción P64 = new Producción(64, 3, "ExprA'", Simbolos64);
            Producciones.Add(P64);

            string[] Simbolos65 = new string[] { "" };
            Producción P65 = new Producción(65, 0, "ExprA'", Simbolos65);
            Producciones.Add(P65);

            string[] Simbolos66 = new string[] { "ExprC", "ExprB'" };
            Producción P66 = new Producción(66, 2, "ExprB", Simbolos66);
            Producciones.Add(P66);

            string[] Simbolos67 = new string[] { "<", "ExprC", "ExprB'" };
            Producción P67 = new Producción(67, 3, "ExprB'", Simbolos67);
            Producciones.Add(P67);

            string[] Simbolos68 = new string[] { "" };
            Producción P68 = new Producción(68, 0, "ExprB'", Simbolos68);
            Producciones.Add(P68);

            string[] Simbolos69 = new string[] { "ExprD", "ExprC'" };
            Producción P69 = new Producción(69, 2, "ExprC", Simbolos69);
            Producciones.Add(P69);

            string[] Simbolos70 = new string[] { "<=", "ExprD", "ExprC'" };
            Producción P70 = new Producción(70, 3, "ExprC'", Simbolos70);
            Producciones.Add(P70);

            string[] Simbolos71 = new string[] { "" };
            Producción P71 = new Producción(71, 0, "ExprC'", Simbolos71);
            Producciones.Add(P71);

            string[] Simbolos72 = new string[] { "ExprE", "ExprD'" };
            Producción P72 = new Producción(72, 2, "ExprD", Simbolos72);
            Producciones.Add(P72);

            string[] Simbolos73 = new string[] { "+", "ExprE", "ExprD'" };
            Producción P73 = new Producción(73, 3, "ExprD'", Simbolos73);
            Producciones.Add(P73);

            string[] Simbolos74 = new string[] { "" };
            Producción P74 = new Producción(74, 0, "ExprD'", Simbolos74);
            Producciones.Add(P74);

            string[] Simbolos75 = new string[] { "ExprF", "ExprE'" };
            Producción P75 = new Producción(75, 2, "ExprE", Simbolos75);
            Producciones.Add(P75);

            string[] Simbolos76 = new string[] { "*", "ExprF", "ExprE'" };
            Producción P76 = new Producción(76, 3, "ExprE'", Simbolos76);
            Producciones.Add(P76);

            string[] Simbolos77 = new string[] { "" };
            Producción P77 = new Producción(77, 0, "ExprE'", Simbolos77);
            Producciones.Add(P77);

            string[] Simbolos78 = new string[] { "ExprG", "ExprF'" };
            Producción P78 = new Producción(78, 2, "ExprF", Simbolos78);
            Producciones.Add(P78);

            string[] Simbolos79 = new string[] { "%", "ExprG", "ExprF'" };
            Producción P79 = new Producción(79, 3, "ExprF'", Simbolos79);
            Producciones.Add(P79);

            string[] Simbolos80 = new string[] { "" };
            Producción P80 = new Producción(80, 0, "ExprF'", Simbolos80);
            Producciones.Add(P80);

            string[] Simbolos81 = new string[] { "!", "Expr" };
            Producción P81 = new Producción(81, 2, "ExprG", Simbolos81);
            Producciones.Add(P81);

            string[] Simbolos82 = new string[] { "New", "(", "ident", ")" };
            Producción P82 = new Producción(82, 4, "ExprG", Simbolos82);
            Producciones.Add(P82);

            string[] Simbolos83 = new string[] { "LValue", "=", "Expr" };
            Producción P83 = new Producción(83, 3, "ExprG", Simbolos83);
            Producciones.Add(P83);

            string[] Simbolos84 = new string[] { "Constant" };
            Producción P84 = new Producción(84, 1, "ExprG", Simbolos84);
            Producciones.Add(P84);

            string[] Simbolos85 = new string[] { "LValue" };
            Producción P85 = new Producción(85, 1, "ExprG", Simbolos85);
            Producciones.Add(P85);

            string[] Simbolos86 = new string[] { "this" };
            Producción P86 = new Producción(86, 1, "ExprG", Simbolos86);
            Producciones.Add(P86);

            string[] Simbolos87 = new string[] { "(", "Expr", ")" };
            Producción P87 = new Producción(87, 3, "ExprG", Simbolos87);
            Producciones.Add(P87);

            string[] Simbolos88 = new string[] { "-", "Expr" };
            Producción P88 = new Producción(88, 2, "ExprG", Simbolos88);
            Producciones.Add(P88);

            string[] Simbolos89 = new string[] { "ident" };
            Producción P89 = new Producción(89, 1, "LValue", Simbolos89);
            Producciones.Add(P89);

            string[] Simbolos90 = new string[] { "Expr", ".", "ident" };
            Producción P90 = new Producción(90, 3, "LValue", Simbolos90);
            Producciones.Add(P90);

            string[] Simbolos91 = new string[] { "intConstant" };
            Producción P91 = new Producción(91, 1, "Constant", Simbolos91);
            Producciones.Add(P91);

            string[] Simbolos92 = new string[] { "doubleConstant" };
            Producción P92 = new Producción(92, 1, "Constant", Simbolos92);
            Producciones.Add(P92);

            string[] Simbolos93 = new string[] { "boolConstant" };
            Producción P93 = new Producción(93, 1, "Constant", Simbolos93);
            Producciones.Add(P93);

            string[] Simbolos94 = new string[] { "stringConstant" };
            Producción P94 = new Producción(94, 1, "Constant", Simbolos94);
            Producciones.Add(P94);

            string[] Simbolos95 = new string[] { "null" };
            Producción P95 = new Producción(95, 1, "Constant", Simbolos95);
            Producciones.Add(P95);

            Dictionary<string, string> Estado0 = new Dictionary<string, string>();
            Estado0.Add("ident", "s12");
            Estado0.Add("const", "s5");
            Estado0.Add("class", "s6");
            Estado0.Add("interface", "s7");
            Estado0.Add("int", "s8");
            Estado0.Add("double", "s9");
            Estado0.Add("bool", "s10");
            Estado0.Add("string", "s11");
            Estado0.Add("void", "s13");
            Estado0.Add("Program", "1");
            Estado0.Add("Decl", "2");
            Estado0.Add("Type", "3");
            Estado0.Add("FunctionDecl", "4");
            Estados.Add(0, Estado0);

            Dictionary<string, string> Estado1 = new Dictionary<string, string>();
            Estado1.Add("$/#", "#ACC");
            Estados.Add(1, Estado1);

            Dictionary<string, string> Estado2 = new Dictionary<string, string>();
            Estado2.Add("ident", "s12");
            Estado2.Add("const", "s5");
            Estado2.Add("class", "s6");
            Estado2.Add("interface", "s7");
            Estado2.Add("int", "s8");
            Estado2.Add("double", "s9");
            Estado2.Add("bool", "s10");
            Estado2.Add("string", "s11");
            Estado2.Add("void", "s13");
            Estado2.Add("$/#", "r3");
            Estado2.Add("Decl'", "14");
            Estado2.Add("Decl", "15");
            Estado2.Add("Type", "3");
            Estado2.Add("FunctionDecl", "4");
            Estados.Add(2, Estado2);

            Dictionary<string, string> Estado3 = new Dictionary<string, string>();
            Estado3.Add("ident", "s16");
            Estado3.Add("[", "s17");
            Estados.Add(3, Estado3);

            Dictionary<string, string> Estado4 = new Dictionary<string, string>();
            Estado4.Add("ident", "r5");
            Estado4.Add("const", "r5");
            Estado4.Add("class", "r5");
            Estado4.Add("interface", "r5");
            Estado4.Add("int", "r5");
            Estado4.Add("double", "r5");
            Estado4.Add("bool", "r5");
            Estado4.Add("string", "r5");
            Estado4.Add("void", "r5");
            Estado4.Add("$/#", "r5");
            Estados.Add(4, Estado4);

            Dictionary<string, string> Estado5 = new Dictionary<string, string>();
            Estado5.Add("int", "s19");
            Estado5.Add("double", "s20");
            Estado5.Add("bool", "s21");
            Estado5.Add("string", "s22");
            Estado5.Add("ConstType", "18");
            Estados.Add(5, Estado5);

            Dictionary<string, string> Estado6 = new Dictionary<string, string>();
            Estado6.Add("ident", "s23");
            Estados.Add(6, Estado6);

            Dictionary<string, string> Estado7 = new Dictionary<string, string>();
            Estado7.Add("ident", "s24");
            Estados.Add(7, Estado7);

            Dictionary<string, string> Estado8 = new Dictionary<string, string>();
            Estado8.Add("ident", "r13");
            Estado8.Add("[", "r13");
            Estados.Add(8, Estado8);

            Dictionary<string, string> Estado9 = new Dictionary<string, string>();
            Estado9.Add("ident", "r14");
            Estado9.Add("[", "r14");
            Estados.Add(9, Estado9);

            Dictionary<string, string> Estado10 = new Dictionary<string, string>();
            Estado10.Add("ident", "r15");
            Estado10.Add("[", "r15");
            Estados.Add(10, Estado10);

            Dictionary<string, string> Estado11 = new Dictionary<string, string>();
            Estado11.Add("ident", "r16");
            Estado11.Add("[", "r16");
            Estados.Add(11, Estado11);

            Dictionary<string, string> Estado12 = new Dictionary<string, string>();
            Estado12.Add("ident", "r17");
            Estado12.Add("[", "r17");
            Estados.Add(12, Estado12);

            Dictionary<string, string> Estado13 = new Dictionary<string, string>();
            Estado13.Add("ident", "s25");
            Estados.Add(13, Estado13);

            Dictionary<string, string> Estado14 = new Dictionary<string, string>();
            Estado14.Add("$/#", "r1");
            Estados.Add(14, Estado14);

            Dictionary<string, string> Estado15 = new Dictionary<string, string>();
            Estado15.Add("ident", "s12");
            Estado15.Add("const", "s5");
            Estado15.Add("class", "s6");
            Estado15.Add("interface", "s7");
            Estado15.Add("int", "s8");
            Estado15.Add("double", "s9");
            Estado15.Add("bool", "s10");
            Estado15.Add("string", "s11");
            Estado15.Add("void", "s13");
            Estado15.Add("$/#", "r3");
            Estado15.Add("Decl'", "26");
            Estado15.Add("Decl", "15");
            Estado15.Add("Type", "3");
            Estado15.Add("FunctionDecl", "4");
            Estados.Add(15, Estado15);

            Dictionary<string, string> Estado16 = new Dictionary<string, string>();
            Estado16.Add(";", "s27");
            Estado16.Add("(", "s28");
            Estados.Add(16, Estado16);

            Dictionary<string, string> Estado17 = new Dictionary<string, string>();
            Estado17.Add("]", "s29");
            Estados.Add(17, Estado17);

            Dictionary<string, string> Estado18 = new Dictionary<string, string>();
            Estado18.Add("ident", "s30");
            Estados.Add(18, Estado18);

            Dictionary<string, string> Estado19 = new Dictionary<string, string>();
            Estado19.Add("ident", "r9");
            Estados.Add(19, Estado19);

            Dictionary<string, string> Estado20 = new Dictionary<string, string>();
            Estado20.Add("ident", "r10");
            Estados.Add(20, Estado20);

            Dictionary<string, string> Estado21 = new Dictionary<string, string>();
            Estado21.Add("ident", "r11");
            Estados.Add(21, Estado21);

            Dictionary<string, string> Estado22 = new Dictionary<string, string>();
            Estado22.Add("ident", "r12");
            Estados.Add(22, Estado22);

            Dictionary<string, string> Estado23 = new Dictionary<string, string>();
            Estado23.Add("ident", "r24");
            Estado23.Add("const", "r24");
            Estado23.Add("class", "r24");
            Estado23.Add("{", "r24");
            Estado23.Add("interface", "r24");
            Estado23.Add("int", "r24");
            Estado23.Add("double", "r24");
            Estado23.Add("bool", "r24");
            Estado23.Add("string", "r24");
            Estado23.Add("void", "r24");
            Estado23.Add(",", "r24");
            Estado23.Add(":", "s32");
            Estado23.Add("$/#", "r24");
            Estado23.Add("ident'", "31");
            Estados.Add(23, Estado23);

            Dictionary<string, string> Estado24 = new Dictionary<string, string>();
            Estado24.Add("{", "s33");
            Estados.Add(24, Estado24);

            Dictionary<string, string> Estado25 = new Dictionary<string, string>();
            Estado25.Add("(", "s34");
            Estados.Add(25, Estado25);

            Dictionary<string, string> Estado26 = new Dictionary<string, string>();
            Estado26.Add("$/#", "r2");
            Estados.Add(26, Estado26);

            Dictionary<string, string> Estado27 = new Dictionary<string, string>();
            Estado27.Add("ident", "r4");
            Estado27.Add("const", "r4");
            Estado27.Add("class", "r4");
            Estado27.Add("interface", "r4");
            Estado27.Add("int", "r4");
            Estado27.Add("double", "r4");
            Estado27.Add("bool", "r4");
            Estado27.Add("string", "r4");
            Estado27.Add("void", "r4");
            Estado27.Add("$/#", "r4");
            Estados.Add(27, Estado27);

            Dictionary<string, string> Estado28 = new Dictionary<string, string>();
            Estado28.Add("ident", "s12");
            Estado28.Add("int", "s8");
            Estado28.Add("double", "s9");
            Estado28.Add("bool", "s10");
            Estado28.Add("string", "s11");
            Estado28.Add("Type", "36");
            Estado28.Add("Formals", "35");
            Estados.Add(28, Estado28);

            Dictionary<string, string> Estado29 = new Dictionary<string, string>();
            Estado29.Add("ident", "r18");
            Estado29.Add("[", "r18");
            Estados.Add(29, Estado29);

            Dictionary<string, string> Estado30 = new Dictionary<string, string>();
            Estado30.Add(";", "s37");
            Estados.Add(30, Estado30);

            Dictionary<string, string> Estado31 = new Dictionary<string, string>();
            Estado31.Add("{", "r26");
            Estado31.Add(",", "s39");
            Estado31.Add("id", "38");
            Estados.Add(31, Estado31);

            Dictionary<string, string> Estado32 = new Dictionary<string, string>();
            Estado32.Add("ident", "s40");
            Estados.Add(32, Estado32);

            Dictionary<string, string> Estado33 = new Dictionary<string, string>();
            Estado33.Add("ident", "s12");
            Estado33.Add("}", "r35");
            Estado33.Add("int", "s8");
            Estado33.Add("double", "s9");
            Estado33.Add("bool", "s10");
            Estado33.Add("string", "s11");
            Estado33.Add("void", "s44");
            Estado33.Add("Type", "43");
            Estado33.Add("Prototype'", "41");
            Estado33.Add("Prototype", "42");
            Estados.Add(33, Estado33);

            Dictionary<string, string> Estado34 = new Dictionary<string, string>();
            Estado34.Add("ident", "s12");
            Estado34.Add("int", "s8");
            Estado34.Add("double", "s9");
            Estado34.Add("bool", "s10");
            Estado34.Add("string", "s11");
            Estado34.Add("Type", "36");
            Estado34.Add("Formals", "45");
            Estados.Add(34, Estado34);

            Dictionary<string, string> Estado35 = new Dictionary<string, string>();
            Estado35.Add(")", "s46");
            Estados.Add(35, Estado35);

            Dictionary<string, string> Estado36 = new Dictionary<string, string>();
            Estado36.Add("ident", "s47");
            Estado36.Add("[", "s17");
            Estados.Add(36, Estado36);

            Dictionary<string, string> Estado37 = new Dictionary<string, string>();
            Estado37.Add("ident", "r6");
            Estado37.Add("const", "r6");
            Estado37.Add("class", "r6");
            Estado37.Add("interface", "r6");
            Estado37.Add("int", "r6");
            Estado37.Add("double", "r6");
            Estado37.Add("bool", "r6");
            Estado37.Add("string", "r6");
            Estado37.Add("void", "r6");
            Estado37.Add("$/#", "r6");
            Estados.Add(37, Estado37);

            Dictionary<string, string> Estado38 = new Dictionary<string, string>();
            Estado38.Add("{", "s48");
            Estados.Add(38, Estado38);

            Dictionary<string, string> Estado39 = new Dictionary<string, string>();
            Estado39.Add("ident", "s49");
            Estados.Add(39, Estado39);

            Dictionary<string, string> Estado40 = new Dictionary<string, string>();
            Estado40.Add("ident", "r23");
            Estado40.Add("const", "r23");
            Estado40.Add("class", "r23");
            Estado40.Add("{", "r23");
            Estado40.Add("interface", "r23");
            Estado40.Add("int", "r23");
            Estado40.Add("double", "r23");
            Estado40.Add("bool", "r23");
            Estado40.Add("string", "r23");
            Estado40.Add("void", "r23");
            Estado40.Add(",", "r23");
            Estado40.Add("$/#", "r23");
            Estados.Add(40, Estado40);

            Dictionary<string, string> Estado41 = new Dictionary<string, string>();
            Estado41.Add("}", "s50");
            Estados.Add(41, Estado41);

            Dictionary<string, string> Estado42 = new Dictionary<string, string>();
            Estado42.Add("ident", "s12");
            Estado42.Add("}", "r35");
            Estado42.Add("int", "s8");
            Estado42.Add("double", "s9");
            Estado42.Add("bool", "s10");
            Estado42.Add("string", "s11");
            Estado42.Add("void", "s44");
            Estado42.Add("Type", "43");
            Estado42.Add("Prototype'", "51");
            Estado42.Add("Prototype", "42");
            Estados.Add(42, Estado42);

            Dictionary<string, string> Estado43 = new Dictionary<string, string>();
            Estado43.Add("ident", "s52");
            Estado43.Add("[", "s17");
            Estados.Add(43, Estado43);

            Dictionary<string, string> Estado44 = new Dictionary<string, string>();
            Estado44.Add("ident", "s53");
            Estados.Add(44, Estado44);

            Dictionary<string, string> Estado45 = new Dictionary<string, string>();
            Estado45.Add(")", "s54");
            Estados.Add(45, Estado45);

            Dictionary<string, string> Estado46 = new Dictionary<string, string>();
            Estado46.Add("{", "s56");
            Estado46.Add("StmtBlock", "55");
            Estados.Add(46, Estado46);

            Dictionary<string, string> Estado47 = new Dictionary<string, string>();
            Estado47.Add(")", "r22");
            Estado47.Add(",", "s57");
            Estados.Add(47, Estado47);

            Dictionary<string, string> Estado48 = new Dictionary<string, string>();
            Estado48.Add("ident", "s12");
            Estado48.Add("const", "s62");
            Estado48.Add("}", "r30");
            Estado48.Add("int", "s8");
            Estado48.Add("double", "s9");
            Estado48.Add("bool", "s10");
            Estado48.Add("string", "s11");
            Estado48.Add("void", "s13");
            Estado48.Add("Type", "60");
            Estado48.Add("FunctionDecl", "61");
            Estado48.Add("Field'", "58");
            Estado48.Add("Field", "59");
            Estados.Add(48, Estado48);

            Dictionary<string, string> Estado49 = new Dictionary<string, string>();
            Estado49.Add("ident", "s64");
            Estado49.Add(",", "r28");
            Estado49.Add("id'", "63");
            Estados.Add(49, Estado49);

            Dictionary<string, string> Estado50 = new Dictionary<string, string>();
            Estado50.Add("ident", "r8");
            Estado50.Add("const", "r8");
            Estado50.Add("class", "r8");
            Estado50.Add("interface", "r8");
            Estado50.Add("int", "r8");
            Estado50.Add("double", "r8");
            Estado50.Add("bool", "r8");
            Estado50.Add("string", "r8");
            Estado50.Add("void", "r8");
            Estado50.Add("$/#", "r8");
            Estados.Add(50, Estado50);

            Dictionary<string, string> Estado51 = new Dictionary<string, string>();
            Estado51.Add("}", "r34");
            Estados.Add(51, Estado51);

            Dictionary<string, string> Estado52 = new Dictionary<string, string>();
            Estado52.Add("(", "s65");
            Estados.Add(52, Estado52);

            Dictionary<string, string> Estado53 = new Dictionary<string, string>();
            Estado53.Add("(", "s66");
            Estados.Add(53, Estado53);

            Dictionary<string, string> Estado54 = new Dictionary<string, string>();
            Estado54.Add("{", "s56");
            Estado54.Add("StmtBlock", "67");
            Estados.Add(54, Estado54);

            Dictionary<string, string> Estado55 = new Dictionary<string, string>();
            Estado55.Add("ident", "r19");
            Estado55.Add("const", "r19");
            Estado55.Add("class", "r19");
            Estado55.Add("}", "r19");
            Estado55.Add("interface", "r19");
            Estado55.Add("int", "r19");
            Estado55.Add("double", "r19");
            Estado55.Add("bool", "r19");
            Estado55.Add("string", "r19");
            Estado55.Add("void", "r19");
            Estado55.Add("$/#", "r19");
            Estados.Add(55, Estado55);

            Dictionary<string, string> Estado56 = new Dictionary<string, string>();
            Estado56.Add("ident", "s12 ");
            Dictionary<string, string> Estado56_ = new Dictionary<string, string>();
            Estado56_.Add("ident", " r40");
            Estado56.Add(";", "r40");
            Estado56.Add("const", "r40");
            Estado56.Add("class", "r40");
            Estado56.Add("{", "r40");
            Estado56.Add("}", "r40");
            Estado56.Add("interface", "r40");
            Estado56.Add("int", "s8 ");
            Estado56_.Add("int", " r40");
            Estado56.Add("double", "s9 ");
            Estado56_.Add("double", " r40");
            Estado56.Add("bool", "s10 ");
            Estado56_.Add("bool", " r40");
            Estado56.Add("string", "s11 ");
            Estado56_.Add("string", " r40");
            Estado56.Add("(", "r40");
            Estado56.Add("void", "r40");
            Estado56.Add("if", "r40");
            Estado56.Add("while", "r40");
            Estado56.Add("for", "r40");
            Estado56.Add("break", "r40");
            Estado56.Add("return", "r40");
            Estado56.Add("Console", "r40");
            Estado56.Add("else", "r40");
            Estado56.Add("&&", "r40");
            Estado56.Add("*", "r40");
            Estado56.Add("!", "r40");
            Estado56.Add("New", "r40");
            Estado56.Add("this", "r40");
            Estado56.Add("-", "r40");
            Estado56.Add("intConstant", "r40");
            Estado56.Add("doubleConstant", "r40");
            Estado56.Add("boolConstant", "r40");
            Estado56.Add("stringConstant", "r40");
            Estado56.Add("null", "r40");
            Estado56.Add("$/#", "r40");
            Estado56.Add("Type", "69");
            Estado56.Add("VariableDecl'", "68");
            Estados.Add(56, Estado56);
            Estados.Add(560, Estado56_);

            Dictionary<string, string> Estado57 = new Dictionary<string, string>();
            Estado57.Add("ident", "s12");
            Estado57.Add("int", "s8");
            Estado57.Add("double", "s9");
            Estado57.Add("bool", "s10");
            Estado57.Add("string", "s11");
            Estado57.Add("Type", "36");
            Estado57.Add("Formals", "70");
            Estados.Add(57, Estado57);

            Dictionary<string, string> Estado58 = new Dictionary<string, string>();
            Estado58.Add("}", "s71");
            Estados.Add(58, Estado58);

            Dictionary<string, string> Estado59 = new Dictionary<string, string>();
            Estado59.Add("ident", "s12");
            Estado59.Add("const", "s62");
            Estado59.Add("}", "r30");
            Estado59.Add("int", "s8");
            Estado59.Add("double", "s9");
            Estado59.Add("bool", "s10");
            Estado59.Add("string", "s11");
            Estado59.Add("void", "s13");
            Estado59.Add("Type", "60");
            Estado59.Add("FunctionDecl", "61");
            Estado59.Add("Field'", "72");
            Estado59.Add("Field", "59");
            Estados.Add(59, Estado59);

            Dictionary<string, string> Estado60 = new Dictionary<string, string>();
            Estado60.Add("ident", "s73");
            Estado60.Add("[", "s17");
            Estados.Add(60, Estado60);

            Dictionary<string, string> Estado61 = new Dictionary<string, string>();
            Estado61.Add("ident", "r32");
            Estado61.Add("const", "r32");
            Estado61.Add("}", "r32");
            Estado61.Add("int", "r32");
            Estado61.Add("double", "r32");
            Estado61.Add("bool", "r32");
            Estado61.Add("string", "r32");
            Estado61.Add("void", "r32");
            Estados.Add(61, Estado61);

            Dictionary<string, string> Estado62 = new Dictionary<string, string>();
            Estado62.Add("int", "s19");
            Estado62.Add("double", "s20");
            Estado62.Add("bool", "s21");
            Estado62.Add("string", "s22");
            Estado62.Add("ConstType", "74");
            Estados.Add(62, Estado62);

            Dictionary<string, string> Estado63 = new Dictionary<string, string>();
            Estado63.Add(",", "s75");
            Estados.Add(63, Estado63);

            Dictionary<string, string> Estado64 = new Dictionary<string, string>();
            Estado64.Add("ident", "s64");
            Estado64.Add(",", "r28");
            Estado64.Add("id'", "76");
            Estados.Add(64, Estado64);

            Dictionary<string, string> Estado65 = new Dictionary<string, string>();
            Estado65.Add("ident", "s12");
            Estado65.Add("int", "s8");
            Estado65.Add("double", "s9");
            Estado65.Add("bool", "s10");
            Estado65.Add("string", "s11");
            Estado65.Add("Type", "36");
            Estado65.Add("Formals", "77");
            Estados.Add(65, Estado65);

            Dictionary<string, string> Estado66 = new Dictionary<string, string>();
            Estado66.Add("ident", "s12");
            Estado66.Add("int", "s8");
            Estado66.Add("double", "s9");
            Estado66.Add("bool", "s10");
            Estado66.Add("string", "s11");
            Estado66.Add("Type", "36");
            Estado66.Add("Formals", "78");
            Estados.Add(66, Estado66);

            Dictionary<string, string> Estado67 = new Dictionary<string, string>();
            Estado67.Add("ident", "r20");
            Estado67.Add("const", "r20");
            Estado67.Add("class", "r20");
            Estado67.Add("}", "r20");
            Estado67.Add("interface", "r20");
            Estado67.Add("int", "r20");
            Estado67.Add("double", "r20");
            Estado67.Add("bool", "r20");
            Estado67.Add("string", "r20");
            Estado67.Add("void", "r20");
            Estado67.Add("$/#", "r20");
            Estados.Add(67, Estado67);

            Dictionary<string, string> Estado68 = new Dictionary<string, string>();
            Estado68.Add("ident", "r42");
            Estado68.Add(";", "r42");
            Estado68.Add("const", "s80 ");
            Dictionary<string, string> Estado68_ = new Dictionary<string, string>();
            Estado68_.Add("const", " r42");
            Estado68.Add("class", "r42");
            Estado68.Add("{", "r42");
            Estado68.Add("}", "r42");
            Estado68.Add("interface", "r42");
            Estado68.Add("int", "r42");
            Estado68.Add("double", "r42");
            Estado68.Add("bool", "r42");
            Estado68.Add("string", "r42");
            Estado68.Add("(", "r42");
            Estado68.Add("void", "r42");
            Estado68.Add("if", "r42");
            Estado68.Add("while", "r42");
            Estado68.Add("for", "r42");
            Estado68.Add("break", "r42");
            Estado68.Add("return", "r42");
            Estado68.Add("Console", "r42");
            Estado68.Add("else", "r42");
            Estado68.Add("&&", "r42");
            Estado68.Add("*", "r42");
            Estado68.Add("!", "r42");
            Estado68.Add("New", "r42");
            Estado68.Add("this", "r42");
            Estado68.Add("-", "r42");
            Estado68.Add("intConstant", "r42");
            Estado68.Add("doubleConstant", "r42");
            Estado68.Add("boolConstant", "r42");
            Estado68.Add("stringConstant", "r42");
            Estado68.Add("null", "r42");
            Estado68.Add("$/#", "r42");
            Estado68.Add("ConstDecl'", "79");
            Estados.Add(68, Estado68);
            Estados.Add(680, Estado68_);

            Dictionary<string, string> Estado69 = new Dictionary<string, string>();
            Estado69.Add("ident", "s81");
            Estado69.Add("[", "s17");
            Estados.Add(69, Estado69);

            Dictionary<string, string> Estado70 = new Dictionary<string, string>();
            Estado70.Add(")", "r21");
            Estados.Add(70, Estado70);

            Dictionary<string, string> Estado71 = new Dictionary<string, string>();
            Estado71.Add("ident", "r7");
            Estado71.Add("const", "r7");
            Estado71.Add("class", "r7");
            Estado71.Add("interface", "r7");
            Estado71.Add("int", "r7");
            Estado71.Add("double", "r7");
            Estado71.Add("bool", "r7");
            Estado71.Add("string", "r7");
            Estado71.Add("void", "r7");
            Estado71.Add("$/#", "r7");
            Estados.Add(71, Estado71);

            Dictionary<string, string> Estado72 = new Dictionary<string, string>();
            Estado72.Add("}", "r29");
            Estados.Add(72, Estado72);

            Dictionary<string, string> Estado73 = new Dictionary<string, string>();
            Estado73.Add(";", "s82");
            Estado73.Add("(", "s28");
            Estados.Add(73, Estado73);

            Dictionary<string, string> Estado74 = new Dictionary<string, string>();
            Estado74.Add("ident", "s83");
            Estados.Add(74, Estado74);

            Dictionary<string, string> Estado75 = new Dictionary<string, string>();
            Estado75.Add("{", "r25");
            Estados.Add(75, Estado75);

            Dictionary<string, string> Estado76 = new Dictionary<string, string>();
            Estado76.Add(",", "r27");
            Estados.Add(76, Estado76);

            Dictionary<string, string> Estado77 = new Dictionary<string, string>();
            Estado77.Add(")", "s84");
            Estados.Add(77, Estado77);

            Dictionary<string, string> Estado78 = new Dictionary<string, string>();
            Estado78.Add(")", "s85");
            Estados.Add(78, Estado78);

            Dictionary<string, string> Estado79 = new Dictionary<string, string>();
            Estado79.Add("ident", "s113 ");
            Dictionary<string, string> Estado79_ = new Dictionary<string, string>();
            Estado79_.Add("ident", " r47");
            Estado79.Add(";", "r47");
            Estado79.Add("{", "s56");
            Estado79.Add("}", "r44");
            Estado79.Add("(", "s111 ");
            Estado79_.Add("(", " r47");
            Estado79.Add(")", "r47");
            Estado79.Add(",", "r47");
            Estado79.Add("if", "s89");
            Estado79.Add("while", "s90");
            Estado79.Add("for", "s91");
            Estado79.Add("break", "s92");
            Estado79.Add("return", "s93");
            Estado79.Add("Console", "s94");
            Estado79.Add(".", "r47");
            Estado79.Add("&&", "s97 ");
            Estado79_.Add("&&", " r47");
            Estado79.Add("==", "r47");
            Estado79.Add("<", "r47");
            Estado79.Add("<=", "r47");
            Estado79.Add("+", "r47");
            Estado79.Add("*", "s104 ");
            Estado79_.Add("*", " r47");
            Estado79.Add("%", "r47");
            Estado79.Add("!", "s106 ");
            Estado79_.Add("!", " r47");
            Estado79.Add("New", "s107 ");
            Estado79_.Add("New", " r47");
            Estado79.Add("this", "s110 ");
            Estado79_.Add("this", " r47");
            Estado79.Add("-", "s112 ");
            Estado79_.Add("-", " r47");
            Estado79.Add("intConstant", "s114 ");
            Estado79_.Add("intConstant", " r47");
            Estado79.Add("doubleConstant", "s115 ");
            Estado79_.Add("doubleConstant", " r47");
            Estado79.Add("boolConstant", "s116 ");
            Estado79_.Add("boolConstant", " r47");
            Estado79.Add("stringConstant", "s117 ");
            Estado79_.Add("stringConstant", " r47");
            Estado79.Add("null", "s118 ");
            Estado79_.Add("null", " r47");
            Estado79.Add("StmtBlock", "95");
            Estado79.Add("Stmt'", "86");
            Estado79.Add("Stmt", "87");
            Estado79.Add("Expr'", "88");
            Estado79.Add("Expr", "96");
            Estado79.Add("ExprA", "98");
            Estado79.Add("ExprB", "99");
            Estado79.Add("ExprC", "100");
            Estado79.Add("ExprD", "101");
            Estado79.Add("ExprE", "102");
            Estado79.Add("ExprF", "103");
            Estado79.Add("ExprG", "105");
            Estado79.Add("LValue", "108");
            Estado79.Add("Constant", "109");
            Estados.Add(79, Estado79);
            Estados.Add(790, Estado79_);

            Dictionary<string, string> Estado80 = new Dictionary<string, string>();
            Estado80.Add("int", "s19");
            Estado80.Add("double", "s20");
            Estado80.Add("bool", "s21");
            Estado80.Add("string", "s22");
            Estado80.Add("ConstType", "119");
            Estados.Add(80, Estado80);

            Dictionary<string, string> Estado81 = new Dictionary<string, string>();
            Estado81.Add(";", "s120");
            Estados.Add(81, Estado81);

            Dictionary<string, string> Estado82 = new Dictionary<string, string>();
            Estado82.Add("ident", "r31");
            Estado82.Add("const", "r31");
            Estado82.Add("}", "r31");
            Estado82.Add("int", "r31");
            Estado82.Add("double", "r31");
            Estado82.Add("bool", "r31");
            Estado82.Add("string", "r31");
            Estado82.Add("void", "r31");
            Estados.Add(82, Estado82);

            Dictionary<string, string> Estado83 = new Dictionary<string, string>();
            Estado83.Add(";", "s121");
            Estados.Add(83, Estado83);

            Dictionary<string, string> Estado84 = new Dictionary<string, string>();
            Estado84.Add(";", "s122");
            Estados.Add(84, Estado84);

            Dictionary<string, string> Estado85 = new Dictionary<string, string>();
            Estado85.Add(";", "s123");
            Estados.Add(85, Estado85);

            Dictionary<string, string> Estado86 = new Dictionary<string, string>();
            Estado86.Add("}", "s124");
            Estados.Add(86, Estado86);

            Dictionary<string, string> Estado87 = new Dictionary<string, string>();
            Estado87.Add("ident", "s113 ");
            Dictionary<string, string> Estado87_ = new Dictionary<string, string>();
            Estado87_.Add("ident", " r47");
            Estado87.Add(";", "r47");
            Estado87.Add("{", "s56");
            Estado87.Add("}", "r44");
            Estado87.Add("(", "s111 ");
            Estado87_.Add("(", " r47");
            Estado87.Add(")", "r47");
            Estado87.Add(",", "r47");
            Estado87.Add("if", "s89");
            Estado87.Add("while", "s90");
            Estado87.Add("for", "s91");
            Estado87.Add("break", "s92");
            Estado87.Add("return", "s93");
            Estado87.Add("Console", "s94");
            Estado87.Add(".", "r47");
            Estado87.Add("&&", "s97 ");
            Estado87_.Add("&&", " r47");
            Estado87.Add("==", "r47");
            Estado87.Add("<", "r47");
            Estado87.Add("<=", "r47");
            Estado87.Add("+", "r47");
            Estado87.Add("*", "s104 ");
            Estado87_.Add("*", " r47");
            Estado87.Add("%", "r47");
            Estado87.Add("!", "s106 ");
            Estado87_.Add("!", " r47");
            Estado87.Add("New", "s107 ");
            Estado87_.Add("New", " r47");
            Estado87.Add("this", "s110 ");
            Estado87_.Add("this", " r47");
            Estado87.Add("-", "s112 ");
            Estado87_.Add("-", " r47");
            Estado87.Add("intConstant", "s114 ");
            Estado87_.Add("intConstant", " r47");
            Estado87.Add("doubleConstant", "s115 ");
            Estado87_.Add("doubleConstant", " r47");
            Estado87.Add("boolConstant", "s116 ");
            Estado87_.Add("boolConstant", " r47");
            Estado87.Add("stringConstant", "s117 ");
            Estado87_.Add("stringConstant", " r47");
            Estado87.Add("null", "s118 ");
            Estado87_.Add("null", " r47");
            Estado87.Add("StmtBlock", "95");
            Estado87.Add("Stmt'", "125");
            Estado87.Add("Stmt", "87");
            Estado87.Add("Expr'", "88");
            Estado87.Add("Expr", "96");
            Estado87.Add("ExprA", "98");
            Estado87.Add("ExprB", "99");
            Estado87.Add("ExprC", "100");
            Estado87.Add("ExprD", "101");
            Estado87.Add("ExprE", "102");
            Estado87.Add("ExprF", "103");
            Estado87.Add("ExprG", "105");
            Estado87.Add("LValue", "108");
            Estado87.Add("Constant", "109");
            Estados.Add(87, Estado87);
            Estados.Add(870, Estado87_);

            Dictionary<string, string> Estado88 = new Dictionary<string, string>();
            Estado88.Add(";", "s126");
            Estados.Add(88, Estado88);

            Dictionary<string, string> Estado89 = new Dictionary<string, string>();
            Estado89.Add("(", "s127");
            Estados.Add(89, Estado89);

            Dictionary<string, string> Estado90 = new Dictionary<string, string>();
            Estado90.Add("(", "s128");
            Estados.Add(90, Estado90);

            Dictionary<string, string> Estado91 = new Dictionary<string, string>();
            Estado91.Add("(", "s129");
            Estados.Add(91, Estado91);

            Dictionary<string, string> Estado92 = new Dictionary<string, string>();
            Estado92.Add(";", "s130");
            Estados.Add(92, Estado92);

            Dictionary<string, string> Estado93 = new Dictionary<string, string>();
            Estado93.Add("ident", "s113");
            Estado93.Add("(", "s111");
            Estado93.Add("*", "s104");
            Estado93.Add("!", "s106");
            Estado93.Add("New", "s107");
            Estado93.Add("this", "s110");
            Estado93.Add("-", "s112");
            Estado93.Add("intConstant", "s114");
            Estado93.Add("doubleConstant", "s115");
            Estado93.Add("boolConstant", "s116");
            Estado93.Add("stringConstant", "s117");
            Estado93.Add("null", "s118");
            Estado93.Add("Expr", "131");
            Estado93.Add("ExprA", "98");
            Estado93.Add("ExprB", "99");
            Estado93.Add("ExprC", "100");
            Estado93.Add("ExprD", "101");
            Estado93.Add("ExprE", "102");
            Estado93.Add("ExprF", "103");
            Estado93.Add("ExprG", "105");
            Estado93.Add("LValue", "108");
            Estado93.Add("Constant", "109");
            Estados.Add(93, Estado93);

            Dictionary<string, string> Estado94 = new Dictionary<string, string>();
            Estado94.Add(".", "s132");
            Estados.Add(94, Estado94);

            Dictionary<string, string> Estado95 = new Dictionary<string, string>();
            Estado95.Add("ident", "r54");
            Estado95.Add(";", "r54");
            Estado95.Add("{", "r54");
            Estado95.Add("}", "r54");
            Estado95.Add("(", "r54");
            Estado95.Add("if", "r54");
            Estado95.Add("while", "r54");
            Estado95.Add("for", "r54");
            Estado95.Add("break", "r54");
            Estado95.Add("return", "r54");
            Estado95.Add("Console", "r54");
            Estado95.Add("else", "r54");
            Estado95.Add("&&", "r54");
            Estado95.Add("*", "r54");
            Estado95.Add("!", "r54");
            Estado95.Add("New", "r54");
            Estado95.Add("this", "r54");
            Estado95.Add("-", "r54");
            Estado95.Add("intConstant", "r54");
            Estado95.Add("doubleConstant", "r54");
            Estado95.Add("boolConstant", "r54");
            Estado95.Add("stringConstant", "r54");
            Estado95.Add("null", "r54");
            Estados.Add(95, Estado95);

            Dictionary<string, string> Estado96 = new Dictionary<string, string>();
            Estado96.Add("ident", "r46");
            Estado96.Add(";", "r46");
            Estado96.Add("(", "r46");
            Estado96.Add(")", "r46");
            Estado96.Add(",", "r46");
            Estado96.Add(".", "s133 ");
            Dictionary<string, string> Estado96_ = new Dictionary<string, string>();
            Estado96_.Add(".", " r46");
            Estado96.Add("&&", "r46");
            Estado96.Add("==", "r46");
            Estado96.Add("<", "r46");
            Estado96.Add("<=", "r46");
            Estado96.Add("+", "r46");
            Estado96.Add("*", "r46");
            Estado96.Add("%", "r46");
            Estado96.Add("!", "r46");
            Estado96.Add("New", "r46");
            Estado96.Add("this", "r46");
            Estado96.Add("-", "r46");
            Estado96.Add("intConstant", "r46");
            Estado96.Add("doubleConstant", "r46");
            Estado96.Add("boolConstant", "r46");
            Estado96.Add("stringConstant", "r46");
            Estado96.Add("null", "r46");
            Estados.Add(96, Estado96);
            Estados.Add(960, Estado96_);

            Dictionary<string, string> Estado97 = new Dictionary<string, string>();
            Estado97.Add("ident", "s113");
            Estado97.Add("(", "s111");
            Estado97.Add("*", "s104");
            Estado97.Add("!", "s106");
            Estado97.Add("New", "s107");
            Estado97.Add("this", "s110");
            Estado97.Add("-", "s112");
            Estado97.Add("intConstant", "s114");
            Estado97.Add("doubleConstant", "s115");
            Estado97.Add("boolConstant", "s116");
            Estado97.Add("stringConstant", "s117");
            Estado97.Add("null", "s118");
            Estado97.Add("Expr", "135");
            Estado97.Add("ExprA", "134");
            Estado97.Add("ExprB", "99");
            Estado97.Add("ExprC", "100");
            Estado97.Add("ExprD", "101");
            Estado97.Add("ExprE", "102");
            Estado97.Add("ExprF", "103");
            Estado97.Add("ExprG", "105");
            Estado97.Add("LValue", "108");
            Estado97.Add("Constant", "109");
            Estados.Add(97, Estado97);

            Dictionary<string, string> Estado98 = new Dictionary<string, string>();
            Estado98.Add("ident", "s113 ");
            Dictionary<string, string> Estado98_ = new Dictionary<string, string>();
            Estado98_.Add("ident", " r47");
            Estado98.Add(";", "r47");
            Estado98.Add("(", "s111 ");
            Estado98_.Add("(", " r47");
            Estado98.Add(")", "r47");
            Estado98.Add(",", "r47");
            Estado98.Add(".", "r47");
            Estado98.Add("&&", "s97 ");
            Estado98_.Add("&&", " r47");
            Estado98.Add("==", "r47");
            Estado98.Add("<", "r47");
            Estado98.Add("<=", "r47");
            Estado98.Add("+", "r47");
            Estado98.Add("*", "s104 ");
            Estado98_.Add("*", " r47");
            Estado98.Add("%", "r47");
            Estado98.Add("!", "s106 ");
            Estado98_.Add("!", " r47");
            Estado98.Add("New", "s107 ");
            Estado98_.Add("New", " r47");
            Estado98.Add("this", "s110 ");
            Estado98_.Add("this", " r47");
            Estado98.Add("-", "s112 ");
            Estado98_.Add("-", " r47");
            Estado98.Add("intConstant", "s114 ");
            Estado98_.Add("intConstant", " r47");
            Estado98.Add("doubleConstant", "s115 ");
            Estado98_.Add("doubleConstant", " r47");
            Estado98.Add("boolConstant", "s116 ");
            Estado98_.Add("boolConstant", " r47");
            Estado98.Add("stringConstant", "s117 ");
            Estado98_.Add("stringConstant", " r47");
            Estado98.Add("null", "s118 ");
            Estado98_.Add("null", " r47");
            Estado98.Add("Expr'", "136");
            Estado98.Add("Expr", "96");
            Estado98.Add("ExprA", "98");
            Estado98.Add("ExprB", "99");
            Estado98.Add("ExprC", "100");
            Estado98.Add("ExprD", "101");
            Estado98.Add("ExprE", "102");
            Estado98.Add("ExprF", "103");
            Estado98.Add("ExprG", "105");
            Estado98.Add("LValue", "108");
            Estado98.Add("Constant", "109");
            Estados.Add(98, Estado98);
            Estados.Add(980, Estado98_);

            Dictionary<string, string> Estado99 = new Dictionary<string, string>();
            Estado99.Add("ident", "r65");
            Estado99.Add(";", "r65");
            Estado99.Add("(", "r65");
            Estado99.Add(")", "r65");
            Estado99.Add(",", "r65");
            Estado99.Add(".", "r65");
            Estado99.Add("&&", "r65");
            Estado99.Add("==", "s138 ");
            Dictionary<string, string> Estado99_ = new Dictionary<string, string>();
            Estado99_.Add("==", " r65");
            Estado99.Add("<", "r65");
            Estado99.Add("<=", "r65");
            Estado99.Add("+", "r65");
            Estado99.Add("*", "r65");
            Estado99.Add("%", "r65");
            Estado99.Add("!", "r65");
            Estado99.Add("New", "r65");
            Estado99.Add("this", "r65");
            Estado99.Add("-", "r65");
            Estado99.Add("intConstant", "r65");
            Estado99.Add("doubleConstant", "r65");
            Estado99.Add("boolConstant", "r65");
            Estado99.Add("stringConstant", "r65");
            Estado99.Add("null", "r65");
            Estado99.Add("ExprA'", "137");
            Estados.Add(99, Estado99);
            Estados.Add(990, Estado99_);

            Dictionary<string, string> Estado100 = new Dictionary<string, string>();
            Estado100.Add("ident", "r68");
            Estado100.Add(";", "r68");
            Estado100.Add("(", "r68");
            Estado100.Add(")", "r68");
            Estado100.Add(",", "r68");
            Estado100.Add(".", "r68");
            Estado100.Add("&&", "r68");
            Estado100.Add("==", "r68");
            Estado100.Add("<", "s140 ");
            Dictionary<string, string> Estado100_ = new Dictionary<string, string>();
            Estado100_.Add("<", " r68");
            Estado100.Add("<=", "r68");
            Estado100.Add("+", "r68");
            Estado100.Add("*", "r68");
            Estado100.Add("%", "r68");
            Estado100.Add("!", "r68");
            Estado100.Add("New", "r68");
            Estado100.Add("this", "r68");
            Estado100.Add("-", "r68");
            Estado100.Add("intConstant", "r68");
            Estado100.Add("doubleConstant", "r68");
            Estado100.Add("boolConstant", "r68");
            Estado100.Add("stringConstant", "r68");
            Estado100.Add("null", "r68");
            Estado100.Add("ExprB'", "139");
            Estados.Add(100, Estado100);
            Estados.Add(1000, Estado100_);

            Dictionary<string, string> Estado101 = new Dictionary<string, string>();
            Estado101.Add("ident", "r71");
            Estado101.Add(";", "r71");
            Estado101.Add("(", "r71");
            Estado101.Add(")", "r71");
            Estado101.Add(",", "r71");
            Estado101.Add(".", "r71");
            Estado101.Add("&&", "r71");
            Estado101.Add("==", "r71");
            Estado101.Add("<", "r71");
            Estado101.Add("<=", "s142 ");
            Dictionary<string, string> Estado101_ = new Dictionary<string, string>();
            Estado101_.Add("<=", " r71");
            Estado101.Add("+", "r71");
            Estado101.Add("*", "r71");
            Estado101.Add("%", "r71");
            Estado101.Add("!", "r71");
            Estado101.Add("New", "r71");
            Estado101.Add("this", "r71");
            Estado101.Add("-", "r71");
            Estado101.Add("intConstant", "r71");
            Estado101.Add("doubleConstant", "r71");
            Estado101.Add("boolConstant", "r71");
            Estado101.Add("stringConstant", "r71");
            Estado101.Add("null", "r71");
            Estado101.Add("ExprC'", "141");
            Estados.Add(101, Estado101);
            Estados.Add(1010, Estado101_);

            Dictionary<string, string> Estado102 = new Dictionary<string, string>();
            Estado102.Add("ident", "r74");
            Estado102.Add(";", "r74");
            Estado102.Add("(", "r74");
            Estado102.Add(")", "r74");
            Estado102.Add(",", "r74");
            Estado102.Add(".", "r74");
            Estado102.Add("&&", "r74");
            Estado102.Add("==", "r74");
            Estado102.Add("<", "r74");
            Estado102.Add("<=", "r74");
            Estado102.Add("+", "s144 ");
            Dictionary<string, string> Estado102_ = new Dictionary<string, string>();
            Estado102_.Add("+", " r74");
            Estado102.Add("*", "r74");
            Estado102.Add("%", "r74");
            Estado102.Add("!", "r74");
            Estado102.Add("New", "r74");
            Estado102.Add("this", "r74");
            Estado102.Add("-", "r74");
            Estado102.Add("intConstant", "r74");
            Estado102.Add("doubleConstant", "r74");
            Estado102.Add("boolConstant", "r74");
            Estado102.Add("stringConstant", "r74");
            Estado102.Add("null", "r74");
            Estado102.Add("ExprD'", "143");
            Estados.Add(102, Estado102);
            Estados.Add(1020, Estado102_);

            Dictionary<string, string> Estado103 = new Dictionary<string, string>();
            Estado103.Add("ident", "r77");
            Estado103.Add(";", "r77");
            Estado103.Add("(", "r77");
            Estado103.Add(")", "r77");
            Estado103.Add(",", "r77");
            Estado103.Add(".", "r77");
            Estado103.Add("&&", "r77");
            Estado103.Add("==", "r77");
            Estado103.Add("<", "r77");
            Estado103.Add("<=", "r77");
            Estado103.Add("+", "r77");
            Estado103.Add("*", "r77");
            Estado103.Add("%", "r77");
            Estado103.Add("!", "r77");
            Estado103.Add("New", "r77");
            Estado103.Add("this", "r77");
            Estado103.Add("-", "r77");
            Estado103.Add("intConstant", "r77");
            Estado103.Add("doubleConstant", "r77");
            Estado103.Add("boolConstant", "r77");
            Estado103.Add("stringConstant", "r77");
            Estado103.Add("null", "r77");
            Estado103.Add("ExprE'", "145");
            Estados.Add(103, Estado103);

            Dictionary<string, string> Estado104 = new Dictionary<string, string>();
            Estado104.Add("ident", "s113");
            Estado104.Add("(", "s111");
            Estado104.Add("*", "s104");
            Estado104.Add("!", "s106");
            Estado104.Add("New", "s107");
            Estado104.Add("this", "s110");
            Estado104.Add("-", "s112");
            Estado104.Add("intConstant", "s114");
            Estado104.Add("doubleConstant", "s115");
            Estado104.Add("boolConstant", "s116");
            Estado104.Add("stringConstant", "s117");
            Estado104.Add("null", "s118");
            Estado104.Add("Expr", "135");
            Estado104.Add("ExprA", "98");
            Estado104.Add("ExprB", "99");
            Estado104.Add("ExprC", "100");
            Estado104.Add("ExprD", "101");
            Estado104.Add("ExprE", "102");
            Estado104.Add("ExprF", "146");
            Estado104.Add("ExprG", "105");
            Estado104.Add("LValue", "108");
            Estado104.Add("Constant", "109");
            Estados.Add(104, Estado104);

            Dictionary<string, string> Estado105 = new Dictionary<string, string>();
            Estado105.Add("ident", "r80");
            Estado105.Add(";", "r80");
            Estado105.Add("(", "r80");
            Estado105.Add(")", "r80");
            Estado105.Add(",", "r80");
            Estado105.Add(".", "r80");
            Estado105.Add("&&", "r80");
            Estado105.Add("==", "r80");
            Estado105.Add("<", "r80");
            Estado105.Add("<=", "r80");
            Estado105.Add("+", "r80");
            Estado105.Add("*", "r80");
            Estado105.Add("%", "s148 ");
            Dictionary<string, string> Estado105_ = new Dictionary<string, string>();
            Estado105_.Add("%", " r80");
            Estado105.Add("!", "r80");
            Estado105.Add("New", "r80");
            Estado105.Add("this", "r80");
            Estado105.Add("-", "r80");
            Estado105.Add("intConstant", "r80");
            Estado105.Add("doubleConstant", "r80");
            Estado105.Add("boolConstant", "r80");
            Estado105.Add("stringConstant", "r80");
            Estado105.Add("null", "r80");
            Estado105.Add("ExprF'", "147");
            Estados.Add(105, Estado105);
            Estados.Add(1050, Estado105_);

            Dictionary<string, string> Estado106 = new Dictionary<string, string>();
            Estado106.Add("ident", "s113");
            Estado106.Add("(", "s111");
            Estado106.Add("*", "s104");
            Estado106.Add("!", "s106");
            Estado106.Add("New", "s107");
            Estado106.Add("this", "s110");
            Estado106.Add("-", "s112");
            Estado106.Add("intConstant", "s114");
            Estado106.Add("doubleConstant", "s115");
            Estado106.Add("boolConstant", "s116");
            Estado106.Add("stringConstant", "s117");
            Estado106.Add("null", "s118");
            Estado106.Add("Expr", "149");
            Estado106.Add("ExprA", "98");
            Estado106.Add("ExprB", "99");
            Estado106.Add("ExprC", "100");
            Estado106.Add("ExprD", "101");
            Estado106.Add("ExprE", "102");
            Estado106.Add("ExprF", "103");
            Estado106.Add("ExprG", "105");
            Estado106.Add("LValue", "108");
            Estado106.Add("Constant", "109");
            Estados.Add(106, Estado106);

            Dictionary<string, string> Estado107 = new Dictionary<string, string>();
            Estado107.Add("(", "s150");
            Estados.Add(107, Estado107);

            Dictionary<string, string> Estado108 = new Dictionary<string, string>();
            Estado108.Add("ident", "r85");
            Estado108.Add(";", "r85");
            Estado108.Add("(", "r85");
            Estado108.Add(")", "r85");
            Estado108.Add(",", "r85");
            Estado108.Add(".", "r85");
            Estado108.Add("&&", "r85");
            Estado108.Add("==", "r85");
            Estado108.Add("<", "r85");
            Estado108.Add("<=", "r85");
            Estado108.Add("+", "r85");
            Estado108.Add("*", "r85");
            Estado108.Add("%", "r85");
            Estado108.Add("!", "r85");
            Estado108.Add("New", "r85");
            Estado108.Add("=", "s151");
            Estado108.Add("this", "r85");
            Estado108.Add("-", "r85");
            Estado108.Add("intConstant", "r85");
            Estado108.Add("doubleConstant", "r85");
            Estado108.Add("boolConstant", "r85");
            Estado108.Add("stringConstant", "r85");
            Estado108.Add("null", "r85");
            Estados.Add(108, Estado108);

            Dictionary<string, string> Estado109 = new Dictionary<string, string>();
            Estado109.Add("ident", "r84");
            Estado109.Add(";", "r84");
            Estado109.Add("(", "r84");
            Estado109.Add(")", "r84");
            Estado109.Add(",", "r84");
            Estado109.Add(".", "r84");
            Estado109.Add("&&", "r84");
            Estado109.Add("==", "r84");
            Estado109.Add("<", "r84");
            Estado109.Add("<=", "r84");
            Estado109.Add("+", "r84");
            Estado109.Add("*", "r84");
            Estado109.Add("%", "r84");
            Estado109.Add("!", "r84");
            Estado109.Add("New", "r84");
            Estado109.Add("this", "r84");
            Estado109.Add("-", "r84");
            Estado109.Add("intConstant", "r84");
            Estado109.Add("doubleConstant", "r84");
            Estado109.Add("boolConstant", "r84");
            Estado109.Add("stringConstant", "r84");
            Estado109.Add("null", "r84");
            Estados.Add(109, Estado109);

            Dictionary<string, string> Estado110 = new Dictionary<string, string>();
            Estado110.Add("ident", "r86");
            Estado110.Add(";", "r86");
            Estado110.Add("(", "r86");
            Estado110.Add(")", "r86");
            Estado110.Add(",", "r86");
            Estado110.Add(".", "r86");
            Estado110.Add("&&", "r86");
            Estado110.Add("==", "r86");
            Estado110.Add("<", "r86");
            Estado110.Add("<=", "r86");
            Estado110.Add("+", "r86");
            Estado110.Add("*", "r86");
            Estado110.Add("%", "r86");
            Estado110.Add("!", "r86");
            Estado110.Add("New", "r86");
            Estado110.Add("this", "r86");
            Estado110.Add("-", "r86");
            Estado110.Add("intConstant", "r86");
            Estado110.Add("doubleConstant", "r86");
            Estado110.Add("boolConstant", "r86");
            Estado110.Add("stringConstant", "r86");
            Estado110.Add("null", "r86");
            Estados.Add(110, Estado110);

            Dictionary<string, string> Estado111 = new Dictionary<string, string>();
            Estado111.Add("ident", "s113");
            Estado111.Add("(", "s111");
            Estado111.Add("*", "s104");
            Estado111.Add("!", "s106");
            Estado111.Add("New", "s107");
            Estado111.Add("this", "s110");
            Estado111.Add("-", "s112");
            Estado111.Add("intConstant", "s114");
            Estado111.Add("doubleConstant", "s115");
            Estado111.Add("boolConstant", "s116");
            Estado111.Add("stringConstant", "s117");
            Estado111.Add("null", "s118");
            Estado111.Add("Expr", "152");
            Estado111.Add("ExprA", "98");
            Estado111.Add("ExprB", "99");
            Estado111.Add("ExprC", "100");
            Estado111.Add("ExprD", "101");
            Estado111.Add("ExprE", "102");
            Estado111.Add("ExprF", "103");
            Estado111.Add("ExprG", "105");
            Estado111.Add("LValue", "108");
            Estado111.Add("Constant", "109");
            Estados.Add(111, Estado111);

            Dictionary<string, string> Estado112 = new Dictionary<string, string>();
            Estado112.Add("ident", "s113");
            Estado112.Add("(", "s111");
            Estado112.Add("*", "s104");
            Estado112.Add("!", "s106");
            Estado112.Add("New", "s107");
            Estado112.Add("this", "s110");
            Estado112.Add("-", "s112");
            Estado112.Add("intConstant", "s114");
            Estado112.Add("doubleConstant", "s115");
            Estado112.Add("boolConstant", "s116");
            Estado112.Add("stringConstant", "s117");
            Estado112.Add("null", "s118");
            Estado112.Add("Expr", "153");
            Estado112.Add("ExprA", "98");
            Estado112.Add("ExprB", "99");
            Estado112.Add("ExprC", "100");
            Estado112.Add("ExprD", "101");
            Estado112.Add("ExprE", "102");
            Estado112.Add("ExprF", "103");
            Estado112.Add("ExprG", "105");
            Estado112.Add("LValue", "108");
            Estado112.Add("Constant", "109");
            Estados.Add(112, Estado112);

            Dictionary<string, string> Estado113 = new Dictionary<string, string>();
            Estado113.Add("ident", "r89");
            Estado113.Add(";", "r89");
            Estado113.Add("(", "r89");
            Estado113.Add(")", "r89");
            Estado113.Add(",", "r89");
            Estado113.Add(".", "r89");
            Estado113.Add("&&", "r89");
            Estado113.Add("==", "r89");
            Estado113.Add("<", "r89");
            Estado113.Add("<=", "r89");
            Estado113.Add("+", "r89");
            Estado113.Add("*", "r89");
            Estado113.Add("%", "r89");
            Estado113.Add("!", "r89");
            Estado113.Add("New", "r89");
            Estado113.Add("=", "r89");
            Estado113.Add("this", "r89");
            Estado113.Add("-", "r89");
            Estado113.Add("intConstant", "r89");
            Estado113.Add("doubleConstant", "r89");
            Estado113.Add("boolConstant", "r89");
            Estado113.Add("stringConstant", "r89");
            Estado113.Add("null", "r89");
            Estados.Add(113, Estado113);

            Dictionary<string, string> Estado114 = new Dictionary<string, string>();
            Estado114.Add("ident", "r91");
            Estado114.Add(";", "r91");
            Estado114.Add("(", "r91");
            Estado114.Add(")", "r91");
            Estado114.Add(",", "r91");
            Estado114.Add(".", "r91");
            Estado114.Add("&&", "r91");
            Estado114.Add("==", "r91");
            Estado114.Add("<", "r91");
            Estado114.Add("<=", "r91");
            Estado114.Add("+", "r91");
            Estado114.Add("*", "r91");
            Estado114.Add("%", "r91");
            Estado114.Add("!", "r91");
            Estado114.Add("New", "r91");
            Estado114.Add("this", "r91");
            Estado114.Add("-", "r91");
            Estado114.Add("intConstant", "r91");
            Estado114.Add("doubleConstant", "r91");
            Estado114.Add("boolConstant", "r91");
            Estado114.Add("stringConstant", "r91");
            Estado114.Add("null", "r91");
            Estados.Add(114, Estado114);

            Dictionary<string, string> Estado115 = new Dictionary<string, string>();
            Estado115.Add("ident", "r92");
            Estado115.Add(";", "r92");
            Estado115.Add("(", "r92");
            Estado115.Add(")", "r92");
            Estado115.Add(",", "r92");
            Estado115.Add(".", "r92");
            Estado115.Add("&&", "r92");
            Estado115.Add("==", "r92");
            Estado115.Add("<", "r92");
            Estado115.Add("<=", "r92");
            Estado115.Add("+", "r92");
            Estado115.Add("*", "r92");
            Estado115.Add("%", "r92");
            Estado115.Add("!", "r92");
            Estado115.Add("New", "r92");
            Estado115.Add("this", "r92");
            Estado115.Add("-", "r92");
            Estado115.Add("intConstant", "r92");
            Estado115.Add("doubleConstant", "r92");
            Estado115.Add("boolConstant", "r92");
            Estado115.Add("stringConstant", "r92");
            Estado115.Add("null", "r92");
            Estados.Add(115, Estado115);

            Dictionary<string, string> Estado116 = new Dictionary<string, string>();
            Estado116.Add("ident", "r93");
            Estado116.Add(";", "r93");
            Estado116.Add("(", "r93");
            Estado116.Add(")", "r93");
            Estado116.Add(",", "r93");
            Estado116.Add(".", "r93");
            Estado116.Add("&&", "r93");
            Estado116.Add("==", "r93");
            Estado116.Add("<", "r93");
            Estado116.Add("<=", "r93");
            Estado116.Add("+", "r93");
            Estado116.Add("*", "r93");
            Estado116.Add("%", "r93");
            Estado116.Add("!", "r93");
            Estado116.Add("New", "r93");
            Estado116.Add("this", "r93");
            Estado116.Add("-", "r93");
            Estado116.Add("intConstant", "r93");
            Estado116.Add("doubleConstant", "r93");
            Estado116.Add("boolConstant", "r93");
            Estado116.Add("stringConstant", "r93");
            Estado116.Add("null", "r93");
            Estados.Add(116, Estado116);

            Dictionary<string, string> Estado117 = new Dictionary<string, string>();
            Estado117.Add("ident", "r94");
            Estado117.Add(";", "r94");
            Estado117.Add("(", "r94");
            Estado117.Add(")", "r94");
            Estado117.Add(",", "r94");
            Estado117.Add(".", "r94");
            Estado117.Add("&&", "r94");
            Estado117.Add("==", "r94");
            Estado117.Add("<", "r94");
            Estado117.Add("<=", "r94");
            Estado117.Add("+", "r94");
            Estado117.Add("*", "r94");
            Estado117.Add("%", "r94");
            Estado117.Add("!", "r94");
            Estado117.Add("New", "r94");
            Estado117.Add("this", "r94");
            Estado117.Add("-", "r94");
            Estado117.Add("intConstant", "r94");
            Estado117.Add("doubleConstant", "r94");
            Estado117.Add("boolConstant", "r94");
            Estado117.Add("stringConstant", "r94");
            Estado117.Add("null", "r94");
            Estados.Add(117, Estado117);

            Dictionary<string, string> Estado118 = new Dictionary<string, string>();
            Estado118.Add("ident", "r95");
            Estado118.Add(";", "r95");
            Estado118.Add("(", "r95");
            Estado118.Add(")", "r95");
            Estado118.Add(",", "r95");
            Estado118.Add(".", "r95");
            Estado118.Add("&&", "r95");
            Estado118.Add("==", "r95");
            Estado118.Add("<", "r95");
            Estado118.Add("<=", "r95");
            Estado118.Add("+", "r95");
            Estado118.Add("*", "r95");
            Estado118.Add("%", "r95");
            Estado118.Add("!", "r95");
            Estado118.Add("New", "r95");
            Estado118.Add("this", "r95");
            Estado118.Add("-", "r95");
            Estado118.Add("intConstant", "r95");
            Estado118.Add("doubleConstant", "r95");
            Estado118.Add("boolConstant", "r95");
            Estado118.Add("stringConstant", "r95");
            Estado118.Add("null", "r95");
            Estados.Add(118, Estado118);

            Dictionary<string, string> Estado119 = new Dictionary<string, string>();
            Estado119.Add("ident", "s154");
            Estados.Add(119, Estado119);

            Dictionary<string, string> Estado120 = new Dictionary<string, string>();
            Estado120.Add("", "s155");
            Estados.Add(120, Estado120);

            Dictionary<string, string> Estado121 = new Dictionary<string, string>();
            Estado121.Add("ident", "r33");
            Estado121.Add("const", "r33");
            Estado121.Add("}", "r33");
            Estado121.Add("int", "r33");
            Estado121.Add("double", "r33");
            Estado121.Add("bool", "r33");
            Estado121.Add("string", "r33");
            Estado121.Add("void", "r33");
            Estados.Add(121, Estado121);

            Dictionary<string, string> Estado122 = new Dictionary<string, string>();
            Estado122.Add("ident", "r36");
            Estado122.Add("}", "r36");
            Estado122.Add("int", "r36");
            Estado122.Add("double", "r36");
            Estado122.Add("bool", "r36");
            Estado122.Add("string", "r36");
            Estado122.Add("void", "r36");
            Estados.Add(122, Estado122);

            Dictionary<string, string> Estado123 = new Dictionary<string, string>();
            Estado123.Add("ident", "r37");
            Estado123.Add("}", "r37");
            Estado123.Add("int", "r37");
            Estado123.Add("double", "r37");
            Estado123.Add("bool", "r37");
            Estado123.Add("string", "r37");
            Estado123.Add("void", "r37");
            Estados.Add(123, Estado123);

            Dictionary<string, string> Estado124 = new Dictionary<string, string>();
            Estado124.Add("ident", "r38");
            Estado124.Add(";", "r38");
            Estado124.Add("const", "r38");
            Estado124.Add("class", "r38");
            Estado124.Add("{", "r38");
            Estado124.Add("}", "r38");
            Estado124.Add("interface", "r38");
            Estado124.Add("int", "r38");
            Estado124.Add("double", "r38");
            Estado124.Add("bool", "r38");
            Estado124.Add("string", "r38");
            Estado124.Add("(", "r38");
            Estado124.Add("void", "r38");
            Estado124.Add("if", "r38");
            Estado124.Add("while", "r38");
            Estado124.Add("for", "r38");
            Estado124.Add("break", "r38");
            Estado124.Add("return", "r38");
            Estado124.Add("Console", "r38");
            Estado124.Add("else", "r38");
            Estado124.Add("&&", "r38");
            Estado124.Add("*", "r38");
            Estado124.Add("!", "r38");
            Estado124.Add("New", "r38");
            Estado124.Add("this", "r38");
            Estado124.Add("-", "r38");
            Estado124.Add("intConstant", "r38");
            Estado124.Add("doubleConstant", "r38");
            Estado124.Add("boolConstant", "r38");
            Estado124.Add("stringConstant", "r38");
            Estado124.Add("null", "r38");
            Estado124.Add("$/#", "r38");
            Estados.Add(124, Estado124);

            Dictionary<string, string> Estado125 = new Dictionary<string, string>();
            Estado125.Add("}", "r43");
            Estados.Add(125, Estado125);

            Dictionary<string, string> Estado126 = new Dictionary<string, string>();
            Estado126.Add("ident", "r45");
            Estado126.Add(";", "r45");
            Estado126.Add("{", "r45");
            Estado126.Add("}", "r45");
            Estado126.Add("(", "r45");
            Estado126.Add("if", "r45");
            Estado126.Add("while", "r45");
            Estado126.Add("for", "r45");
            Estado126.Add("break", "r45");
            Estado126.Add("return", "r45");
            Estado126.Add("Console", "r45");
            Estado126.Add("else", "r45");
            Estado126.Add("&&", "r45");
            Estado126.Add("*", "r45");
            Estado126.Add("!", "r45");
            Estado126.Add("New", "r45");
            Estado126.Add("this", "r45");
            Estado126.Add("-", "r45");
            Estado126.Add("intConstant", "r45");
            Estado126.Add("doubleConstant", "r45");
            Estado126.Add("boolConstant", "r45");
            Estado126.Add("stringConstant", "r45");
            Estado126.Add("null", "r45");
            Estados.Add(126, Estado126);

            Dictionary<string, string> Estado127 = new Dictionary<string, string>();
            Estado127.Add("ident", "s113");
            Estado127.Add("(", "s111");
            Estado127.Add("*", "s104");
            Estado127.Add("!", "s106");
            Estado127.Add("New", "s107");
            Estado127.Add("this", "s110");
            Estado127.Add("-", "s112");
            Estado127.Add("intConstant", "s114");
            Estado127.Add("doubleConstant", "s115");
            Estado127.Add("boolConstant", "s116");
            Estado127.Add("stringConstant", "s117");
            Estado127.Add("null", "s118");
            Estado127.Add("Expr", "156");
            Estado127.Add("ExprA", "98");
            Estado127.Add("ExprB", "99");
            Estado127.Add("ExprC", "100");
            Estado127.Add("ExprD", "101");
            Estado127.Add("ExprE", "102");
            Estado127.Add("ExprF", "103");
            Estado127.Add("ExprG", "105");
            Estado127.Add("LValue", "108");
            Estado127.Add("Constant", "109");
            Estados.Add(127, Estado127);

            Dictionary<string, string> Estado128 = new Dictionary<string, string>();
            Estado128.Add("ident", "s113");
            Estado128.Add("(", "s111");
            Estado128.Add("*", "s104");
            Estado128.Add("!", "s106");
            Estado128.Add("New", "s107");
            Estado128.Add("this", "s110");
            Estado128.Add("-", "s112");
            Estado128.Add("intConstant", "s114");
            Estado128.Add("doubleConstant", "s115");
            Estado128.Add("boolConstant", "s116");
            Estado128.Add("stringConstant", "s117");
            Estado128.Add("null", "s118");
            Estado128.Add("Expr", "157");
            Estado128.Add("ExprA", "98");
            Estado128.Add("ExprB", "99");
            Estado128.Add("ExprC", "100");
            Estado128.Add("ExprD", "101");
            Estado128.Add("ExprE", "102");
            Estado128.Add("ExprF", "103");
            Estado128.Add("ExprG", "105");
            Estado128.Add("LValue", "108");
            Estado128.Add("Constant", "109");
            Estados.Add(128, Estado128);

            Dictionary<string, string> Estado129 = new Dictionary<string, string>();
            Estado129.Add("ident", "s113");
            Estado129.Add("(", "s111");
            Estado129.Add("*", "s104");
            Estado129.Add("!", "s106");
            Estado129.Add("New", "s107");
            Estado129.Add("this", "s110");
            Estado129.Add("-", "s112");
            Estado129.Add("intConstant", "s114");
            Estado129.Add("doubleConstant", "s115");
            Estado129.Add("boolConstant", "s116");
            Estado129.Add("stringConstant", "s117");
            Estado129.Add("null", "s118");
            Estado129.Add("Expr", "158");
            Estado129.Add("ExprA", "98");
            Estado129.Add("ExprB", "99");
            Estado129.Add("ExprC", "100");
            Estado129.Add("ExprD", "101");
            Estado129.Add("ExprE", "102");
            Estado129.Add("ExprF", "103");
            Estado129.Add("ExprG", "105");
            Estado129.Add("LValue", "108");
            Estado129.Add("Constant", "109");
            Estados.Add(129, Estado129);

            Dictionary<string, string> Estado130 = new Dictionary<string, string>();
            Estado130.Add("ident", "r51");
            Estado130.Add(";", "r51");
            Estado130.Add("{", "r51");
            Estado130.Add("}", "r51");
            Estado130.Add("(", "r51");
            Estado130.Add("if", "r51");
            Estado130.Add("while", "r51");
            Estado130.Add("for", "r51");
            Estado130.Add("break", "r51");
            Estado130.Add("return", "r51");
            Estado130.Add("Console", "r51");
            Estado130.Add("else", "r51");
            Estado130.Add("&&", "r51");
            Estado130.Add("*", "r51");
            Estado130.Add("!", "r51");
            Estado130.Add("New", "r51");
            Estado130.Add("this", "r51");
            Estado130.Add("-", "r51");
            Estado130.Add("intConstant", "r51");
            Estado130.Add("doubleConstant", "r51");
            Estado130.Add("boolConstant", "r51");
            Estado130.Add("stringConstant", "r51");
            Estado130.Add("null", "r51");
            Estados.Add(130, Estado130);

            Dictionary<string, string> Estado131 = new Dictionary<string, string>();
            Estado131.Add(";", "s159");
            Estado131.Add(".", "s133");
            Estados.Add(131, Estado131);

            Dictionary<string, string> Estado132 = new Dictionary<string, string>();
            Estado132.Add("WriteLine", "s160");
            Estados.Add(132, Estado132);

            Dictionary<string, string> Estado133 = new Dictionary<string, string>();
            Estado133.Add("ident", "s161");
            Estados.Add(133, Estado133);

            Dictionary<string, string> Estado134 = new Dictionary<string, string>();
            Estado134.Add("ident", "s113 ");
            Dictionary<string, string> Estado134_ = new Dictionary<string, string>();
            Estado134_.Add("ident", " r47");
            Estado134.Add(";", "r47");
            Estado134.Add("(", "s111 ");
            Estado134_.Add("(", " r47");
            Estado134.Add(")", "r47");
            Estado134.Add(",", "r47");
            Estado134.Add(".", "r47");
            Estado134.Add("&&", "s97 ");
            Estado134_.Add("&&", " r47");
            Estado134.Add("==", "r47");
            Estado134.Add("<", "r47");
            Estado134.Add("<=", "r47");
            Estado134.Add("+", "r47");
            Estado134.Add("*", "s104 ");
            Estado134_.Add("*", " r47");
            Estado134.Add("%", "r47");
            Estado134.Add("!", "s106 ");
            Estado134_.Add("!", " r47");
            Estado134.Add("New", "s107 ");
            Estado134_.Add("New", " r47");
            Estado134.Add("this", "s110 ");
            Estado134_.Add("this", " r47");
            Estado134.Add("-", "s112 ");
            Estado134_.Add("-", " r47");
            Estado134.Add("intConstant", "s114 ");
            Estado134_.Add("intConstant", " r47");
            Estado134.Add("doubleConstant", "s115 ");
            Estado134_.Add("doubleConstant", " r47");
            Estado134.Add("boolConstant", "s116 ");
            Estado134_.Add("boolConstant", " r47");
            Estado134.Add("stringConstant", "s117 ");
            Estado134_.Add("stringConstant", " r47");
            Estado134.Add("null", "s118 ");
            Estado134_.Add("null", " r47");
            Estado134.Add("Expr'", "162");
            Estado134.Add("Expr", "96");
            Estado134.Add("ExprA", "98");
            Estado134.Add("ExprB", "99");
            Estado134.Add("ExprC", "100");
            Estado134.Add("ExprD", "101");
            Estado134.Add("ExprE", "102");
            Estado134.Add("ExprF", "103");
            Estado134.Add("ExprG", "105");
            Estado134.Add("LValue", "108");
            Estado134.Add("Constant", "109");
            Estados.Add(134, Estado134);
            Estados.Add(1340, Estado134_);

            Dictionary<string, string> Estado135 = new Dictionary<string, string>();
            Estado135.Add(".", "s133");
            Estados.Add(135, Estado135);

            Dictionary<string, string> Estado136 = new Dictionary<string, string>();
            Estado136.Add("ident", "r60");
            Estado136.Add(";", "r60");
            Estado136.Add("(", "r60");
            Estado136.Add(")", "r60");
            Estado136.Add(",", "r60");
            Estado136.Add(".", "r60");
            Estado136.Add("&&", "r60");
            Estado136.Add("==", "r60");
            Estado136.Add("<", "r60");
            Estado136.Add("<=", "r60");
            Estado136.Add("+", "r60");
            Estado136.Add("*", "r60");
            Estado136.Add("%", "r60");
            Estado136.Add("!", "r60");
            Estado136.Add("New", "r60");
            Estado136.Add("this", "r60");
            Estado136.Add("-", "r60");
            Estado136.Add("intConstant", "r60");
            Estado136.Add("doubleConstant", "r60");
            Estado136.Add("boolConstant", "r60");
            Estado136.Add("stringConstant", "r60");
            Estado136.Add("null", "r60");
            Estados.Add(136, Estado136);

            Dictionary<string, string> Estado137 = new Dictionary<string, string>();
            Estado137.Add("ident", "r63");
            Estado137.Add(";", "r63");
            Estado137.Add("(", "r63");
            Estado137.Add(")", "r63");
            Estado137.Add(",", "r63");
            Estado137.Add(".", "r63");
            Estado137.Add("&&", "r63");
            Estado137.Add("==", "r63");
            Estado137.Add("<", "r63");
            Estado137.Add("<=", "r63");
            Estado137.Add("+", "r63");
            Estado137.Add("*", "r63");
            Estado137.Add("%", "r63");
            Estado137.Add("!", "r63");
            Estado137.Add("New", "r63");
            Estado137.Add("this", "r63");
            Estado137.Add("-", "r63");
            Estado137.Add("intConstant", "r63");
            Estado137.Add("doubleConstant", "r63");
            Estado137.Add("boolConstant", "r63");
            Estado137.Add("stringConstant", "r63");
            Estado137.Add("null", "r63");
            Estados.Add(137, Estado137);

            Dictionary<string, string> Estado138 = new Dictionary<string, string>();
            Estado138.Add("ident", "s113");
            Estado138.Add("(", "s111");
            Estado138.Add("*", "s104");
            Estado138.Add("!", "s106");
            Estado138.Add("New", "s107");
            Estado138.Add("this", "s110");
            Estado138.Add("-", "s112");
            Estado138.Add("intConstant", "s114");
            Estado138.Add("doubleConstant", "s115");
            Estado138.Add("boolConstant", "s116");
            Estado138.Add("stringConstant", "s117");
            Estado138.Add("null", "s118");
            Estado138.Add("Expr", "135");
            Estado138.Add("ExprA", "98");
            Estado138.Add("ExprB", "163");
            Estado138.Add("ExprC", "100");
            Estado138.Add("ExprD", "101");
            Estado138.Add("ExprE", "102");
            Estado138.Add("ExprF", "103");
            Estado138.Add("ExprG", "105");
            Estado138.Add("LValue", "108");
            Estado138.Add("Constant", "109");
            Estados.Add(138, Estado138);

            Dictionary<string, string> Estado139 = new Dictionary<string, string>();
            Estado139.Add("ident", "r66");
            Estado139.Add(";", "r66");
            Estado139.Add("(", "r66");
            Estado139.Add(")", "r66");
            Estado139.Add(",", "r66");
            Estado139.Add(".", "r66");
            Estado139.Add("&&", "r66");
            Estado139.Add("==", "r66");
            Estado139.Add("<", "r66");
            Estado139.Add("<=", "r66");
            Estado139.Add("+", "r66");
            Estado139.Add("*", "r66");
            Estado139.Add("%", "r66");
            Estado139.Add("!", "r66");
            Estado139.Add("New", "r66");
            Estado139.Add("this", "r66");
            Estado139.Add("-", "r66");
            Estado139.Add("intConstant", "r66");
            Estado139.Add("doubleConstant", "r66");
            Estado139.Add("boolConstant", "r66");
            Estado139.Add("stringConstant", "r66");
            Estado139.Add("null", "r66");
            Estados.Add(139, Estado139);

            Dictionary<string, string> Estado140 = new Dictionary<string, string>();
            Estado140.Add("ident", "s113");
            Estado140.Add("(", "s111");
            Estado140.Add("*", "s104");
            Estado140.Add("!", "s106");
            Estado140.Add("New", "s107");
            Estado140.Add("this", "s110");
            Estado140.Add("-", "s112");
            Estado140.Add("intConstant", "s114");
            Estado140.Add("doubleConstant", "s115");
            Estado140.Add("boolConstant", "s116");
            Estado140.Add("stringConstant", "s117");
            Estado140.Add("null", "s118");
            Estado140.Add("Expr", "135");
            Estado140.Add("ExprA", "98");
            Estado140.Add("ExprB", "99");
            Estado140.Add("ExprC", "164");
            Estado140.Add("ExprD", "101");
            Estado140.Add("ExprE", "102");
            Estado140.Add("ExprF", "103");
            Estado140.Add("ExprG", "105");
            Estado140.Add("LValue", "108");
            Estado140.Add("Constant", "109");
            Estados.Add(140, Estado140);

            Dictionary<string, string> Estado141 = new Dictionary<string, string>();
            Estado141.Add("ident", "r69");
            Estado141.Add(";", "r69");
            Estado141.Add("(", "r69");
            Estado141.Add(")", "r69");
            Estado141.Add(",", "r69");
            Estado141.Add(".", "r69");
            Estado141.Add("&&", "r69");
            Estado141.Add("==", "r69");
            Estado141.Add("<", "r69");
            Estado141.Add("<=", "r69");
            Estado141.Add("+", "r69");
            Estado141.Add("*", "r69");
            Estado141.Add("%", "r69");
            Estado141.Add("!", "r69");
            Estado141.Add("New", "r69");
            Estado141.Add("this", "r69");
            Estado141.Add("-", "r69");
            Estado141.Add("intConstant", "r69");
            Estado141.Add("doubleConstant", "r69");
            Estado141.Add("boolConstant", "r69");
            Estado141.Add("stringConstant", "r69");
            Estado141.Add("null", "r69");
            Estados.Add(141, Estado141);

            Dictionary<string, string> Estado142 = new Dictionary<string, string>();
            Estado142.Add("ident", "s113");
            Estado142.Add("(", "s111");
            Estado142.Add("*", "s104");
            Estado142.Add("!", "s106");
            Estado142.Add("New", "s107");
            Estado142.Add("this", "s110");
            Estado142.Add("-", "s112");
            Estado142.Add("intConstant", "s114");
            Estado142.Add("doubleConstant", "s115");
            Estado142.Add("boolConstant", "s116");
            Estado142.Add("stringConstant", "s117");
            Estado142.Add("null", "s118");
            Estado142.Add("Expr", "135");
            Estado142.Add("ExprA", "98");
            Estado142.Add("ExprB", "99");
            Estado142.Add("ExprC", "100");
            Estado142.Add("ExprD", "165");
            Estado142.Add("ExprE", "102");
            Estado142.Add("ExprF", "103");
            Estado142.Add("ExprG", "105");
            Estado142.Add("LValue", "108");
            Estado142.Add("Constant", "109");
            Estados.Add(142, Estado142);

            Dictionary<string, string> Estado143 = new Dictionary<string, string>();
            Estado143.Add("ident", "r72");
            Estado143.Add(";", "r72");
            Estado143.Add("(", "r72");
            Estado143.Add(")", "r72");
            Estado143.Add(",", "r72");
            Estado143.Add(".", "r72");
            Estado143.Add("&&", "r72");
            Estado143.Add("==", "r72");
            Estado143.Add("<", "r72");
            Estado143.Add("<=", "r72");
            Estado143.Add("+", "r72");
            Estado143.Add("*", "r72");
            Estado143.Add("%", "r72");
            Estado143.Add("!", "r72");
            Estado143.Add("New", "r72");
            Estado143.Add("this", "r72");
            Estado143.Add("-", "r72");
            Estado143.Add("intConstant", "r72");
            Estado143.Add("doubleConstant", "r72");
            Estado143.Add("boolConstant", "r72");
            Estado143.Add("stringConstant", "r72");
            Estado143.Add("null", "r72");
            Estados.Add(143, Estado143);

            Dictionary<string, string> Estado144 = new Dictionary<string, string>();
            Estado144.Add("ident", "s113");
            Estado144.Add("(", "s111");
            Estado144.Add("*", "s104");
            Estado144.Add("!", "s106");
            Estado144.Add("New", "s107");
            Estado144.Add("this", "s110");
            Estado144.Add("-", "s112");
            Estado144.Add("intConstant", "s114");
            Estado144.Add("doubleConstant", "s115");
            Estado144.Add("boolConstant", "s116");
            Estado144.Add("stringConstant", "s117");
            Estado144.Add("null", "s118");
            Estado144.Add("Expr", "135");
            Estado144.Add("ExprA", "98");
            Estado144.Add("ExprB", "99");
            Estado144.Add("ExprC", "100");
            Estado144.Add("ExprD", "101");
            Estado144.Add("ExprE", "166");
            Estado144.Add("ExprF", "103");
            Estado144.Add("ExprG", "105");
            Estado144.Add("LValue", "108");
            Estado144.Add("Constant", "109");
            Estados.Add(144, Estado144);

            Dictionary<string, string> Estado145 = new Dictionary<string, string>();
            Estado145.Add("ident", "r75");
            Estado145.Add(";", "r75");
            Estado145.Add("(", "r75");
            Estado145.Add(")", "r75");
            Estado145.Add(",", "r75");
            Estado145.Add(".", "r75");
            Estado145.Add("&&", "r75");
            Estado145.Add("==", "r75");
            Estado145.Add("<", "r75");
            Estado145.Add("<=", "r75");
            Estado145.Add("+", "r75");
            Estado145.Add("*", "r75");
            Estado145.Add("%", "r75");
            Estado145.Add("!", "r75");
            Estado145.Add("New", "r75");
            Estado145.Add("this", "r75");
            Estado145.Add("-", "r75");
            Estado145.Add("intConstant", "r75");
            Estado145.Add("doubleConstant", "r75");
            Estado145.Add("boolConstant", "r75");
            Estado145.Add("stringConstant", "r75");
            Estado145.Add("null", "r75");
            Estados.Add(145, Estado145);

            Dictionary<string, string> Estado146 = new Dictionary<string, string>();
            Estado146.Add("ident", "r77");
            Estado146.Add(";", "r77");
            Estado146.Add("(", "r77");
            Estado146.Add(")", "r77");
            Estado146.Add(",", "r77");
            Estado146.Add(".", "r77");
            Estado146.Add("&&", "r77");
            Estado146.Add("==", "r77");
            Estado146.Add("<", "r77");
            Estado146.Add("<=", "r77");
            Estado146.Add("+", "r77");
            Estado146.Add("*", "r77");
            Estado146.Add("%", "r77");
            Estado146.Add("!", "r77");
            Estado146.Add("New", "r77");
            Estado146.Add("this", "r77");
            Estado146.Add("-", "r77");
            Estado146.Add("intConstant", "r77");
            Estado146.Add("doubleConstant", "r77");
            Estado146.Add("boolConstant", "r77");
            Estado146.Add("stringConstant", "r77");
            Estado146.Add("null", "r77");
            Estado146.Add("ExprE'", "167");
            Estados.Add(146, Estado146);

            Dictionary<string, string> Estado147 = new Dictionary<string, string>();
            Estado147.Add("ident", "r78");
            Estado147.Add(";", "r78");
            Estado147.Add("(", "r78");
            Estado147.Add(")", "r78");
            Estado147.Add(",", "r78");
            Estado147.Add(".", "r78");
            Estado147.Add("&&", "r78");
            Estado147.Add("==", "r78");
            Estado147.Add("<", "r78");
            Estado147.Add("<=", "r78");
            Estado147.Add("+", "r78");
            Estado147.Add("*", "r78");
            Estado147.Add("%", "r78");
            Estado147.Add("!", "r78");
            Estado147.Add("New", "r78");
            Estado147.Add("this", "r78");
            Estado147.Add("-", "r78");
            Estado147.Add("intConstant", "r78");
            Estado147.Add("doubleConstant", "r78");
            Estado147.Add("boolConstant", "r78");
            Estado147.Add("stringConstant", "r78");
            Estado147.Add("null", "r78");
            Estados.Add(147, Estado147);

            Dictionary<string, string> Estado148 = new Dictionary<string, string>();
            Estado148.Add("ident", "s113");
            Estado148.Add("(", "s111");
            Estado148.Add("*", "s104");
            Estado148.Add("!", "s106");
            Estado148.Add("New", "s107");
            Estado148.Add("this", "s110");
            Estado148.Add("-", "s112");
            Estado148.Add("intConstant", "s114");
            Estado148.Add("doubleConstant", "s115");
            Estado148.Add("boolConstant", "s116");
            Estado148.Add("stringConstant", "s117");
            Estado148.Add("null", "s118");
            Estado148.Add("Expr", "135");
            Estado148.Add("ExprA", "98");
            Estado148.Add("ExprB", "99");
            Estado148.Add("ExprC", "100");
            Estado148.Add("ExprD", "101");
            Estado148.Add("ExprE", "102");
            Estado148.Add("ExprF", "103");
            Estado148.Add("ExprG", "168");
            Estado148.Add("LValue", "108");
            Estado148.Add("Constant", "109");
            Estados.Add(148, Estado148);

            Dictionary<string, string> Estado149 = new Dictionary<string, string>();
            Estado149.Add("ident", "r81");
            Estado149.Add(";", "r81");
            Estado149.Add("(", "r81");
            Estado149.Add(")", "r81");
            Estado149.Add(",", "r81");
            Estado149.Add(".", "s133 ");
            Dictionary<string, string> Estado149_ = new Dictionary<string, string>();
            Estado149_.Add(".", " r81");
            Estado149.Add("&&", "r81");
            Estado149.Add("==", "r81");
            Estado149.Add("<", "r81");
            Estado149.Add("<=", "r81");
            Estado149.Add("+", "r81");
            Estado149.Add("*", "r81");
            Estado149.Add("%", "r81");
            Estado149.Add("!", "r81");
            Estado149.Add("New", "r81");
            Estado149.Add("this", "r81");
            Estado149.Add("-", "r81");
            Estado149.Add("intConstant", "r81");
            Estado149.Add("doubleConstant", "r81");
            Estado149.Add("boolConstant", "r81");
            Estado149.Add("stringConstant", "r81");
            Estado149.Add("null", "r81");
            Estados.Add(149, Estado149);
            Estados.Add(1490, Estado149_);

            Dictionary<string, string> Estado150 = new Dictionary<string, string>();
            Estado150.Add("ident", "s169");
            Estados.Add(150, Estado150);

            Dictionary<string, string> Estado151 = new Dictionary<string, string>();
            Estado151.Add("ident", "s113");
            Estado151.Add("(", "s111");
            Estado151.Add("*", "s104");
            Estado151.Add("!", "s106");
            Estado151.Add("New", "s107");
            Estado151.Add("this", "s110");
            Estado151.Add("-", "s112");
            Estado151.Add("intConstant", "s114");
            Estado151.Add("doubleConstant", "s115");
            Estado151.Add("boolConstant", "s116");
            Estado151.Add("stringConstant", "s117");
            Estado151.Add("null", "s118");
            Estado151.Add("Expr", "170");
            Estado151.Add("ExprA", "98");
            Estado151.Add("ExprB", "99");
            Estado151.Add("ExprC", "100");
            Estado151.Add("ExprD", "101");
            Estado151.Add("ExprE", "102");
            Estado151.Add("ExprF", "103");
            Estado151.Add("ExprG", "105");
            Estado151.Add("LValue", "108");
            Estado151.Add("Constant", "109");
            Estados.Add(151, Estado151);

            Dictionary<string, string> Estado152 = new Dictionary<string, string>();
            Estado152.Add(")", "s171");
            Estado152.Add(".", "s133");
            Estados.Add(152, Estado152);

            Dictionary<string, string> Estado153 = new Dictionary<string, string>();
            Estado153.Add("ident", "r88");
            Estado153.Add(";", "r88");
            Estado153.Add("(", "r88");
            Estado153.Add(")", "r88");
            Estado153.Add(",", "r88");
            Estado153.Add(".", "s133 ");
            Dictionary<string, string> Estado153_ = new Dictionary<string, string>();
            Estado153_.Add(".", " r88");
            Estado153.Add("&&", "r88");
            Estado153.Add("==", "r88");
            Estado153.Add("<", "r88");
            Estado153.Add("<=", "r88");
            Estado153.Add("+", "r88");
            Estado153.Add("*", "r88");
            Estado153.Add("%", "r88");
            Estado153.Add("!", "r88");
            Estado153.Add("New", "r88");
            Estado153.Add("this", "r88");
            Estado153.Add("-", "r88");
            Estado153.Add("intConstant", "r88");
            Estado153.Add("doubleConstant", "r88");
            Estado153.Add("boolConstant", "r88");
            Estado153.Add("stringConstant", "r88");
            Estado153.Add("null", "r88");
            Estados.Add(153, Estado153);
            Estados.Add(1530, Estado153_);

            Dictionary<string, string> Estado154 = new Dictionary<string, string>();
            Estado154.Add(";", "s172");
            Estados.Add(154, Estado154);

            Dictionary<string, string> Estado155 = new Dictionary<string, string>();
            Estado155.Add("ident", "s12 ");
            Dictionary<string, string> Estado155_ = new Dictionary<string, string>();
            Estado155_.Add("ident", " r40");
            Estado155.Add(";", "r40");
            Estado155.Add("const", "r40");
            Estado155.Add("class", "r40");
            Estado155.Add("{", "r40");
            Estado155.Add("}", "r40");
            Estado155.Add("interface", "r40");
            Estado155.Add("int", "s8 ");
            Estado155_.Add("int", " r40");
            Estado155.Add("double", "s9 ");
            Estado155_.Add("double", " r40");
            Estado155.Add("bool", "s10 ");
            Estado155_.Add("bool", " r40");
            Estado155.Add("string", "s11 ");
            Estado155_.Add("string", " r40");
            Estado155.Add("(", "r40");
            Estado155.Add("void", "r40");
            Estado155.Add("if", "r40");
            Estado155.Add("while", "r40");
            Estado155.Add("for", "r40");
            Estado155.Add("break", "r40");
            Estado155.Add("return", "r40");
            Estado155.Add("Console", "r40");
            Estado155.Add("else", "r40");
            Estado155.Add("&&", "r40");
            Estado155.Add("*", "r40");
            Estado155.Add("!", "r40");
            Estado155.Add("New", "r40");
            Estado155.Add("this", "r40");
            Estado155.Add("-", "r40");
            Estado155.Add("intConstant", "r40");
            Estado155.Add("doubleConstant", "r40");
            Estado155.Add("boolConstant", "r40");
            Estado155.Add("stringConstant", "r40");
            Estado155.Add("null", "r40");
            Estado155.Add("$/#", "r40");
            Estado155.Add("Type", "69");
            Estado155.Add("VariableDecl'", "173");
            Estados.Add(155, Estado155);
            Estados.Add(1550, Estado155_);

            Dictionary<string, string> Estado156 = new Dictionary<string, string>();
            Estado156.Add(")", "s174");
            Estado156.Add(".", "s133");
            Estados.Add(156, Estado156);

            Dictionary<string, string> Estado157 = new Dictionary<string, string>();
            Estado157.Add(")", "s175");
            Estado157.Add(".", "s133");
            Estados.Add(157, Estado157);

            Dictionary<string, string> Estado158 = new Dictionary<string, string>();
            Estado158.Add(";", "s176");
            Estado158.Add(".", "s133");
            Estados.Add(158, Estado158);

            Dictionary<string, string> Estado159 = new Dictionary<string, string>();
            Estado159.Add("ident", "r52");
            Estado159.Add(";", "r52");
            Estado159.Add("{", "r52");
            Estado159.Add("}", "r52");
            Estado159.Add("(", "r52");
            Estado159.Add("if", "r52");
            Estado159.Add("while", "r52");
            Estado159.Add("for", "r52");
            Estado159.Add("break", "r52");
            Estado159.Add("return", "r52");
            Estado159.Add("Console", "r52");
            Estado159.Add("else", "r52");
            Estado159.Add("&&", "r52");
            Estado159.Add("*", "r52");
            Estado159.Add("!", "r52");
            Estado159.Add("New", "r52");
            Estado159.Add("this", "r52");
            Estado159.Add("-", "r52");
            Estado159.Add("intConstant", "r52");
            Estado159.Add("doubleConstant", "r52");
            Estado159.Add("boolConstant", "r52");
            Estado159.Add("stringConstant", "r52");
            Estado159.Add("null", "r52");
            Estados.Add(159, Estado159);

            Dictionary<string, string> Estado160 = new Dictionary<string, string>();
            Estado160.Add("(", "s177");
            Estados.Add(160, Estado160);

            Dictionary<string, string> Estado161 = new Dictionary<string, string>();
            Estado161.Add("ident", "r90");
            Estado161.Add(";", "r90");
            Estado161.Add("(", "r90");
            Estado161.Add(")", "r90");
            Estado161.Add(",", "r90");
            Estado161.Add(".", "r90");
            Estado161.Add("&&", "r90");
            Estado161.Add("==", "r90");
            Estado161.Add("<", "r90");
            Estado161.Add("<=", "r90");
            Estado161.Add("+", "r90");
            Estado161.Add("*", "r90");
            Estado161.Add("%", "r90");
            Estado161.Add("!", "r90");
            Estado161.Add("New", "r90");
            Estado161.Add("=", "r90");
            Estado161.Add("this", "r90");
            Estado161.Add("-", "r90");
            Estado161.Add("intConstant", "r90");
            Estado161.Add("doubleConstant", "r90");
            Estado161.Add("boolConstant", "r90");
            Estado161.Add("stringConstant", "r90");
            Estado161.Add("null", "r90");
            Estados.Add(161, Estado161);

            Dictionary<string, string> Estado162 = new Dictionary<string, string>();
            Estado162.Add("ident", "r61 ");
            Dictionary<string, string> Estado162_ = new Dictionary<string, string>();
            Estado162_.Add("ident", " r60");
            Estado162.Add(";", "r61 ");
            Estado162_.Add(";", " r60");
            Estado162.Add("(", "r61 ");
            Estado162_.Add("(", " r60");
            Estado162.Add(")", "r61 ");
            Estado162_.Add(")", " r60");
            Estado162.Add(",", "r61 ");
            Estado162_.Add(",", " r60");
            Estado162.Add(".", "r61 ");
            Estado162_.Add(".", " r60");
            Estado162.Add("&&", "r61 ");
            Estado162_.Add("&&", " r60");
            Estado162.Add("==", "r61 ");
            Estado162_.Add("==", " r60");
            Estado162.Add("<", "r61 ");
            Estado162_.Add("<", " r60");
            Estado162.Add("<=", "r61 ");
            Estado162_.Add("<=", " r60");
            Estado162.Add("+", "r61 ");
            Estado162_.Add("+", " r60");
            Estado162.Add("*", "r61 ");
            Estado162_.Add("*", " r60");
            Estado162.Add("%", "r61 ");
            Estado162_.Add("%", " r60");
            Estado162.Add("!", "r61 ");
            Estado162_.Add("!", " r60");
            Estado162.Add("New", "r61 ");
            Estado162_.Add("New", " r60");
            Estado162.Add("this", "r61 ");
            Estado162_.Add("this", " r60");
            Estado162.Add("-", "r61 ");
            Estado162_.Add("-", " r60");
            Estado162.Add("intConstant", "r61 ");
            Estado162_.Add("intConstant", " r60");
            Estado162.Add("doubleConstant", "r61 ");
            Estado162_.Add("doubleConstant", " r60");
            Estado162.Add("boolConstant", "r61 ");
            Estado162_.Add("boolConstant", " r60");
            Estado162.Add("stringConstant", "r61 ");
            Estado162_.Add("stringConstant", " r60");
            Estado162.Add("null", "r61 ");
            Estado162_.Add("null", " r60");
            Estados.Add(162, Estado162);
            Estados.Add(1620, Estado162_);

            Dictionary<string, string> Estado163 = new Dictionary<string, string>();
            Estado163.Add("ident", "r65");
            Estado163.Add(";", "r65");
            Estado163.Add("(", "r65");
            Estado163.Add(")", "r65");
            Estado163.Add(",", "r65");
            Estado163.Add(".", "r65");
            Estado163.Add("&&", "r65");
            Estado163.Add("==", "s138 ");
            Dictionary<string, string> Estado163_ = new Dictionary<string, string>();
            Estado163_.Add("==", " r65");
            Estado163.Add("<", "r65");
            Estado163.Add("<=", "r65");
            Estado163.Add("+", "r65");
            Estado163.Add("*", "r65");
            Estado163.Add("%", "r65");
            Estado163.Add("!", "r65");
            Estado163.Add("New", "r65");
            Estado163.Add("this", "r65");
            Estado163.Add("-", "r65");
            Estado163.Add("intConstant", "r65");
            Estado163.Add("doubleConstant", "r65");
            Estado163.Add("boolConstant", "r65");
            Estado163.Add("stringConstant", "r65");
            Estado163.Add("null", "r65");
            Estado163.Add("ExprA'", "178");
            Estados.Add(163, Estado163);
            Estados.Add(1630, Estado163_);

            Dictionary<string, string> Estado164 = new Dictionary<string, string>();
            Estado164.Add("ident", "r68");
            Estado164.Add(";", "r68");
            Estado164.Add("(", "r68");
            Estado164.Add(")", "r68");
            Estado164.Add(",", "r68");
            Estado164.Add(".", "r68");
            Estado164.Add("&&", "r68");
            Estado164.Add("==", "r68");
            Estado164.Add("<", "s140 ");
            Dictionary<string, string> Estado164_ = new Dictionary<string, string>();
            Estado164_.Add("<", " r68");
            Estado164.Add("<=", "r68");
            Estado164.Add("+", "r68");
            Estado164.Add("*", "r68");
            Estado164.Add("%", "r68");
            Estado164.Add("!", "r68");
            Estado164.Add("New", "r68");
            Estado164.Add("this", "r68");
            Estado164.Add("-", "r68");
            Estado164.Add("intConstant", "r68");
            Estado164.Add("doubleConstant", "r68");
            Estado164.Add("boolConstant", "r68");
            Estado164.Add("stringConstant", "r68");
            Estado164.Add("null", "r68");
            Estado164.Add("ExprB'", "179");
            Estados.Add(164, Estado164);
            Estados.Add(1640, Estado164_);

            Dictionary<string, string> Estado165 = new Dictionary<string, string>();
            Estado165.Add("ident", "r71");
            Estado165.Add(";", "r71");
            Estado165.Add("(", "r71");
            Estado165.Add(")", "r71");
            Estado165.Add(",", "r71");
            Estado165.Add(".", "r71");
            Estado165.Add("&&", "r71");
            Estado165.Add("==", "r71");
            Estado165.Add("<", "r71");
            Estado165.Add("<=", "s142 ");
            Dictionary<string, string> Estado165_ = new Dictionary<string, string>();
            Estado165_.Add("<=", " r71");
            Estado165.Add("+", "r71");
            Estado165.Add("*", "r71");
            Estado165.Add("%", "r71");
            Estado165.Add("!", "r71");
            Estado165.Add("New", "r71");
            Estado165.Add("this", "r71");
            Estado165.Add("-", "r71");
            Estado165.Add("intConstant", "r71");
            Estado165.Add("doubleConstant", "r71");
            Estado165.Add("boolConstant", "r71");
            Estado165.Add("stringConstant", "r71");
            Estado165.Add("null", "r71");
            Estado165.Add("ExprC'", "180");
            Estados.Add(165, Estado165);
            Estados.Add(1650, Estado165_);

            Dictionary<string, string> Estado166 = new Dictionary<string, string>();
            Estado166.Add("ident", "r74");
            Estado166.Add(";", "r74");
            Estado166.Add("(", "r74");
            Estado166.Add(")", "r74");
            Estado166.Add(",", "r74");
            Estado166.Add(".", "r74");
            Estado166.Add("&&", "r74");
            Estado166.Add("==", "r74");
            Estado166.Add("<", "r74");
            Estado166.Add("<=", "r74");
            Estado166.Add("+", "s144 ");
            Dictionary<string, string> Estado166_ = new Dictionary<string, string>();
            Estado166_.Add("+", " r74");
            Estado166.Add("*", "r74");
            Estado166.Add("%", "r74");
            Estado166.Add("!", "r74");
            Estado166.Add("New", "r74");
            Estado166.Add("this", "r74");
            Estado166.Add("-", "r74");
            Estado166.Add("intConstant", "r74");
            Estado166.Add("doubleConstant", "r74");
            Estado166.Add("boolConstant", "r74");
            Estado166.Add("stringConstant", "r74");
            Estado166.Add("null", "r74");
            Estado166.Add("ExprD'", "181");
            Estados.Add(166, Estado166);
            Estados.Add(1660, Estado166_);

            Dictionary<string, string> Estado167 = new Dictionary<string, string>();
            Estado167.Add("ident", "r76 ");
            Dictionary<string, string> Estado167_ = new Dictionary<string, string>();
            Estado167_.Add("ident", " r75");
            Estado167.Add(";", "r76 ");
            Estado167_.Add(";", " r75");
            Estado167.Add("(", "r76 ");
            Estado167_.Add("(", " r75");
            Estado167.Add(")", "r76 ");
            Estado167_.Add(")", " r75");
            Estado167.Add(",", "r76 ");
            Estado167_.Add(",", " r75");
            Estado167.Add(".", "r76 ");
            Estado167_.Add(".", " r75");
            Estado167.Add("&&", "r76 ");
            Estado167_.Add("&&", " r75");
            Estado167.Add("==", "r76 ");
            Estado167_.Add("==", " r75");
            Estado167.Add("<", "r76 ");
            Estado167_.Add("<", " r75");
            Estado167.Add("<=", "r76 ");
            Estado167_.Add("<=", " r75");
            Estado167.Add("+", "r76 ");
            Estado167_.Add("+", " r75");
            Estado167.Add("*", "r76 ");
            Estado167_.Add("*", " r75");
            Estado167.Add("%", "r76 ");
            Estado167_.Add("%", " r75");
            Estado167.Add("!", "r76 ");
            Estado167_.Add("!", " r75");
            Estado167.Add("New", "r76 ");
            Estado167_.Add("New", " r75");
            Estado167.Add("this", "r76 ");
            Estado167_.Add("this", " r75");
            Estado167.Add("-", "r76 ");
            Estado167_.Add("-", " r75");
            Estado167.Add("intConstant", "r76 ");
            Estado167_.Add("intConstant", " r75");
            Estado167.Add("doubleConstant", "r76 ");
            Estado167_.Add("doubleConstant", " r75");
            Estado167.Add("boolConstant", "r76 ");
            Estado167_.Add("boolConstant", " r75");
            Estado167.Add("stringConstant", "r76 ");
            Estado167_.Add("stringConstant", " r75");
            Estado167.Add("null", "r76 ");
            Estado167_.Add("null", " r75");
            Estados.Add(167, Estado167);
            Estados.Add(1670, Estado167_);

            Dictionary<string, string> Estado168 = new Dictionary<string, string>();
            Estado168.Add("ident", "r80");
            Estado168.Add(";", "r80");
            Estado168.Add("(", "r80");
            Estado168.Add(")", "r80");
            Estado168.Add(",", "r80");
            Estado168.Add(".", "r80");
            Estado168.Add("&&", "r80");
            Estado168.Add("==", "r80");
            Estado168.Add("<", "r80");
            Estado168.Add("<=", "r80");
            Estado168.Add("+", "r80");
            Estado168.Add("*", "r80");
            Estado168.Add("%", "s148 ");
            Dictionary<string, string> Estado168_ = new Dictionary<string, string>();
            Estado168_.Add("%", " r80");
            Estado168.Add("!", "r80");
            Estado168.Add("New", "r80");
            Estado168.Add("this", "r80");
            Estado168.Add("-", "r80");
            Estado168.Add("intConstant", "r80");
            Estado168.Add("doubleConstant", "r80");
            Estado168.Add("boolConstant", "r80");
            Estado168.Add("stringConstant", "r80");
            Estado168.Add("null", "r80");
            Estado168.Add("ExprF'", "182");
            Estados.Add(168, Estado168);
            Estados.Add(1680, Estado168_);

            Dictionary<string, string> Estado169 = new Dictionary<string, string>();
            Estado169.Add(")", "s183");
            Estados.Add(169, Estado169);

            Dictionary<string, string> Estado170 = new Dictionary<string, string>();
            Estado170.Add("ident", "r83");
            Estado170.Add(";", "r83");
            Estado170.Add("(", "r83");
            Estado170.Add(")", "r83");
            Estado170.Add(",", "r83");
            Estado170.Add(".", "s133 ");
            Dictionary<string, string> Estado170_ = new Dictionary<string, string>();
            Estado170_.Add(".", " r83");
            Estado170.Add("&&", "r83");
            Estado170.Add("==", "r83");
            Estado170.Add("<", "r83");
            Estado170.Add("<=", "r83");
            Estado170.Add("+", "r83");
            Estado170.Add("*", "r83");
            Estado170.Add("%", "r83");
            Estado170.Add("!", "r83");
            Estado170.Add("New", "r83");
            Estado170.Add("this", "r83");
            Estado170.Add("-", "r83");
            Estado170.Add("intConstant", "r83");
            Estado170.Add("doubleConstant", "r83");
            Estado170.Add("boolConstant", "r83");
            Estado170.Add("stringConstant", "r83");
            Estado170.Add("null", "r83");
            Estados.Add(170, Estado170);
            Estados.Add(1700, Estado170_);

            Dictionary<string, string> Estado171 = new Dictionary<string, string>();
            Estado171.Add("ident", "r87");
            Estado171.Add(";", "r87");
            Estado171.Add("(", "r87");
            Estado171.Add(")", "r87");
            Estado171.Add(",", "r87");
            Estado171.Add(".", "r87");
            Estado171.Add("&&", "r87");
            Estado171.Add("==", "r87");
            Estado171.Add("<", "r87");
            Estado171.Add("<=", "r87");
            Estado171.Add("+", "r87");
            Estado171.Add("*", "r87");
            Estado171.Add("%", "r87");
            Estado171.Add("!", "r87");
            Estado171.Add("New", "r87");
            Estado171.Add("this", "r87");
            Estado171.Add("-", "r87");
            Estado171.Add("intConstant", "r87");
            Estado171.Add("doubleConstant", "r87");
            Estado171.Add("boolConstant", "r87");
            Estado171.Add("stringConstant", "r87");
            Estado171.Add("null", "r87");
            Estados.Add(171, Estado171);

            Dictionary<string, string> Estado172 = new Dictionary<string, string>();
            Estado172.Add("ident", "r42");
            Estado172.Add(";", "r42");
            Estado172.Add("const", "s80 ");
            Dictionary<string, string> Estado172_ = new Dictionary<string, string>();
            Estado172_.Add("const", " r42");
            Estado172.Add("class", "r42");
            Estado172.Add("{", "r42");
            Estado172.Add("}", "r42");
            Estado172.Add("interface", "r42");
            Estado172.Add("int", "r42");
            Estado172.Add("double", "r42");
            Estado172.Add("bool", "r42");
            Estado172.Add("string", "r42");
            Estado172.Add("(", "r42");
            Estado172.Add("void", "r42");
            Estado172.Add("if", "r42");
            Estado172.Add("while", "r42");
            Estado172.Add("for", "r42");
            Estado172.Add("break", "r42");
            Estado172.Add("return", "r42");
            Estado172.Add("Console", "r42");
            Estado172.Add("else", "r42");
            Estado172.Add("&&", "r42");
            Estado172.Add("*", "r42");
            Estado172.Add("!", "r42");
            Estado172.Add("New", "r42");
            Estado172.Add("this", "r42");
            Estado172.Add("-", "r42");
            Estado172.Add("intConstant", "r42");
            Estado172.Add("doubleConstant", "r42");
            Estado172.Add("boolConstant", "r42");
            Estado172.Add("stringConstant", "r42");
            Estado172.Add("null", "r42");
            Estado172.Add("$/#", "r42");
            Estado172.Add("ConstDecl'", "184");
            Estados.Add(172, Estado172);
            Estados.Add(1720, Estado172_);

            Dictionary<string, string> Estado173 = new Dictionary<string, string>();
            Estado173.Add("ident", "r39");
            Estado173.Add(";", "r39");
            Estado173.Add("const", "r39");
            Estado173.Add("class", "r39");
            Estado173.Add("{", "r39");
            Estado173.Add("}", "r39");
            Estado173.Add("interface", "r39");
            Estado173.Add("int", "r39");
            Estado173.Add("double", "r39");
            Estado173.Add("bool", "r39");
            Estado173.Add("string", "r39");
            Estado173.Add("(", "r39");
            Estado173.Add("void", "r39");
            Estado173.Add("if", "r39");
            Estado173.Add("while", "r39");
            Estado173.Add("for", "r39");
            Estado173.Add("break", "r39");
            Estado173.Add("return", "r39");
            Estado173.Add("Console", "r39");
            Estado173.Add("else", "r39");
            Estado173.Add("&&", "r39");
            Estado173.Add("*", "r39");
            Estado173.Add("!", "r39");
            Estado173.Add("New", "r39");
            Estado173.Add("this", "r39");
            Estado173.Add("-", "r39");
            Estado173.Add("intConstant", "r39");
            Estado173.Add("doubleConstant", "r39");
            Estado173.Add("boolConstant", "r39");
            Estado173.Add("stringConstant", "r39");
            Estado173.Add("null", "r39");
            Estado173.Add("$/#", "r39");
            Estados.Add(173, Estado173);

            Dictionary<string, string> Estado174 = new Dictionary<string, string>();
            Estado174.Add("ident", "s113 ");
            Dictionary<string, string> Estado174_ = new Dictionary<string, string>();
            Estado174_.Add("ident", " r47");
            Estado174.Add(";", "r47");
            Estado174.Add("{", "s56");
            Estado174.Add("(", "s111 ");
            Estado174_.Add("(", " r47");
            Estado174.Add(")", "r47");
            Estado174.Add(",", "r47");
            Estado174.Add("if", "s89");
            Estado174.Add("while", "s90");
            Estado174.Add("for", "s91");
            Estado174.Add("break", "s92");
            Estado174.Add("return", "s93");
            Estado174.Add("Console", "s94");
            Estado174.Add(".", "r47");
            Estado174.Add("&&", "s97 ");
            Estado174_.Add("&&", " r47");
            Estado174.Add("==", "r47");
            Estado174.Add("<", "r47");
            Estado174.Add("<=", "r47");
            Estado174.Add("+", "r47");
            Estado174.Add("*", "s104 ");
            Estado174_.Add("*", " r47");
            Estado174.Add("%", "r47");
            Estado174.Add("!", "s106 ");
            Estado174_.Add("!", " r47");
            Estado174.Add("New", "s107 ");
            Estado174_.Add("New", " r47");
            Estado174.Add("this", "s110 ");
            Estado174_.Add("this", " r47");
            Estado174.Add("-", "s112 ");
            Estado174_.Add("-", " r47");
            Estado174.Add("intConstant", "s114 ");
            Estado174_.Add("intConstant", " r47");
            Estado174.Add("doubleConstant", "s115 ");
            Estado174_.Add("doubleConstant", " r47");
            Estado174.Add("boolConstant", "s116 ");
            Estado174_.Add("boolConstant", " r47");
            Estado174.Add("stringConstant", "s117 ");
            Estado174_.Add("stringConstant", " r47");
            Estado174.Add("null", "s118 ");
            Estado174_.Add("null", " r47");
            Estado174.Add("StmtBlock", "95");
            Estado174.Add("Stmt", "185");
            Estado174.Add("Expr'", "88");
            Estado174.Add("Expr", "96");
            Estado174.Add("ExprA", "98");
            Estado174.Add("ExprB", "99");
            Estado174.Add("ExprC", "100");
            Estado174.Add("ExprD", "101");
            Estado174.Add("ExprE", "102");
            Estado174.Add("ExprF", "103");
            Estado174.Add("ExprG", "105");
            Estado174.Add("LValue", "108");
            Estado174.Add("Constant", "109");
            Estados.Add(174, Estado174);
            Estados.Add(1740, Estado174_);

            Dictionary<string, string> Estado175 = new Dictionary<string, string>();
            Estado175.Add("ident", "s113 ");
            Dictionary<string, string> Estado175_ = new Dictionary<string, string>();
            Estado175_.Add("ident", " r47");
            Estado175.Add(";", "r47");
            Estado175.Add("{", "s56");
            Estado175.Add("(", "s111 ");
            Estado175_.Add("(", " r47");
            Estado175.Add(")", "r47");
            Estado175.Add(",", "r47");
            Estado175.Add("if", "s89");
            Estado175.Add("while", "s90");
            Estado175.Add("for", "s91");
            Estado175.Add("break", "s92");
            Estado175.Add("return", "s93");
            Estado175.Add("Console", "s94");
            Estado175.Add(".", "r47");
            Estado175.Add("&&", "s97 ");
            Estado175_.Add("&&", " r47");
            Estado175.Add("==", "r47");
            Estado175.Add("<", "r47");
            Estado175.Add("<=", "r47");
            Estado175.Add("+", "r47");
            Estado175.Add("*", "s104 ");
            Estado175_.Add("*", " r47");
            Estado175.Add("%", "r47");
            Estado175.Add("!", "s106 ");
            Estado175_.Add("!", " r47");
            Estado175.Add("New", "s107 ");
            Estado175_.Add("New", " r47");
            Estado175.Add("this", "s110 ");
            Estado175_.Add("this", " r47");
            Estado175.Add("-", "s112 ");
            Estado175_.Add("-", " r47");
            Estado175.Add("intConstant", "s114 ");
            Estado175_.Add("intConstant", " r47");
            Estado175.Add("doubleConstant", "s115 ");
            Estado175_.Add("doubleConstant", " r47");
            Estado175.Add("boolConstant", "s116 ");
            Estado175_.Add("boolConstant", " r47");
            Estado175.Add("stringConstant", "s117 ");
            Estado175_.Add("stringConstant", " r47");
            Estado175.Add("null", "s118 ");
            Estado175_.Add("null", " r47");
            Estado175.Add("StmtBlock", "95");
            Estado175.Add("Stmt", "186");
            Estado175.Add("Expr'", "88");
            Estado175.Add("Expr", "96");
            Estado175.Add("ExprA", "98");
            Estado175.Add("ExprB", "99");
            Estado175.Add("ExprC", "100");
            Estado175.Add("ExprD", "101");
            Estado175.Add("ExprE", "102");
            Estado175.Add("ExprF", "103");
            Estado175.Add("ExprG", "105");
            Estado175.Add("LValue", "108");
            Estado175.Add("Constant", "109");
            Estados.Add(175, Estado175);
            Estados.Add(1750, Estado175_);

            Dictionary<string, string> Estado176 = new Dictionary<string, string>();
            Estado176.Add("ident", "s113");
            Estado176.Add("(", "s111");
            Estado176.Add("*", "s104");
            Estado176.Add("!", "s106");
            Estado176.Add("New", "s107");
            Estado176.Add("this", "s110");
            Estado176.Add("-", "s112");
            Estado176.Add("intConstant", "s114");
            Estado176.Add("doubleConstant", "s115");
            Estado176.Add("boolConstant", "s116");
            Estado176.Add("stringConstant", "s117");
            Estado176.Add("null", "s118");
            Estado176.Add("Expr", "187");
            Estado176.Add("ExprA", "98");
            Estado176.Add("ExprB", "99");
            Estado176.Add("ExprC", "100");
            Estado176.Add("ExprD", "101");
            Estado176.Add("ExprE", "102");
            Estado176.Add("ExprF", "103");
            Estado176.Add("ExprG", "105");
            Estado176.Add("LValue", "108");
            Estado176.Add("Constant", "109");
            Estados.Add(176, Estado176);

            Dictionary<string, string> Estado177 = new Dictionary<string, string>();
            Estado177.Add("ident", "s113");
            Estado177.Add("(", "s111");
            Estado177.Add("*", "s104");
            Estado177.Add("!", "s106");
            Estado177.Add("New", "s107");
            Estado177.Add("this", "s110");
            Estado177.Add("-", "s112");
            Estado177.Add("intConstant", "s114");
            Estado177.Add("doubleConstant", "s115");
            Estado177.Add("boolConstant", "s116");
            Estado177.Add("stringConstant", "s117");
            Estado177.Add("null", "s118");
            Estado177.Add("Ex", "188");
            Estado177.Add("Expr", "189");
            Estado177.Add("ExprA", "98");
            Estado177.Add("ExprB", "99");
            Estado177.Add("ExprC", "100");
            Estado177.Add("ExprD", "101");
            Estado177.Add("ExprE", "102");
            Estado177.Add("ExprF", "103");
            Estado177.Add("ExprG", "105");
            Estado177.Add("LValue", "108");
            Estado177.Add("Constant", "109");
            Estados.Add(177, Estado177);

            Dictionary<string, string> Estado178 = new Dictionary<string, string>();
            Estado178.Add("ident", "r64 ");
            Dictionary<string, string> Estado178_ = new Dictionary<string, string>();
            Estado178_.Add("ident", " r63");
            Estado178.Add(";", "r64 ");
            Estado178_.Add(";", " r63");
            Estado178.Add("(", "r64 ");
            Estado178_.Add("(", " r63");
            Estado178.Add(")", "r64 ");
            Estado178_.Add(")", " r63");
            Estado178.Add(",", "r64 ");
            Estado178_.Add(",", " r63");
            Estado178.Add(".", "r64 ");
            Estado178_.Add(".", " r63");
            Estado178.Add("&&", "r64 ");
            Estado178_.Add("&&", " r63");
            Estado178.Add("==", "r64 ");
            Estado178_.Add("==", " r63");
            Estado178.Add("<", "r64 ");
            Estado178_.Add("<", " r63");
            Estado178.Add("<=", "r64 ");
            Estado178_.Add("<=", " r63");
            Estado178.Add("+", "r64 ");
            Estado178_.Add("+", " r63");
            Estado178.Add("*", "r64 ");
            Estado178_.Add("*", " r63");
            Estado178.Add("%", "r64 ");
            Estado178_.Add("%", " r63");
            Estado178.Add("!", "r64 ");
            Estado178_.Add("!", " r63");
            Estado178.Add("New", "r64 ");
            Estado178_.Add("New", " r63");
            Estado178.Add("this", "r64 ");
            Estado178_.Add("this", " r63");
            Estado178.Add("-", "r64 ");
            Estado178_.Add("-", " r63");
            Estado178.Add("intConstant", "r64 ");
            Estado178_.Add("intConstant", " r63");
            Estado178.Add("doubleConstant", "r64 ");
            Estado178_.Add("doubleConstant", " r63");
            Estado178.Add("boolConstant", "r64 ");
            Estado178_.Add("boolConstant", " r63");
            Estado178.Add("stringConstant", "r64 ");
            Estado178_.Add("stringConstant", " r63");
            Estado178.Add("null", "r64 ");
            Estado178_.Add("null", " r63");
            Estados.Add(178, Estado178);
            Estados.Add(1780, Estado178_);

            Dictionary<string, string> Estado179 = new Dictionary<string, string>();
            Estado179.Add("ident", "r67 ");
            Dictionary<string, string> Estado179_ = new Dictionary<string, string>();
            Estado179_.Add("ident", " r66");
            Estado179.Add(";", "r67 ");
            Estado179_.Add(";", " r66");
            Estado179.Add("(", "r67 ");
            Estado179_.Add("(", " r66");
            Estado179.Add(")", "r67 ");
            Estado179_.Add(")", " r66");
            Estado179.Add(",", "r67 ");
            Estado179_.Add(",", " r66");
            Estado179.Add(".", "r67 ");
            Estado179_.Add(".", " r66");
            Estado179.Add("&&", "r67 ");
            Estado179_.Add("&&", " r66");
            Estado179.Add("==", "r67 ");
            Estado179_.Add("==", " r66");
            Estado179.Add("<", "r67 ");
            Estado179_.Add("<", " r66");
            Estado179.Add("<=", "r67 ");
            Estado179_.Add("<=", " r66");
            Estado179.Add("+", "r67 ");
            Estado179_.Add("+", " r66");
            Estado179.Add("*", "r67 ");
            Estado179_.Add("*", " r66");
            Estado179.Add("%", "r67 ");
            Estado179_.Add("%", " r66");
            Estado179.Add("!", "r67 ");
            Estado179_.Add("!", " r66");
            Estado179.Add("New", "r67 ");
            Estado179_.Add("New", " r66");
            Estado179.Add("this", "r67 ");
            Estado179_.Add("this", " r66");
            Estado179.Add("-", "r67 ");
            Estado179_.Add("-", " r66");
            Estado179.Add("intConstant", "r67 ");
            Estado179_.Add("intConstant", " r66");
            Estado179.Add("doubleConstant", "r67 ");
            Estado179_.Add("doubleConstant", " r66");
            Estado179.Add("boolConstant", "r67 ");
            Estado179_.Add("boolConstant", " r66");
            Estado179.Add("stringConstant", "r67 ");
            Estado179_.Add("stringConstant", " r66");
            Estado179.Add("null", "r67 ");
            Estado179_.Add("null", " r66");
            Estados.Add(179, Estado179);
            Estados.Add(1790, Estado179_);

            Dictionary<string, string> Estado180 = new Dictionary<string, string>();
            Estado180.Add("ident", "r70 ");
            Dictionary<string, string> Estado180_ = new Dictionary<string, string>();
            Estado180_.Add("ident", " r69");
            Estado180.Add(";", "r70 ");
            Estado180_.Add(";", " r69");
            Estado180.Add("(", "r70 ");
            Estado180_.Add("(", " r69");
            Estado180.Add(")", "r70 ");
            Estado180_.Add(")", " r69");
            Estado180.Add(",", "r70 ");
            Estado180_.Add(",", " r69");
            Estado180.Add(".", "r70 ");
            Estado180_.Add(".", " r69");
            Estado180.Add("&&", "r70 ");
            Estado180_.Add("&&", " r69");
            Estado180.Add("==", "r70 ");
            Estado180_.Add("==", " r69");
            Estado180.Add("<", "r70 ");
            Estado180_.Add("<", " r69");
            Estado180.Add("<=", "r70 ");
            Estado180_.Add("<=", " r69");
            Estado180.Add("+", "r70 ");
            Estado180_.Add("+", " r69");
            Estado180.Add("*", "r70 ");
            Estado180_.Add("*", " r69");
            Estado180.Add("%", "r70 ");
            Estado180_.Add("%", " r69");
            Estado180.Add("!", "r70 ");
            Estado180_.Add("!", " r69");
            Estado180.Add("New", "r70 ");
            Estado180_.Add("New", " r69");
            Estado180.Add("this", "r70 ");
            Estado180_.Add("this", " r69");
            Estado180.Add("-", "r70 ");
            Estado180_.Add("-", " r69");
            Estado180.Add("intConstant", "r70 ");
            Estado180_.Add("intConstant", " r69");
            Estado180.Add("doubleConstant", "r70 ");
            Estado180_.Add("doubleConstant", " r69");
            Estado180.Add("boolConstant", "r70 ");
            Estado180_.Add("boolConstant", " r69");
            Estado180.Add("stringConstant", "r70 ");
            Estado180_.Add("stringConstant", " r69");
            Estado180.Add("null", "r70 ");
            Estado180_.Add("null", " r69");
            Estados.Add(180, Estado180);
            Estados.Add(1800, Estado180_);

            Dictionary<string, string> Estado181 = new Dictionary<string, string>();
            Estado181.Add("ident", "r73 ");
            Dictionary<string, string> Estado181_ = new Dictionary<string, string>();
            Estado181_.Add("ident", " r72");
            Estado181.Add(";", "r73 ");
            Estado181_.Add(";", " r72");
            Estado181.Add("(", "r73 ");
            Estado181_.Add("(", " r72");
            Estado181.Add(")", "r73 ");
            Estado181_.Add(")", " r72");
            Estado181.Add(",", "r73 ");
            Estado181_.Add(",", " r72");
            Estado181.Add(".", "r73 ");
            Estado181_.Add(".", " r72");
            Estado181.Add("&&", "r73 ");
            Estado181_.Add("&&", " r72");
            Estado181.Add("==", "r73 ");
            Estado181_.Add("==", " r72");
            Estado181.Add("<", "r73 ");
            Estado181_.Add("<", " r72");
            Estado181.Add("<=", "r73 ");
            Estado181_.Add("<=", " r72");
            Estado181.Add("+", "r73 ");
            Estado181_.Add("+", " r72");
            Estado181.Add("*", "r73 ");
            Estado181_.Add("*", " r72");
            Estado181.Add("%", "r73 ");
            Estado181_.Add("%", " r72");
            Estado181.Add("!", "r73 ");
            Estado181_.Add("!", " r72");
            Estado181.Add("New", "r73 ");
            Estado181_.Add("New", " r72");
            Estado181.Add("this", "r73 ");
            Estado181_.Add("this", " r72");
            Estado181.Add("-", "r73 ");
            Estado181_.Add("-", " r72");
            Estado181.Add("intConstant", "r73 ");
            Estado181_.Add("intConstant", " r72");
            Estado181.Add("doubleConstant", "r73 ");
            Estado181_.Add("doubleConstant", " r72");
            Estado181.Add("boolConstant", "r73 ");
            Estado181_.Add("boolConstant", " r72");
            Estado181.Add("stringConstant", "r73 ");
            Estado181_.Add("stringConstant", " r72");
            Estado181.Add("null", "r73 ");
            Estado181_.Add("null", " r72");
            Estados.Add(181, Estado181);
            Estados.Add(1810, Estado181_);

            Dictionary<string, string> Estado182 = new Dictionary<string, string>();
            Estado182.Add("ident", "r79 ");
            Dictionary<string, string> Estado182_ = new Dictionary<string, string>();
            Estado182_.Add("ident", " r78");
            Estado182.Add(";", "r79 ");
            Estado182_.Add(";", " r78");
            Estado182.Add("(", "r79 ");
            Estado182_.Add("(", " r78");
            Estado182.Add(")", "r79 ");
            Estado182_.Add(")", " r78");
            Estado182.Add(",", "r79 ");
            Estado182_.Add(",", " r78");
            Estado182.Add(".", "r79 ");
            Estado182_.Add(".", " r78");
            Estado182.Add("&&", "r79 ");
            Estado182_.Add("&&", " r78");
            Estado182.Add("==", "r79 ");
            Estado182_.Add("==", " r78");
            Estado182.Add("<", "r79 ");
            Estado182_.Add("<", " r78");
            Estado182.Add("<=", "r79 ");
            Estado182_.Add("<=", " r78");
            Estado182.Add("+", "r79 ");
            Estado182_.Add("+", " r78");
            Estado182.Add("*", "r79 ");
            Estado182_.Add("*", " r78");
            Estado182.Add("%", "r79 ");
            Estado182_.Add("%", " r78");
            Estado182.Add("!", "r79 ");
            Estado182_.Add("!", " r78");
            Estado182.Add("New", "r79 ");
            Estado182_.Add("New", " r78");
            Estado182.Add("this", "r79 ");
            Estado182_.Add("this", " r78");
            Estado182.Add("-", "r79 ");
            Estado182_.Add("-", " r78");
            Estado182.Add("intConstant", "r79 ");
            Estado182_.Add("intConstant", " r78");
            Estado182.Add("doubleConstant", "r79 ");
            Estado182_.Add("doubleConstant", " r78");
            Estado182.Add("boolConstant", "r79 ");
            Estado182_.Add("boolConstant", " r78");
            Estado182.Add("stringConstant", "r79 ");
            Estado182_.Add("stringConstant", " r78");
            Estado182.Add("null", "r79 ");
            Estado182_.Add("null", " r78");
            Estados.Add(182, Estado182);
            Estados.Add(1820, Estado182_);

            Dictionary<string, string> Estado183 = new Dictionary<string, string>();
            Estado183.Add("ident", "r82");
            Estado183.Add(";", "r82");
            Estado183.Add("(", "r82");
            Estado183.Add(")", "r82");
            Estado183.Add(",", "r82");
            Estado183.Add(".", "r82");
            Estado183.Add("&&", "r82");
            Estado183.Add("==", "r82");
            Estado183.Add("<", "r82");
            Estado183.Add("<=", "r82");
            Estado183.Add("+", "r82");
            Estado183.Add("*", "r82");
            Estado183.Add("%", "r82");
            Estado183.Add("!", "r82");
            Estado183.Add("New", "r82");
            Estado183.Add("this", "r82");
            Estado183.Add("-", "r82");
            Estado183.Add("intConstant", "r82");
            Estado183.Add("doubleConstant", "r82");
            Estado183.Add("boolConstant", "r82");
            Estado183.Add("stringConstant", "r82");
            Estado183.Add("null", "r82");
            Estados.Add(183, Estado183);

            Dictionary<string, string> Estado184 = new Dictionary<string, string>();
            Estado184.Add("ident", "r41");
            Estado184.Add(";", "r41");
            Estado184.Add("const", "r41");
            Estado184.Add("class", "r41");
            Estado184.Add("{", "r41");
            Estado184.Add("}", "r41");
            Estado184.Add("interface", "r41");
            Estado184.Add("int", "r41");
            Estado184.Add("double", "r41");
            Estado184.Add("bool", "r41");
            Estado184.Add("string", "r41");
            Estado184.Add("(", "r41");
            Estado184.Add("void", "r41");
            Estado184.Add("if", "r41");
            Estado184.Add("while", "r41");
            Estado184.Add("for", "r41");
            Estado184.Add("break", "r41");
            Estado184.Add("return", "r41");
            Estado184.Add("Console", "r41");
            Estado184.Add("else", "r41");
            Estado184.Add("&&", "r41");
            Estado184.Add("*", "r41");
            Estado184.Add("!", "r41");
            Estado184.Add("New", "r41");
            Estado184.Add("this", "r41");
            Estado184.Add("-", "r41");
            Estado184.Add("intConstant", "r41");
            Estado184.Add("doubleConstant", "r41");
            Estado184.Add("boolConstant", "r41");
            Estado184.Add("stringConstant", "r41");
            Estado184.Add("null", "r41");
            Estado184.Add("$/#", "r41");
            Estados.Add(184, Estado184);

            Dictionary<string, string> Estado185 = new Dictionary<string, string>();
            Estado185.Add("ident", "r56");
            Estado185.Add(";", "r56");
            Estado185.Add("{", "r56");
            Estado185.Add("}", "r56");
            Estado185.Add("(", "r56");
            Estado185.Add("if", "r56");
            Estado185.Add("while", "r56");
            Estado185.Add("for", "r56");
            Estado185.Add("break", "r56");
            Estado185.Add("return", "r56");
            Estado185.Add("Console", "r56");
            Estado185.Add("else", "s191 ");
            Dictionary<string, string> Estado185_ = new Dictionary<string, string>();
            Estado185_.Add("else", " r56");
            Estado185.Add("&&", "r56");
            Estado185.Add("*", "r56");
            Estado185.Add("!", "r56");
            Estado185.Add("New", "r56");
            Estado185.Add("this", "r56");
            Estado185.Add("-", "r56");
            Estado185.Add("intConstant", "r56");
            Estado185.Add("doubleConstant", "r56");
            Estado185.Add("boolConstant", "r56");
            Estado185.Add("stringConstant", "r56");
            Estado185.Add("null", "r56");
            Estado185.Add("Else", "190");
            Estados.Add(185, Estado185);
            Estados.Add(1850, Estado185_);

            Dictionary<string, string> Estado186 = new Dictionary<string, string>();
            Estado186.Add("ident", "r49");
            Estado186.Add(";", "r49");
            Estado186.Add("{", "r49");
            Estado186.Add("}", "r49");
            Estado186.Add("(", "r49");
            Estado186.Add("if", "r49");
            Estado186.Add("while", "r49");
            Estado186.Add("for", "r49");
            Estado186.Add("break", "r49");
            Estado186.Add("return", "r49");
            Estado186.Add("Console", "r49");
            Estado186.Add("else", "r49");
            Estado186.Add("&&", "r49");
            Estado186.Add("*", "r49");
            Estado186.Add("!", "r49");
            Estado186.Add("New", "r49");
            Estado186.Add("this", "r49");
            Estado186.Add("-", "r49");
            Estado186.Add("intConstant", "r49");
            Estado186.Add("doubleConstant", "r49");
            Estado186.Add("boolConstant", "r49");
            Estado186.Add("stringConstant", "r49");
            Estado186.Add("null", "r49");
            Estados.Add(186, Estado186);

            Dictionary<string, string> Estado187 = new Dictionary<string, string>();
            Estado187.Add(";", "s192");
            Estado187.Add(".", "s133");
            Estados.Add(187, Estado187);

            Dictionary<string, string> Estado188 = new Dictionary<string, string>();
            Estado188.Add(",", "s193");
            Estados.Add(188, Estado188);

            Dictionary<string, string> Estado189 = new Dictionary<string, string>();
            Estado189.Add("ident", "s113");
            Estado189.Add("(", "s111");
            Estado189.Add(",", "r59");
            Estado189.Add(".", "s133");
            Estado189.Add("*", "s104");
            Estado189.Add("!", "s106");
            Estado189.Add("New", "s107");
            Estado189.Add("this", "s110");
            Estado189.Add("-", "s112");
            Estado189.Add("intConstant", "s114");
            Estado189.Add("doubleConstant", "s115");
            Estado189.Add("boolConstant", "s116");
            Estado189.Add("stringConstant", "s117");
            Estado189.Add("null", "s118");
            Estado189.Add("Ex'", "194");
            Estado189.Add("Expr", "195");
            Estado189.Add("ExprA", "98");
            Estado189.Add("ExprB", "99");
            Estado189.Add("ExprC", "100");
            Estado189.Add("ExprD", "101");
            Estado189.Add("ExprE", "102");
            Estado189.Add("ExprF", "103");
            Estado189.Add("ExprG", "105");
            Estado189.Add("LValue", "108");
            Estado189.Add("Constant", "109");
            Estados.Add(189, Estado189);

            Dictionary<string, string> Estado190 = new Dictionary<string, string>();
            Estado190.Add("ident", "r48");
            Estado190.Add(";", "r48");
            Estado190.Add("{", "r48");
            Estado190.Add("}", "r48");
            Estado190.Add("(", "r48");
            Estado190.Add("if", "r48");
            Estado190.Add("while", "r48");
            Estado190.Add("for", "r48");
            Estado190.Add("break", "r48");
            Estado190.Add("return", "r48");
            Estado190.Add("Console", "r48");
            Estado190.Add("else", "r48");
            Estado190.Add("&&", "r48");
            Estado190.Add("*", "r48");
            Estado190.Add("!", "r48");
            Estado190.Add("New", "r48");
            Estado190.Add("this", "r48");
            Estado190.Add("-", "r48");
            Estado190.Add("intConstant", "r48");
            Estado190.Add("doubleConstant", "r48");
            Estado190.Add("boolConstant", "r48");
            Estado190.Add("stringConstant", "r48");
            Estado190.Add("null", "r48");
            Estados.Add(190, Estado190);

            Dictionary<string, string> Estado191 = new Dictionary<string, string>();
            Estado191.Add("ident", "s113 ");
            Dictionary<string, string> Estado191_ = new Dictionary<string, string>();
            Estado191_.Add("ident", " r47");
            Estado191.Add(";", "r47");
            Estado191.Add("{", "s56");
            Estado191.Add("(", "s111 ");
            Estado191_.Add("(", " r47");
            Estado191.Add(")", "r47");
            Estado191.Add(",", "r47");
            Estado191.Add("if", "s89");
            Estado191.Add("while", "s90");
            Estado191.Add("for", "s91");
            Estado191.Add("break", "s92");
            Estado191.Add("return", "s93");
            Estado191.Add("Console", "s94");
            Estado191.Add(".", "r47");
            Estado191.Add("&&", "s97 ");
            Estado191_.Add("&&", " r47");
            Estado191.Add("==", "r47");
            Estado191.Add("<", "r47");
            Estado191.Add("<=", "r47");
            Estado191.Add("+", "r47");
            Estado191.Add("*", "s104 ");
            Estado191_.Add("*", " r47");
            Estado191.Add("%", "r47");
            Estado191.Add("!", "s106 ");
            Estado191_.Add("!", " r47");
            Estado191.Add("New", "s107 ");
            Estado191_.Add("New", " r47");
            Estado191.Add("this", "s110 ");
            Estado191_.Add("this", " r47");
            Estado191.Add("-", "s112 ");
            Estado191_.Add("-", " r47");
            Estado191.Add("intConstant", "s114 ");
            Estado191_.Add("intConstant", " r47");
            Estado191.Add("doubleConstant", "s115 ");
            Estado191_.Add("doubleConstant", " r47");
            Estado191.Add("boolConstant", "s116 ");
            Estado191_.Add("boolConstant", " r47");
            Estado191.Add("stringConstant", "s117 ");
            Estado191_.Add("stringConstant", " r47");
            Estado191.Add("null", "s118 ");
            Estado191_.Add("null", " r47");
            Estado191.Add("StmtBlock", "95");
            Estado191.Add("Stmt", "196");
            Estado191.Add("Expr'", "88");
            Estado191.Add("Expr", "96");
            Estado191.Add("ExprA", "98");
            Estado191.Add("ExprB", "99");
            Estado191.Add("ExprC", "100");
            Estado191.Add("ExprD", "101");
            Estado191.Add("ExprE", "102");
            Estado191.Add("ExprF", "103");
            Estado191.Add("ExprG", "105");
            Estado191.Add("LValue", "108");
            Estado191.Add("Constant", "109");
            Estados.Add(191, Estado191);
            Estados.Add(1910, Estado191_);

            Dictionary<string, string> Estado192 = new Dictionary<string, string>();
            Estado192.Add("ident", "s113");
            Estado192.Add("(", "s111");
            Estado192.Add("*", "s104");
            Estado192.Add("!", "s106");
            Estado192.Add("New", "s107");
            Estado192.Add("this", "s110");
            Estado192.Add("-", "s112");
            Estado192.Add("intConstant", "s114");
            Estado192.Add("doubleConstant", "s115");
            Estado192.Add("boolConstant", "s116");
            Estado192.Add("stringConstant", "s117");
            Estado192.Add("null", "s118");
            Estado192.Add("Expr", "197");
            Estado192.Add("ExprA", "98");
            Estado192.Add("ExprB", "99");
            Estado192.Add("ExprC", "100");
            Estado192.Add("ExprD", "101");
            Estado192.Add("ExprE", "102");
            Estado192.Add("ExprF", "103");
            Estado192.Add("ExprG", "105");
            Estado192.Add("LValue", "108");
            Estado192.Add("Constant", "109");
            Estados.Add(192, Estado192);

            Dictionary<string, string> Estado193 = new Dictionary<string, string>();
            Estado193.Add(")", "s198");
            Estados.Add(193, Estado193);

            Dictionary<string, string> Estado194 = new Dictionary<string, string>();
            Estado194.Add(",", "r57");
            Estados.Add(194, Estado194);

            Dictionary<string, string> Estado195 = new Dictionary<string, string>();
            Estado195.Add("ident", "s113");
            Estado195.Add("(", "s111");
            Estado195.Add(",", "r59");
            Estado195.Add(".", "s133");
            Estado195.Add("*", "s104");
            Estado195.Add("!", "s106");
            Estado195.Add("New", "s107");
            Estado195.Add("this", "s110");
            Estado195.Add("-", "s112");
            Estado195.Add("intConstant", "s114");
            Estado195.Add("doubleConstant", "s115");
            Estado195.Add("boolConstant", "s116");
            Estado195.Add("stringConstant", "s117");
            Estado195.Add("null", "s118");
            Estado195.Add("Ex'", "199");
            Estado195.Add("Expr", "195");
            Estado195.Add("ExprA", "98");
            Estado195.Add("ExprB", "99");
            Estado195.Add("ExprC", "100");
            Estado195.Add("ExprD", "101");
            Estado195.Add("ExprE", "102");
            Estado195.Add("ExprF", "103");
            Estado195.Add("ExprG", "105");
            Estado195.Add("LValue", "108");
            Estado195.Add("Constant", "109");
            Estados.Add(195, Estado195);

            Dictionary<string, string> Estado196 = new Dictionary<string, string>();
            Estado196.Add("ident", "r55");
            Estado196.Add(";", "r55");
            Estado196.Add("{", "r55");
            Estado196.Add("}", "r55");
            Estado196.Add("(", "r55");
            Estado196.Add("if", "r55");
            Estado196.Add("while", "r55");
            Estado196.Add("for", "r55");
            Estado196.Add("break", "r55");
            Estado196.Add("return", "r55");
            Estado196.Add("Console", "r55");
            Estado196.Add("else", "r55");
            Estado196.Add("&&", "r55");
            Estado196.Add("*", "r55");
            Estado196.Add("!", "r55");
            Estado196.Add("New", "r55");
            Estado196.Add("this", "r55");
            Estado196.Add("-", "r55");
            Estado196.Add("intConstant", "r55");
            Estado196.Add("doubleConstant", "r55");
            Estado196.Add("boolConstant", "r55");
            Estado196.Add("stringConstant", "r55");
            Estado196.Add("null", "r55");
            Estados.Add(196, Estado196);

            Dictionary<string, string> Estado197 = new Dictionary<string, string>();
            Estado197.Add(")", "s200");
            Estado197.Add(".", "s133");
            Estados.Add(197, Estado197);

            Dictionary<string, string> Estado198 = new Dictionary<string, string>();
            Estado198.Add(";", "s201");
            Estados.Add(198, Estado198);

            Dictionary<string, string> Estado199 = new Dictionary<string, string>();
            Estado199.Add(",", "r58");
            Estados.Add(199, Estado199);

            Dictionary<string, string> Estado200 = new Dictionary<string, string>();
            Estado200.Add("ident", "s113 ");
            Dictionary<string, string> Estado200_ = new Dictionary<string, string>();
            Estado200_.Add("ident", " r47");
            Estado200.Add(";", "r47");
            Estado200.Add("{", "s56");
            Estado200.Add("(", "s111 ");
            Estado200_.Add("(", " r47");
            Estado200.Add(")", "r47");
            Estado200.Add(",", "r47");
            Estado200.Add("if", "s89");
            Estado200.Add("while", "s90");
            Estado200.Add("for", "s91");
            Estado200.Add("break", "s92");
            Estado200.Add("return", "s93");
            Estado200.Add("Console", "s94");
            Estado200.Add(".", "r47");
            Estado200.Add("&&", "s97 ");
            Estado200_.Add("&&", " r47");
            Estado200.Add("==", "r47");
            Estado200.Add("<", "r47");
            Estado200.Add("<=", "r47");
            Estado200.Add("+", "r47");
            Estado200.Add("*", "s104 ");
            Estado200_.Add("*", " r47");
            Estado200.Add("%", "r47");
            Estado200.Add("!", "s106 ");
            Estado200_.Add("!", " r47");
            Estado200.Add("New", "s107 ");
            Estado200_.Add("New", " r47");
            Estado200.Add("this", "s110 ");
            Estado200_.Add("this", " r47");
            Estado200.Add("-", "s112 ");
            Estado200_.Add("-", " r47");
            Estado200.Add("intConstant", "s114 ");
            Estado200_.Add("intConstant", " r47");
            Estado200.Add("doubleConstant", "s115 ");
            Estado200_.Add("doubleConstant", " r47");
            Estado200.Add("boolConstant", "s116 ");
            Estado200_.Add("boolConstant", " r47");
            Estado200.Add("stringConstant", "s117 ");
            Estado200_.Add("stringConstant", " r47");
            Estado200.Add("null", "s118 ");
            Estado200_.Add("null", " r47");
            Estado200.Add("StmtBlock", "95");
            Estado200.Add("Stmt", "202");
            Estado200.Add("Expr'", "88");
            Estado200.Add("Expr", "96");
            Estado200.Add("ExprA", "98");
            Estado200.Add("ExprB", "99");
            Estado200.Add("ExprC", "100");
            Estado200.Add("ExprD", "101");
            Estado200.Add("ExprE", "102");
            Estado200.Add("ExprF", "103");
            Estado200.Add("ExprG", "105");
            Estado200.Add("LValue", "108");
            Estado200.Add("Constant", "109");
            Estados.Add(200, Estado200);
            Estados.Add(2000, Estado200_);

            Dictionary<string, string> Estado201 = new Dictionary<string, string>();
            Estado201.Add("ident", "r53");
            Estado201.Add(";", "r53");
            Estado201.Add("{", "r53");
            Estado201.Add("}", "r53");
            Estado201.Add("(", "r53");
            Estado201.Add("if", "r53");
            Estado201.Add("while", "r53");
            Estado201.Add("for", "r53");
            Estado201.Add("break", "r53");
            Estado201.Add("return", "r53");
            Estado201.Add("Console", "r53");
            Estado201.Add("else", "r53");
            Estado201.Add("&&", "r53");
            Estado201.Add("*", "r53");
            Estado201.Add("!", "r53");
            Estado201.Add("New", "r53");
            Estado201.Add("this", "r53");
            Estado201.Add("-", "r53");
            Estado201.Add("intConstant", "r53");
            Estado201.Add("doubleConstant", "r53");
            Estado201.Add("boolConstant", "r53");
            Estado201.Add("stringConstant", "r53");
            Estado201.Add("null", "r53");
            Estados.Add(201, Estado201);

            Dictionary<string, string> Estado202 = new Dictionary<string, string>();
            Estado202.Add("ident", "r50");
            Estado202.Add(";", "r50");
            Estado202.Add("{", "r50");
            Estado202.Add("}", "r50");
            Estado202.Add("(", "r50");
            Estado202.Add("if", "r50");
            Estado202.Add("while", "r50");
            Estado202.Add("for", "r50");
            Estado202.Add("break", "r50");
            Estado202.Add("return", "r50");
            Estado202.Add("Console", "r50");
            Estado202.Add("else", "r50");
            Estado202.Add("&&", "r50");
            Estado202.Add("*", "r50");
            Estado202.Add("!", "r50");
            Estado202.Add("New", "r50");
            Estado202.Add("this", "r50");
            Estado202.Add("-", "r50");
            Estado202.Add("intConstant", "r50");
            Estado202.Add("doubleConstant", "r50");
            Estado202.Add("boolConstant", "r50");
            Estado202.Add("stringConstant", "r50");
            Estado202.Add("null", "r50");
            Estados.Add(202, Estado202);

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
                    linea = token_actual.numLinea;
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

                            case "#ACC": // Aceptacion
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
                        Console.WriteLine("Error en linea " + linea + " se esperaba [" + StrToken(token_actual) + "]");
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
                if (action == "#ACC")
                    return "#ACC";
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