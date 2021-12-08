using halproject_api.DataAccess;
using halproject_core;
using halproject_core.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace halproject_api.Helpers
{
    public class RequestControl : DelegatingHandler
    {
        readonly ApiDataAccess _apiDA = new ApiDataAccess();
        readonly LogDataAccess _logDA = new LogDataAccess();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ApiResult<string> result = new ApiResult<string>() { R = null, STATUS = false, DATE = DateTime.Now };
            try
            {
                if (!request.RequestUri.ToString().Contains("token"))
                {
                    var checkToken = request.Headers.TryGetValues("Token", out IEnumerable<string> requestHeaderTokens);

                    string token = string.Empty;
                    if (checkToken)
                    {
                        token = requestHeaderTokens.FirstOrDefault();
                        APIKULLANICILOG log = _apiDA.GetApiLoginByToken(token);
                        if (log != null)
                        {
                            if (log.SONTARIH < DateTime.Now)
                            {
                                result.MESSAGE = ApiResponseCode.TOKEN_TIMEOUT.ToString();
                                result.STATUS_CODE = ApiResponseCode.TOKEN_TIMEOUT.GetHashCode();
                                result.STATUS = false;
                                return request.CreateResponse<ApiResult<string>>(result);
                            }
                        }
                        else
                        {
                            result.MESSAGE = ApiResponseCode.WRONG_TOKEN.ToString();
                            result.STATUS_CODE = ApiResponseCode.WRONG_TOKEN.GetHashCode();
                            result.STATUS = false;
                            return request.CreateResponse<ApiResult<string>>(result);
                        }
                    }
                    else
                    {
                        result.MESSAGE = ApiResponseCode.MISSING_TOKEN.ToString();
                        result.STATUS_CODE = ApiResponseCode.MISSING_TOKEN.GetHashCode();
                        result.STATUS = false;
                        return request.CreateResponse<ApiResult<string>>(result);
                    }
                }

                APILOG apiLog = BuildRequest(request);
                var responseLogin = await base.SendAsync(request, cancellationToken);
                apiLog = BuildResponse(apiLog, responseLogin);
                await _logDA.Insert(apiLog);

                return responseLogin;
            }
            catch (Exception ex)
            {
                result.MESSAGE = ApiResponseCode.SERVIS_EXCEPTION.ToString() + " " + ex.Message;
                result.STATUS_CODE = ApiResponseCode.SERVIS_EXCEPTION.GetHashCode();
                result.STATUS = false;
                return request.CreateResponse<ApiResult<string>>(result);
            }
        }

        private APILOG BuildRequest(HttpRequestMessage request)
        {
            APILOG apiLog = new APILOG()
            {
                REQ_IP= GetClientIp(request),
                REQ_METHOD = request.Method.Method,
                REQ_DATE = DateTime.Now,
                REQ_URL = request.RequestUri.ToString()
            };
            return apiLog;
        }

        private APILOG BuildResponse(APILOG apiLog, HttpResponseMessage responseLogin)
        {
            var jsonResponse = responseLogin.Content.ReadAsStringAsync();
            ApiResult<object> response = JsonConvert.DeserializeObject<ApiResult<object>>(jsonResponse.Result);

            apiLog.RES_DATE = response.DATE == DateTime.MinValue ? DateTime.Now : response.DATE;
            apiLog.RES_MESSAGE = response.MESSAGE;
            apiLog.RES_STATUS = response.STATUS;
            apiLog.RES_STATUS_CODE = response.STATUS_CODE;
            apiLog.RES_R = JsonConvert.SerializeObject(response.R);

            return apiLog;
        }

        private string GetClientIp(HttpRequestMessage request = null)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
                return HttpContext.Current.Request.UserHostAddress;
            else
                return null;
        }
    }
}