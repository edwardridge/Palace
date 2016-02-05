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
                "~/Scripts/dist/Game/Card.js",
                "~/Scripts/dist/Game/VisibleCardPile.js",
                "~/Scripts/dist/Game/FaceDownCards.js",
                "~/Scripts/dist/Game/GameStatus.js",
                "~/Scripts/dist/Game/CannotPlay.js",
                "~/Scripts/dist/Game/GameRules.js",
                "~/Scripts/dist/Game/GameStatusForOpponents.js",
                "~/Scripts/dist/Game/GameStatusForOpponent.js",
                "~/Scripts/dist/Game/main.js"
                ));
        }
    }
}
