using Ansario.Web.API.Models;
using Ansario.Web.API.Orchestrators;
using Ansario.Web.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Ansario.Web.API.Controllers
{
    [RoutePrefix("v1/account")]
    public class AccountController : ApiController
    {
        public IAuthenticationOrchestrator _orechestrator;

        public AccountController(IAuthenticationOrchestrator orechestrator)
        {
            _orechestrator = orechestrator;
        }

        [Route("register")]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<string> Register(RegisterUser user)
        {
            return await _orechestrator.RegisterUser(user);
        }

        [Route("login")]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<string> Get(LoginUser user)
        {
            return await _orechestrator.LoginUser(user);
        }
    }
}