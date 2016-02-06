using Palace.Repository;
using Palace.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
namespace Palace.Api.Controllers
{
    public class CreateGameCommand
    {
        public IEnumerable<string> Players { get; set; }

        public IEnumerable<Rule> Rules { get; set; }
    }

    //Todo: Replace!
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        private GameRepository gameRepository;
        public GameController(GameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        [Route("creategame"), HttpPost]
        public HttpStatusCode CreateGame(CreateGameCommand createGameCommand)
        {
            var players = createGameCommand.Players.Select(s => new Player(s));
            var id = CreateGame(players, createGameCommand.Rules);
            return HttpStatusCode.OK;
        }

        [Route("createstandardgame"), HttpGet]
        public Guid CreateStandardGame()
        {
            var player1 = new Player("Ed");
            var player2 = new Player("Liam");

            var rules = new List<Rule>();

            rules.Add(new Rule(CardValue.Ten, RuleForCard.Burn));
            rules.Add(new Rule(CardValue.Two, RuleForCard.Reset));
            rules.Add(new Rule(CardValue.Seven, RuleForCard.LowerThan));
            rules.Add(new Rule(CardValue.Ace, RuleForCard.ReverseOrderOfPlay));
            rules.Add(new Rule(CardValue.Eight, RuleForCard.SeeThrough));
            rules.Add(new Rule(CardValue.Jack, RuleForCard.SkipPlayer));

            var id = CreateGame(new List<Player>() { player1, player2 }, rules);
            
            return id;
        }

        public Guid CreateGame(IEnumerable<Player> players, IEnumerable<Rule> rules)
        {
            var rulesForGame = new RulesForGame();
            foreach (var rule in rules)
            {
                rulesForGame.Add(rule);
            }

            var dealer = new Dealer(StandardDeck.CreateDeck(), new DefaultStartGameRules(), rulesForGame);

            foreach (var player in players)
            {
                dealer.AddPlayer(player);
            }

            var gameInit = dealer.CreateGameInitialisation();

            gameInit.DealInitialCards();

            foreach (var player in players)
            {
                var playerFromGameInit = gameInit.Players.First(f => f.Name == player.Name);
                gameInit.PutCardFaceUp(playerFromGameInit, playerFromGameInit.CardsInHand.ToArray()[0]);
                gameInit.PutCardFaceUp(playerFromGameInit, playerFromGameInit.CardsInHand.ToArray()[1]);
                gameInit.PutCardFaceUp(playerFromGameInit, playerFromGameInit.CardsInHand.ToArray()[2]);
            }
            
            var game = gameInit.StartGame();
            gameRepository.Save(game);

            return game.Id;
        } 
        
        [Route("get/{id}/{player}")]
        public Result GetGameState(string id, string player)
        {
            var state = gameRepository.Open(id).State;
            var result = new Result(state.Players.First(f => f.Name == player), state);
            return result;  
        }

        [Route("getrules/{id}")]
        public RulesForGame GetGameRules(string id)
        {
            var rules = gameRepository.Open(id).GetRules();
            return rules;
        }

        [Route("playinhandcard/{gameid}/{playername}"), HttpPost]
        public Result PlayInHandCard(string gameId, string playerName, IEnumerable<Card> card)
        {
            var game = gameRepository.Open(gameId);
            var result = game.PlayInHandCards(playerName, card.ToArray());
            gameRepository.Save(game);
            return result;
        }

        [Route("playfaceupcard/{gameid}/{playername}"), HttpPost]
        public Result PlayFaceUpCard(string gameId, string playerName, IEnumerable<Card> card)
        {
            var game = gameRepository.Open(gameId);
            var result = game.PlayFaceUpCards(playerName, card.ToArray());
            gameRepository.Save(game);
            return result;
        }

        [Route("playfacedowncard/{gameid}/{playername}"), HttpPost]
        public Result PlayFaceDownCard(string gameId, string playerName)
        {
            var game = gameRepository.Open(gameId);
            var result = game.PlayFaceDownCards(playerName, game.State.Players.First(f=>f.Name == playerName).CardsFaceDown.ToArray()[0]);
            gameRepository.Save(game);
            return result;
        }

        [Route("cannotplaycard/{gameid}/{playername}"), HttpPost]
        public Result CannotPlayCard(string gameId, string playerName)
        {
            var game = gameRepository.Open(gameId);

            var result = game.PlayerCannotPlayCards(playerName);
            gameRepository.Save(game);
            return result;
        }
    }
}
