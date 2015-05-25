namespace UnitTests
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Linq;
    using System.Runtime.InteropServices;

    using FluentAssertions;

    using NUnit.Framework;
    using NUnit.Framework.Constraints;

    using Palace;

    using UnitTests.Helpers;

    [TestFixture]
    public class GameplayTests
    {
        [Test]
        public void Player_Cannot_Play_Card_Player_Doesnt_Have()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsPlayerHas);

            var dealer = DealerHelper.TestDealer(new []{player1});
            var cardsPlayerPlays = Card.AceOfClubs;
            var game = dealer.StartGame(player1);

            Action playingCardsPlayerHasOutcome = () => game.PlayCards(player1, cardsPlayerPlays);

            playingCardsPlayerHasOutcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Player_Cannot_Play_Face_Up_Card_Player_Doesnt_Have()
        {
            var cardToPlay = Card.AceOfClubs;
            var player = PlayerHelper.CreatePlayer(cardToPlay);
            player.PutCardFaceUp(cardToPlay);
            var dealer = DealerHelper.TestDealer(new[]{player});
            var game = dealer.StartGame();
            Action outcome = () => game.PlayFaceUpCards(player, Card.EightOfClubs);

            outcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Player_Can_Play_Card_Player_Has()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsPlayerHas);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame();
            var playingCardsPlayerHasOutcome = game.PlayCards(player1, cardsPlayerHas[0]);

            playingCardsPlayerHasOutcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void When_Playing_Multiple_Cards_Player_Cannot_PLay_Card_They_Dont_Have()
        {
            var cardsPlayerHas = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsPlayerHas);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardsPlayerPlays = new[] { Card.FourOfClubs, Card.FourOfSpades };
            var game = dealer.StartGame(player1);
            Action result = () => game.PlayCards(player1, cardsPlayerPlays);

            result.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void When_Player_Plays_Card_Card_Is_Removed_From_Hand()
        {
            var deck = new PredeterminedDeck(new[] { Card.ThreeOfClubs });
            var cardToPlay = Card.FourOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame( player1);
            game.PlayCards(player1, cardToPlay);

            player1.CardsFaceUp.Should().NotContain(Card.FourOfClubs);
        }

        [Test]
        public void Can_Play_Multiple_Cards_Of_Same_Value_And_Same_Suit()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame(player1);
            
            var playerPlaysMultipleCardsOfSameValueOutcome = game.PlayCards(player1, cardsToPlay);

            playerPlaysMultipleCardsOfSameValueOutcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Can_Play_Multiple_Cards_Of_Same_Value_And_Different_Suit()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfSpades };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame();
            
            var outcome = game.PlayCards(player1, cardsToPlay);

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void When_PLayer_Plays_Multiple_Cards_Cards_Are_Removed_From_Hand()
        {
            var deck = new PredeterminedDeck(new[] { Card.AceOfClubs, Card.AceOfClubs });
            
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var dealer = new Dealer(new[] { player1 }, deck, new DummyCanStartGame());
            var game = dealer.StartGame();
            game.PlayCards(player1, cardsToPlay);

            player1.CardsFaceUp.Should().NotContain(cardsToPlay);
        }

        [Test]
        public void Cannot_Play_Multiple_Cards_Of_Different_Value()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.AceOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var dealer = new Dealer(new[] { player1 }, new StandardDeck(), new DummyCanStartGame());
            var game = dealer.StartGame(player1);
            Action playerPlaysMultipleCardsOfDifferentValueOutcome = () => game.PlayCards(player1, cardsToPlay);

            playerPlaysMultipleCardsOfDifferentValueOutcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void When_Player_Plays_Card_Card_Is_Added_To_PlayPile()
        {
            var cardToPlay = Card.FourOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = new Dealer(new[] { player1 }, new StandardDeck(), new DummyCanStartGame());
            var game = dealer.StartGame(player1);
            game.PlayCards(player1, cardToPlay);

            game.PlayPileCardCount.Should().Be(1);
        }

        [Test]
        public void When_Player_Plays_Card_Its_Next_Players_Turn()
        {
            var cardToPlay = Card.AceOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var dealer = new Dealer(new[] { player1, player2 }, new StandardDeck(), new DummyCanStartGame());
            var game = dealer.StartGame(player1);
            game.PlayCards(player1, cardToPlay);

            game.CurrentPlayer.Should().Be(player2);
        }

        [Test]
        public void When_Player_Plays_One_Card_They_Receive_One_Card()
        {
            var player1 = PlayerHelper.CreatePlayer(new[]{Card.AceOfClubs, Card.EightOfClubs, Card.FourOfClubs});
            var dealer = new Dealer(new[] { player1 }, new StandardDeck(), new DummyCanStartGame());
            var game = dealer.StartGame();
            game.PlayCards(player1, Card.AceOfClubs);

            player1.NumCardsInHand.Should().Be(3);
        }

        [Test]
        public void When_Player_Plays_Two_Cards_They_Receive_Two_Cards()
        {
            var cardsToPlay = new List<Card>() { Card.AceOfClubs, Card.AceOfClubs };
            var otherCard = new List<Card>() { Card.EightOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay.Union(otherCard));
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame();
            game.PlayCards(player1, cardsToPlay);

            player1.NumCardsInHand.Should().Be(3);
        }

        [Test]
        public void Player_Doesnt_Get_Card_If_They_Have_More_Than_Three_Cards()
        {
            var cards = new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FourOfClubs, Card.SevenOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cards);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame();

            game.PlayCards(player1, Card.AceOfClubs);

            player1.NumCardsInHand.Should().Be(3);
        }

        [Test]
        public void When_Player_Has_Four_Cards_And_Takes_Turn_They_Dont_Get_Any_More_Cards()
        {
            var cards = new[] { Card.AceOfClubs, Card.AceOfClubs, Card.AceOfClubs, Card.AceOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cards);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame();

            game.PlayCards(player1, cards);

            player1.NumCardsInHand.Should().Be(3);
        }

        [Test]
        public void When_Player_Has_Five_Cards_And_Takes_Turn_They_Dont_Get_Any_More_Cards()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs, Card.FourOfSpades, Card.SixOfClubs });
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame();

            game.PlayCards(player1, Card.AceOfClubs);

            player1.NumCardsInHand.Should().Be(4);
        }

        [Test]
        public void Player_Cannot_Play_Face_up_card_With_Three_In_Hand_Cards()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs, Card.FiveOfClubs, Card.JackOfClubs, Card.JackOfClubs });
            player1.PutCardFaceUp(Card.AceOfClubs);
            player1.PutCardFaceUp(Card.EightOfClubs);
            player1.PutCardFaceUp(Card.FiveOfClubs);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame();

            var outcome = game.PlayFaceUpCards(player1, Card.FiveOfClubs);

            outcome.Should().Be(ResultOutcome.Fail);
        }
    }

    
    [TestFixture]
    public class GameSimulation
    {
        [Test]
        public void Simulation()
        {
            var initialCards = new List<Card>()
                                {
                                    //Face down for player1
                                    Card.FiveOfClubs,
                                    Card.SixOfClubs,
                                    Card.FourOfClubs,
                                    //In Hand for player 1
                                    Card.ThreeOfClubs,
                                    Card.FourOfClubs,
                                    Card.FiveOfClubs,
                                    Card.SixOfClubs,
                                    Card.SevenOfClubs,
                                    Card.TenOfClubs,
                                    //Face down for player 2
                                    Card.SixOfClubs,
                                    Card.FourOfClubs,
                                    Card.ThreeOfClubs,
                                    //In hand for player 2
                                    Card.ThreeOfClubs,
                                    Card.FourOfClubs,
                                    Card.FiveOfClubs,
                                    Card.SixOfClubs,
                                    Card.SevenOfClubs,
                                    Card.TenOfClubs
                                };

            var nextCardsToBeDrawn = new List<Card>()
                                         {
                                             
                                         };
            var deck = new PredeterminedDeck(initialCards);
            var rules = new Dictionary<CardValue, RuleForCard>();
            rules.Add(CardValue.Two, RuleForCard.Reset);
            rules.Add(CardValue.Seven, RuleForCard.LowerThan);
            rules.Add(CardValue.Ten, RuleForCard.Burn);
            rules.Add(CardValue.Jack, RuleForCard.ReverseOrderOfPlay);

            var player1 = new Player("Ed");
            var player2 = new Player("Liam");
            var dealer = new Dealer(new[]{player1, player2}, deck, new DefaultStartGameRules(), rules);
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

            game.PlayCards(player1, Card.SixOfClubs);
            game.PlayCards(player2, Card.SevenOfClubs);

            game.CurrentPlayer.Should().Be(player1);
            player1.NumCardsInHand.Should().Be(2);

            game.PlayCards(player2, Card.TenOfClubs);
            game.PlayPileCardCount.Should().Be(0);


        }
    }
}