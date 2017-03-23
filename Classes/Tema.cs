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
    public class Tema
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Visibilidade { get; set; }
        public string Icone { get; set; }

        public Tema()
        {
            //empty constructor
        }
        public Tema(string Titulo, string Descricao, string Visibilidade, string Icone)
        {
            this.Titulo = Titulo;
            this.Descricao = Descricao;
            this.Visibilidade = Visibilidade;
            this.Icone = Icone;
        }

    }
}
