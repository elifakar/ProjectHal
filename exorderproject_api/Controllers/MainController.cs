using exorderproject_api.DataAccess;
using exorderproject_core;
using exorderproject_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace exorderproject_api.Controllers
{
    [RoutePrefix("main")]
    public class MainController : ApiController
    {
        readonly MainDataAccess _mainDA = new MainDataAccess();

        /// <summary>
        /// Durum tablosundaki tüm kayıtları listeler (Aktif, Pasif)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all-kategori/{id}")]
        public ApiResult<List<CATEGORY>> AllKategori(int id)
        {
            ApiResult<List<CATEGORY>> result = new ApiResult<List<CATEGORY>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<CATEGORY> kategoriler = _mainDA.GetAllKategori(id);

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = kategoriler;
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
        [Route("kategori/{id}")]
        public ApiResult<List<CATEGORY>> AktifKategori(int id)
        {
            ApiResult<List<CATEGORY>> result = new ApiResult<List<CATEGORY>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<CATEGORY> kategoriler = _mainDA.GetAllKategori(id);

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = kategoriler;
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
        [Route("birim")]
        public ApiResult<List<UNIT>> Birim()
        {
            ApiResult<List<UNIT>> result = new ApiResult<List<UNIT>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<UNIT> birimler = _mainDA.GetAllUnit();

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = birimler;
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
        [Route("durum")]
        public ApiResult<List<STATUS>> Durum()
        {
            ApiResult<List<STATUS>> result = new ApiResult<List<STATUS>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<STATUS> durumlar = _mainDA.GetAllStatus();

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = durumlar;
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
