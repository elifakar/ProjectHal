using halproject_core;
using halproject_core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace halproject_client.Helpers
{
    public class ApiHelpers
    {
        readonly private string _localApiUrl = ConfigurationManager.AppSettings["LocalApiUrl"];
        readonly private string _apiUserName = ConfigurationManager.AppSettings["ApiUserName"];
        readonly private string _apiPassword = ConfigurationManager.AppSettings["ApiPassword"];

        private string ApiLogin()
        {
            try
            {
                string token = "";
                if (HttpContext.Current.Session["token"] != null)
                    token = HttpContext.Current.Session["token"].ToString();
                else
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_localApiUrl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var data = new ApiLogin()
                        {
                            username = _apiUserName,
                            password = _apiPassword
                        };

                        string reqUrl = _localApiUrl + "/token";
                        var response = client.PostAsync(reqUrl, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")).Result.Content;

                        token = JsonConvert.DeserializeObject<ApiResult<string>>(response.ReadAsStringAsync().Result).R;
                        HttpContext.Current.Session.Add("token", token);
                    }
                }
                return token;
            }
            catch
            {
                return "";
            }
        }

        public ApiResult<T> GetRequest<T>(string url)
        {
            string checkToken = ApiLogin();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_localApiUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("token", checkToken);

                string reqUrl = _localApiUrl + url;
                var response = client.GetAsync(reqUrl).Result.Content;
                var result = response.ReadAsStringAsync().Result;

                ApiResult<T> res = JsonConvert.DeserializeObject<ApiResult<T>>(result);

                if (res.STATUS_CODE == 100 || res.STATUS_CODE == 101 || res.STATUS_CODE == 102)
                {
                    HttpContext.Current.Session.Remove("token");
                    res = GetRequest<T>(url);
                }

                return res;
            }
        }

        public ApiResult<T> PostRequest<T>(string url, object data)
        {
            string checkToken = ApiLogin();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_localApiUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("token", checkToken);

                //var jsonData = JObject.FromObject(data);
                string reqUrl = _localApiUrl + url;
                var response = client.PostAsync(reqUrl, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")).Result.Content;

                ApiResult<T> res = JsonConvert.DeserializeObject<ApiResult<T>>(response.ReadAsStringAsync().Result);
                if (res.STATUS_CODE == 100 || res.STATUS_CODE == 101 || res.STATUS_CODE == 102)
                {
                    HttpContext.Current.Session.Remove("token");
                    res = PostRequest<T>(url, data);
                }

                return res;
            }
        }
    }
}