using System;
using System.Collections.Generic;
using System.Text;

namespace minic
{
    class Producción
    {
        public Producción(int ID, int cantSimbolos, string productor, string[] Simbolos)
        {
            this.ID = ID;
            this.cantSimbolos = cantSimbolos;
            this.productor = productor;
            this.Simbolos = new List<string>(Simbolos);
        }
        public int ID { get; set; }
        public int cantSimbolos { get; set; }
        public string productor { get; set; }
        public List<string> Simbolos { get; set; }
    }
}
