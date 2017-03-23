using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ADS.Classes
{
    public class Conteudo_Tema
    {
        public string ID_Tema { get; set; }
        public string Codigo_Idioma { get; set; }
        public string Titulo { get; set; }
        public string Visibilidade { get; set; }
        public string Documento { get; set; }

        public Tema parent_tema { get; set; }
        //public BitmapImage img { get; set; }

        public Conteudo_Tema()
        {
            //empty constructor
        }
        public Conteudo_Tema(string ID_Tema, string Codigo_Idioma, string Titulo, string Visibilidade, string Documento)
        {
            this.ID_Tema = ID_Tema;
            this.Codigo_Idioma = Codigo_Idioma;
            this.Titulo = Titulo;
            this.Visibilidade = Visibilidade;
            this.Documento = Documento;
        }
    }
}
