using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace exorderproject_client.Controllers
{
    [RoutePrefix("error")]
    public class ErrorController : Controller
    {
        public ActionResult PageError()
        {
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        [Route("403")]
        public ActionResult Page403(string errorpath)
        {
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;
            if (!string.IsNullOrEmpty(errorpath))
                ViewBag.Kaynak = errorpath;

            return View("PageError");
        }

        [Route("404")]
        public ActionResult Page404(string errorpath)
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            if (!string.IsNullOrEmpty(errorpath))
                ViewBag.Kaynak = errorpath;

            return View("PageError");
        }

        [Route("500")]
        public ActionResult Page500(string errorpath)
        {
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;
            if (!string.IsNullOrEmpty(errorpath))
                ViewBag.Kaynak = errorpath;

            return View("PageError");
        }
    }
}