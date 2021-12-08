using halproject_api.DataAccess;
using halproject_core;
using halproject_core.DTO;
using halproject_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace halproject_api.Controllers
{
    public class LoginController : ApiController
    {
        readonly ApiDataAccess _apiDA = new ApiDataAccess();

        /// <summary>
        /// Api kullanıcısı olup-olmadıgı dogrular. Api kullanıcılarınınn  metotları kullanabilmesi için gerekli olan anahtarı (token) üretir
        /// </summary>
        /// <param name="apiLogin"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("token")]
        public ApiResult<string> Token([FromBody] ApiLogin apiLogin)
        {
            ApiResult<string> result = new ApiResult<string> { R = string.Empty, DATE = DateTime.Now, STATUS = false };

            try
            {
                if (apiLogin == null || string.IsNullOrEmpty(apiLogin.username) || string.IsNullOrEmpty(apiLogin.password))
                {
                    result.MESSAGE = "Kullanıcı adı/şifre boş geçilemez.";
                    result.STATUS_CODE = ApiResponseCode.MISSING_BODY_PARAMS.GetHashCode();
                    return result;
                }

                APIKULLANICI checkLogin = _apiDA.CheckApiUser(apiLogin.username, apiLogin.password);
                if (checkLogin != null)
                {
                    if (checkLogin.CONTINUE_TIME == 1)
                    {
                        string token = Guid.NewGuid().ToString().Replace("-", "");
                        int logId = _apiDA.CreateLog(new APIKULLANICILOG()
                        {
                            KULLANICI_ID = checkLogin.ID,
                            TOKEN = token,
                            TARIH = DateTime.Now,
                            SONTARIH = DateTime.Now.AddHours(2)
                        });

                        result.STATUS = true;
                        result.MESSAGE = "Başarılı";
                        result.STATUS_CODE = ApiResponseCode.SUCCESS.GetHashCode();
                        result.R = token;
                        return result;
                    }
                    else
                    {
                        result.MESSAGE = "Süresi devam eden token bulunmaktadır.";
                        result.STATUS_CODE = ApiResponseCode.CONTINUE_TOKEN.GetHashCode();
                        result.R = checkLogin.TOKEN;
                        return result;
                    }
                }
                else
                {
                    result.MESSAGE = "Kullanıcı adı/şifre hatalı.";
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
    }
}
