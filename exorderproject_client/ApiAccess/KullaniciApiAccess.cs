using exorderproject_client.Helpers;
using exorderproject_core;
using exorderproject_core.DTO;
using exorderproject_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exorderproject_client.ApiAccess
{
    public class KullaniciApiAccess
    {
        readonly ApiHelpers _api = new ApiHelpers();

        internal bool IsKullaniciHaveFirma(int kullaniciId)
        {
            ApiResult<FIRMA> apiResult = _api.GetRequest<FIRMA>($"firma/sorumlu/{kullaniciId}");
            return apiResult.STATUS_CODE != (int)ApiResponseCode.NOT_EXIST;
        }

        internal ApiResult<KULLANICI> GirisYap(Login login)
        {
            ApiResult<KULLANICI> apiResult = _api.PostRequest<KULLANICI>("kullanici/login", login);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<KULLANICI> GetById(int kullaniciId, int firmaId)
        {
            ApiResult<KULLANICI> apiResult = _api.GetRequest<KULLANICI>($"kullanici/{kullaniciId}/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<KULLANICI> KayitOl(Register register)
        {
            ApiResult<KULLANICI> apiResult = _api.PostRequest<KULLANICI>("kullanici/register", register);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<KULLANICI> Update(KULLANICI kullanici, int firmaId)
        {
            ApiResult<KULLANICI> apiResult = _api.PostRequest<KULLANICI>($"kullanici/u/{firmaId}", kullanici);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<bool> SifremiUnuttum(ForgotPassword forgotPassword)
        {
            ApiResult<bool> apiResult = _api.PostRequest<bool>("kullanici/forgotpassword", forgotPassword);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<bool> SifreSifirla(ResetPassword resetPassword)
        {
            ApiResult<bool> apiResult = _api.PostRequest<bool>("kullanici/resetpassword", resetPassword);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<bool> SifreGuncelle(UpdatePassword updatePassword)
        {
            ApiResult<bool> apiResult = _api.PostRequest<bool>("kullanici/updatepassword", updatePassword);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
    }
}