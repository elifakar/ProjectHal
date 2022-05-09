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
    [RoutePrefix("calisan")]
    public class EmployeeController : ApiController
    {
        readonly CalisanDataAccess _calisanDA = new CalisanDataAccess();

        /// <summary>
        /// Firma id ye göre çalışan bilgilerini listeler
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list/{firma}")]
        public ApiResult<List<CALISAN>> All(int firma)
        {
            ApiResult<List<CALISAN>> result = new ApiResult<List<CALISAN>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<CALISAN> bolgeler = _calisanDA.GetAll(firma);

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
        /// Belirtilen calisan id ye ait calisan bilgilerini listeler.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/{firma}")]
        public ApiResult<CALISAN> ById(int id, int firma)
        {
            ApiResult<CALISAN> result = new ApiResult<CALISAN> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                CALISAN calisan = _calisanDA.GetById(id);
                if (calisan != null)
                {
                    if (calisan.EMPLOYEES_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    result.STATUS = true;
                    result.MESSAGE = "Başarılı";
                    result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                    result.R = calisan;
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait çalışan bulunmamaktadır.";
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
        /// Yeni çalışan ekler
        /// </summary>
        /// <param name="calisan"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("i/{firma}")]
        public ApiResult<CALISAN> Insert([FromBody] CALISAN calisan, int firma)
        {
            ApiResult<CALISAN> result = new ApiResult<CALISAN> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (calisan == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (calisan.EMPLOYEES_C_ID != firma)
                {
                    result.MESSAGE = "Bu işlem için yetkiniz yok.";
                    result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                    return result;
                }

                CALISAN insertedCalisan = _calisanDA.Insert(calisan);
                if (insertedCalisan == null)
                {
                    result.MESSAGE = "Çalışan Eklenemedi.";
                    result.STATUS_CODE = ApiResponseCode.INSERT_ERROR.GetHashCode();
                    return result;
                }

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = insertedCalisan;
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
        ///  Body parametresinde belirtilen çalışan id ye ait çalışsan bilgilerini günceller
        /// </summary>
        /// <param name="calisan"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <param name="updateCode"> [Sürücü Genel Bilgiler Code=4,(Adı,Soyadı,Tc Kimlik No,Cinsiyet,Hes Kodu,GSM,Ehliyet Sınıfı, Doğum Tarihi, Eğitim Durumu, Kan Grubu)] , [Sürücü Şirket Çalışanı Bilgileri Code=5,(Şirket Çalışanı Durumu,Medeni Durum, Çocuk Sayısı, SGK No)]</param>
        /// <returns></returns>
        [HttpPost]
        [Route("u/{firma}/{updateCode}")]
        public ApiResult<CALISAN> Update([FromBody] CALISAN calisan, int firma, int updateCode)
        {
            ApiResult<CALISAN> result = new ApiResult<CALISAN> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (calisan == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                CALISAN updatedBolge = null;
                if (_calisanDA.GetById(calisan.EMPLOYEES_C_ID) != null)
                {
                    if (calisan.EMPLOYEES_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    updatedBolge = _calisanDA.Update(calisan);

                    if (updatedBolge == null)
                    {
                        result.MESSAGE = "Çalışan Güncellenemedi.";
                        result.STATUS_CODE = ApiResponseCode.UPDATE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait çalışan bulunmamaktadır.";
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
        /// Body parametresinde belirtilen calisan id ye ait kaydı siler.
        /// </summary>
        /// <param name="calisan"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("d/{firma}")]
        public ApiResult<bool> Delete([FromBody] CALISAN calisan, int firma)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (calisan == null)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (_calisanDA.GetById(calisan.EMPLOYEES_ID) != null)
                {
                    if (calisan.EMPLOYEES_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    bool deleteResult = _calisanDA.Delete(calisan.EMPLOYEES_ID);
                    if (deleteResult == false)
                    {
                        result.MESSAGE = "Çalışan Silinemedi.";
                        result.STATUS_CODE = ApiResponseCode.DELETE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait çalışan bulunmamaktadır.";
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
