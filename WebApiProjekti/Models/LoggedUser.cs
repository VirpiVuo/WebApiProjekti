using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProjekti.Models
{
    public class LoggedUser
    {
        //luodut luokat eivät edusta tietokantatauluja vaan dataa, jota halutaan tietyssä tilanteessa kuljettaa
        //edustaa dataa joka lähetetään backendistä frontendiin kun kirjautuminen onnistuu (eli ei palauteta kaikkia Userin tietoja)
        public string Username { get; set; }
        public int AccessLevelId { get; set; }
        public string? Token { get; set; }
    }
}
