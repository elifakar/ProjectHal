using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace halproject_client
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/wizard-and-notification")
                .Include("~/Content/assets/vendor/jquery/jquery.js",
                         "~/Content/assets/vendor/bootstrap/js/bootstrap.js",
                         "~/Content/assets/vendor/jquery-validation/jquery.validate.min.js",
                         "~/Content/assets/vendor/bootstrap-wizard/jquery.bootstrap.wizard.js",
                         "~/Content/assets/js/examples/examples.wizard.js"));

            bundles.Add(new ScriptBundle("~/bundles/masked-date-for-partial-and-dropdown-searchable-field")
                .Include("~/Content/assets/js/theme.init.js"));

           /* bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/plugins/font-awesome/css/font-awesome.min.css",
                         "~/dist/css/adminlte.min.css")); */
        }
    }
}