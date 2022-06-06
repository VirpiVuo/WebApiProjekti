using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApiProjekti.Models;

namespace WebApiProjekti.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        //käytetään tietokannan alustukseen depedency injektiota
        private readonly NorthwindContext db;

        //luodaan metodi jossa asetetaan tyhjäksi alustettuun db -olioon ohjelman ajosta tuleva arvo joka program -tiedostossa injektoidaan tänne
        public DocumentationController(NorthwindContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet] //TAPA 1: Palauta Documentation -taulun data kannasta, jollei ole niin palauta virheilmoitus
        [Route("{key}")] //avaintieto
        public ActionResult GetDocumentation(string key)
        {
            NorthwindContext context = new NorthwindContext();

            List<Documentation> DocumentationList = (from d in context.Documentations
                                                     where d.Keycode == key
                                                     select d).ToList();
            if (DocumentationList.Count > 0) //jos listalla on enemmän kuin 0 kappaletta palautetaan listan sisältö
            {
                return Ok(DocumentationList);
            }
            else //muuten palautetaan BadRequest
            {
                return BadRequest("Sinulla ei ole pääsyä kyseisiin tietoihin, päiväys " + DateTime.Now.ToString());
            }
        }
        
        //[HttpGet]  //TAPA 2: Palauta Documentation -taulun data kannasta, jollei ole niin palauta virheilmoitus
        //[Route("{key}")]
        //public List<PublicDocument> Get(string key)
        //{
        //    NorthwindContext context = new NorthwindContext();

        //    List<Documentation> privateDocList = (from d in context.Documentations //luodaan kantaluokan mukainen oliolista johon haetaan LINQ-kyselyllä haluttua dataa tietokannasta
        //                                          where d.Keycode == key
        //                                          select d).ToList();
        //    //return PrivateDocList;

        //    //tässä välissä voisi olla muuta koodia jolla esim haetaan kannan muista tauluista haluttuja tietoja

        //    List<PublicDocument> publicDocList = new List<PublicDocument>(); //luodaan itsetehdyn PublicDocument-luokkamäärityksen mukainen uusi lista

        //    foreach (Documentation privateDoc in privateDocList) //käydään silmukassa läpi kannasta tullutta datalistaa
        //    {
        //        PublicDocument publicDoc = new PublicDocument(); //joka kierroksella luodaan uusi PublicDocument-luokan mukainen olio nimeltään publicDoc
        //        publicDoc.DocumentationId = privateDoc.DocumentationId; //sijoitetaan kannan puolelta arvoja itsemääritetyn luokan mukaisiin olioihin
        //        publicDoc.AvailableRoute = privateDoc.AvailableRoute;
        //        publicDoc.Method = privateDoc.Method;
        //        publicDoc.Description = privateDoc.Description;
        //        publicDocList.Add(publicDoc); //lisätään olio luotuun PublicDocument -luokan mukaiseen listaan nimeltä publicDocList
        //    }
        //    if (publicDocList.Count == 0) //jos jostain syystä rivejä ei olisikaan saatu vietyä listalle yhtään 
        //    {
        //        PublicDocument publiDoc = new PublicDocument(); //luodaan silti listalle yksi olio seuraavilla arvoilla
        //        publiDoc.DocumentationId = 0;
        //        publiDoc.AvailableRoute = DateTime.Now.ToString();
        //        publiDoc.Method = "Documentation missing!";
        //        publiDoc.Description = "Empty";
        //        publicDocList.Add(publiDoc); //lisätään olio luodulle listalle
        //    }

        //    return publicDocList; //palautetaan itseluodun PublicDocument -luokan mukainen lista, joko silmukan mukainen jolloin kannasta löytyy dataa tai pelkkä yksi alkio/olio jollei dataa löydy

        //}
    }
}
