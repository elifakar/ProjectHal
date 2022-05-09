using exorderproject_client.ApiAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace exorderproject_client.Helpers
{
    public class SessionCheck : ActionFilterAttribute
    {
        private readonly KullaniciApiAccess _kullaniciAA = new KullaniciApiAccess();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            string controller = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();

            if (session != null && session["KULLANICI_ID"] == null)
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Kullanici" }, { "Action", "GirisYap" } });
            else if (session != null && session["FIRMA_ID"] == null)
            {
                bool checkKullaniciFirma = _kullaniciAA.IsKullaniciHaveFirma((int)session["KULLANICI_ID"]);
                if (!checkKullaniciFirma && (controller != "Firma" || action != "Create") && (controller != "Firma" || action != "CreateF"))
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Firma" }, { "Action", "Create" } });
            }
        }
    }
}