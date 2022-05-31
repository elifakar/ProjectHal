using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Mvc;
using exorderproject_api.Areas.HelpPage.ModelDescriptions;
using exorderproject_api.Areas.HelpPage.Models;

namespace exorderproject_api.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult Login(string txtusername,string txtpassword)
        {
            Session["apiLoginStatus"] = "Ok";
            SqlConnection con;
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            //using (con = new SqlConnection(connectionString))
            //{
            //    string sqlQuery = $@"SELECT * FROM TAPIKULLANICI WHERE APIKULLANICI_KULLANICI_ADI = @APIKULLANICI_KULLANICI_ADI AND APIKULLANICI_SIFRE=@APIKULLANICI_SIFRE";

            //    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            //    {
            //        con.Open();
            //        cmd.Parameters.AddWithValue("@APIKULLANICI_KULLANICI_ADI", txtusername);
            //        cmd.Parameters.AddWithValue("@APIKULLANICI_SIFRE", txtpassword);
            //        int count = Convert.ToInt32(cmd.ExecuteScalar());
            //        Session["apiLoginStatus"] = count > 0 ? "Ok" : null;

            //        con.Close();
            //    }
            //}
            return RedirectToAction("Index", "Help");
        }

        public ActionResult Logout()
        {
            Session.Remove("apiLoginStatus");
            return RedirectToAction("Index", "Help");
        }
    }
}