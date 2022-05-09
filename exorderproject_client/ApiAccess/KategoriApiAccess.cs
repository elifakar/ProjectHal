using exorderproject_client.Helpers;
using exorderproject_core;
using exorderproject_core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exorderproject_client.ApiAccess
{
    public class KategoriApiAccess
    {
        readonly ApiHelpers _api = new ApiHelpers();
        internal ApiResult<List<int>> GetForCache(int firmaId)
        {
            ApiResult<List<int>> apiResult = _api.GetRequest<List<int>>($"kategori/for-cache/{firmaId}/{1}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
        internal ApiResult<List<KATEGORI>> GetForList(int firmaId)
        {
            ApiResult<List<KATEGORI>> apiResult = _api.GetRequest<List<KATEGORI>>($"kategori/list/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
        internal ApiResult<KATEGORI> GetById(int id, int firmaId)
        {
            ApiResult<KATEGORI> apiResult = _api.GetRequest<KATEGORI>($"kategori/{id}/{firmaId}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<KATEGORI> Create(KATEGORI urun, int firmaId)
        {
            ApiResult<KATEGORI> apiResult = _api.PostRequest<KATEGORI>($"kategori/i/{firmaId}", urun);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<KATEGORI> Edit(KATEGORI urun, int firmaId, int updateCode)
        {
            ApiResult<KATEGORI> apiResult = _api.PostRequest<KATEGORI>($"kategori/u/{firmaId}/{updateCode}", urun);
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
    }
}