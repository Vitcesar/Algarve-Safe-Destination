using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Tipo_Entidade {
    [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public string Nome{ get; set; }

        public Tipo_Entidade()
        {
            //empty constructor
        }

        public Tipo_Entidade(string Nome)
        {
            this.Nome = Nome;
        }
    }
}
