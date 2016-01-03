using Palace.Repository;
using Palace.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
namespace Palace.Api.Controllers
{
    //Todo: Replace!
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        public GameController()
        {
        }

        [Route("createstandardgame"), HttpGet]
        public Guid CreateStandardGame()
        {
            var player1 = new Player("Ed");
            var player2 = new Player("Liam");

            var rules = new RulesForGame();

            rules.Add(new Rule(CardValue.Ten, RuleForCard.Burn));
            rules.Add(new Rule(CardValue.Two, RuleForCard.Reset));
            rules.Add(new Rule(CardValue.Seven, RuleForCard.LowerThan));
            rules.Add(new Rule(CardValue.Ace, RuleForCard.ReverseOrderOfPlay));
            rules.Add(new Rule(CardValue.Eight, RuleForCard.SeeThrough));
            rules.Add(new Rule(CardValue.Jack, RuleForCard.SkipPlayer));

            var dealer = new Dealer(StandardDeck.CreateDeck(), new DefaultStartGameRules(), rules);

            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);

            var gameInit = dealer.CreateGameInitialisation();
            
            gameInit.DealInitialCards();

            gameInit.PutCardFaceUp(player1, player1.CardsInHand.ToArray()[0]);
            gameInit.PutCardFaceUp(player1, player1.CardsInHand.ToArray()[1]);
            gameInit.PutCardFaceUp(player1, player1.CardsInHand.ToArray()[2]);

            gameInit.PutCardFaceUp(player2, player2.CardsInHand.ToArray()[0]);
            gameInit.PutCardFaceUp(player2, player2.CardsInHand.ToArray()[1]);
            gameInit.PutCardFaceUp(player2, player2.CardsInHand.ToArray()[2]);

            var game = gameInit.StartGame();
            var gameRepo = new GameRepository(new PalaceDocumentSession());
            gameRepo.Save(game);
            
            return game.Id;
        }
        
        [Route("get/{id}/{player}")]
        public Result GetGameState(string id, string player)
        {
            var state = new GameRepository(new PalaceDocumentSession()).Open(id).State;
            var result = new Result(state.Players.First(f => f.Name == player), state);
            return result;  
        }

        [Route("getrules/{id}")]
        public RulesForGame GetGameRules(string id)
        {
            var rules = new GameRepository(new PalaceDocumentSession()).Open(id).Rules;
            return rules;
        }

        [Route("playinhandcard/{gameid}/{playername}"), HttpPost]
        public Result PlayInHandCard(string gameId, string playerName, IEnumerable<Card> card)
        {
            var gameRepo = new GameRepository(new PalaceDocumentSession());
            var game = gameRepo.Open(gameId);
            var result = game.PlayInHandCards(playerName, card.ToArray());
            gameRepo.Save(game);
            return result;
        }

        [Route("playfaceupcard/{gameid}/{playername}"), HttpPost]
        public Result PlayFaceUpCard(string gameId, string playerName, IEnumerable<Card> card)
        {
            var gameRepo = new GameRepository(new PalaceDocumentSession());
            var game = gameRepo.Open(gameId);
            var result = game.PlayFaceUpCards(playerName, card.ToArray());
            gameRepo.Save(game);
            return result;
        }

        [Route("playfacedowncard/{gameid}/{playername}"), HttpPost]
        public Result PlayFaceDownCard(string gameId, string playerName)
        {
            var gameRepo = new GameRepository(new PalaceDocumentSession());
            var game = gameRepo.Open(gameId);
            var result = game.PlayFaceDownCards(playerName, game.State.Players.First(f=>f.Name == playerName).CardsFaceDown.ToArray()[0]);
            gameRepo.Save(game);
            return result;
        }

        [Route("cannotplaycard/{gameid}/{playername}"), HttpPost]
        public Result CannotPlayCard(string gameId, string playerName)
        {
            var gameRepo = new GameRepository(new PalaceDocumentSession());
            var game = gameRepo.Open(gameId);

            var result = game.PlayerCannotPlayCards(playerName);
            gameRepo.Save(game);
            return result;
        }
    }
}
