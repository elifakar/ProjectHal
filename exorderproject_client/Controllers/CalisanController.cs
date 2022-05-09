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
    public class CalisanController : Controller
    {
        // GET: Calisan
        readonly CalisanApiAccess _calisanAA = new CalisanApiAccess();
        readonly MainApiAccess _mainAA = new MainApiAccess();
        InMemoryCache _cache = new InMemoryCache();

        [Route("liste")]
        [SessionCheck]
        public ActionResult Index()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            ApiResult<List<CALISAN>> bolgeler = _calisanAA.GetForList(firmaId);
            return View(bolgeler.R);
        }

        [Route("yeni")]
        [SessionCheck]
        public ActionResult Create()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            CALISAN bolge = new CALISAN() { EMPLOYEES_C_ID = firmaId };
            return View(bolge);
        }

        [HttpPost]
        [SessionCheck]
        //[PaketCheck]
        [ValidateAntiForgeryToken]
        public ActionResult CreateF(CALISAN bolge)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int firmaId = Session["FIRMA_ID"].milToInt32();
                    bolge.EMPLOYEES_CREATED_USER_ID = Session["KULLANICI_ID"].milToInt32();
                    bolge.EMPLOYEES_STATUS = 1;
                    bolge.EMPLOYEES_C_ID = firmaId;
                    ApiResult<CALISAN> result = _calisanAA.Create(bolge, firmaId);

                    return Json(new { Title = "Success", Message = "Yeni Çalışan Oluşturuldu.", ReturnUrl = Url.Action("Detail", "Calisan", new { id = result.R.EMPLOYEES_ID }) });
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
            ApiResult<CALISAN> bolgeResult = _calisanAA.GetById(id, firmaId);

            if (bolgeResult.STATUS)
            {
                CALISAN bolge = bolgeResult.R;
                StatusDDL(bolge.EMPLOYEES_STATUS);

                return View(bolgeResult.R);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [SessionCheck]
        //[PaketCheck]
        [ValidateAntiForgeryToken]
        public ActionResult EditF(CALISAN bolge, int updateCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int firmaId = Session["FIRMA_ID"].milToInt32();
                    bolge.EMPLOYEES_EDITED_USER_ID = Session["KULLANICI_ID"].milToInt32();

                    ApiResult<CALISAN> result = _calisanAA.Edit(bolge, firmaId, updateCode);
                    return Json(new { Title = "Success", Message = "Çalışan Bilgileri Güncellendi.", ReturnUrl = Url.Action("Detail", "Calisan", new { id = result.R.EMPLOYEES_ID }) });
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
            ViewBag.EMPLOYEES_STATUS = new SelectList(durumlar, "STATUS_ID", "STATUS_NAME", selected);
        }
    }
}