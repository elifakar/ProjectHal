using exorderproject_client.ApiAccess;
using exorderproject_client.Helpers;
using exorderproject_core.DTO;
using exorderproject_core.Helpers;
using exorderproject_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace exorderproject_client.Controllers
{
    public class HomeController : Controller
    {
        readonly PaketApiAccess _paketAA = new PaketApiAccess();

        readonly InMemoryCache _cache = new InMemoryCache();

        [Route("~/")]
        [SessionCheck]
        public ActionResult Index()
        {
            return View();
        }

        [Route("profil")]
        [SessionCheck]
        [PaketCheck]
        public ActionResult Profil()
        {
            ViewBag.ID = Session["KULLANICI_ID"];
            return View();
        }
        public ActionResult _Menu()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            List<int> paketler = _cache.GetOrSet("paketler", () => _paketAA.GetForCache(firmaId).R);
            PaketList paketList = new PaketList() { Paketler = paketler };
            return View(paketList);
        }
    }
}