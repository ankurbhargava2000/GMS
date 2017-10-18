using System.Web;
using System.Web.Optimization;

namespace StockManager
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                        "~/Scripts/js/jquery.js",
                        "~/Scripts/js/jquery-1.8.3.min.js",
                        "~/Scripts/js/bootstrap.min.js",
                        "~/Scripts/js/jquery.dcjqaccordion.2.7.js",
                        "~/Scripts/js/jquery.scrollTo.min.js",
                        "~/Scripts/js/jquery.nicescroll.js",
                        "~/Scripts/js/jquery.sparkline.js",
                        "~/Scripts/js/common-scripts.js",
                        "~/Scripts/js/gritter/js/jquery.gritter.js",
                        "~/Scripts/js/gritter-conf.js",
                        "~/Scripts/js/sparkline-chart.js",
                        "~/Scripts/js/zabuto_calendar.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/DatePicker").Include(
                       "~/Scripts/js/bootstrap-datetimepicker.js",
                       "~/Scripts/js/locales/bootstrap-datetimepicker.fr.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            "~/Scripts/jquery.unobtrusive*",
            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/css/zabuto_calendar.css",
                      "~/Scripts/js/gritter/css/jquery.gritter.css",
                      "~/Content/lineicons/style.css",
                      "~/Content/css/style.css",
                      "~/Content/css/style-responsive.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/DateCSS").Include(                      
                      "~/Content/css/bootstrap-datetimepicker.css"
                      ));
        }
    }
}
