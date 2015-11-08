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
        [Route("create"), HttpGet]
        public Guid CreateGame()
        {
            var player1 = new Player("Ed");
            var player2 = new Player("Liam");

            var rules = new Dictionary<CardValue, RuleForCard>();
            rules.Add(CardValue.Ten, RuleForCard.Burn);
            rules.Add(CardValue.Two, RuleForCard.Reset);
            rules.Add(CardValue.Seven, RuleForCard.LowerThan);
            var dealer = new Dealer(StandardDeck.CreateDeck(), new DefaultStartGameRules(), rules);

            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);

            var gameInit =  dealer.CreateGameInitialisation();
            
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
            //Result resultFromCache = (Result) memoryCache.Get(id + "/" + player);
            //if (resultFromCache != null) return resultFromCache;

            var state = new GameRepository(new PalaceDocumentSession()).Open(id).State;
            var result = new Result(state.Players.First(f => f.Name == player), state);
            //memoryCache.Set(new CacheItem(id + "/" + player, result), new CacheItemPolicy());
            return result;
        }

        [Route("playinhandcard/{gameid}/{playername}"), HttpPost]
        public Result PlayInHandCard(string gameId, string playerName, IEnumerable<Card> card)
        {
            var gameRepo = new GameRepository(new PalaceDocumentSession());
            var game = gameRepo.Open(gameId);
           // var cardToPlay = GetCard(card);
            var result = game.PlayInHandCards(playerName, card.ToArray());
            gameRepo.Save(game);
            //memoryCache.Set(new CacheItem(gameId + "/" + playerName, result), new CacheItemPolicy());
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

        private static Card GetCard(string card)
        {
            Suit suit;
            string suitString = card.Substring(0, 1);
            switch (card.Substring(0, 1))
            {
                case "S":
                    suit = Suit.Spade;
                    break;
                case "C":
                    suit = Suit.Club;
                    break;
                case "D":
                    suit = Suit.Diamond;
                    break;
                case "H":
                    suit = Suit.Heart;
                    break;
                default: throw new Exception();
            }

            int cardValueInt = int.Parse(card.Substring(1));
            CardValue cardValue = (CardValue)cardValueInt;
            var cardToPlay = new Card(cardValue, suit);
            return cardToPlay;
        }
    }
}
