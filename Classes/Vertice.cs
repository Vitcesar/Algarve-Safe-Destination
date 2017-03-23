using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Vertice
    {
        //The Id property is marked as the Primary Key
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int N_Vertice { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Vertice()
        {
            //empty constructor
        }
        public Vertice(int N_Vertice, double Latitude, double Longitude)
        {
            this.N_Vertice = N_Vertice;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }
    }
}
