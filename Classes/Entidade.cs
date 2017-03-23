using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace ADS.Classes
{
    public class Entidade
    {
        //The Id property is marked as the Primary Key
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID { get; set; }
        public string Nome { get; set; }
        public int Telefone { get; set; }
        public string Morada { get; set; }
        public string Email { get; set; }
        public string Anexo { get; set; }
        public string Codigo_Postal { get; set; }
        public string Nome_Tipo_Entidade { get; set; }
        public string Codigo_Pais { get; set; }
        public int Fax { get; set; }
        public int N_Vertice { get; set; }

        public Geopoint geo { get; set; }
        public Point point_n { get; set; }
        public double dist { get; set; }
        public Boolean dest { get; set; }
        public Boolean closest_police { get; set; } //necessário

        public Entidade()
        {
            //empty constructor
        }
        public Entidade(int ID, string Nome, int Telefone, string Morada, string Email, string Anexo, string Codigo_Postal, string Nome_Tipo_Entidade, string Codigo_Pais, int Fax, int N_Vertice)
        {
            this.ID = ID;
            this.Nome = Nome;
            this.Telefone = Telefone;
            this.Morada = Morada;
            this.Email = Email;
            this.Anexo = Anexo;
            this.Codigo_Postal = Codigo_Postal;
            this.Nome_Tipo_Entidade = Nome_Tipo_Entidade;
            this.Codigo_Pais = Codigo_Pais;
            this.Fax = Fax;
            this.N_Vertice = N_Vertice;
        }
    }
}
