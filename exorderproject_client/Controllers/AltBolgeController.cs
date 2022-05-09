using exorderproject_client.ApiAccess;
using exorderproject_client.Helpers;
using exorderproject_core;
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
    [RoutePrefix("altbolge")]
    public class AltBolgeController : Controller
    {
        readonly AltBolgeApiAccess _altbolgeAA = new AltBolgeApiAccess();
        readonly MainApiAccess _mainAA = new MainApiAccess();
        InMemoryCache _cache = new InMemoryCache();

        [Route("liste")]
        [SessionCheck]
        public ActionResult Index()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            ApiResult<List<ALTBOLGE>> altbolgeler = _altbolgeAA.GetForList(firmaId);
            return View(altbolgeler.R);
        }

        [Route("yeni")]
        [SessionCheck]
        public ActionResult Create()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            ALTBOLGE altbolge = new ALTBOLGE() { REGIONS_C_ID = firmaId };
            return View(altbolge);
        }

        [HttpPost]
        [SessionCheck]
        //[PaketCheck]
        [ValidateAntiForgeryToken]
        public ActionResult CreateF(ALTBOLGE altbolge)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int firmaId = Session["FIRMA_ID"].milToInt32();
                    altbolge.REGIONS_CREATED_USER_ID = Session["KULLANICI_ID"].milToInt32();
                    altbolge.REGIONS_STATUS = 1;
                    altbolge.REGIONS_C_ID = firmaId;
                    ApiResult<ALTBOLGE> result = _altbolgeAA.Create(altbolge, firmaId);

                    return Json(new { Title = "Success", Message = "Yeni Alt Bölge Oluşturuldu.", ReturnUrl = Url.Action("Detail", "AltBolge", new { id = result.R.REGIONS_ID }) });
                }
                else
                    return Json(new { Title = "Error", Message = "HATA! Tüm zorunlu alanları doldurunuz." });
            }
            catch (Exception ex)
            {
                return Json(new { Title = "Error", ex.Message });
            }
        }


        [Route("detay/{id}")]
        [SessionCheck]
        //[PaketCheck]
        public ActionResult Detail(int id)
        {
            ViewBag.ID = id;
            return View();
        }

        public ActionResult _Genel(int id)
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            ApiResult<ALTBOLGE> bolgeResult = _altbolgeAA.GetById(id, firmaId);

            if (bolgeResult.STATUS)
            {
                ALTBOLGE altbolge = bolgeResult.R;
                StatusDDL(altbolge.REGIONS_STATUS);

                return View(bolgeResult.R);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [SessionCheck]
        //[PaketCheck]
        [ValidateAntiForgeryToken]
        public ActionResult EditF(ALTBOLGE altbolge, int updateCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int firmaId = Session["FIRMA_ID"].milToInt32();
                    altbolge.REGIONS_EDITED_USER_ID = Session["KULLANICI_ID"].milToInt32();

                    ApiResult<ALTBOLGE> result = _altbolgeAA.Edit(altbolge, firmaId, updateCode);
                    return Json(new { Title = "Success", Message = "Alt Bölge Bilgileri Güncellendi.", ReturnUrl = Url.Action("Detail", "AltBolge", new { id = result.R.REGIONS_ID }) });
                }
                else
                    return Json(new { Title = "Error", Message = "HATA! Tüm zorunlu alanları doldurunuz." });
            }
            catch (Exception ex)
            {
                return Json(new { Title = "Error", ex.Message });
            }
        }

        private void StatusDDL(object selected = null)
        {
            List<STATUS> durumlar = _mainAA.GetStatus().R;
            ViewBag.REGIONS_STATUS = new SelectList(durumlar, "STATUS_ID", "STATUS_NAME", selected);
        }
    }
}