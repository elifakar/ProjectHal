using exorderproject_api.DataAccess;
using exorderproject_core;
using exorderproject_core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace exorderproject_api.Controllers
{
    [RoutePrefix("altbolge")]
    public class RegionsController : ApiController
    {
        readonly AltBolgeDataAccess _altbolgeDA = new AltBolgeDataAccess();

        /// <summary>
        /// Firma id ye göre alt bölge bilgilerini listeler
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list/{firma}")]
        public ApiResult<List<ALTBOLGE>> All(int firma)
        {
            ApiResult<List<ALTBOLGE>> result = new ApiResult<List<ALTBOLGE>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<ALTBOLGE> bolgeler = _altbolgeDA.GetAll(firma);

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = bolgeler;
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
        /// Belirtilen alt bölge id ye ait bölgenin bilgilerini listeler.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/{firma}")]
        public ApiResult<ALTBOLGE> ById(int id, int firma)
        {
            ApiResult<ALTBOLGE> result = new ApiResult<ALTBOLGE> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                ALTBOLGE surucu = _altbolgeDA.GetById(id);
                if (surucu != null)
                {
                    if (surucu.REGIONS_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    result.STATUS = true;
                    result.MESSAGE = "Başarılı";
                    result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                    result.R = surucu;
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait alt bölge bulunmamaktadır.";
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
        /// Yeni alt bölge ekler
        /// </summary>
        /// <param name="bolge"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("i/{firma}")]
        public ApiResult<ALTBOLGE> Insert([FromBody] ALTBOLGE bolge, int firma)
        {
            ApiResult<ALTBOLGE> result = new ApiResult<ALTBOLGE> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (bolge == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (bolge.REGIONS_C_ID != firma)
                {
                    result.MESSAGE = "Bu işlem için yetkiniz yok.";
                    result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                    return result;
                }

                ALTBOLGE insertedBolge = _altbolgeDA.Insert(bolge);
                if (insertedBolge == null)
                {
                    result.MESSAGE = "Alt Bölge Eklenemedi.";
                    result.STATUS_CODE = ApiResponseCode.INSERT_ERROR.GetHashCode();
                    return result;
                }

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = insertedBolge;
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
        ///  Body parametresinde belirtilen alt bölge id ye ait alt bolge bilgilerini günceller
        /// </summary>
        /// <param name="altbolge"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <param name="updateCode"> [Sürücü Genel Bilgiler Code=4,(Adı,Soyadı,Tc Kimlik No,Cinsiyet,Hes Kodu,GSM,Ehliyet Sınıfı, Doğum Tarihi, Eğitim Durumu, Kan Grubu)] , [Sürücü Şirket Çalışanı Bilgileri Code=5,(Şirket Çalışanı Durumu,Medeni Durum, Çocuk Sayısı, SGK No)]</param>
        /// <returns></returns>
        [HttpPost]
        [Route("u/{firma}/{updateCode}")]
        public ApiResult<ALTBOLGE> Update([FromBody] ALTBOLGE altbolge, int firma, int updateCode)
        {
            ApiResult<ALTBOLGE> result = new ApiResult<ALTBOLGE> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (altbolge == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                ALTBOLGE updatedBolge = null; ;
                if (_altbolgeDA.GetById(altbolge.REGIONS_ID) != null)
                {
                    if (altbolge.REGIONS_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    updatedBolge = _altbolgeDA.Update(altbolge);

                    if (updatedBolge == null)
                    {
                        result.MESSAGE = "Alt Bölge Güncellenemedi.";
                        result.STATUS_CODE = ApiResponseCode.UPDATE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait alt bölge bulunmamaktadır.";
                    result.STATUS_CODE = ApiResponseCode.NOT_EXIST.GetHashCode();
                    return result;
                }

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = updatedBolge;
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
        /// Body parametresinde belirtilen alt bölge id ye ait kaydı siler.
        /// </summary>
        /// <param name="altbolge"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("d/{firma}")]
        public ApiResult<bool> Delete([FromBody] ALTBOLGE altbolge, int firma)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (altbolge == null)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (_altbolgeDA.GetById(altbolge.REGIONS_ID) != null)
                {
                    if (altbolge.REGIONS_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    bool deleteResult = _altbolgeDA.Delete(altbolge.REGIONS_ID);
                    if (deleteResult == false)
                    {
                        result.MESSAGE = "Alt Bölge Silinemedi.";
                        result.STATUS_CODE = ApiResponseCode.DELETE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait alt bölge bulunmamaktadır.";
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
