﻿using exorderproject_client.ApiAccess;
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
    [RoutePrefix("bolge")]
    public class BolgeController : Controller
    {
        readonly BolgeApiAccess _bolgeAA = new BolgeApiAccess();
        readonly MainApiAccess _mainAA = new MainApiAccess();
        InMemoryCache _cache = new InMemoryCache();

        [Route("liste")]
        [SessionCheck]
        public ActionResult Index()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            ApiResult<List<BOLGE>> bolgeler = _bolgeAA.GetForList(firmaId);
            return View(bolgeler.R);
        }

        [Route("yeni")]
        [SessionCheck]
        public ActionResult Create()
        {
            int firmaId = Session["FIRMA_ID"].milToInt32();
            BOLGE bolge = new BOLGE() { REGION_C_ID = firmaId };
            return View(bolge);
        }

        [HttpPost]
        [SessionCheck]
        //[PaketCheck]
        [ValidateAntiForgeryToken]
        public ActionResult CreateF(BOLGE bolge)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int firmaId = Session["FIRMA_ID"].milToInt32();
                    bolge.REGION_CREATED_USER_ID = Session["KULLANICI_ID"].milToInt32();
                    bolge.REGION_STATUS = 1;
                    bolge.REGION_C_ID = firmaId;
                    ApiResult<BOLGE> result = _bolgeAA.Create(bolge, firmaId);

                    return Json(new { Title = "Success", Message = "Yeni Bölge Oluşturuldu.", ReturnUrl = Url.Action("Detail", "Bolge", new { id = result.R.REGION_ID }) });
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
            ApiResult<BOLGE> bolgeResult = _bolgeAA.GetById(id, firmaId);

            if (bolgeResult.STATUS)
            {
                BOLGE bolge = bolgeResult.R;
                StatusDDL(bolge.REGION_STATUS);

                return View(bolgeResult.R);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [SessionCheck]
        //[PaketCheck]
        [ValidateAntiForgeryToken]
        public ActionResult EditF(BOLGE bolge, int updateCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int firmaId = Session["FIRMA_ID"].milToInt32();
                    bolge.REGION_EDITED_USER_ID = Session["KULLANICI_ID"].milToInt32();

                    ApiResult<BOLGE> result = _bolgeAA.Edit(bolge, firmaId, updateCode);
                    return Json(new { Title = "Success", Message = "Bölge Bilgileri Güncellendi.", ReturnUrl = Url.Action("Detail", "Bolge", new { id = result.R.REGION_ID }) });
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
            ViewBag.REGION_STATUS = new SelectList(durumlar, "STATUS_ID", "STATUS_NAME", selected);
        }
    }
}