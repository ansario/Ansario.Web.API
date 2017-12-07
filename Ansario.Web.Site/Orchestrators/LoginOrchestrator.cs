using Ansario.Web.Site.Models;
using Ansario.Web.Site.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using System.IdentityModel.Tokens.Jwt;

namespace Ansario.Web.Site.Orchestrators
{
    public class LoginOrchestrator : ILoginOrchestrator
    {
        private readonly ISiteRepository _repository;

        public LoginOrchestrator(ISiteRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Login(LoginModel model)
        {
            bool authenticated;

            var response = await _repository.Login(model.Username, model.Password);
            if (!string.IsNullOrWhiteSpace(response))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(response) as JwtSecurityToken;

                var ident = new ClaimsIdentity(
                new[] { 
                    // adding following 2 claim just for supporting default antiforgery provider
                    new Claim(ClaimTypes.NameIdentifier, token.Claims.First(claim => claim.Type == "username").Value),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                    new Claim("username", token.Claims.First(claim => claim.Type == "username").Value),
                    new Claim("firstName", token.Claims.First(claim => claim.Type == "firstName").Value),
                    new Claim("lastName", token.Claims.First(claim => claim.Type == "lastName").Value),
                    new Claim("email", token.Claims.First(claim => claim.Type == "email").Value),
                    new Claim("id", token.Claims.First(claim => claim.Type == "id").Value),
                    new Claim("token", response)
                },
                DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.Current.GetOwinContext().Authentication.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);

                authenticated = true;
            }
            else
            {
                authenticated = false;
            }

            return authenticated;
        }
    }
}