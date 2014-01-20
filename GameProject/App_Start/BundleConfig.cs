using System.Web;
using System.Web.Optimization;

namespace GameProject
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/timer_countdown").Include(
                      "~/Scripts/jquery.countdown.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/admin-css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/admin-site.css"));
            bundles.Add(new ScriptBundle("~/bundles/responsiveslides").Include(
          "~/Scripts/responsiveslides.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jq.easing").Include(
                      "~/Scripts/jquery.easing.1.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/jq.elastis").Include(
                      "~/Scripts/jquery.elastislide.js"));

            bundles.Add(new StyleBundle("~/Content/skeleton").Include(
                      "~/Content/base.css",
                      "~/Content/skeleton.css",
                      "~/Content/layout.css"));
        }
    }
}
