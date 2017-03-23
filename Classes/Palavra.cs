using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Palavra
    {
        public int Codigo_Frase { get; set; }
        public string Frase{ get; set; }
        public int Codigo_Ascendente { get; set; }
        public string Codigo_Idioma { get; set; }

        public Palavra()
        {
            //empty constructor
        }

        public Palavra(int Codigo_Frase, string Frase, int Codigo_Ascendente, string Codigo_Idioma)
        {
            this.Codigo_Frase = Codigo_Frase;
            this.Frase = Frase;
            this.Codigo_Ascendente = Codigo_Ascendente;
            this.Codigo_Idioma = Codigo_Idioma;
        }
    }
}
