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
    [RoutePrefix("kategori")]
    public class KategoriController : Controller
    {
        readonly KategoriApiAccess _kategoriAA = new KategoriApiAccess();
        readonly MainApiAccess _mainAA = new MainApiAccess();
        InMemoryCache _cache = new InMemoryCache();

        [Route("liste")]
        [SessionCheck]
        public ActionResult Index()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            ApiResult<List<KATEGORI>> kategoriler = _kategoriAA.GetForList(firmaId);
            return View(kategoriler.R);
        }

        [Route("yeni")]
        [SessionCheck]
        public ActionResult Create()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            KATEGORI kategori = new KATEGORI() { CATEGORY_C_ID = firmaId };
            return View(kategori);
        }

        [HttpPost]
        [SessionCheck]
       //[PaketCheck]
        [ValidateAntiForgeryToken]
        public ActionResult CreateF(KATEGORI kategori)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int firmaId = Session["FIRMA_ID"].milToInt32();
                    kategori.CATEGORY_CREATED_USER_ID = Session["KULLANICI_ID"].milToInt32();
                    kategori.CATEGORY_STATUS = 1;
                    kategori.CATEGORY_C_ID = firmaId;
                    ApiResult<KATEGORI> result = _kategoriAA.Create(kategori, firmaId);

                    return Json(new { Title = "Success", Message = "Yeni Kategori Oluşturuldu.", ReturnUrl = Url.Action("Detail", "Kategori", new { id = result.R.CATEGORY_ID }) });
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
            ApiResult<KATEGORI> kategoriResult = _kategoriAA.GetById(id, firmaId);

            if (kategoriResult.STATUS)
            {
                KATEGORI kategori = kategoriResult.R;
                StatusDDL(kategori.CATEGORY_STATUS);
                return View(kategoriResult.R);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [SessionCheck]
       //[PaketCheck]
        [ValidateAntiForgeryToken]
        public ActionResult EditF(KATEGORI kategori, int updateCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int firmaId = Session["FIRMA_ID"].milToInt32();
                    kategori.CATEGORY_EDITED_USER_ID = Session["KULLANICI_ID"].milToInt32();

                    ApiResult<KATEGORI> result = _kategoriAA.Edit(kategori, firmaId, updateCode);
                    return Json(new { Title = "Success", Message = "Kategori Bilgileri Güncellendi.", ReturnUrl = Url.Action("Detail", "Kategori", new { id = result.R.CATEGORY_ID }) });
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
            ViewBag.CATEGORY_STATUS = new SelectList(durumlar, "STATUS_ID", "STATUS_NAME", selected);
        }
    }
}