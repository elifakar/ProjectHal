using exorderproject_client.Helpers;
using exorderproject_core;
using exorderproject_core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exorderproject_client.ApiAccess
{
    public class AltBolgeApiAccess
    {
        readonly ApiHelpers _api = new ApiHelpers();
        internal ApiResult<List<int>> GetForCache(int firmaId)
        {
            ApiResult<List<int>> apiResult = _api.GetRequest<List<int>>($"altbolge/for-cache/{firmaId}/{1}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
        internal ApiResult<List<ALTBOLGE>> GetForList(int firmaId)
        {
            ApiResult<List<ALTBOLGE>> apiResult = _api.GetRequest<List<ALTBOLGE>>($"altbolge/list/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
        internal ApiResult<ALTBOLGE> GetById(int id, int firmaId)
        {
            ApiResult<ALTBOLGE> apiResult = _api.GetRequest<ALTBOLGE>($"altbolge/{id}/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<ALTBOLGE> Create(ALTBOLGE urun, int firmaId)
        {
            ApiResult<ALTBOLGE> apiResult = _api.PostRequest<ALTBOLGE>($"altbolge/i/{firmaId}", urun);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<ALTBOLGE> Edit(ALTBOLGE urun, int firmaId, int updateCode)
        {
            ApiResult<ALTBOLGE> apiResult = _api.PostRequest<ALTBOLGE>($"altbolge/u/{firmaId}/{updateCode}", urun);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
    }
}