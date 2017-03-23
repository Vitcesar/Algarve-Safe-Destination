using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Tipo_Entidade_Idioma
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public string Codigo_Idioma{ get; set; }
        public string Nome_Tipo_Entidade { get; set; }
        public string Nome_Idioma { get; set; }

        public Tipo_Entidade_Idioma()
        {
            //empty constructor
        }

        public Tipo_Entidade_Idioma(string Codigo_Idioma, string Nome_Tipo_Entidade, string Nome_Idioma)
        {
            this.Codigo_Idioma = Codigo_Idioma;
            this.Nome_Tipo_Entidade = Nome_Tipo_Entidade;
            this.Nome_Idioma = Nome_Idioma;
        }
    }
}
