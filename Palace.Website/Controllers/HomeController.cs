using Palace.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Palace.Website.Controllers
{
    public class HomeController : Controller
    {
        IPalaceDocumentSessionFactory palaceSession;

        public HomeController(IPalaceDocumentSessionFactory palaceSession)
        {
            this.palaceSession = palaceSession;
        }

        public ActionResult Index()
        {
            using (var session = palaceSession.GetDocumentSession())
            {
                var games = session
                    .Query<Game>()
                    .OrderByDescending(o => o.State.DateSaved)
                    //.Take(10)
                    ;

                List<GameInfoModel> gameInfos = new List<GameInfoModel>();

                foreach(var game in games)
                {
                    foreach(var player in game.State.Players)
                    {
                        gameInfos.Add(new GameInfoModel()
                        {
                            GameId = game.Id.ToString(),
                            PlayerName = player.Name,
                            ValidMoves = game.State.NumberOfValdMoves
                        });
                    }
                }

                return View(gameInfos);
            }
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

        public ActionResult CreateGame()
        {
            return View();
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

        public int ValidMoves { get; set; }
    }
}