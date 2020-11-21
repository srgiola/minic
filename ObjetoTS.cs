using System;
using System.Collections.Generic;
using System.Text;

namespace minic
{
    public class ObjetoTS
    {
        public string ident { get; set; }
        public string tipo { get; set; }
        public string valor { get; set; }
        public int caso { get; set; } //Variable de control interna para TS

        public List<ObjetoTS> atributos = new List<ObjetoTS>();
    }
}
