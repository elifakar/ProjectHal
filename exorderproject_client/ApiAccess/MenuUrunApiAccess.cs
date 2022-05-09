using exorderproject_client.Helpers;
using exorderproject_core;
using exorderproject_core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exorderproject_client.ApiAccess
{
    public class MenuUrunApiAccess
    {
        readonly ApiHelpers _api = new ApiHelpers();
        internal ApiResult<List<int>> GetForCache(int firmaId)
        {
            ApiResult<List<int>> apiResult = _api.GetRequest<List<int>>($"menuurun/for-cache/{firmaId}/{1}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
        internal ApiResult<List<URUN>> GetForList(int firmaId)
        {
            ApiResult<List<URUN>> apiResult = _api.GetRequest<List<URUN>>($"menuurun/list/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
        internal ApiResult<URUN> GetById(int id, int firmaId)
        {
            ApiResult<URUN> apiResult = _api.GetRequest<URUN>($"menuurun/{id}/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<URUN> Create(URUN urun, int firmaId)
        {
            ApiResult<URUN> apiResult = _api.PostRequest<URUN>($"menuurun/i/{firmaId}", urun);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<URUN> Edit(URUN urun, int firmaId, int updateCode)
        {
            ApiResult<URUN> apiResult = _api.PostRequest<URUN>($"menuurun/u/{firmaId}/{updateCode}", urun);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
    }
}