using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Pais
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int rowid { get; set; }
        public string Codigo_Pais{ get; set; }
        public string Nome { get; set; }

        public Pais()
        {
            //empty constructor
        }

        public Pais(int rowid, string Codigo_Pais, string Nome)
        {
            this.rowid = rowid;
            this.Codigo_Pais = Codigo_Pais;
            this.Nome = Nome;
        }
    }
}
