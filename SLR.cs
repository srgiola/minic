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
        Stack<string> PilaTokens = new Stack<string>();
        Dictionary<int, Dictionary<string, string>> Estados = new Dictionary<int, Dictionary<string, string>>();
        //TKey = Numero estado
        //TValue = Dicionario con  TKey = Simbolo/Goto, TValue = acción

        //DEBIDO A QUE EL SIMBOLO '$' PUEDE VENIR INGRESADO COMO UN TOKEN STRING SE HA CAMBIADO EL SIMBOLO FINAL A ESTA COMBINACIÓN '$/#'
        //ASI MISMO 'ACC' O 'ACEPTAR' A SERA UTILIZADO LA COMBINACIÓN '/ACC'

        public SLR(List<Token> Tokens)
        {
            this.Tokens = Tokens;
            Token t_dollar = new Token("PR", "$/#", 0);
            this.Tokens.Add(t_dollar);

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
                    string token_actual = Tokens[0].content;
                    string action = "";

                    AuxBool = AuxDicc.TryGetValue(token_actual, out action);

                    if (proviene_de_R) //IR_A  -- Ya que siempre que se ejecuta un r# se realiza un Ir_A luego
                    {
                        string token_Cima = PilaTokens.First();
                        string tmp_Estado_Actual;
                        bool tmpR = AuxDicc.TryGetValue(token_Cima, out tmp_Estado_Actual);

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
                                for (int i = 0; i < P.cantSimbolos; i++)
                                {
                                    PilaEstados.Pop();
                                    PilaTokens.Pop();
                                }
                                PilaTokens.Push(P.productor);
                                estado_Actual = int.Parse(PilaEstados.First());
                                //estado_Actual = int.Parse(matchRgx);
                                proviene_de_R = true;
                                break;

                            case "D": //Desplazamiento
                                string token_cargar = Tokens[0].content;
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
        }
        private void Consumir()
        {
            Tokens.RemoveAt(0);
        }
    }
}
