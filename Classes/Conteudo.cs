using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Conteudo
    {
        //The Id property is marked as the Primary Key
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int id { get; set; }
        public string titulo { get; set; }
        public string documento { get; set; }
        public Tema parent_tema { get; set; }
        public Topico parent_topico { get; set; }
        public Subtopico parent_subtopico { get; set; }

        public Conteudo()
        {
            //empty constructor
        }
        public Conteudo(string titulo, string documento)
        {
            this.titulo = titulo;
            this.documento = documento;
        }
    }
}
