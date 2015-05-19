using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests
{
    using FluentAssertions;

    using Palace;
    using Palace.Repository;

    using Raven.Client;
    using Raven.Client.Document;

    using UnitTests.Helpers;

    using DocumentSession = Palace.Repository.DocumentSession;

    [TestFixture, Ignore]
    public class RhinoTests
    {
        private IDocumentSession GetDocumentSession()
        {
            var documentStore = new DocumentStore()
                                              {
                                                  Url = "http://localhost:8080"
                                              };
            documentStore.Initialize();
            return documentStore.OpenSession("Palace");
        }
        [Test]
        public void Can_Save_Game()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var player2 = PlayerHelper.CreatePlayer("Soph");

            var rulesForCardByValue = new Dictionary<CardValue, RuleForCard>();
            rulesForCardByValue.Add(CardValue.Ten, RuleForCard.Burn);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, rulesForCardByValue);
            dealer.DealIntialCards();
            var game = dealer.StartGame();
            var gameRepository = new GameRepository(DocumentSession.GetDocumentSession());
            gameRepository.Save(game);
            
        }

        [Test]
        public void Can_Open_Game()
        {
            GameRepository gameRepository = new GameRepository(DocumentSession.GetDocumentSession());
            Game game = gameRepository.Open(993);
            var currentPlayer = game.CurrentPlayer;
            game.PlayCards(currentPlayer, currentPlayer.CardsInHand[0]);
            game.NumberOfPlayers.Should().Be(2);
        }

        [Test]
        public void Save_Player()
        {
            var playerRepository = new PlayerRepository(DocumentSession.GetDocumentSession());
            var player = new Player("Ed", new[]{Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs});
            player.PutCardFaceUp(Card.AceOfClubs);
            player.Ready();
            playerRepository.Save(player);
        }

        [Test]
        public void Load_Player()
        {
            Player player = new PlayerRepository(DocumentSession.GetDocumentSession()).Load(97);
            var test = 1;
        }

    }

}
