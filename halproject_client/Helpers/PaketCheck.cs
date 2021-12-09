using halproject_client.ApiAccess;
using halproject_core;
using halproject_core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace halproject_client.Helpers
{
    public class PaketCheck : ActionFilterAttribute
    {
        InMemoryCache _cache = new InMemoryCache();
        PaketApiAccess _paketAA = new PaketApiAccess();

        public PAKETLER Paket1 { get; set; }
        public PAKETLER Paket2 { get; set; }
        public PAKETLER Paket3 { get; set; }
        public PAKETLER Paket4 { get; set; }
        public PAKETLER Paket5 { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            if (session != null && session["FIRMA_ID"] != null)
            {
                int kullaniciId = session["KULLAMICI_ID"].milToInt32();
                int firmaId = session["FIRMA_ID"].milToInt32();

                List<int> paketler = _cache.GetOrSet("paketler", () => _paketAA.GetForCache(firmaId).R);

                if (paketler.Count <= 0)
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Paket" }, { "Action", "Index" } });
                else
                {
                    if (
                        Paket1 != 0 && !paketler.Contains((int)Paket1) ||
                        Paket2 != 0 && !paketler.Contains((int)Paket2) ||
                        Paket3 != 0 && !paketler.Contains((int)Paket3) ||
                        Paket4 != 0 && !paketler.Contains((int)Paket4) ||
                        Paket5 != 0 && !paketler.Contains((int)Paket5)
                        )
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "Action", "Index" } });
                }
            }
        }
    }
}