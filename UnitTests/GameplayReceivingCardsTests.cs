namespace UnitTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    using Palace;

    using TestHelpers;

    [TestFixture]
    public class GameplayReceivingCardsTests
    {
        [Test]
        public void When_Player_Plays_Card_Card_Is_Removed_From_Hand()
        {
            var deck = new PredeterminedDeck(new[] { Card.ThreeOfClubs });
            var cardToPlay = Card.FourOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame(player1);
            game.PlayInHandCards(player1, cardToPlay);

            player1.CardsFaceUp.Should().NotContain(Card.FourOfClubs);
        }

        [Test]
        public void When_PLayer_Plays_Multiple_Cards_Cards_Are_Removed_From_Hand()
        {
            var deck = new PredeterminedDeck(new[] { Card.AceOfClubs, Card.AceOfClubs });

            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay, "Ed");
            var dealer = new Dealer(deck, new DummyCanStartGame());
            dealer.AddPlayer(player1);
            var game = dealer.CreateGameInitialisation().StartGame();
            game.PlayInHandCards(player1, cardsToPlay);

            player1.CardsFaceUp.Should().NotContain(cardsToPlay);
        }

        [Test]
        public void When_Player_Plays_One_Card_They_Receive_One_Card()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FourOfClubs }, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            dealer.AddPlayer(player1);
            var game = dealer.CreateGameInitialisation().StartGame();
            game.PlayInHandCards(player1, Card.AceOfClubs);

            player1.CardsInHand.Count.Should().Be(3);
        }

        [Test]
        public void When_Player_Plays_Two_Cards_They_Receive_Two_Cards()
        {
            var cardsToPlay = new List<Card>() { Card.AceOfClubs, Card.AceOfClubs };
            var otherCard = new List<Card>() { Card.EightOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay.Union(otherCard), "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame();
            game.PlayInHandCards(player1, cardsToPlay);

            player1.CardsInHand.Count.Should().Be(3);
        }

        [Test]
        public void When_Player_Has_Four_Cards_And_Plays_One_Card_They_Dont_Get_Any_More_Cards()
        {
            var cards = new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FourOfClubs, Card.SevenOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cards, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame();

            game.PlayInHandCards(player1, Card.AceOfClubs);

            player1.CardsInHand.Count.Should().Be(3);
        }

        [Test]
        public void When_Player_Has_Four_Cards_And_Plays_Four_Cards_They_Get_Three_Cards()
        {
            var cards = new[] { Card.AceOfClubs, Card.AceOfClubs, Card.AceOfClubs, Card.AceOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cards, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame();

            game.PlayInHandCards(player1, cards);

            player1.CardsInHand.Count.Should().Be(3);
        }

        [Test]
        public void When_Player_Has_Five_Cards_And_PLays_One_Card_They_Dont_Get_Any_More_Cards()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs, Card.FourOfSpades, Card.SixOfClubs }, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.CreateGameInitialisation().StartGame();

            game.PlayInHandCards(player1, Card.AceOfClubs);

            player1.CardsInHand.Count.Should().Be(4);
        }
    }
}