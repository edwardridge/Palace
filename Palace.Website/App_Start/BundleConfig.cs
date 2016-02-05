using System.Web;
using System.Web.Optimization;

namespace Palace.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/libraries").Include(
                      "~/Scripts/Libraries/jquery.js",
                      "~/Scripts/Libraries/underscore.js",
                      "~/Scripts/Libraries/react.js",
                      "~/Scripts/Libraries/react-dom.js",
                      "~/Scripts/Libraries/bootstrap.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Libraries/*.css",
                      "~/Content/Site/*.css"));

            bundles.Add(new ScriptBundle("~/creategame").Include(
                "~/Scripts/dist/CreateGame/CreateGame.js"
                ));

            bundles.Add(new ScriptBundle("~/game").Include(
                "~/Scripts/webpack_bundle.js"
                ));
        }
    }
}
