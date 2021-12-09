using halproject_client.ApiAccess;
using halproject_client.Helpers;
using halproject_core;
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
    [RoutePrefix("paket")]
    public class PaketController : Controller
    {
        PaketApiAccess _paketAA = new PaketApiAccess();
        InMemoryCache _cache = new InMemoryCache();

        [Route("liste")]
        [SessionCheck]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _AldigimPaketler()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            List<PAKET> paketler = _paketAA.GetAldigimPaketler(firmaId).R;
            return View(paketler);
        }

        public ActionResult _AlabilecegimPaketler()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            List<PAKET> paketler = _paketAA.GetAlabilecegimPaketler(firmaId).R;
            return View(paketler);
        }

        public ActionResult _Detail(int id)
        {
            PAKET paket = _paketAA.GetById(id).R;
            return View(paket);
        }

        [Route("satin-al/{id}")]
        [SessionCheck]
        public ActionResult SatinAl(int id)
        {
            PAKET paket = _paketAA.GetById(id).R;
            if (paket == null)
                return RedirectToAction("Index");

            return View(paket);
        }

        public ActionResult _SatinAl(int id)
        {
            PaketSatinAl satinAl = new PaketSatinAl();
            satinAl.PAKET_ID = id;
            return View(satinAl);
        }

        [HttpPost]
        [SessionCheck]
        [ValidateAntiForgeryToken]
        public ActionResult SatinAlF(PaketSatinAl satinAl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO 
                    // Kredi kartından ücret çekilecek.

                    int firmaId = Session["FIRMA_ID"].milToInt32();

                    FIRMAPAKET fp = new FIRMAPAKET()
                    {
                        PAKET_ID = satinAl.PAKET_ID,
                        FIRMA_ID = firmaId,
                        PAKETDURUM_ID = 2
                    };

                    ApiResult<FIRMAPAKET> result = _paketAA.SatinAl(fp);

                    if (result.R == null)
                        return Json(new { Title = "Error", Message = "HATA! Satın alma işlemi yapılamadı." });
                    else
                    {
                        _cache.Remove("paketler");
                        return Json(new { Title = "Success", Message = "Paket satın alındı.", ReturnUrl = Url.Action("Index", "Home") });
                    }
                }
                else
                    return Json(new { Title = "Error", Message = "HATA! Tüm zorunlu alanları doldurunuz." });
            }
            catch (Exception ex)
            {
                return Json(new { Title = "Error", ex.Message });
            }
        }
    }
}
