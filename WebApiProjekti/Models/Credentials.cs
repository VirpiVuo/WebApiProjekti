using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProjekti.Models
{
    public class Credentials
    {
        //tätä luokkaa/ tämäntyyppinen olio tulee sisään kun yritetään kirjautua 
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
