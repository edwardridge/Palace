using System.Collections.Generic;
using NUnit.Framework;

namespace UnitTests
{
    using FluentAssertions;

    using Palace;
    using Palace.Repository;

    using Raven.Client;
    using Raven.Client.Embedded;

    using UnitTests.Helpers;

    public class TestPalaceDocumentSession
    {
        public IDocumentSession GetDocumentSession()
        {
            var documentStore = new EmbeddableDocumentStore()
            {
                RunInMemory = true
            };
            documentStore.Initialize();
            return documentStore.OpenSession("Palace");
        }
      
    }

    [TestFixture]
    public class RhinoTests
    {
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
            var gameRepository = new GameRepository(new PalaceDocumentSession().GetDocumentSession());
            gameRepository.Save(game);
        }

        [Test]
        public void Can_Open_Game()
        {
            GameRepository gameRepository = new GameRepository(new PalaceDocumentSession().GetDocumentSession());
            Game game = gameRepository.Open("993");
            var currentPlayer = game.CurrentPlayer;
            game.PlayCards(currentPlayer, currentPlayer.CardsInHand[0]);
            game.NumberOfPlayers.Should().Be(2);
        }

        [Test]

        public void Game_Is_Same_Object_Once_Saved()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var player2 = PlayerHelper.CreatePlayer("Soph");

            var rulesForCardByValue = new Dictionary<CardValue, RuleForCard>();
            rulesForCardByValue.Add(CardValue.Ten, RuleForCard.Burn);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, rulesForCardByValue);
            dealer.DealIntialCards();
            var game = dealer.StartGame();
            var gameRepository = new GameRepository(new PalaceDocumentSession().GetDocumentSession());
            gameRepository.Save(game);

            var gameFromRepository = gameRepository.Open(game.Id.ToString());
            gameFromRepository.Should().Be(game);
        }

        [Test]
        public void Save_Player()
        {
            var playerRepository = new PlayerRepository(new PalaceDocumentSession().GetDocumentSession());
            var player = new Player("Ed", new[]{Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs});
            player.PutCardFaceUp(Card.AceOfClubs);
            player.Ready();
            playerRepository.Save(player);
        }

        [Test]
        public void Load_Player()
        {
            Player player = new PlayerRepository(new PalaceDocumentSession().GetDocumentSession()).Load(97);
            var test = 1;
        }

    }

}
