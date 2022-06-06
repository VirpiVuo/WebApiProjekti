﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiProjekti.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Maito", "Juusto", "Voi", "Leipä", "Leikkele" };
        }

        // GET api/<ValuesController>/Virpi
        [HttpGet("{nimi}")]
        public ActionResult<string> Get(string nimi)
        {
            return "Tervehdys " + nimi + "!";
        }

        // GET api/<ValuesController>/Virpi/Vuolli
        [HttpGet("{etunimi}/{sukunimi}")]
        public ActionResult<string> Get(string etunimi, string sukunimi)
        {
            return "Hyvää iltaa " + etunimi + " " + sukunimi + "!";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
