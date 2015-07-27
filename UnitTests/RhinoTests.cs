using System.Collections.Generic;
using NUnit.Framework;

namespace UnitTests
{
    using System.Linq;

    using FluentAssertions;

    using Palace;
    using Palace.Repository;

    using Raven.Client;
    using Raven.Client.Embedded;

    using TestHelpers;
    using System;
    using System.Diagnostics;

    public class TestPalaceDocumentSession : PalaceDocumentSession
    {
        public override IDocumentSession GetDocumentSession()
        {
            var documentStore = new EmbeddableDocumentStore()
            {
                RunInMemory = true,
                
            };
            documentStore.Initialize();
            return documentStore.OpenSession();
        }
      
    }

    [TestFixture]
    public class RhinoTests
    {
        [Test, Ignore]
        public void Can_Save_Game()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var player2 = PlayerHelper.CreatePlayer("Soph");

            var rulesForCardByValue = new Dictionary<CardValue, RuleForCard>();
            rulesForCardByValue.Add(CardValue.Ten, RuleForCard.Burn);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, rulesForCardByValue);
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.DealInitialCards();
            var game = gameInit.StartGame();
            var gameRepository = new GameRepository(new PalaceDocumentSession());
            gameRepository.Save(game);
        }

        [Test, Ignore]
        public void Can_Open_Game()
        {
            var gameRepository = new GameRepository(new PalaceDocumentSession());
            var game = gameRepository.Open("6b200db4-594f-480a-b9ff-2b3a6eaafd5a");
            var currentPlayer = game.State.CurrentPlayer;
            game.PlayInHandCards(currentPlayer.Name, currentPlayer.CardsInHand.First());
            game.State.Players.Count.Should().Be(2);
        }

        [Test, Ignore]
        public void Game_Is_Same_Object_Once_Saved()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed5");
            var player2 = PlayerHelper.CreatePlayer("Soph");

            var rulesForCardByValue = new Dictionary<CardValue, RuleForCard>();
            rulesForCardByValue.Add(CardValue.Ten, RuleForCard.Burn);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, rulesForCardByValue);
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.DealInitialCards();
            var game = gameInit.StartGame();
            var gameRepository = new GameRepository(new PalaceDocumentSession());
            gameRepository.Save(game);

            var gameFromRepository = gameRepository.Open(game.State.GameId.ToString());
            gameFromRepository.Should().Be(game);
        }
    }

}
