using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjekti.Models;

namespace WebApiProjekti.Controllers
{
    [Route("omareitti/[controller]")]
    [ApiController]
    public class HenkilotController : ControllerBase
    {
        //käytetään tietokannan alustukseen depedency injektiota
        private readonly NorthwindContext db;

        //luodaan metodi jossa asetetaan tyhjäksi alustettuun db -olioon ohjelman ajosta tuleva arvo joka program -tiedostossa injektoidaan tänne
        public HenkilotController(NorthwindContext dbparam)
        {
            db = dbparam;
        }
        //tämän ohjelmalohkon sisään koodataan kaikki metodit
        [HttpGet] //määritetään metodin verbi
        [Route("merkkijono/{nimi}")] //määritetään tarkempi reitti
        public string MerkkiJono(string nimi) //lisätään string -muotoista dataa palauttava metodi
        {
            return "Päivää " + nimi + "!";
        }
        [HttpGet] //määritetään metodin verbi
        [Route("paivamaara")] //määritetään tarkempi reitti
        public DateTime PaivaMaara() 
        {
            return DateTime.Now;
        }
        [HttpGet] //määritetään metodin verbi
        [Route("tervehdys")] //määritetään tarkempi reitti
        public string Tervehdys()
        {
            return "Heipä hei! Tänään on " + DateTime.Now;
        }

        [HttpGet]
        [Route("olio")]
        public Henkilo Olio() //määritetään Henkilot -luokan mukainen metodi
        {
            return new Henkilo() //palautetaan uusi Henkilot -luokan mukainen olio jolle kovakoodataan 
            //tietyt arvot olemassaoleviin ominaisuuksiin
            {
                Nimi = "Maija Mehiläinen", 
                Osoite = "Hunajakatu 2",
                Ika = 10
            };
        }
        [HttpGet]
        [Route("oliolista")]
        public List<Henkilo> Oliolista() //metodi joka palauttaa uuden oliolista
        {
            List<Henkilo> henkilot = new List<Henkilo>() //luodaan uusi Henkilot -luokan mukainen lista
            { 
                new Henkilo() //listan sisään lisätään uudet oliot
                {
                    Nimi = "Virpi Vuolli",
                    Osoite = "Kartanotie 1",
                    Ika = 30
                },
                new Henkilo()
                {
                    Nimi = "Ville Pekola",
                    Osoite = "Kotipolku 34",
                    Ika = 36
                },
                new Henkilo()
                {
                    Nimi = "Tiina Hirvonen",
                    Osoite = "Puomitie 455",
                    Ika = 24
                }
            };
            return henkilot; //palautetaan listan sisältö
        }
    }
}
