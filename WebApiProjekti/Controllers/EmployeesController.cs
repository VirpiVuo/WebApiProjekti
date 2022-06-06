using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjekti.Models;

namespace WebApiProjekti.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("nw/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //käytetään tietokannan alustukseen depedency injektiota
        private readonly NorthwindContext db;

        //luodaan metodi jossa asetetaan tyhjäksi alustettuun db -olioon ohjelman ajosta tuleva arvo joka program -tiedostossa injektoidaan tänne
        public EmployeesController(NorthwindContext dbparam)
        {
            db = dbparam;
        }
        [HttpGet]
        [Route("")] //jos haluaisi laittaa jonkin reitin tai avaintiedon, sen voisi laittaa tähän
        public List<Employee> HaeKaikkiTyontekijat()
        {
            NorthwindContext tk = new NorthwindContext(); //tk- niminen olio joka ajaa tietokannan asiaa
            List<Employee> tyontekijat = tk.Employees.ToList();
            return tyontekijat;
        }
        [HttpGet]
        [Route("{id}")]
        public Employee HaeTyontekija(int id) //määritetään metodille parametriksi string-muotoinen muuttuja id
        {
            NorthwindContext tk = new NorthwindContext(); //tk- niminen olio joka ajaa tietokannan asiaa
            Employee tyontekija = tk.Employees.Find(id);
            return tyontekija;
        }
        [HttpGet]
        [Route("country/{key}")] 
        public List<Employee> HaeJoitainTyontekijoita(string key) //lista -metodi jolle annetaan parametriksi avaintieto (joka määritellään myöhemmin aaltosulkeiden sisällä)
        {
            NorthwindContext tk = new NorthwindContext(); //tk -olio joka pitää paikkaa tietokannalle

            var JotkinTyontekijat = from t in tk.Employees //haetaan muuttujaan dataa LINQ- kyselyllä Employees -tietokannasta
                                 where t.Country == key //määritellään että key -tieto on Employees -tietokannan Country
                                 select t; //valitaan haetut

            return JotkinTyontekijat.ToList(); //palautetaan näkymään tietokannasta haettu data listana

        }
        [HttpPost] //UUDEN LISÄYS
        [Route("")]
        public ActionResult PostLisaaUusiTyontekija([FromBody] Employee uusiTyontekija) //tarvitaan Actionresult-tyyppinen metodi koska käytetään catchissa BadRequestia
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try //try & catch 
            {
                db.Employees.Add(uusiTyontekija); //lisätään Customers -tauluun Post-pyynnöllä HTML:n Bodysta JSON-tyyppinen objekti
                db.SaveChanges();
                return Ok(uusiTyontekija.EmployeeId); //palautetaan frontille EmployeeId = voi varmistaa että uuden lisäys meni oikein
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen uutta työntekijää lisättäessä, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpPut] //PÄIVITYS
        [Route("{key}")] //routemääritys asiakasavaimelle, key= EmployeeId
        public ActionResult PutEditEmployee(int key, [FromBody] Employee paivitettyTyontekija)
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try
            {
                Employee tyontekija = db.Employees.Find(key); //katsotaan löytyykö päivitettävä työntekijä tietokannasta
                if (tyontekija != null)
                {
                    tyontekija.LastName = paivitettyTyontekija.LastName; //sijoitetaan kannassa olevaan olioon uuden paivitettyAsiakas-olion arvot
                    tyontekija.FirstName = paivitettyTyontekija.FirstName;
                    tyontekija.Title = paivitettyTyontekija.Title;
                    tyontekija.TitleOfCourtesy = paivitettyTyontekija.TitleOfCourtesy;
                    tyontekija.Address = paivitettyTyontekija.Address;
                    tyontekija.City = paivitettyTyontekija.City;
                    tyontekija.Country = paivitettyTyontekija.Country;

                    db.SaveChanges();
                    return Ok(tyontekija.EmployeeId); //palautetaan kannasta työntekijän avaintieto
                }
                else
                {
                    return NotFound("Päivitettävää työntekijää ei löydy!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen työntekijän tietoja päivitettäessä, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpDelete] //POISTO
        [Route("{key}")] //routemääritys asiakasavaimelle jotta poistetaan varmasti oikeat tiedot
        public ActionResult DeleteEmployee(int key)
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try
            {
                Employee tyontekija = db.Employees.Find(key); //katsotaan löytyykö poistettava työntekijä tietokannasta
                if (tyontekija != null)
                {
                    db.Employees.Remove(tyontekija); //poistetaan kyseisen asiakkaan tiedot kannasta
                    db.SaveChanges();
                    return Ok("Työntekijä " + key + " poistettiin onnistuneesti!");
                }
                else
                {
                    return NotFound("Työntekijää " + key + " ei löydy!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen työntekijää poistettaessa, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
