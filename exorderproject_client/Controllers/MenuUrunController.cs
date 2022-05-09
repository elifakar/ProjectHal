using exorderproject_client.ApiAccess;
using exorderproject_client.Helpers;
using exorderproject_core;
using exorderproject_core.DTO;
using exorderproject_core.Helpers;
using exorderproject_core.Models;
using System;
using System.Collections.Generic;
using System.Data;

using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace exorderproject_client.Controllers
{
    [RoutePrefix("menuurun")]
    public class MenuUrunController : Controller
    {
        readonly MenuUrunApiAccess _urunAA = new MenuUrunApiAccess();
        readonly MainApiAccess _mainAA = new MainApiAccess();
        InMemoryCache _cache = new InMemoryCache();

        [Route("liste")]
        [SessionCheck]
        //[PaketCheck(Paket1 = PAKETLER.OGRENCI_TASIMACILIGI, Paket2 = PAKETLER.PESONEL_TASIMACILIGI)]
        public ActionResult Index()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            ApiResult<List<URUN>> urunler = _urunAA.GetForList(firmaId);
            return View(urunler.R);
        }

        [Route("yeni")]
        [SessionCheck]
        public ActionResult Create()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            URUN urun = new URUN() { PRODUCT_C_ID = firmaId };
            CategoryDDL();
            return View(urun);
        }

        [HttpPost]
        [SessionCheck]
        //[PaketCheck]
        [ValidateAntiForgeryToken]
        public ActionResult CreateF(URUN urun)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int firmaId = Session["FIRMA_ID"].milToInt32();
                    urun.PRODUCT_CREATED_USER_ID = Session["KULLANICI_ID"].milToInt32();
                    urun.PRODUCT_STATUS = 1;
                    urun.PRODUCT_C_ID = firmaId;
                    ApiResult<URUN> result = _urunAA.Create(urun, firmaId);
                    return Json(new { Title = "Success", Message = "Yeni Ürün Oluşturuldu.", ReturnUrl = Url.Action("Detail", "Urun", new { id = result.R.PRODUCT_ID }) });
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
            ApiResult<URUN> urunResult = _urunAA.GetById(id, firmaId);

            if (urunResult.STATUS)
            {
                URUN urun = urunResult.R;

                CategoryDDL(urun.PRODUCT_CATEGORY_ID);
                StatusDDL(urun.PRODUCT_STATUS);

                return View(urunResult.R);
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult _TedBilgileri(int id)
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            ApiResult<URUN> urunResult = _urunAA.GetById(id, firmaId);

            if (urunResult.STATUS)
            {
                URUN urun = urunResult.R;

                CategoryDDL(urun.PRODUCT_CATEGORY_ID);

                return View(urunResult.R);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [SessionCheck]
        //[PaketCheck]
        [ValidateAntiForgeryToken]
        public ActionResult EditF(URUN urun, int updateCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int firmaId = Session["FIRMA_ID"].milToInt32();
                    urun.PRODUCT_EDITED_USER_ID = Session["KULLANICI_ID"].milToInt32();

                    ApiResult<URUN> result = _urunAA.Edit(urun, firmaId, updateCode);
                    return Json(new { Title = "Success", Message = "Ürün Bilgileri Güncellendi.", ReturnUrl = Url.Action("Detail", "Urun", new { id = result.R.PRODUCT_ID }) });
                }
                else
                    return Json(new { Title = "Error", Message = "HATA! Tüm zorunlu alanları doldurunuz." });
            }
            catch (Exception ex)
            {
                return Json(new { Title = "Error", ex.Message });
            }
        }
        private void CategoryDDL(object selected = null)
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            List<CATEGORY> durumlar = _mainAA.GetCategory(firmaId).R;
            ViewBag.PRODUCT_CATEGORY_ID = new SelectList(durumlar, "CATEGORY_ID", "CATEGORY_NAME", selected);
        }
        private void StatusDDL(object selected = null)
        {
            List<STATUS> durumlar = _mainAA.GetStatus().R;
            ViewBag.PRODUCT_STATUS = new SelectList(durumlar, "STATUS_ID", "STATUS_NAME", selected);
        }
    }
}
