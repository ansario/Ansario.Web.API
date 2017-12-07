using Ansario.Web.Site.Models;
using Ansario.Web.Site.Orchestrators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ansario.Web.Site.Controllers
{
    public class LoginController : Controller
    {
        ILoginOrchestrator _loginOrchestrator;

        public LoginController(ILoginOrchestrator loginOrchestrator)
        {
            _loginOrchestrator = loginOrchestrator;
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var authenticated = await _loginOrchestrator.Login(model);

            if(authenticated)
            {

            }
            else
            {
                TempData["error"] = "Invalid username or password.";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}