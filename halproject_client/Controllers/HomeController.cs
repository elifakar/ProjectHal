using halproject_client.ApiAccess;
using halproject_client.Helpers;
using halproject_core.DTO;
using halproject_core.Helpers;
using halproject_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace halproject_client.Controllers
{
    public class HomeController : Controller
    {

        readonly InMemoryCache _cache = new InMemoryCache();

        [Route("~/")]
        [SessionCheck]
        public ActionResult Index()
        {
            return View();
        }

       
    }
}