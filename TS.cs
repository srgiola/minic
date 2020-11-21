using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace minic
{
    class TS
    {
        List<Token> Tokens = new List<Token>();
        List<ObjetoTS> LTS = new List<ObjetoTS>();
        public TS(List<Token> Tokens)
        {
            this.Tokens = Tokens;
        }

        public void Iniciar()
        {
            CrearOTS();
            CrearOTS2();
        }

        void CrearOTS() //Se realiza una primera leida de los tokens
        {
            var Tokens2 = Tokens;
            for(int i = 0; i < Tokens2.Count; i++)
            {
                var item = Tokens2[i];
                ObjetoTS OTS = item.crearTS(item);
                if (OTS != null && OTS.caso == 0) //CREAR EL ITEM CUANDO ES UNA DE DECLARACTIÓN DE VARIABLE O FUNCIÓN
                {
                    i++;
                    OTS.ident = Tokens2[i].identOTS(Tokens2[i]);
                    if (OTS.ident != null) //Debe siempre ident o []
                    {
                        i++;

                        while (Tokens2[i].content == "[]")
                            i++;

                        //Si contiene parentesis luego es por que es 
                        //FuctionDecl - Prototype
                        if (Tokens2[i].type == "Operador" && Tokens2[i].content == "(")
                        {
                            i++;
                            while (Tokens2[i].type != "Operador" && Tokens2[i].content != ")")
                            {
                                var item2 = Tokens2[i];
                                ObjetoTS OTS2 = item2.crearTS(item2);
                                if (OTS2 != null)
                                {
                                    i++;
                                    OTS2.ident = Tokens2[i].identOTS(Tokens2[i]);
                                    if (OTS2.ident != null) //Debe siempre ident o []
                                    {
                                        i++;
                                        while (Tokens2[i].content == "[]")
                                            i++;

                                        if (Tokens2[i].content == ",")
                                            i++;
                                    }
                                }
                                OTS.atributos.Add(OTS2);
                            }
                        }

                        LTS.Add(OTS);
                    }
                }
                else if (OTS != null && OTS.caso == 1)
                {
                    i++;
                    OTS.ident = Tokens2[i].identOTS(Tokens2[i]);
                    if (OTS.ident != null) //Debe siempre ident o []
                    {
                        i++;

                        //Si contiene parentesis luego es por que es 
                        //FuctionDecl - Prototype
                        if (Tokens2[i].type == "Operador" && Tokens2[i].content == "(")
                        {
                            i++;
                            while (Tokens2[i].type != "Operador" && Tokens2[i].content != ")")
                            {
                                var item2 = Tokens2[i];
                                ObjetoTS OTS2 = item2.crearTS(item2);
                                if (OTS2 != null)
                                {
                                    i++;
                                    OTS2.ident = Tokens2[i].identOTS(Tokens2[i]);
                                    if (OTS2.ident != null) //Debe siempre ident o []
                                    {
                                        i++;

                                        if (Tokens2[i].content == ",")
                                            i++;
                                    }
                                }
                                OTS.atributos.Add(OTS2);
                            }
                        }

                        LTS.Add(OTS);
                    }
                }
                else if (OTS != null && OTS.caso == 2) //Class
                {
                    i++;
                    OTS.ident = Tokens2[i].identOTS(Tokens2[i]);
                    if (OTS.ident != null) //Debe siempre ident
                    {
                        OTS.tipo = "class";
                        i++;
                        if (Tokens2[i].type == "Operador" && Tokens2[i].content == ":")
                        {
                            i++;
                            if (Tokens2[i].type == "Identificador")
                            {
                                OTS.ident += " : " + Tokens2[i].content;
                                i++;
                            }
                        }
                        else if (Tokens2[i].type == "Operador" && Tokens2[i].content == ",")
                        {
                            i++;

                            while (Tokens2[i].content != "{")
                            {
                                if (Tokens2[i].content == ",")
                                    i++;
                                OTS.ident += " , " + Tokens2[i].content;
                                i++;
                            }
                        }
                    }
                    LTS.Add(OTS);
                }
                else if (OTS != null && OTS.caso == 3) //interface
                {
                    i++;
                    OTS.ident = Tokens2[i].content;
                    OTS.tipo = "interface";
                    LTS.Add(OTS);
                }
            }
        }

        void CrearOTS2() //Se realiza una segunda leida para crear tokens faltas debido a que son objetos
        {
            var Tokens2 = Tokens;
            for (int i = 0; i < Tokens2.Count; i++)
            {
                var item = Tokens2[i];
                ObjetoTS OTS = new ObjetoTS();
                OTS.ident = Tokens2[i].identOTS(Tokens2[i]);
                if (ExisteOTS(Tokens2[i]) && OTS.ident != null)
                {
                    
                }
            }
        }

        bool ExisteOTS(Token tk)
        {
            ObjetoTS OTS = new ObjetoTS();
            ObjetoTS tmp = new ObjetoTS();
            //PRIMERO SE REVISA SI EXISTE EN ATRIBUTOS
            var LTSA = LTS.Find(x => (x.tipo == "class" && x.ident == tk.content) || (x.tipo == "inteface" && x.ident == tk.content));

            if (LTSA != null)
            {
                return true;
            }
            else
                return false;
        }

        public void pritTS(string pathDirectory)
        {
            try
            {
                string path = pathDirectory + "TS.csv";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine("Ident;Tipo;Valor;Atributos");
                    foreach (var item in LTS)
                    {
                        string atri = "";
                        foreach (var item2 in item.atributos)
                        {
                            atri += " - " + item2.tipo + " " + item2.ident;
                        }
                        sw.WriteLine(item.ident + ";" + item.tipo + ";" + item.valor + ";" + atri);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Debe cerrar el archivo para continuar, inicie de nuevo");
            }
        }
    }
}
