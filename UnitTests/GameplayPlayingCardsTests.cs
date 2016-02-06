namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    using Palace;

    using TestHelpers;

    [TestFixture]
    public class GameplayPlayingCardsTests
    {
        [Test]
        public void Cannot_Play_Card_Player_Doesnt_Have()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsPlayerHas, "Ed");

            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardsPlayerPlays = Card.AceOfClubs;
            var game = dealer.CreateGameInitialisation().StartGame(player1);

            var result = game.PlayInHandCards(player1.Name, cardsPlayerPlays);

            result.ResultOutcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Cannot_Play_Face_Up_Card_Player_Doesnt_Have()
        {
            var cardToPlay = Card.AceOfClubs;
            var player = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            //player.PutCardFaceUp(cardToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, cardToPlay);
            var game = gameInit.StartGame();
            var result = game.PlayFaceUpCards(player.Name, Card.EightOfClubs);

            result.ResultOutcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Can_Play_Multiple_Face_Up_Cards_Of_Same_Value()
        {
            var cardsToPlay = new[] { Card.AceOfClubs, Card.AceOfClubs };
            var player = PlayerHelper.CreatePlayer(cardsToPlay.Concat(new[]{ Card.FiveOfClubs }), "Ed");
            
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, Card.AceOfClubs);
            gameInit.PutCardFaceUp(player, Card.AceOfClubs);
            var game = gameInit.StartGame();

            var outcome = game.PlayFaceUpCards(player.Name, cardsToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Cannot_Play_Multiple_Face_Up_Cards_Of_Different_Value()
        {
            var cardsToPlay = new[] { Card.AceOfClubs, Card.EightOfClubs };
            var player = PlayerHelper.CreatePlayer(cardsToPlay, "Ed");
            
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, cardsToPlay[0]);
            gameInit.PutCardFaceUp(player, cardsToPlay[1]);
            var game = gameInit.StartGame();

            var result = game.PlayFaceUpCards(player.Name, cardsToPlay);

            result.ResultOutcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Can_Play_Card_Player_Has()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsPlayerHas, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame();
            var playingCardsPlayerHasOutcome = game.PlayInHandCards(player1.Name, cardsPlayerHas[0]).ResultOutcome;

            playingCardsPlayerHasOutcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void When_Playing_Multiple_Cards_Player_Cannot_Play_Card_They_Dont_Have()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsPlayerHas, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardsPlayerPlays = new[] { Card.FourOfClubs, Card.FourOfSpades };
            var game = dealer.CreateGameInitialisation().StartGame(player1);
            var result = game.PlayInHandCards(player1.Name, cardsPlayerPlays);

            result.ResultOutcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Cannot_Play_Multiple_Cards_Of_Different_Value()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.AceOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            dealer.AddPlayer(player1);
            var game = dealer.CreateGameInitialisation().StartGame(player1);
            var result = game.PlayInHandCards(player1.Name, cardsToPlay);

            result.ResultOutcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Can_Play_Multiple_Cards_Of_Same_Value_And_Same_Suit()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame(player1);

            var playerPlaysMultipleCardsOfSameValueOutcome = game.PlayInHandCards(player1.Name, cardsToPlay).ResultOutcome;

            playerPlaysMultipleCardsOfSameValueOutcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Can_Play_Multiple_Cards_Of_Same_Value_And_Different_Suit()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfSpades };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame();

            var outcome = game.PlayInHandCards(player1.Name, cardsToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Cannot_Play_Face_Up_Card_With_Three_In_Hand_Cards()
        {
            var player1 =
                PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs, Card.FiveOfClubs, Card.JackOfClubs, Card.JackOfClubs }, "Ed");
            
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player1, Card.AceOfClubs);
            gameInit.PutCardFaceUp(player1, Card.EightOfClubs);
            gameInit.PutCardFaceUp(player1, Card.FiveOfClubs);
            var game = gameInit.StartGame();

            var outcome = game.PlayFaceUpCards(player1.Name, Card.FiveOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Fail);
        }
        
        [Test]
        public void Playing_In_Hand_Card_Doesnt_Remove_Card_from_Players_In_Hand_Array_In_Game()
        {
            var deck = new PredeterminedDeck(new List<Card>() { Card.AceOfClubs, Card.AceOfClubs, Card.AceOfClubs });
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.EightOfClubs, Card.AceOfClubs }, "Ed");
            var dealer = new Dealer(deck, new DummyCanStartGame());
            dealer.AddPlayer(player1);
            var game = dealer.CreateGameInitialisation().StartGame();
            game.PlayInHandCards("Ed", Card.EightOfClubs);

            game.State.Players.First(w => w.Name == "Ed").CardsInHand.Should().NotContain(Card.EightOfClubs);
        }
    }
}