using exorderproject_client.Helpers;
using exorderproject_core;
using exorderproject_core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exorderproject_client.ApiAccess
{
    public class PaketApiAccess
    {
        readonly ApiHelpers _api = new ApiHelpers();

        internal ApiResult<List<int>> GetForCache(int firmaId)
        {
            ApiResult<List<int>> apiResult = _api.GetRequest<List<int>>($"paket/for-cache/{firmaId}/{1}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<List<PAKET>> GetAldigimPaketler(int firmaId)
        {
            ApiResult<List<PAKET>> apiResult = _api.GetRequest<List<PAKET>>($"paket/aldigim/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<List<PAKET>> GetAlabilecegimPaketler(int firmaId)
        {
            ApiResult<List<PAKET>> apiResult = _api.GetRequest<List<PAKET>>($"paket/alabilecegim/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<PAKET> GetById(int id)
        {
            ApiResult<PAKET> apiResult = _api.GetRequest<PAKET>($"paket/{id}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<FIRMAPAKET> SatinAl(FIRMAPAKET fp)
        {
            ApiResult<FIRMAPAKET> apiResult = _api.PostRequest<FIRMAPAKET>($"paket/satin-al", fp);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
    }
}