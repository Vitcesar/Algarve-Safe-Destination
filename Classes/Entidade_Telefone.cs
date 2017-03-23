using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{
    public class Entidade_Telefone
    {
        public int ID_Entidade { get; set; }
        public int Telefone { get; set; }

        public Entidade_Telefone()
        {
            //empty constructor
        }
        public Entidade_Telefone(int ID_Entidade, int Telefone)
        {
            this.ID_Entidade = ID_Entidade;
            this.Telefone = Telefone;
        }
    }
}
