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