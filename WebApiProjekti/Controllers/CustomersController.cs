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
    public class CustomersController : ControllerBase
    {
        //käytetään tietokannan alustukseen depedency injektiota
        private readonly NorthwindContext db;

        //luodaan metodi jossa asetetaan tyhjäksi alustettuun db -olioon ohjelman ajosta tuleva arvo joka program -tiedostossa injektoidaan tänne
        public CustomersController(NorthwindContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        [Route("")] //jos haluaisi laittaa jonkin reitin tai avaintiedon, sen voisi laittaa tähän
        public List<Customer> HaeKaikkiAsiakkaat()
        {
            NorthwindContext tk = new NorthwindContext(); //tk- niminen olio joka ajaa tietokannan asiaa
            List<Customer> asiakkaat = tk.Customers.ToList();
            return asiakkaat;
        }
        [HttpGet]
        [Route("{id}")]
        public Customer HaeAsiakas(string id) //määritetään metodille parametriksi string-muotoinen muuttuja id
        {
            NorthwindContext tk = new NorthwindContext(); //tk- niminen olio joka ajaa tietokannan asiaa
            Customer asiakas = tk.Customers.Find(id); //luodaan Customer- luokan mukainen olio nimeltä asiakas johon etsitään tietyn id:n tiedot
            //Find- metodi hakee aina vain pääavaimella yhden rivin!!
            return asiakas;
        }
        [HttpGet]
        [Route("country/{key}")] //tärkeä määritellä reitti jottei sekoitu (ilman country/ -reittiä reititin ei tietäisi kumpaan mennä)
        public List<Customer> HaeJoitainAsiakkaita(string key) //lista -metodi jolle annetaan parametriksi avaintieto (joka määritellään myöhemmin aaltosulkeiden sisällä)
        {
            NorthwindContext tk = new NorthwindContext(); //tk -olio joka pitää paikkaa tietokannalle

            var JotkinAsiakkaat = from a in tk.Customers //haetaan muuttujaan dataa LINQ- kyselyllä Customers -tietokannasta
                                  where a.Country == key //määritellään että key -tieto on Customers -tietokannan Country
                                  select a; //valitaan haetut

            return JotkinAsiakkaat.ToList(); //palautetaan näkymään tietokannasta haettu data listana

            //eli kun reittiin kirjoitetaan esim ..country/Finland, saa jäjrestelmä tiedon string -muotoiseen olioon key ja 
            //suorittaa metodin mukaisen haun tietokannasta -> näkymään palautetaan listamuotoisen kaikki suomalaiset 
            //asiakkaat tietokannasta Customers -taulusta
        }
        [HttpPost] //UUDEN L ISÄYS
        [Route("")]
        public ActionResult PostLisaaUusi([FromBody] Customer uusiAsiakas) //tarvitaan Actionresult-tyyppinen metodi koska käytetään catchissa BadRequestia
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try //try & catch 
            {
                db.Customers.Add(uusiAsiakas); //lisätään Customers -tauluun Post-pyynnöllä HTML:n Bodysta JSON-tyyppinen objekti
                db.SaveChanges();
                return Ok(uusiAsiakas.CustomerId); //palautetaan frontille customerId = voi varmistaa että uuden lisäys meni oikein
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen asiakasta lisättäessä, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {

            }
        }
        [HttpPut] //PÄIVITYS
        [Route("{key}")] //routemääritys asiakasavaimelle, key= CustomerId
        public ActionResult PutEdit(string key, [FromBody] Customer paivitettyAsiakas)
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try
            {
                Customer asiakas = db.Customers.Find(key); //katsotaan löytyykö päivitettävä asiakas tietokannasta
                if (asiakas != null)
                {
                    asiakas.CompanyName = paivitettyAsiakas.CompanyName; //sijoitetaan kannassa olevaan olioon uuden paivitettyAsiakas-olion arvot
                    asiakas.ContactName = paivitettyAsiakas.ContactName;
                    asiakas.ContactTitle = paivitettyAsiakas.ContactTitle;
                    asiakas.Country = paivitettyAsiakas.Country;
                    asiakas.Address = paivitettyAsiakas.Address;
                    asiakas.City = paivitettyAsiakas.City;
                    asiakas.PostalCode = paivitettyAsiakas.PostalCode;
                    asiakas.Phone = paivitettyAsiakas.Phone;

                    db.SaveChanges();
                    return Ok(asiakas.CustomerId); //palautetaan kannasta asiakkaan avaintieto
                }
                else
                {
                    return NotFound("Päivitettävää asiakasta ei löydy!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen asiakasta päivitettäessä, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpDelete] //POISTO
        [Route("{key}")] //routemääritys asiakasavaimelle jotta poistetaan varmasti oikeat tiedot
        public ActionResult DeleteCustomer (string key)
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try
            {
                Customer asiakas = db.Customers.Find(key); //katsotaan löytyykö päivitettävä asiakas tietokannasta
                if (asiakas != null)
                {
                    db.Customers.Remove(asiakas); //poistetaan kyseisen asiakkaan tiedot kannasta
                    db.SaveChanges();
                    return Ok("Asiakas " + key + " poistettiin onnistuneesti!");
                }
                else
                {
                    return NotFound("Asiakasta " + key + " ei löydy!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen asiakasta poistettaessa, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
