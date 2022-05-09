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
    [RoutePrefix("kategori")]
    public class CategoryController : ApiController
    {
        readonly KategoriDataAccess _kategoriDA = new KategoriDataAccess();

        /// <summary>
        /// Firma id ye göre kategori bilgilerini listeler
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list/{firma}")]
        public ApiResult<List<KATEGORI>> All(int firma)
        {
            ApiResult<List<KATEGORI>> result = new ApiResult<List<KATEGORI>> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                List<KATEGORI> kategoriler = _kategoriDA.GetAll(firma);

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

        /// <summary>
        /// Belirtilen kategori id ye ait kategorinin bilgilerini listeler.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/{firma}")]
        public ApiResult<KATEGORI> ById(int id, int firma)
        {
            ApiResult<KATEGORI> result = new ApiResult<KATEGORI> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                KATEGORI surucu = _kategoriDA.GetById(id);
                if (surucu != null)
                {
                    if (surucu.CATEGORY_C_ID != firma)
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
                    result.MESSAGE = "Girdiğiniz id değerine ait kategori bulunmamaktadır.";
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
        /// Yeni kategori ekler
        /// </summary>
        /// <param name="kategori"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("i/{firma}")]
        public ApiResult<KATEGORI> Insert([FromBody] KATEGORI kategori, int firma)
        {
            ApiResult<KATEGORI> result = new ApiResult<KATEGORI> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (kategori == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (kategori.CATEGORY_C_ID != firma)
                {
                    result.MESSAGE = "Bu işlem için yetkiniz yok.";
                    result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                    return result;
                }

                KATEGORI insertedKategori = _kategoriDA.Insert(kategori);
                if (insertedKategori == null)
                {
                    result.MESSAGE = "Kategori Eklenemedi.";
                    result.STATUS_CODE = ApiResponseCode.INSERT_ERROR.GetHashCode();
                    return result;
                }

                result.STATUS = true;
                result.MESSAGE = "Başarılı";
                result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                result.R = insertedKategori;
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
        ///  Body parametresinde belirtilen kategori id ye ait kategori bilgilerini günceller
        /// </summary>
        /// <param name="kategori"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <param name="updateCode"> [Sürücü Genel Bilgiler Code=4,(Adı,Soyadı,Tc Kimlik No,Cinsiyet,Hes Kodu,GSM,Ehliyet Sınıfı, Doğum Tarihi, Eğitim Durumu, Kan Grubu)] , [Sürücü Şirket Çalışanı Bilgileri Code=5,(Şirket Çalışanı Durumu,Medeni Durum, Çocuk Sayısı, SGK No)]</param>
        /// <returns></returns>
        [HttpPost]
        [Route("u/{firma}/{updateCode}")]
        public ApiResult<KATEGORI> Update([FromBody] KATEGORI kategori, int firma, int updateCode)
        {
            ApiResult<KATEGORI> result = new ApiResult<KATEGORI> { R = null, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (kategori == null || !ModelState.IsValid)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                KATEGORI updatedSurucu = null;
                if (_kategoriDA.GetById(kategori.CATEGORY_ID) != null)
                {
                    if (kategori.CATEGORY_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    if (updateCode == UpdateCodes.KATEGORI_GENEL.GetHashCode())
                    {
                        updatedSurucu = _kategoriDA.Update(kategori);
                    }
                    
                    if (updatedSurucu == null)
                    {
                        result.MESSAGE = "Kategori Güncellenemedi.";
                        result.STATUS_CODE = ApiResponseCode.UPDATE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait kategori bulunmamaktadır.";
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
        /// Body parametresinde belirtilen kategori id ye ait kaydı siler.
        /// </summary>
        /// <param name="kategori"></param>
        /// <param name="firma">Firma id yetki kontrolü için kullanılmaktadır</param>
        /// <returns></returns>
        [HttpPost]
        [Route("d/{firma}")]
        public ApiResult<bool> Delete([FromBody] KATEGORI kategori, int firma)
        {
            ApiResult<bool> result = new ApiResult<bool> { R = false, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (kategori == null)
                {
                    result.MESSAGE = "Zorunlu alanları doldurunuz.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                if (_kategoriDA.GetById(kategori.CATEGORY_ID) != null)
                {
                    if (kategori.CATEGORY_C_ID != firma)
                    {
                        result.MESSAGE = "Bu işlem için yetkiniz yok.";
                        result.STATUS_CODE = ApiResponseCode.UNAUTHORIZED.GetHashCode();
                        return result;
                    }

                    bool deleteResult = _kategoriDA.Delete(kategori.CATEGORY_ID);
                    if (deleteResult == false)
                    {
                        result.MESSAGE = "Kategori Silinemedi.";
                        result.STATUS_CODE = ApiResponseCode.DELETE_ERROR.GetHashCode();
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Girdiğiniz id değerine ait kategori bulunmamaktadır.";
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
