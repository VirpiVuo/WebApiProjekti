using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjekti.Models;

namespace WebApiProjekti.Controllers
{
    [Route("nw/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //käytetään tietokannan alustukseen depedency injektiota
        private readonly NorthwindContext db;

        //luodaan metodi jossa asetetaan tyhjäksi alustettuun db -olioon ohjelman ajosta tuleva arvo joka program -tiedostossa injektoidaan tänne
        public ProductsController(NorthwindContext dbparam)
        {
            db = dbparam;
        }
        [HttpGet]
        [Route("")] //jos haluaisi laittaa jonkin reitin tai avaintiedon, sen voisi laittaa tähän
        public List<Product> HaeKaikkiTuotteet()
        {
            NorthwindContext tk = new NorthwindContext(); //tk- niminen olio joka ajaa tietokannan asiaa
            List<Product> tuotteet = tk.Products.ToList();
            return tuotteet;
        }
        [HttpGet]
        [Route("{id}")]
        public Product HaeTuote(int id) //määritetään metodille parametriksi string-muotoinen muuttuja id
        {
            NorthwindContext tk = new NorthwindContext(); //tk- niminen olio joka ajaa tietokannan asiaa
            Product tuote = tk.Products.Find(id);
            return tuote;
        }
        [HttpGet]
        [Route("supplierid/{key}")] //tärkeä määritellä reitti jottei sekoitu (ilman supplierid/ -reittiä reititin ei tietäisi kumpaan mennä)
        public List<Product> HaeJoitainTuotteita(string key) //lista -metodi jolle annetaan parametriksi avaintieto (joka määritellään myöhemmin aaltosulkeiden sisällä)
        {
            NorthwindContext tk = new NorthwindContext(); //tk -olio joka pitää paikkaa tietokannalle

            var JotkinTuotteet = from a in tk.Products //haetaan muuttujaan dataa LINQ- kyselyllä Products -tietokannasta
                                  where a.SupplierId.ToString() == key //määritellään että key -tieto on Products -tietokannan SupplierId
                                  select a; //valitaan haetut

            return JotkinTuotteet.ToList(); //palautetaan näkymään tietokannasta haettu data listana

        }
        [HttpPost] //UUDEN LISÄYS
        [Route("")]
        public ActionResult PostLisaaUusiTuote([FromBody] Product uusiTuote) //tarvitaan Actionresult-tyyppinen metodi koska käytetään catchissa BadRequestia
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try //try & catch 
            {
                db.Products.Add(uusiTuote); //lisätään Customers -tauluun Post-pyynnöllä HTML:n Bodysta JSON-tyyppinen objekti
                db.SaveChanges();
                return Ok(uusiTuote.ProductId); //palautetaan frontille customerId = voi varmistaa että uuden lisäys meni oikein
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tuotetta lisättäessä, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpPut] //PÄIVITYS/ MUOKKAUS
        [Route("{key}")] //routemääritys asiakasavaimelle, key= ProductId
        public ActionResult PutEditProduct(int key, [FromBody] Product paivitettyTuote)
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try
            {
                Product tuote = db.Products.Find(key); //katsotaan löytyykö päivitettävä tuote tietokannasta
                if (tuote != null)
                {
                    tuote.ProductName = paivitettyTuote.ProductName; //sijoitetaan kannassa olevaan olioon uuden paivitettyTuote-olion arvot
                    tuote.SupplierId = paivitettyTuote.SupplierId;
                    tuote.UnitPrice = paivitettyTuote.UnitPrice;
                    tuote.UnitsInStock = paivitettyTuote.UnitsInStock;
                    tuote.UnitsOnOrder = paivitettyTuote.UnitsOnOrder;
                    tuote.ReorderLevel = paivitettyTuote.ReorderLevel;
                    tuote.Discontinued = paivitettyTuote.Discontinued;

                    db.SaveChanges();
                    return Ok(tuote.ProductId); //palautetaan kannasta tuotteen avaintieto
                }
                else
                {
                    return NotFound("Päivitettävää tuotetta ei löydy!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tuotetta päivitettäessä, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpDelete] //POISTO
        [Route("{key}")] //routemääritys asiakasavaimelle jotta poistetaan varmasti oikeat tiedot
        public ActionResult DeleteProduct(int key)
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try
            {
                Product tuote = db.Products.Find(key); //katsotaan löytyykö poistettava tuote tietokannasta
                if (tuote != null)
                {
                    db.Products.Remove(tuote); //poistetaan kyseisen asiakkaan tiedot kannasta
                    db.SaveChanges();
                    return Ok("Tuote " + key + " poistettiin onnistuneesti!");
                }
                else
                {
                    return NotFound("Tuotetta " + key + " ei löydy!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tuotetta poistettaessa, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
