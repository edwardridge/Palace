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
                "~/Scripts/CreateGame/CreateGame.jsx"
                ));

            bundles.Add(new ScriptBundle("~/game").Include(
                "~/Scripts/Game/Card.jsx",
                "~/Scripts/Game/VisibleCardPile.jsx",
                "~/Scripts/Game/FaceDownCards.jsx",
                "~/Scripts/Game/GameStatus.jsx",
                "~/Scripts/Game/CannotPlay.jsx",
                "~/Scripts/Game/GameRules.jsx",
                "~/Scripts/Game/GameStatusForOpponents.jsx",
                "~/Scripts/Game/GameStatusForOpponent.jsx",
                "~/Scripts/Game/main.jsx"
                ));
        }
    }
}
