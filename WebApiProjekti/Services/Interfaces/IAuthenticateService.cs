using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjekti.Models;

namespace WebApiProjekti.Services.Interfaces
{
    public interface IAuthenticateService
    {
        //metodi, joka palauttaa LoggedUser -tyyppisen olion
        //ottaa vastaan kaksi parametria jotka molemmat ovat merkkijonoja (username, password)
        LoggedUser Authenticate(string username, string password);
    
        //antamalla parametrit kutsutaan luodun rajapinnan kautta Authenticate -metodiaS
    }
}
