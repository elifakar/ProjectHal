using exorderproject_client.Helpers;
using exorderproject_core;
using exorderproject_core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exorderproject_client.ApiAccess
{
    public class CalisanApiAccess
    {
        readonly ApiHelpers _api = new ApiHelpers();
        internal ApiResult<List<int>> GetForCache(int firmaId)
        {
            ApiResult<List<int>> apiResult = _api.GetRequest<List<int>>($"calisan/for-cache/{firmaId}/{1}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
        internal ApiResult<List<CALISAN>> GetForList(int firmaId)
        {
            ApiResult<List<CALISAN>> apiResult = _api.GetRequest<List<CALISAN>>($"calisan/list/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
        internal ApiResult<CALISAN> GetById(int id, int firmaId)
        {
            ApiResult<CALISAN> apiResult = _api.GetRequest<CALISAN>($"calisan/{id}/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<CALISAN> Create(CALISAN urun, int firmaId)
        {
            ApiResult<CALISAN> apiResult = _api.PostRequest<CALISAN>($"calisan/i/{firmaId}", urun);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<CALISAN> Edit(CALISAN urun, int firmaId, int updateCode)
        {
            ApiResult<CALISAN> apiResult = _api.PostRequest<CALISAN>($"calisan/u/{firmaId}/{updateCode}", urun);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
    }
}