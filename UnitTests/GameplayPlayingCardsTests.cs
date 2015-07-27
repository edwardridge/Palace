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
            var player1 = PlayerHelper.CreatePlayer(cardsPlayerHas);

            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardsPlayerPlays = Card.AceOfClubs;
            var game = dealer.CreateGameInitialisation().StartGame(player1);

            Action playingCardsPlayerHasOutcome = () => game.PlayInHandCards(player1, cardsPlayerPlays);

            playingCardsPlayerHasOutcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Cannot_Play_Face_Up_Card_Player_Doesnt_Have()
        {
            var cardToPlay = Card.AceOfClubs;
            var player = PlayerHelper.CreatePlayer(cardToPlay);
            //player.PutCardFaceUp(cardToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, cardToPlay);
            var game = gameInit.StartGame();
            Action outcome = () => game.PlayFaceUpCards(player, Card.EightOfClubs);

            outcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Can_Play_Multiple_Face_Up_Cards_Of_Same_Value()
        {
            var cardsToPlay = new[] { Card.AceOfClubs, Card.AceOfClubs };
            var player = PlayerHelper.CreatePlayer(cardsToPlay.Concat(new[]{ Card.FiveOfClubs }));
            
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, Card.AceOfClubs);
            gameInit.PutCardFaceUp(player, Card.AceOfClubs);
            var game = gameInit.StartGame();

            var outcome = game.PlayFaceUpCards(player, cardsToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Cannot_Play_Multiple_Face_Up_Cards_Of_Different_Value()
        {
            var cardsToPlay = new[] { Card.AceOfClubs, Card.EightOfClubs };
            var player = PlayerHelper.CreatePlayer(cardsToPlay);
            
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, cardsToPlay[0]);
            gameInit.PutCardFaceUp(player, cardsToPlay[1]);
            var game = gameInit.StartGame();

            Action outcome = () => game.PlayFaceUpCards(player, cardsToPlay);

            outcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Can_Play_Card_Player_Has()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsPlayerHas);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame();
            var playingCardsPlayerHasOutcome = game.PlayInHandCards(player1, cardsPlayerHas[0]).ResultOutcome;

            playingCardsPlayerHasOutcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void When_Playing_Multiple_Cards_Player_Cannot_Play_Card_They_Dont_Have()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsPlayerHas);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardsPlayerPlays = new[] { Card.FourOfClubs, Card.FourOfSpades };
            var game = dealer.CreateGameInitialisation().StartGame(player1);
            Action result = () => game.PlayInHandCards(player1, cardsPlayerPlays);

            result.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Cannot_Play_Multiple_Cards_Of_Different_Value()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.AceOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            dealer.AddPlayer(player1);
            var game = dealer.CreateGameInitialisation().StartGame(player1);
            Action playerPlaysMultipleCardsOfDifferentValueOutcome = () => game.PlayInHandCards(player1, cardsToPlay);

            playerPlaysMultipleCardsOfDifferentValueOutcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Can_Play_Multiple_Cards_Of_Same_Value_And_Same_Suit()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame(player1);

            var playerPlaysMultipleCardsOfSameValueOutcome = game.PlayInHandCards(player1, cardsToPlay).ResultOutcome;

            playerPlaysMultipleCardsOfSameValueOutcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Can_Play_Multiple_Cards_Of_Same_Value_And_Different_Suit()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfSpades };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame();

            var outcome = game.PlayInHandCards(player1, cardsToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Cannot_Play_Face_Up_Card_With_Three_In_Hand_Cards()
        {
            var player1 =
                PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs, Card.FiveOfClubs, Card.JackOfClubs, Card.JackOfClubs });
            
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player1, Card.AceOfClubs);
            gameInit.PutCardFaceUp(player1, Card.EightOfClubs);
            gameInit.PutCardFaceUp(player1, Card.FiveOfClubs);
            var game = gameInit.StartGame();

            var outcome = game.PlayFaceUpCards(player1, Card.FiveOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Fail);
        }
    }
}