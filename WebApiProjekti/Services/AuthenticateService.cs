using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProjekti.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebApiProjekti.Services.Interfaces;

namespace WebApiProjekti.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        //alustetaan tietokanta Dependency Injection -tyylillä
        private readonly NorthwindContext db;

        //alustetaan AppSettings -luokan mukainen tyhjä olio johon AuthenticateService -metodissa asetetaan arvo dependency injektiolla
        private readonly AppSettings _appSettings;
        public AuthenticateService(IOptions<AppSettings> appSettings, NorthwindContext nwc)
        {
            _appSettings = appSettings.Value;
            db = nwc;
        }

        //metodin paluutyyppi on LoggedUser- luokan mukainen olio (kysymysmerkillä siksi että jollei LoggedUser -luokan mukaista oliota löydy, palauttaa nullin)
        public LoggedUser? Authenticate(string username, string password)
        {
            //tarkistus tietokannasta, löytyykö annetuilla käyttäjätunnuksella ja salasanalla käyttäjää tietokannasta
            var foundUser = db.Users.SingleOrDefault(x => x.UserName == username && x.Password == password);

            //jos käyttäjää ei löydy palautetaan null:
            if (foundUser == null)
            {
                return null;
            }
            //jos käyttäjä löytyy:
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, foundUser.UserId.ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Version, "V3.1")
                }),
                Expires = DateTime.UtcNow.AddDays(1), //montako päivää token on voimassa

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoggedUser loggedUser = new LoggedUser();

            loggedUser.Username = foundUser.UserName;
            loggedUser.AccessLevelId = foundUser.AccessLevelId;
            loggedUser.Token = tokenHandler.WriteToken(token);

            return loggedUser; //palautetaan kutsuvalle controller metodille user ilman salasanaa
        }
    }
}
