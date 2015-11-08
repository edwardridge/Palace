using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Palace.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Game(string playerName, string gameId)
        {
            GameInfoModel gameInfoForModel = new GameInfoModel
            {
                GameId = gameId,
                PlayerName = playerName
            };

            return View(gameInfoForModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class GameInfoModel
    {
        public string PlayerName { get; set; }

        public string GameId { get; set; }
    }
}