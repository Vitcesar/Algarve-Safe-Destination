using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Idioma
    {
        public string Codigo_Idioma { get; set; }
        public string Nome { get; set; }

        public Idioma()
        {
            //empty constructor
        }

        public Idioma(string Codigo_Idioma, string Nome)
        {
            this.Codigo_Idioma = Codigo_Idioma;
            this.Nome = Nome;
        }
    }
}
