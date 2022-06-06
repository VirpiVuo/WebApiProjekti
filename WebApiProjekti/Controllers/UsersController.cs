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
    public class UsersController : ControllerBase
    {
        //käytetään tietokannan alustukseen depedency injektiota
        private readonly NorthwindContext db;

        //luodaan metodi jossa asetetaan tyhjäksi alustettuun db -olioon ohjelman ajosta tuleva arvo joka program -tiedostossa injektoidaan tänne
        public UsersController(NorthwindContext dbparam)
        {
            db = dbparam;
        }

        //kaikkien listaaminen
        [HttpGet]
        public ActionResult GetAll()
        {
            var users = db.Users;

            foreach (var user in users) //nollataan jokaisen käyttäjän salasana ennen kuin näytetään listaus
            {
                user.Password = null;
            }
            return Ok(users);
        }

        //uuden lisääminen 
        [HttpPost]
        public ActionResult PostCreateNew([FromBody] User u)
        {
            try
            {
                db.Users.Add(u);
                db.SaveChanges();
                return Ok("Lisättiin käyttäjä" + u.UserName);
            }
            catch (Exception e)
            {
                return BadRequest("Käyttäjän lisääminen ei onnistunut. Lisätietoa: " + e);
            }
        }
        [HttpPut] //PÄIVITYS
        [Route("{key}")] //routemääritys asiakasavaimelle, key= UserId
        public ActionResult PutEdit(int key, [FromBody] User paivitettyKayttaja)
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try
            {
                User kayttaja = db.Users.Find(key); //katsotaan löytyykö päivitettävä asiakas tietokannasta
                if (kayttaja != null)
                {
                    kayttaja.FirstName = paivitettyKayttaja.FirstName; //sijoitetaan kannassa olevaan olioon uuden paivitettyKayttaja-olion arvot
                    kayttaja.LastName = paivitettyKayttaja.LastName;
                    kayttaja.Email = paivitettyKayttaja.Email;
                    kayttaja.AccessLevelId = paivitettyKayttaja.AccessLevelId;
                    kayttaja.UserName = paivitettyKayttaja.UserName;
                    kayttaja.Password = paivitettyKayttaja.Password;

                    db.SaveChanges();
                    return Ok(kayttaja.UserId); //palautetaan kannasta käyttäjän avaintieto
                }
                else
                {
                    return NotFound("Päivitettävää käyttäjää ei löydy!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen käyttäjää päivitettäessä, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpDelete] //POISTO
        [Route("{key}")] //routemääritys asiakasavaimelle jotta poistetaan varmasti oikeat tiedot
        public ActionResult DeleteUser(int key) 
        {
            NorthwindContext db = new NorthwindContext(); //luodaan uusi tietokantaolio
            try
            {
                User kayttaja = db.Users.Find(key); //katsotaan löytyykö poistettava käyttäjä tietokannasta
                if (kayttaja != null)
                {
                    db.Users.Remove(kayttaja); //poistetaan kyseisen käyttäjän tiedot kannasta
                    db.SaveChanges();
                    return Ok("Käyttäjä " + key + " poistettiin onnistuneesti!");
                }
                else
                {
                    return NotFound("Käyttäjää " + key + " ei löydy!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen käyttäjää poistettaessa, ota yhteyttä asiakaspalveluumme!");
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
