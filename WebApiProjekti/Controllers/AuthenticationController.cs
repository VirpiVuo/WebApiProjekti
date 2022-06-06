using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiProjekti.Services.Interfaces;
using WebApiProjekti.Models;

namespace WebApiProjekti.Controllers
{
    [Route("nw/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        // tähän tulee front endin kirjautumisyritys
        [HttpPost]
        public ActionResult Post([FromBody] Credentials tunnukset)
        {
            var loggedUser = _authenticateService.Authenticate(tunnukset.UserName, tunnukset.Password);

            if (loggedUser == null)
                return BadRequest(new { message = "KÄyttäjätunnus tai salasana on virheellinen!" });

            return Ok(loggedUser); //palautus front endiin (sisältää vain loggedUser- luokan mukaiset kentät
            
        }
    }
}
