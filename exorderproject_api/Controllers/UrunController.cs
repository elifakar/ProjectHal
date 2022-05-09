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
    //[RoutePrefix("urun")]
    public class UrunController : ApiController
    {
        readonly UrunDataAccess _urunDA = new UrunDataAccess();
        /// <summary>
        /// Firma id ye göre ürün bilgilerini listeler
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("urun/list/{firma}")]
        public ApiResult<List<URUN>> All(int firma)
        {
            ApiResult<List<URUN>> result = new ApiResult<List<URUN>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<URUN> urunler = _urunDA.GetAll(firma);

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = urunler;
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
