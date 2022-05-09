using exorderproject_api.DataAccess;
using exorderproject_api.Helpers;
using exorderproject_core;
using exorderproject_core.DTO;
using exorderproject_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace exorderproject_api.Controllers
{
    [RoutePrefix("kullanici")]
    public class KullaniciController : ApiController
    {
        readonly KullaniciDataAccess _kullaniciDA = new KullaniciDataAccess();
        readonly MailHelper _mail = new MailHelper();

        /// <summary>
        /// Kullanıcı adı ve şifre kontrolü yapar
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public ApiResult<KULLANICI> Login([FromBody] Login login)
        {
            ApiResult<KULLANICI> result = new ApiResult<KULLANICI> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (login == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Kullanıcı adı/şifre boş geçilemez.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                KULLANICI kullanici = _kullaniciDA.GetByEmail(login.EMAIL);
                if (kullanici == null || kullanici.SIFRE != login.SIFRE)
                {
                    result.MESSAGE = "Kullanıcı adı/şifre hatalı.";
                    result.STATUS_CODE = ApiResponseCode.WRONG_BODY_PARAMS.GetHashCode();
                    return result;
                }
                kullanici.SIFRE = "";
                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = kullanici;
                return result;
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Yeni kullanıcı oluşturur
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public ApiResult<KULLANICI> Register([FromBody] Register register)
        {
            ApiResult<KULLANICI> result = new ApiResult<KULLANICI> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (register == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Kullanıcı adı/şifre boş geçilemez.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (register.SIFRE == register.SIFRE_TEKRAR)
                {
                    if (!_kullaniciDA.CheckEmail(register.EMAIL))
                    {
                        KULLANICI kullanici = _kullaniciDA.Register(register);
                        if (kullanici != null)
                        {
                            kullanici.SIFRE = "";

                            result.STATUS = true;
                            result.MESSAGE = "Başarılı";
                            result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                            result.R = kullanici;
                            return result;
                        }
                        else
                        {
                            result.MESSAGE = "Kullanıcı eklenemedi.";
                            result.STATUS_CODE = ApiResponseCode.INSERT_ERROR.GetHashCode();
                            result.R = null;
                            return result;
                        }
                    }
                    else
                    {
                        result.MESSAGE = "Email adresi ile kaydedilmiş bir kullanıcı var.";
                        result.STATUS_CODE = ApiResponseCode.WRONG_BODY_PARAMS.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Şifre tekrarları uyuşmuyor";
                    result.STATUS_CODE = ApiResponseCode.NOT_EQUAL.GetHashCode();
                    return result;
                }                
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Belirtilen mail adresini kontrol eder. Eger mail adresi veritabanına kayıtlı ise kullanıcının şifre degişikliği yapabilmesi için bir link gönderir. Bu link içerisinde kendine ait özel bir guid bulundurur.
        /// </summary>
        /// <param name="forgotpassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgotpassword")]
        public ApiResult<bool> ForgotPassword([FromBody] ForgotPassword forgotpassword)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (forgotpassword == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                KULLANICI checkKullanici = _kullaniciDA.GetByEmail(forgotpassword.EMAIL);
                if (checkKullanici == null)
                {
                    result.MESSAGE = "Bu email adresi kayıtlı değildir.";
                    result.STATUS_CODE = ApiResponseCode.WRONG_BODY_PARAMS.GetHashCode();
                    return result;
                }

                string guid = Guid.NewGuid().ToString("N");
                PASSWORDRESETTEMP passwordresettemp = _kullaniciDA.ForgotPassword(forgotpassword, checkKullanici.ID, guid);
                if (passwordresettemp != null)
                {
                    string mailBody = "<a href='exorder/sifre-sifirla/" + guid + "'>Şifremi Kurtar</a>";
                    _mail.Send(checkKullanici.EMAIL, "Hal Project Şifre Kurtarma", mailBody);

                    result.STATUS = true;
                    result.MESSAGE = "Başarılı";
                    result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                    result.R = true;
                    return result;
                }
                else
                {
                    result.MESSAGE = "Guid oluşturulamadı.";
                    result.STATUS_CODE = ApiResponseCode.INSERT_ERROR.GetHashCode();
                    result.R = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Şifre yenileme işlemi yapar. Bu işlemi yapmadan önce aşağıdaki kontrolleri yapar.
        /// *Belirtilen guid veritabanında var mı yok mu onu kontrol eder
        /// *Guid süresini kontrol eder. 
        /// *Şifre ve şifre tekrarlarını kontrol eder
        /// </summary>
        /// <param name="resetpassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("resetpassword")]
        public ApiResult<bool> ResetPassword([FromBody] ResetPassword resetpassword)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (resetpassword == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (resetpassword.YENISIFRE == resetpassword.YENISIFRE_TEKRAR)
                {
                    PASSWORDRESETTEMP checkGuidAndGetTime = _kullaniciDA.CheckGuidAndGetTime(resetpassword);

                    if (checkGuidAndGetTime != null)
                    {
                        if (checkGuidAndGetTime.SONTARIH > DateTime.Now)
                        {
                            if (_kullaniciDA.ChangePassword(resetpassword.YENISIFRE, checkGuidAndGetTime.KULLANICI_ID))
                            {
                                result.STATUS = true;
                                result.MESSAGE = "Şifre Güncellendi.";
                                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                                result.R = true;
                                return result;
                            }
                            else
                            {
                                result.MESSAGE = "Şifre güncellenemedi";
                                result.STATUS_CODE = ApiResponseCode.UPDATE_ERROR.GetHashCode();
                                return result;
                            }
                        }
                        else
                        {
                            result.MESSAGE = "Guid süresi sona ermiştir";
                            result.STATUS_CODE = ApiResponseCode.TOKEN_TIMEOUT.GetHashCode();
                            return result;
                        }
                    }
                    else
                    {
                        result.MESSAGE = "Geçersiz guid";
                        result.STATUS_CODE = ApiResponseCode.WRONG_BODY_PARAMS.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = " Şifre tekrarları uyuşmuyor";
                    result.STATUS_CODE = ApiResponseCode.NOT_EQUAL.GetHashCode();
                    return result;
                }                   
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Şifre güncelleme işlemi yapar.
        /// </summary>
        /// <param name="updatepassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updatepassword")]
        public ApiResult<bool> UpdatePassword([FromBody] UpdatePassword updatepassword)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (updatepassword == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (updatepassword.YENISIFRE == updatepassword.YENISIFRE_TEKRAR)
                {
                    string kayitlliEskisifre = _kullaniciDA.GetPasswordById(updatepassword.KULLANICI_ID);

                    if (kayitlliEskisifre != null)
                    {
                        if (kayitlliEskisifre == updatepassword.ESKISIFRE)
                        {
                            if (_kullaniciDA.ChangePassword(updatepassword.YENISIFRE, updatepassword.KULLANICI_ID))
                            {
                                result.STATUS = true;
                                result.MESSAGE = "Şifre güncellendi.";
                                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                                result.R = true;
                                return result;
                            }
                            else
                            {
                                result.MESSAGE = "Şifre güncellenemedi.";
                                result.STATUS_CODE = ApiResponseCode.UPDATE_ERROR.GetHashCode();
                                return result;
                            }
                        }
                        else
                        {
                            result.MESSAGE = "Eski şifreniz doğru değil.";
                            result.STATUS_CODE = ApiResponseCode.WRONG_DATA.GetHashCode();
                            return result;
                        }
                    }
                    else
                    {
                        result.MESSAGE = "Kullanıcı bulunamadı.";
                        result.STATUS_CODE = ApiResponseCode.NOT_EXIST.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = " Şifre tekrarları uyuşmuyor";
                    result.STATUS_CODE = ApiResponseCode.NOT_EQUAL.GetHashCode();
                    return result;
                }                
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Firma id ye göre kullanıcıların bilgilerini listeler.
        /// </summary>
        /// <param name="firma"></param>
        /// <param name="durum">Gönderilen durum parametresi 0 (Aktif-Pasif dahil hepsini getir )- 1 (Aktif) - 2 (Pasif) şeklindedir.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("all/{firma}/{durum}")]
        public ApiResult<List<KULLANICI>> All(int firma, int? durum)
        {
            ApiResult<List<KULLANICI>> result = new ApiResult<List<KULLANICI>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<KULLANICI> kullanicilar = _kullaniciDA.GetAll(firma, durum);

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = kullanicilar;
                return result;
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Kullanıcı id ye göre kullanıcı bilgisini listeler
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/{firma}")]
        public ApiResult<KULLANICI> ById(int id, int firma)
        {
            ApiResult<KULLANICI> result = new ApiResult<KULLANICI> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                KULLANICI kullanici = _kullaniciDA.GetById(id);
                if (kullanici != null)
                {
                    if (kullanici.FIRMA_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    result.STATUS = true;
                    result.MESSAGE = "Başarılı";
                    result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                    result.R = kullanici;
                }
                else
                {
                    result.MESSAGE = "Kullanıcı bulunamadı.";
                    result.STATUS_CODE = ApiResponseCode.NOT_EXIST.GetHashCode();
                }
                return result;
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Yeni kullanıcı eklemek için kullanılır. Belirtilen mail adresi ile kayıt olup-olmadığı kontrol edilir
        /// </summary>
        /// <param name="kullanici"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır </param>
        /// <returns></returns>
        [HttpPost]
        [Route("i/{firma}")]
        public ApiResult<KULLANICI> Insert([FromBody] KULLANICI kullanici, int firma)
        {
            ApiResult<KULLANICI> result = new ApiResult<KULLANICI> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (kullanici == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (kullanici.FIRMA_ID != firma)
                {
                    result.MESSAGE = "Bu işlem için yetkiniz yok.";
                    result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                    return result;
                }

                if (!_kullaniciDA.CheckEmail(kullanici.EMAIL))
                {
                    KULLANICI insertedKullanici = _kullaniciDA.Insert(kullanici);
                    if (insertedKullanici == null)
                    {
                        result.MESSAGE = "Kullanıcı eklenemedi.";
                        result.STATUS_CODE = ApiResponseCode.INSERT_ERROR.GetHashCode();
                        return result;
                    }

                    result.STATUS = true;
                    result.MESSAGE = "Başarılı";
                    result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                    result.R = insertedKullanici;
                    return result;
                }
                else
                {
                    result.MESSAGE = "Email adresi ile kaydedilmiş bir kullanıcı var.";
                    result.STATUS_CODE = ApiResponseCode.WRONG_BODY_PARAMS.GetHashCode();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Kullanıcı bilgilerini güncellemek için kullanılır.
        /// </summary>
        /// <param name="kullanici"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("u/{firma}")]
        public ApiResult<KULLANICI> Update([FromBody] KULLANICI kullanici, int firma)
        {
            ApiResult<KULLANICI> result = new ApiResult<KULLANICI> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (kullanici == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (firma != 0 && kullanici.FIRMA_ID != firma)
                {
                    result.MESSAGE = "Bu işlem için yetkiniz yok.";
                    result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                    return result;
                }

                KULLANICI updatedKullanici;
                if (_kullaniciDA.GetById(kullanici.ID) != null)
                {
                    updatedKullanici = _kullaniciDA.Update(kullanici);
                    if (updatedKullanici == null)
                    {
                        result.MESSAGE = "Kullanıcı güncellenemedi.";
                        result.STATUS_CODE = ApiResponseCode.UPDATE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Kullanıcı bulunamadı.";
                    result.STATUS_CODE = ApiResponseCode.NOT_EXIST.GetHashCode();
                    return result;
                }

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = updatedKullanici;
                return result;
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Body parametresi olarak gönderilen kullanıcı id ye ait kayıt silinir.
        /// </summary>
        /// <param name="kullanici"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("d/{firma}")]
        public ApiResult<bool> Delete([FromBody] KULLANICI kullanici, int firma)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (kullanici == null)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (_kullaniciDA.GetById(kullanici.ID) != null)
                {
                    if (kullanici.FIRMA_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    bool deleteResult = _kullaniciDA.Delete(kullanici.ID);
                    if (deleteResult == false)
                    {
                        result.MESSAGE = "Kullanıcı silinemedi.";
                        result.STATUS_CODE = ApiResponseCode.DELETE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Kullanıcı bulunamadı.";
                    result.STATUS_CODE = ApiResponseCode.NOT_EXIST.GetHashCode();
                    return result;
                }

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = true;
                return result;
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }
    }
}
