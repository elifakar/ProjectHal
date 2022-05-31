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
    [RoutePrefix("urun")]
    public class ProductController : ApiController
    {
        readonly UrunDataAccess _urunDA = new UrunDataAccess();

        /// <summary>
        /// Firma id ye göre ürün bilgilerini listeler
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list/{firma}")]
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

        /// <summary>
        /// Belirtilen ürün id ye ait ürünün bilgilerini listeler.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/{firma}")]
        public ApiResult<URUN> ById(int id, int firma)
        {
            ApiResult<URUN> result = new ApiResult<URUN> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                URUN surucu = _urunDA.GetById(id);
                if (surucu != null)
                {
                    if (surucu.PRODUCT_C_ID != firma)
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
                    result.MESSAGE = "Girdiğiniz id değerine ait ürün bulunmamaktadır.";
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
        /// Yeni ürün ekler
        /// </summary>
        /// <param name="urun"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("i/{firma}")]
        public ApiResult<URUN> Insert([FromBody] URUN urun, int firma)
        {
            ApiResult<URUN> result = new ApiResult<URUN> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (urun == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (urun.PRODUCT_C_ID != firma)
                {
                    result.MESSAGE = "Bu işlem için yetkiniz yok.";
                    result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                    return result;
                }

                URUN insertedSurucu = _urunDA.Insert(urun);
                if (insertedSurucu == null)
                {
                    result.MESSAGE = "Ürün Eklenemedi.";
                    result.STATUS_CODE = ApiResponseCode.INSERT_ERROR.GetHashCode();
                    return result;
                }

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = insertedSurucu;
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
        ///  Body parametresinde belirtilen ürün id ye ait ürün bilgilerini günceller
        /// </summary>
        /// <param name="urun"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <param name="updateCode"> [Sürücü Genel Bilgiler Code=4,(Adı,Soyadı,Tc Kimlik No,Cinsiyet,Hes Kodu,GSM,Ehliyet Sınıfı, Doğum Tarihi, Eğitim Durumu, Kan Grubu)] , [Sürücü Şirket Çalışanı Bilgileri Code=5,(Şirket Çalışanı Durumu,Medeni Durum, Çocuk Sayısı, SGK No)]</param>
        /// <returns></returns>
        [HttpPost]
        [Route("u/{firma}/{updateCode}")]
        public ApiResult<URUN> Update([FromBody] URUN urun, int firma, int updateCode)
        {
            ApiResult<URUN> result = new ApiResult<URUN> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (urun == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                URUN updatedSurucu = null; ;
                if (_urunDA.GetById(urun.PRODUCT_ID) != null)
                {
                    if (urun.PRODUCT_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    if (updateCode == UpdateCodes.URUN_GENEL.GetHashCode())
                    {
                        updatedSurucu = _urunDA.Update(urun);
                    }
                    else if (updateCode == UpdateCodes.URUN_TEDARIKCI.GetHashCode())
                    {
                        updatedSurucu = _urunDA.UpdateTedBilgileri(urun);
                    }
                    if (updatedSurucu == null)
                    {
                        result.MESSAGE = "Ürün Güncellenemedi.";
                        result.STATUS_CODE = ApiResponseCode.UPDATE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait ürün bulunmamaktadır.";
                    result.STATUS_CODE = ApiResponseCode.NOT_EXIST.GetHashCode();
                    return result;
                }

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = updatedSurucu;
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
        /// Body parametresinde belirtilen ürün id ye ait kaydı siler.
        /// </summary>
        /// <param name="urun"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("d/{firma}")]
        public ApiResult<bool> Delete([FromBody] URUN urun, int firma)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (urun == null)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (_urunDA.GetById(urun.PRODUCT_ID) != null)
                {
                    if (urun.PRODUCT_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    bool deleteResult = _urunDA.Delete(urun.PRODUCT_ID);
                    if (deleteResult == false)
                    {
                        result.MESSAGE = "Ürün Silinemedi.";
                        result.STATUS_CODE = ApiResponseCode.DELETE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait ürün bulunmamaktadır.";
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

        /// <summary>
        /// Body parametresinde belirtilen ürün id ye ait stok bilgilerini işlem sırasına göre getirir.
        /// </summary>
        /// <param name="id">Id ürün id için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpGet]
        [Route("stoklist/{id}")]
        public ApiResult<List<STOK>> StockInformation(int id)
        {
            ApiResult<List<STOK>> result = new ApiResult<List<STOK>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<STOK> stokBilgileri = _urunDA.StockInformation(id);

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = stokBilgileri;
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
        /// Body parametresinde belirtilen stok id ye ait stok bilgilerini siler.
        /// </summary>
        /// <param name="stok"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("dstok/{firma}")]
        public ApiResult<bool> StokDelete([FromBody] STOK stok, int firma)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (stok == null)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (_urunDA.GetById(stok.STOCK_PRODUCT_ID) != null)
                {
                    if (stok.STOCK_PRODUCT_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    bool deleteResult = _urunDA.StockDelete(stok.STOCK_ID);
                    if (deleteResult == false)
                    {
                        result.MESSAGE = "Stok Silinemedi.";
                        result.STATUS_CODE = ApiResponseCode.DELETE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait stok bulunmamaktadır.";
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

        /// <summary>
        /// Body parametresinde belirtilen ürün id ye ait ürüne stok girişi sağlar.
        /// </summary>
        /// <param name="stok"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("stok/add/{firma}")]
        public ApiResult<bool> StokEkle([FromBody] STOK stok, int firma)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (stok == null)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (_urunDA.GetById(stok.STOCK_PRODUCT_ID) != null)
                {
                    if (stok.STOCK_PRODUCT_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    bool deleteResult = _urunDA.StockDelete(stok.STOCK_ID);
                    if (deleteResult == false)
                    {
                        result.MESSAGE = "Stok Silinemedi.";
                        result.STATUS_CODE = ApiResponseCode.DELETE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait stok bulunmamaktadır.";
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

        /// <summary>
        /// Body parametresinde belirtilen ürün id ye ait ürüne stok çıkışı sağlar.
        /// </summary>
        /// <param name="stok"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("stok/drop/{firma}")]
        public ApiResult<bool> StokDus([FromBody] STOK stok, int firma)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (stok == null)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (_urunDA.GetById(stok.STOCK_PRODUCT_ID) != null)
                {
                    if (stok.STOCK_PRODUCT_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    bool deleteResult = _urunDA.StockDelete(stok.STOCK_ID);
                    if (deleteResult == false)
                    {
                        result.MESSAGE = "Stok Silinemedi.";
                        result.STATUS_CODE = ApiResponseCode.DELETE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait stok bulunmamaktadır.";
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
