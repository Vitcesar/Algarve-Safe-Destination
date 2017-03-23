using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Conteudo_Subtopico
    {
        public string ID_Tema { get; set; }
        public string ID_Topico { get; set; }
        [SQLite.PrimaryKey]
        public string ID_Subtopico { get; set; }
        public string Codigo_Idioma { get; set; }
        public string Titulo { get; set; }
        public string Visibilidade { get; set; }
        public string Documento { get; set; }

        public Subtopico parent_subtopico { get; set; }

        public Conteudo_Subtopico()
        {
            //empty constructor
        }
        public Conteudo_Subtopico(string ID_Tema, string ID_Topico, string ID_Subtopico, string Codigo_Idioma, string Titulo, string Visibilidade, string Documento)
        {
            this.ID_Tema = ID_Tema;
            this.ID_Topico = ID_Topico;
            this.ID_Subtopico = ID_Subtopico;
            this.Codigo_Idioma = Codigo_Idioma;
            this.Titulo = Titulo;
            this.Visibilidade = Visibilidade;
            this.Documento = Documento;
        }
    }
}
