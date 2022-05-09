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
    [RoutePrefix("bolge")]
    public class RegionController : ApiController
    {
        readonly BolgeDataAccess _bolgeDA = new BolgeDataAccess();

        /// <summary>
        /// Firma id ye göre bölge bilgilerini listeler
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list/{firma}")]
        public ApiResult<List<BOLGE>> All(int firma)
        {
            ApiResult<List<BOLGE>> result = new ApiResult<List<BOLGE>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<BOLGE> bolgeler = _bolgeDA.GetAll(firma);

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
        /// Belirtilen bölge id ye ait bölgenin bilgilerini listeler.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/{firma}")]
        public ApiResult<BOLGE> ById(int id, int firma)
        {
            ApiResult<BOLGE> result = new ApiResult<BOLGE> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                BOLGE surucu = _bolgeDA.GetById(id);
                if (surucu != null)
                {
                    if (surucu.REGION_C_ID != firma)
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
                    result.MESSAGE = "Girdiğiniz id değerine ait bölge bulunmamaktadır.";
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
        /// Yeni bölge ekler
        /// </summary>
        /// <param name="bolge"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("i/{firma}")]
        public ApiResult<BOLGE> Insert([FromBody] BOLGE bolge, int firma)
        {
            ApiResult<BOLGE> result = new ApiResult<BOLGE> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (bolge == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (bolge.REGION_C_ID != firma)
                {
                    result.MESSAGE = "Bu işlem için yetkiniz yok.";
                    result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                    return result;
                }

                BOLGE insertedBolge = _bolgeDA.Insert(bolge);
                if (insertedBolge == null)
                {
                    result.MESSAGE = "Bölge Eklenemedi.";
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
        ///  Body parametresinde belirtilen bolge id ye ait bolge bilgilerini günceller
        /// </summary>
        /// <param name="bolge"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <param name="updateCode"> [Sürücü Genel Bilgiler Code=4,(Adı,Soyadı,Tc Kimlik No,Cinsiyet,Hes Kodu,GSM,Ehliyet Sınıfı, Doğum Tarihi, Eğitim Durumu, Kan Grubu)] , [Sürücü Şirket Çalışanı Bilgileri Code=5,(Şirket Çalışanı Durumu,Medeni Durum, Çocuk Sayısı, SGK No)]</param>
        /// <returns></returns>
        [HttpPost]
        [Route("u/{firma}/{updateCode}")]
        public ApiResult<BOLGE> Update([FromBody] BOLGE bolge, int firma, int updateCode)
        {
            ApiResult<BOLGE> result = new ApiResult<BOLGE> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (bolge == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                BOLGE updatedBolge = null; 
                if (_bolgeDA.GetById(bolge.REGION_ID) != null)
                {
                    if (bolge.REGION_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                        updatedBolge = _bolgeDA.Update(bolge);
                    
                    if (updatedBolge == null)
                    {
                        result.MESSAGE = "Bölge Güncellenemedi.";
                        result.STATUS_CODE = ApiResponseCode.UPDATE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait bölge bulunmamaktadır.";
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
        /// Body parametresinde belirtilen bolge id ye ait kaydı siler.
        /// </summary>
        /// <param name="bolge"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("d/{firma}")]
        public ApiResult<bool> Delete([FromBody] BOLGE bolge, int firma)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (bolge == null)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (_bolgeDA.GetById(bolge.REGION_ID) != null)
                {
                    if (bolge.REGION_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    bool deleteResult = _bolgeDA.Delete(bolge.REGION_ID);
                    if (deleteResult == false)
                    {
                        result.MESSAGE = "Bölge Silinemedi.";
                        result.STATUS_CODE = ApiResponseCode.DELETE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait bölge bulunmamaktadır.";
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
