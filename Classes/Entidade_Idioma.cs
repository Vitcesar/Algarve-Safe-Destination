using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Entidade_Idioma {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public string Nome_Entidade { get; set; }
        public string Codigo_Postal_Entidade { get; set; }
        public string Codigo_Idioma{ get; set; }
        public string Nome_Idioma { get; set; }

        public Entidade_Idioma()
        {
            //empty constructor
        }

        public Entidade_Idioma(string Nome_Entidade, string Codigo_Postal_Entidade, string Codigo_Idioma, string Nome_Idioma)
        {
            this.Nome_Entidade = Nome_Entidade;
            this.Codigo_Postal_Entidade = Codigo_Postal_Entidade;
            this.Codigo_Idioma = Codigo_Idioma;
            this.Nome_Idioma = Nome_Idioma;
        }
    }
}
