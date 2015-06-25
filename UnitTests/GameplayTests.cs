namespace UnitTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using NUnit.Framework;

    using Palace;

    using TestHelpers;

    [TestFixture]
    public class GameplayChangingGameStateTests
    {
        [Test]
        public void When_Player_Plays_Card_Card_Is_Added_To_PlayPile()
        {
            var cardToPlay = Card.FourOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = new Dealer(new[] { player1 }, new StandardDeck(), new DummyCanStartGame());
            var game = dealer.StartGame(player1);
            game.PlayInHandCards(player1, cardToPlay);

            game.PlayPile.Count.Should().Be(1);
        }

        [Test]
        public void When_Player_Plays_Card_Its_Next_Players_Turn()
        {
            var cardToPlay = Card.AceOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var dealer = new Dealer(new[] { player1, player2 }, new StandardDeck(), new DummyCanStartGame());
            var game = dealer.StartGame(player1);
            game.PlayInHandCards(player1, cardToPlay);

            game.CurrentPlayer.Should().Be(player2);
        }
    }

    [TestFixture, Ignore]
    public class GameSimulation
    {
        [Test]
        public void Simulation()
        {
            var initialCards = new List<Card>()
                                   {
                                       // Face down for player1
                                       Card.FiveOfClubs, 
                                       Card.SixOfClubs, 
                                       Card.FourOfClubs, 
                                       
                                       // In Hand for player 1
                                       Card.ThreeOfClubs, 
                                       Card.FourOfClubs, 
                                       Card.FiveOfClubs, 
                                       Card.SixOfClubs, 
                                       Card.SevenOfClubs, 
                                       Card.TenOfClubs, 
                                       
                                       // Face down for player 2
                                       Card.SixOfClubs, 
                                       Card.FourOfClubs, 
                                       Card.ThreeOfClubs, 
                                       
                                       // In hand for player 2
                                       Card.ThreeOfClubs, 
                                       Card.FourOfClubs, 
                                       Card.FiveOfClubs, 
                                       Card.SixOfClubs, 
                                       Card.SevenOfClubs, 
                                       Card.TenOfClubs
                                   };

            var nextCardsToBeDrawn = new List<Card>() { };
            var deck = new PredeterminedDeck(initialCards);
            var rules = new Dictionary<CardValue, RuleForCard>();
            rules.Add(CardValue.Two, RuleForCard.Reset);
            rules.Add(CardValue.Seven, RuleForCard.LowerThan);
            rules.Add(CardValue.Ten, RuleForCard.Burn);
            rules.Add(CardValue.Jack, RuleForCard.ReverseOrderOfPlay);

            var player1 = new Player("Ed");
            var player2 = new Player("Liam");
            var dealer = new Dealer(new[] { player1, player2 }, deck, new DefaultStartGameRules(), rules);
            dealer.DealIntialCards();

            player1.PutCardFaceUp(Card.ThreeOfClubs);
            player1.PutCardFaceUp(Card.FourOfClubs);
            player1.PutCardFaceUp(Card.FiveOfClubs);
            player1.Ready();

            player2.PutCardFaceUp(Card.ThreeOfClubs);
            player2.PutCardFaceUp(Card.FourOfClubs);
            player2.PutCardFaceUp(Card.FiveOfClubs);
            player2.Ready();

            var game = dealer.StartGame();
            player1.NumCardsInHand.Should().Be(3);

            game.PlayInHandCards(player1, Card.SixOfClubs);
            game.PlayInHandCards(player2, Card.SevenOfClubs);

            game.CurrentPlayer.Should().Be(player1);
            player1.NumCardsInHand.Should().Be(2);

            game.PlayInHandCards(player2, Card.TenOfClubs);
            game.PlayPile.Count.Should().Be(0);
        }
    }
}