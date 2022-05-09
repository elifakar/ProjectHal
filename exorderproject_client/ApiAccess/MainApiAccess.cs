using exorderproject_client.Helpers;
using exorderproject_core;
using exorderproject_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exorderproject_client.ApiAccess
{
    public class MainApiAccess
    {
        readonly ApiHelpers _api = new ApiHelpers();

        internal ApiResult<List<CATEGORY>> GetCategory(int id)
        {
            ApiResult<List<CATEGORY>> apiResult = _api.GetRequest<List<CATEGORY>>($"main/kategori/{id}");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<List<UNIT>> GetUnit()
        {
            ApiResult<List<UNIT>> apiResult = _api.GetRequest<List<UNIT>>($"main/birim");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }

        internal ApiResult<List<STATUS>> GetStatus()
        {
            ApiResult<List<STATUS>> apiResult = _api.GetRequest<List<STATUS>>($"main/durum");
            return apiResult.STATUS ? apiResult : throw new Exception(apiResult.MESSAGE);
        }
    }
}