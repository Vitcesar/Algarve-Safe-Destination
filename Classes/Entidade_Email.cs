using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Classes
{

    public class Entidade_Email
    {

        public int ID_Entidade { get; set; }
        public string Email { get; set; }



        public Entidade_Email()
        {
            //empty constructor
        }
        public Entidade_Email(int ID_Entidade, string Email)
        {
            this.ID_Entidade = ID_Entidade;
            this.Email = Email;

        }
    }
}
