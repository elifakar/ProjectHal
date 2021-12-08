using System.Web.Http;
using System.Web.Mvc;

namespace halproject_api.Areas.HelpPage
{
    public class HelpPageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HelpPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
             /*context.MapRoute(
                "HelpPage_Default",
                "Help/{action}/{apiId}",
                new { controller = "Help", action = "Index", apiId = UrlParameter.Optional });

            HelpPageConfig.Register(GlobalConfiguration.Configuration);*/

            context.MapRoute(
            "HelpPage_Default",
            "Help/{action}/{apiId}",
            new { controller = "Help", action = "Index", apiId = UrlParameter.Optional });
            context.MapRoute(
                "Help Area",
                "",
                new { controller = "Help", action = "Index" }
                );
            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}