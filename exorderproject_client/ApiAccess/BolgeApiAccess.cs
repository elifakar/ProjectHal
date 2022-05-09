using exorderproject_client.Helpers;
using exorderproject_core;
using exorderproject_core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exorderproject_client.ApiAccess
{
    public class BolgeApiAccess
    {
        readonly ApiHelpers _api = new ApiHelpers();
        internal ApiResult<List<int>> GetForCache(int firmaId)
        {
            ApiResult<List<int>> apiResult = _api.GetRequest<List<int>>($"bolge/for-cache/{firmaId}/{1}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
        internal ApiResult<List<BOLGE>> GetForList(int firmaId)
        {
            ApiResult<List<BOLGE>> apiResult = _api.GetRequest<List<BOLGE>>($"bolge/list/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
        internal ApiResult<BOLGE> GetById(int id, int firmaId)
        {
            ApiResult<BOLGE> apiResult = _api.GetRequest<BOLGE>($"bolge/{id}/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<BOLGE> Create(BOLGE urun, int firmaId)
        {
            ApiResult<BOLGE> apiResult = _api.PostRequest<BOLGE>($"bolge/i/{firmaId}", urun);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<BOLGE> Edit(BOLGE urun, int firmaId, int updateCode)
        {
            ApiResult<BOLGE> apiResult = _api.PostRequest<BOLGE>($"bolge/u/{firmaId}/{updateCode}", urun);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
    }
}