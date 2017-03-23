using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ADS.Classes
{
    public class Subtopico
    {
        public string ID_Tema { get; set; }
        public string ID_Topico { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Visibilidade { get; set; }
        public string Icone { get; set; }
        //public BitmapImage img { get; set; }

        public Subtopico()
        {
            //empty constructor
        }
        public Subtopico(string ID_Tema, string ID_Topico, string Titulo, string Descricao, string Visibilidade, string Icone)
        {
            this.ID_Tema = ID_Tema;
            this.ID_Topico = ID_Topico;
            this.Titulo = Titulo;
            this.Descricao = Descricao;
            this.Visibilidade = Visibilidade;
            this.Icone = Icone;
        }
    }
}
