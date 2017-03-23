using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Search_Result
    {
        public Conteudo_Tema tema { get; set; }
        public Conteudo_Topico topico { get; set; }
        public Conteudo_Subtopico subtopico { get; set; }
        public Entidade entidade { get; set; }
        public string tipo { get; set; }
        public string nametitle { get; set; }

        public Search_Result()
        {
            //empty constructor
        }

        public Search_Result(Conteudo_Tema tema, Conteudo_Topico topico, Conteudo_Subtopico subtopico, Entidade entidade, string tipo, string nametitle)
        {
            this.tema = tema;
            this.topico = topico;
            this.subtopico = subtopico;
            this.entidade = entidade;
            this.tipo = tipo;
            this.nametitle = nametitle;
        }
    }
}
