using System;
using NUnit.Framework;
using Palace;
using System.Linq;
using FluentAssertions;
using System.Collections.Generic;

namespace UnitTests
{
    using UnitTests.Helpers;

    [TestFixture]
    public class GameplayTests
    {
        private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            dealer = DealerHelper.TestDealer();
        }

        [Test]
        public void Player_Cannot_Play_Card_Player_Doesnt_Have()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs };
            var player1 = new StubReadyPlayer(cardsPlayerHas);

            var cardsPlayerPlays = Card.AceOfClubs;
            var game = dealer.StartGame(new[] { player1 }, player1);

            Action playingCardsPlayerHasOutcome = () => game.PlayCards(player1, cardsPlayerPlays);

            playingCardsPlayerHasOutcome.ShouldThrow<ArgumentException>();
        }


        [Test]
        public void Player_Can_Play_Card_Player_Has()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = new StubReadyPlayer(cardsPlayerHas);

            var game = dealer.StartGame(new[] { player1 }, player1);
            var playingCardsPlayerHasOutcome = game.PlayCards(player1, cardsPlayerHas[0]);

            playingCardsPlayerHasOutcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void When_Playing_Multiple_Cards_Player_Cannot_PLay_Card_They_Dont_Have()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = new StubReadyPlayer(cardsPlayerHas);

            var cardsPlayerPlays = new[] { Card.FourOfClubs, Card.FourOfSpades };
            var game = dealer.StartGame(new[] { player1 }, player1);
            Action result = () => game.PlayCards(player1, cardsPlayerPlays);

            result.ShouldThrow<ArgumentException>();

        }

        [Test]
        public void When_Player_Plays_Card_Card_Is_Removed_From_Hand()
        {
            var cardToPlay = Card.FourOfClubs;
            var player1 = new StubReadyPlayer(cardToPlay);

            var game = dealer.StartGame(new[] { player1 }, player1);
            game.PlayCards(player1, cardToPlay);

            player1.Cards.Count.Should().Be(0);
        }

        [Test]
        public void Can_Play_Multiple_Cards_Of_Same_Value()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = new StubReadyPlayer(cardsToPlay);

            var game = dealer.StartGame(new[] { player1 }, player1);
            var playerPlaysMultipleCardsOfSameValueOutcome = game.PlayCards(player1, cardsToPlay);

            playerPlaysMultipleCardsOfSameValueOutcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void When_PLayer_Plays_Multiple_Cards_Cards_Are_Removed_From_Hand()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = new StubReadyPlayer(cardsToPlay);

            var game = dealer.StartGame(new[] { player1 }, player1);
            game.PlayCards(player1, cardsToPlay);

            player1.Cards.Count().Should().Be(0);
        }

        [Test]
        public void Cannot_Play_Multiple_Cards_Of_Different_Value(){
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.AceOfClubs };
            var player1 = new StubReadyPlayer (cardsToPlay);

            var game = dealer.StartGame(new[] { player1 }, player1);
            Action playerPlaysMultipleCardsOfDifferentValueOutcome = () => game.PlayCards (player1, cardsToPlay);

            playerPlaysMultipleCardsOfDifferentValueOutcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void When_Player_Plays_Card_Card_Is_Added_To_PlayPile()
        {
            var cardToPlay = Card.FourOfClubs;
            var player1 = new StubReadyPlayer(cardToPlay);

            var game = dealer.StartGame(new[] { player1 }, player1);
            game.PlayCards(player1, cardToPlay);

            game.PlayPileCardCount.Should().Be(1);
        }

        [Test]
        public void When_Player_Plays_Card_Its_Next_Players_Turn()
        {
            var cardToPlay = Card.AceOfClubs;
            var player1 = new StubReadyPlayer(cardToPlay, "Ed");
            var player2 = new StubReadyPlayer("Liam");

            var game = dealer.StartGame(new[] { player1, player2 }, player1);
            game.PlayCards(player1, cardToPlay);

            game.CurrentPlayer.Should().Be(player2);
        }

        [TestFixture]
        public class StandardClassRules{

                private Dealer dealer;

                [SetUp]
                public void Setup()
                {
                    dealer = DealerHelper.TestDealer();
                }

                [Test]
                public void Playing_Card_Higher_In_Value_Is_Valid()
                {
                    var cardToPlay = Card.ThreeOfClubs;
                    var player1 = new StubReadyPlayer (cardToPlay);

                    var cardInPile = new Stack<Card>();
                    cardInPile.Push (Card.TwoOfClubs);
                    var game = dealer.ResumeGameWithPlayPile(new[] { player1 }, player1, cardInPile);
                    var outcome = game.PlayCards (player1, cardToPlay);

                    outcome.Should ().Be (ResultOutcome.Success);
                    player1.Cards.Count ().Should ().Be (0);
                }

                [Test]
                public void Playing_Card_Lower_In_Value_Isnt_Valid()
                {
                    var cardToPlay = Card.ThreeOfClubs;
                    var player1 = new StubReadyPlayer(cardToPlay);

                    var cardInPile = Card.FiveOfClubs;
                    var game = dealer.ResumeGameWithPlayPile(new[] { player1 }, player1, new[]{cardInPile});
                    var outcome = game.PlayCards(player1, cardToPlay);

                    outcome.Should().Be(ResultOutcome.Fail);
                    player1.Cards.Count().Should().Be(1);
                }

                [Test]
                public void Play_Card_Of_Same_Value_Is_Valid()
                {
                    var cardToPlay = Card.FourOfClubs;
                    var player1 = new StubReadyPlayer(cardToPlay);

                    var cardInPile = new List<Card>(new[] { Card.FourOfClubs });
                    var game = dealer.ResumeGameWithPlayPile(new[] { player1 }, player1, cardInPile);
                    var outcome = game.PlayCards(player1, cardToPlay);

                    outcome.Should().Be(ResultOutcome.Success);
                    player1.Cards.Count().Should().Be(0);
                }
            }

            [TestFixture]
            public class WithSevenAsLowerThanCard
            {
                Dictionary<CardValue, RuleForCard> cardTypes;

                private Dealer dealer;
                [SetUp]
                public void Setup()
                {
                    cardTypes = new Dictionary<CardValue, RuleForCard>();
                    cardTypes.Add(CardValue.Seven, RuleForCard.LowerThan);
                    dealer = DealerHelper.TestDealerWithRules(cardTypes);
                }

                [Test]
                public void Playing_Card_Higher_In_Value_Isnt_Valid()
                {
                    var cardToPlay = Card.EightOfClubs;
                    var player1 = new StubReadyPlayer(cardToPlay);

                    var cardInPile = new List<Card>(new[] { Card.SevenOfClubs });
                    var game = dealer.ResumeGameWithPlayPile(new[] { player1 }, player1, cardInPile);
                    var outcome = game.PlayCards(player1, cardToPlay);

                    outcome.Should().Be(ResultOutcome.Fail);
                    player1.Cards.Count().Should().Be(1);
                }

                [Test]
                public void Playing_Card_Lower_In_Value_Is_Valid()
                {
                    var cardToPlay = Card.SixOfClubs;
                    var player1 = new StubReadyPlayer(cardToPlay);

                    var cardInPile = new List<Card>(new[] { Card.SevenOfClubs });
                    var game = dealer.ResumeGameWithPlayPile(new[] { player1 }, player1, cardInPile);
                    var outcome = game.PlayCards(player1, cardToPlay);

                    outcome.Should().Be(ResultOutcome.Success);
                    player1.Cards.Count().Should().Be(0);
                }

                [Test]
                public void Playing_Card_Of_Same_Value_Is_Valid()
                {
                    var cardToPlay = Card.SevenOfClubs;
                    var player1 = new StubReadyPlayer(cardToPlay);

                    var cardInPile = new List<Card>(new[] { Card.SevenOfClubs });
                    var game = dealer.ResumeGameWithPlayPile(new[] { player1 }, player1, cardInPile);
                    var outcome = game.PlayCards(player1, cardToPlay);

                    outcome.Should().Be(ResultOutcome.Success);
                    player1.Cards.Count().Should().Be(0);
                }
            }

            [TestFixture]
            public class WithTwoAsResetCard
            {
                Dictionary<CardValue, RuleForCard> rulesForCardsByValue;

                private Dealer dealer;

                [SetUp]
                public void Setup()
                {
                    rulesForCardsByValue = new Dictionary<CardValue, RuleForCard>();
                    rulesForCardsByValue.Add(CardValue.Two, RuleForCard.Reset);
                    dealer = DealerHelper.TestDealerWithRules(rulesForCardsByValue);
                }

                [Test]
                public void AnyCardIsValid()
                {
                    var cardToPlay = Card.SixOfClubs;
                    var player1 = new StubReadyPlayer(cardToPlay);

                    var cardInPile = new List<Card>(new[] { Card.TwoOfClubs });
                    var game = dealer.ResumeGameWithPlayPile(new[] { player1 }, player1, cardInPile);
                    var outcome = game.PlayCards(player1, cardToPlay);

                    outcome.Should().Be(ResultOutcome.Success);
                }

                [Test]
                public void TwoCardResetsThePlayPile()
                {
                    var player1 = new StubReadyPlayer(new List<Card>(){Card.SixOfClubs, Card.SevenOfClubs});
                    var player2 = new StubReadyPlayer(Card.TwoOfClubs);

                    var game = dealer.ResumeGameWithPlayPile(new[] { player1, player2 }, player1, new List<Card>());
                    game.PlayCards(player1, Card.SevenOfClubs);
                    game.PlayCards(player2, Card.TwoOfClubs);
                    //Playing this card without the reset card would not be valid
                    var outcome = game.PlayCards(player1, Card.SixOfClubs); 

                    outcome.Should().Be(ResultOutcome.Success);
                }
            }
        

        public static Deck NonShufflingDeck()
        {
            return new Deck(new NonShuffler());
        }
    }
}

