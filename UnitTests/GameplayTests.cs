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
        //private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            
        }

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
        public void Can_Play_Multiple_Cards_Of_Same_Value()
        {
            var cardsToPlay = new List<Card>() { Card.FourOfClubs, Card.FourOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame(player1);
            var playerPlaysMultipleCardsOfSameValueOutcome = game.PlayCards(player1, cardsToPlay);

            playerPlaysMultipleCardsOfSameValueOutcome.Should().Be(ResultOutcome.Success);
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
            var player1 = PlayerHelper.CreatePlayer(Card.AceOfClubs);
            var dealer = new Dealer(new[] { player1 }, new StandardDeck(), new DummyCanStartGame());
            var game = dealer.StartGame();
            game.PlayCards(player1, Card.AceOfClubs);

            player1.NumCardsInHand.Should().Be(1);
        }

        [Test]
        public void When_Player_Plays_Two_Cards_They_Receive_Two_Cards()
        {
            var cards = new List<Card>() { Card.AceOfClubs, Card.AceOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cards);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGame();
            game.PlayCards(player1, cards);

            player1.NumCardsInHand.Should().Be(cards.Count);
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
    }

    [TestFixture]
    public class StandardClassRules
    {
        //private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Playing_Card_Higher_In_Value_Is_Valid()
        {
            var cardToPlay = Card.ThreeOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = DealerHelper.TestDealer(new[]{player1});
            var cardInPile = new Stack<Card>();
            cardInPile.Push(Card.TwoOfClubs);
            var game = dealer.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayCards(player1, cardToPlay);

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Playing_Card_Lower_In_Value_Isnt_Valid()
        {
            var cardToPlay = Card.ThreeOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardInPile = Card.FiveOfClubs;
            var game = dealer.StartGameWithPlayPile(player1, new[] { cardInPile });
            var outcome = game.PlayCards(player1, cardToPlay);

            outcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Play_Card_Of_Same_Value_Is_Valid()
        {
            var cardToPlay = Card.FourOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardInPile = new List<Card>(new[] { Card.FourOfClubs });
            var game = dealer.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayCards(player1, cardToPlay);

            outcome.Should().Be(ResultOutcome.Success);
        }
    }

    [TestFixture]
    public class WithSevenAsLowerThanCard
    {
        private Dictionary<CardValue, RuleForCard> cardTypes;

        //private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            cardTypes = new Dictionary<CardValue, RuleForCard>();
            cardTypes.Add(CardValue.Seven, RuleForCard.LowerThan);
            
        }

        [Test]
        public void Playing_Card_Higher_In_Value_Isnt_Valid()
        {
            var cardToPlay = Card.EightOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = DealerHelper.TestDealerWithRules(new[]{player1}, cardTypes);
            var cardInPile = new List<Card>(new[] { Card.SevenOfClubs });
            var game = dealer.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayCards(player1, cardToPlay);

            outcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Playing_Card_Lower_In_Value_Is_Valid()
        {
            var cardToPlay = Card.SixOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, cardTypes);
            var cardInPile = new List<Card>(new[] { Card.SevenOfClubs });
            var game = dealer.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayCards(player1, cardToPlay);

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Playing_Card_Of_Same_Value_Is_Valid()
        {
            var cardToPlay = Card.SevenOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, cardTypes);
            var cardInPile = new List<Card>(new[] { Card.SevenOfClubs });
            var game = dealer.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayCards(player1, cardToPlay);

            outcome.Should().Be(ResultOutcome.Success);
        }
    }

    [TestFixture]
    public class WithTwoAsResetCard
    {
        private Dictionary<CardValue, RuleForCard> rulesForCardsByValue;

        //private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            rulesForCardsByValue = new Dictionary<CardValue, RuleForCard>();
            rulesForCardsByValue.Add(CardValue.Two, RuleForCard.Reset);
            
        }

        [Test]
        public void After_Playing_Reset_Card_Any_Card_IsValid()
        {
            var cardToPlay = Card.SixOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var cardInPile = new List<Card>(new[] { Card.TwoOfClubs });
            var game = dealer.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayCards(player1, cardToPlay);

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Resets_The_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer(new List<Card>() { Card.SixOfClubs, Card.SevenOfClubs });
            var player2 = PlayerHelper.CreatePlayer(Card.TwoOfClubs);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var game = dealer.StartGame(player1);
            game.PlayCards(player1, Card.SevenOfClubs);
            game.PlayCards(player2, Card.TwoOfClubs);

            // Playing this card without the reset card would not be valid
            var outcome = game.PlayCards(player1, Card.SixOfClubs);

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Can_Be_Played_Over_Cards_Of_Higher_Value()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TwoOfClubs);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player1, new List<Card>(){Card.EightOfClubs});
            var outcome = game.PlayCards(player1, Card.TwoOfClubs);

            outcome.Should().Be(ResultOutcome.Success);
        }
    }

    [TestFixture]
    public class WithJackAsReverseOrderOfPlayCard
    {
        //private Dealer dealer;
        private Dictionary<CardValue, RuleForCard> rulesForCardByValue;
            [SetUp]
        public void Setup()
        {
            rulesForCardByValue = new Dictionary<CardValue, RuleForCard>();
            rulesForCardByValue.Add(CardValue.Jack, RuleForCard.ReverseOrderOfPlay);
            
        }

        [Test]
        public void After_Playing_Reverse_Order_Card_Order_Is_Reversed_For_One_Turn()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.JackOfClubs);
            var player3 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, rulesForCardByValue);
            var game = dealer.StartGame(player2);
            game.PlayCards(player2, Card.JackOfClubs);

            game.CurrentPlayer.Should().Be(player1);
        }

        [Test]
        public void After_Playing_Reverse_Order_Card_Order_Is_Reversed_For_Two_Turns()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.AceOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.JackOfClubs);
            var player3 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, rulesForCardByValue);
            var game = dealer.StartGame(player2);
            game.PlayCards(player2, Card.JackOfClubs);
            game.PlayCards(player1, Card.AceOfClubs);

            game.CurrentPlayer.Should().Be(player3);
        }

    }

    [TestFixture]
    public class WithEightAsReverseOrderOfPlayCard
    {
        //private Dealer dealer;

        private Dictionary<CardValue, RuleForCard> rulesForCardsByValue;
        [SetUp]
        public void Setup()
        {
            rulesForCardsByValue = new Dictionary<CardValue, RuleForCard>();
            rulesForCardsByValue.Add(CardValue.Eight, RuleForCard.ReverseOrderOfPlay);
            //dealer = DealerHelper.TestDealerWithRules(rulesForCardsByValue);
        }

        [Test]
        public void Playing_Reverse_Order_Of_Play_Card_Reverses_Order_Of_Play()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.JackOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.EightOfClubs);
            var player3 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, rulesForCardsByValue);
            var game = dealer.StartGame(player2);
            game.PlayCards(player2, Card.EightOfClubs);

            game.CurrentPlayer.Should().Be(player1);
        }
    }

    [TestFixture]
    public class WithTenAsBurnCard
    {
        //private Dealer dealer;

        private Dictionary<CardValue, RuleForCard> rulesForCardsByValue;
        [SetUp]
        public void Setup()
        {
            rulesForCardsByValue = new Dictionary<CardValue, RuleForCard>();
            rulesForCardsByValue.Add(CardValue.Ten, RuleForCard.Burn);
            
        }

        [Test]
        public void Clears_The_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TenOfClubs);
            var cardsInPile = new List<Card>() { Card.TwoOfClubs, Card.ThreeOfClubs };
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player1, cardsInPile);
            
            game.PlayCards(player1, Card.TenOfClubs);

            game.PlayPileCardCount.Should().Be(0);
        }

        [Test]
        public void Can_Be_Played_Over_Cards_Of_Higher_Value()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TenOfClubs);
            var cardInPile = new List<Card>() { Card.JackOfClubs };
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player1, cardInPile);

            var outcome = game.PlayCards(player1, Card.TenOfClubs);

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void After_Playing_Burn_Card_Player_Gets_Another_Turn()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TenOfClubs);
            var player2 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, rulesForCardsByValue);
            var game = dealer.StartGame(player1);

            game.PlayCards(player1, Card.TenOfClubs);

            game.CurrentPlayer.Should().Be(player1);
        }
    }

    [TestFixture]
    public class WithEightAsSkipPlayerCard
    {
        private Dictionary<CardValue, RuleForCard> ruleForCardsByValue;
        
        [SetUp]
        public void Setup()
        {
            ruleForCardsByValue = new Dictionary<CardValue, RuleForCard>();
            ruleForCardsByValue.Add(CardValue.Eight, RuleForCard.SkipPlayer);
        }

        [Test]
        public void After_Playing_Skips_Next_Player()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.EightOfClubs);
            var player2 = PlayerHelper.CreatePlayer();
            var player3 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, ruleForCardsByValue);
            var game = dealer.StartGame();

            game.PlayCards(player1, Card.EightOfClubs);

            game.CurrentPlayer.Should().Be(player3);
        }

        [Test]
        public void With_Second_Player_As_Current_Player_After_Playing_Skips_Next_Player()
        {
            var player1 = PlayerHelper.CreatePlayer();
            var player2 = PlayerHelper.CreatePlayer(Card.EightOfClubs);
            var player3 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, ruleForCardsByValue);
            var game = dealer.StartGame(player2);

            game.PlayCards(player2, Card.EightOfClubs);

            game.CurrentPlayer.Should().Be(player1);
        }

        [Test]
        public void Playing_Two_Skip_Cards_Skips_Two_Players()
        {
            var cardsToPlay = new[]{Card.EightOfClubs, Card.EightOfClubs};
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay, "Ed");
            var player2 = PlayerHelper.CreatePlayer();
            var player3 = PlayerHelper.CreatePlayer();
            var player4 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3, player4 }, ruleForCardsByValue);
            var game = dealer.StartGame();

            game.PlayCards(player1, cardsToPlay);

            game.CurrentPlayer.Should().Be(player4);
        }
    }

    [TestFixture]
    public class WithFiveAsSeeThroughCard
    {
        private Dictionary<CardValue, RuleForCard> ruleForCardsByValue; 
        
        [SetUp]
        public void Setup()
        {
            ruleForCardsByValue = new Dictionary<CardValue, RuleForCard>();  
            ruleForCardsByValue.Add(CardValue.Five, RuleForCard.SeeThrough);
        }

        [Test]
        public void Can_Be_Played_Over_Card_Of_Higher_Value()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.FiveOfClubs);
            var player2 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, ruleForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player1, new[]{Card.SevenOfClubs});
            
            var outcome = game.PlayCards(player1, Card.FiveOfClubs);

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void After_Playing_Card_Next_Player_Cannot_Play_Card_Lower_Than_Previous_Card()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.FiveOfClubs);
            var player2 = PlayerHelper.CreatePlayer(Card.SixOfClubs);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, ruleForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.SevenOfClubs });

            game.PlayCards(player1, Card.FiveOfClubs);
            var outcome = game.PlayCards(player2, Card.SixOfClubs);

            outcome.Should().Be(ResultOutcome.Fail);

        }

        [Test]
        public void After_Two_Players_Play_See_Through_Card_Next_Player_Cannot_Play_Lower_Card_Than_Previous_Card()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.FiveOfClubs);
            var player2 = PlayerHelper.CreatePlayer(Card.FiveOfClubs);
            var player3 = PlayerHelper.CreatePlayer(Card.SixOfClubs);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, ruleForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player1, new[]{Card.SevenOfClubs});
            game.PlayCards(player1, Card.FiveOfClubs);
            game.PlayCards(player2, Card.FiveOfClubs);

            var outcome = game.PlayCards(player3, Card.SixOfClubs);

            outcome.Should().Be(ResultOutcome.Fail);
        }
    }

    [TestFixture]
    public class PlayingFourOfAKind
    {
        [Test]
        public void Burns_The_Play_Pile()
        {
            var cardsToPlay = new[] { Card.FiveOfClubs, Card.FiveOfClubs, Card.FiveOfClubs, Card.FiveOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.TwoOfClubs, Card.ThreeOfClubs });

            var outcome = game.PlayCards(player1, cardsToPlay);

            game.PlayPileCardCount.Should().Be(0);

        }

        [Test]
        public void Player_Gets_Another_Turn()
        {
            var cardsToPlay = new[] { Card.FiveOfClubs, Card.FiveOfClubs, Card.FiveOfClubs, Card.FiveOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var player2 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.TwoOfClubs, Card.ThreeOfClubs });

            var outcome = game.PlayCards(player1, cardsToPlay);

            game.CurrentPlayer.Should().Be(player1);
        }
    }

    [TestFixture]
    public class WhenPlayerCannotPlayCards
    {
        [Test]
        public void Player_Gets_All_Cards_In_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.FiveOfClubs, Card.FourOfClubs });

            game.PlayerCannotPlayCards(player1);

            player1.NumCardsInHand.Should().Be(2);
        }

        [Test]
        public void Play_Pile_Is_Cleared()
        {
            var player1 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.EightOfClubs, Card.FourOfSpades, Card.SixOfClubs });

            game.PlayerCannotPlayCards(player1);

            game.PlayPileCardCount.Should().Be(0);
        }

        [Test]
        public void Its_Next_Players_Turn()
        {
            var player1 = PlayerHelper.CreatePlayer();
            var player2 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.AceOfClubs, Card.FiveOfClubs });

            game.PlayerCannotPlayCards(player1);

            game.CurrentPlayer.Should().Be(player2);
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