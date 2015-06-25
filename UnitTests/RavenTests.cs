using System.Collections.Generic;
using NUnit.Framework;

namespace UnitTests
{
    using FluentAssertions;

    using Palace;
    using Palace.Repository;

    using Raven.Client;
    using Raven.Client.Embedded;

    using TestHelpers;

    public class TestPalaceDocumentSession
    {
        public IDocumentSession GetDocumentSession()
        {
            var documentStore = new EmbeddableDocumentStore()
            {
                RunInMemory = true,
                
            };
            documentStore.Initialize();
            return documentStore.OpenSession();
        }
      
    }

    [TestFixture, Ignore]
    public class RavenTests
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
            var gameRepository = new GameRepository(new TestPalaceDocumentSession().GetDocumentSession());
            gameRepository.Save(game);
        }

        [Test, Ignore]
        public void Can_Open_Game()
        {
            var gameRepository = new GameRepository(new TestPalaceDocumentSession().GetDocumentSession());
            var game = gameRepository.Open("e23c8755-a935-4cda-a2b7-4cd027446958");
            var currentPlayer = game.CurrentPlayer;
            game.PlayInHandCards(currentPlayer, currentPlayer.CardsInHand[0]);
            game.Players.Count.Should().Be(2);
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
            var gameRepository = new GameRepository(new TestPalaceDocumentSession().GetDocumentSession());
            gameRepository.Save(game);

            var gameFromRepository = gameRepository.Open(game.Id.ToString());
            gameFromRepository.Should().Be(game);
        }

        [Test]
        public void Game_Over_State_Is_Saved()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.AceOfClubs);
            var dealer = new Dealer(new[] { player1 }, new PredeterminedDeck(new List<Card>()), new TestHelpers.DummyCanStartGame());
            var game = dealer.StartGame();
            var result = game.PlayInHandCards(player1, Card.AceOfClubs);
            var gameRepository = new GameRepository(new TestPalaceDocumentSession().GetDocumentSession());
            gameRepository.Save(game);

            var gameFromRepository = gameRepository.Open(game.Id.ToString());
            gameFromRepository.Should().Be(game);
        }

        [Test]
        public void Save_Player()
        {
            var playerRepository = new PlayerRepository(new TestPalaceDocumentSession().GetDocumentSession());
            var player = new Player("Ed", new[]{Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs});
            player.PutCardFaceUp(Card.AceOfClubs);
            player.Ready();
            playerRepository.Save(player);
        }

        [Test]
        public void Load_Player()
        {
            Player player = new PlayerRepository(new TestPalaceDocumentSession().GetDocumentSession()).Load(97);
            var test = 1;
        }

    }

}
