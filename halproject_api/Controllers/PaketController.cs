using halproject_api.DataAccess;
using halproject_core;
using halproject_core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace halproject_api.Controllers
{
    [RoutePrefix("paket")]
    public class PaketController : ApiController
    {
        PaketDataAccess _paketDA = new PaketDataAccess();

        [HttpGet]
        [Route("for-cache/{firma}/{durum}")]
        public ApiResult<List<int>> ForCache(int firma, int? durum)
        {
            ApiResult<List<int>> result = new ApiResult<List<int>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<int> paketler = _paketDA.GetForCache(firma, durum);

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = paketler;
                return result;
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        [HttpGet]
        [Route("aldigim/{firma}")]
        public ApiResult<List<PAKET>> AldigimPaketler(int firma)
        {
            ApiResult<List<PAKET>> result = new ApiResult<List<PAKET>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<PAKET> paketler = _paketDA.GetAldigimPaketler(firma);

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = paketler;
                return result;
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        [HttpGet]
        [Route("alabilecegim/{firma}")]
        public ApiResult<List<PAKET>> AlabilecegimPaketler(int firma)
        {
            ApiResult<List<PAKET>> result = new ApiResult<List<PAKET>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<PAKET> paketler = _paketDA.GetAlabilecegimPaketler(firma);

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = paketler;
                return result;
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + ": " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                return result;
            }
        }

        [HttpGet]
        [Route("{id}")]
        public ApiResult<PAKET> ById(int id)
        {
            ApiResult<PAKET> result = new ApiResult<PAKET> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                PAKET paket = _paketDA.GetById(id);
                if (paket != null)
                {
                    result.STATUS = true;
                    result.MESSAGE = "Başarılı";
                    result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                    result.R = paket;
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait arac bulunmamaktadır.";
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

        [HttpPost]
        [Route("satin-al")]
        public ApiResult<FIRMAPAKET> Insert([FromBody] FIRMAPAKET fp)
        {
            ApiResult<FIRMAPAKET> result = new ApiResult<FIRMAPAKET> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (fp == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                FIRMAPAKET insertedFp = _paketDA.Buy(fp);
                if (insertedFp == null)
                {
                    result.MESSAGE = "Araç Eklenemedi.";
                    result.STATUS_CODE = ApiResponseCode.INSERT_ERROR.GetHashCode();
                    return result;
                }

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = insertedFp;
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
