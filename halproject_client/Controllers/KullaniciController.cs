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
    public class KullaniciController : Controller
    {
        readonly KullaniciApiAccess _kullaniciAA = new KullaniciApiAccess();
        readonly InMemoryCache _cache = new InMemoryCache();

        #region Authentication

        [Route("giris-yap")]
        public ActionResult GirisYap()
        {
            Login login = new Login() { EMAIL = "elifakar", SIFRE = "1234" };
            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GirisYapF(Login login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApiResult<KULLANICI> result = _kullaniciAA.GirisYap(login);
                    KULLANICI kullanici = result.R;

                    Session.Add("KULLANICI_ID", kullanici.ID);
                    Session.Add("KULLANICI_ADI_SOYADI", kullanici.KULLANICI_ADSOYAD);
                    if (kullanici.FIRMA_ID > 0)
                        Session.Add("FIRMA_ID", kullanici.FIRMA_ID);

                    _cache.Remove("paketler");

                    return Json(new { Title = "Success", Message = "Giriş başarılı.", ReturnUrl = Url.Action("Index", "Home") });
                }
                else
                    return Json(new { Title = "Error", Message = "HATA! Tüm zorunlu alanları doldurunuz." });
            }
            catch (Exception ex)
            {
                return Json(new { Title = "Error", ex.Message});
            }
        }

        [Route("kayit-ol")]
        public ActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KayitOlF(Register register)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ApiResult<KULLANICI> result = _kullaniciAA.KayitOl(register);

                    return Json(new { Title = "Success", Message = "Kayıt işleminiz yapıldı. Giriş yapabilirsiniz.", ReturnUrl = Url.Action("GirisYap", "Kullanici") });
                }
                else
                    return Json(new { Title = "Error", Message = "HATA! Tüm zorunlu alanları doldurunuz." });
            }
            catch (Exception ex)
            {
                return Json(new { Title = "Error", ex.Message });
            }
        }

        [Route("sifremi-unuttum")]
        public ActionResult SifremiUnuttum()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SifremiUnuttumF(ForgotPassword forgotPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _kullaniciAA.SifremiUnuttum(forgotPassword);

                    return Json(new { Title = "Success", Message = "Şifre yenileme bağlantısı email adresinize gönderildi.", ReturnUrl = Url.Action("GirisYap", "Kullanici") });
                }
                else
                    return Json(new { Title = "Error", Message = "HATA! Tüm zorunlu alanları doldurunuz." });
            }
            catch (Exception ex)
            {
                return Json(new { Title = "Error", ex.Message });
            }
        }

        [Route("sifre-sifirla/{guid}")]
        public ActionResult SifreSifirla(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return RedirectToAction("Login");

            ResetPassword resetPassword = new ResetPassword() { GUID = guid };
            return View(resetPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SifreSifirlaF(ResetPassword resetPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _kullaniciAA.SifreSifirla(resetPassword);

                    return Json(new { Title = "Success", Message = "Şifreniz yenilenmiştir. Yeni şifreniz ile giriş yapabilirsiniz.", ReturnUrl = Url.Action("GirisYap", "Kullanici") });
                }
                else
                    return Json(new { Title = "Error", Message = "HATA! Tüm zorunlu alanları doldurunuz." });
            }
            catch (Exception ex)
            {
                return Json(new { Title = "Error", ex.Message });
            }
        }

        public ActionResult _SifreGuncelle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SifreGuncelleF(UpdatePassword updatePassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    updatePassword.KULLANICI_ID = Session["KULLANICI_ID"].milToInt32();
                    _kullaniciAA.SifreGuncelle(updatePassword);

                    return Json(new { Title = "Success", Message = "Şifreniz yenilenmiştir. Yeni şifreniz ile giriş yapabilirsiniz.", ReturnUrl = Url.Action("GirisYap", "Kullanici") });
                }
                else
                    return Json(new { Title = "Error", Message = "HATA! Tüm zorunlu alanları doldurunuz." });
            }
            catch (Exception ex)
            {
                return Json(new { Title = "Error", ex.Message});
            }
        }

        [Route("cikis-yap")]
        public ActionResult CikisYap()
        {
            Session.RemoveAll();
            return RedirectToAction("GirisYap");
        }
        #endregion

        [Route("kullanici/liste")]
        [SessionCheck]
        public ActionResult Index()
        {
            return View();
        }
    }
}