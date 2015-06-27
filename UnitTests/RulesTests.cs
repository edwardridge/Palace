using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    using FluentAssertions;

    using NUnit.Framework;

    using Palace;

    using TestHelpers;

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
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardInPile = new Stack<Card>();
            cardInPile.Push(Card.TwoOfClubs);
            var game = dealer.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayInHandCards(player1, cardToPlay).ResultOutcome;

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
            var outcome = game.PlayInHandCards(player1, cardToPlay).ResultOutcome;

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
            var outcome = game.PlayInHandCards(player1, cardToPlay).ResultOutcome;

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
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, cardTypes);
            var cardInPile = new List<Card>(new[] { Card.SevenOfClubs });
            var game = dealer.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayInHandCards(player1, cardToPlay).ResultOutcome;

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
            var outcome = game.PlayInHandCards(player1, cardToPlay).ResultOutcome;

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
            var outcome = game.PlayInHandCards(player1, cardToPlay).ResultOutcome;

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
            var outcome = game.PlayInHandCards(player1, cardToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Resets_The_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer(new List<Card>() { Card.SixOfClubs, Card.SevenOfClubs });
            var player2 = PlayerHelper.CreatePlayer(Card.TwoOfClubs);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var game = dealer.StartGame(player1);
            game.PlayInHandCards(player1, Card.SevenOfClubs);
            game.PlayInHandCards(player2, Card.TwoOfClubs);

            // Playing this card without the reset card would not be valid
            var outcome = game.PlayInHandCards(player1, Card.SixOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Can_Be_Played_Over_Cards_Of_Higher_Value()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TwoOfClubs);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player1, new List<Card>() { Card.EightOfClubs });
            var outcome = game.PlayInHandCards(player1, Card.TwoOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }
    }

    [TestFixture]
    public class WithSevenAsLowerThanCardWhenPlayerPlaysFaceUpCard
    {
        private Dictionary<CardValue, RuleForCard> rulesForCardsByValue;

        //private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            rulesForCardsByValue = new Dictionary<CardValue, RuleForCard>();
            rulesForCardsByValue.Add(CardValue.Seven, RuleForCard.LowerThan);
        }

        [Test]
        public void Playing_Card_Higher_In_Value_Isnt_Valid()
        {
            var cardToPlay = Card.EightOfClubs;
            var player = PlayerHelper.CreatePlayer(new[]{cardToPlay});
            
            var dealer = DealerHelper.TestDealerWithRules(new[]{player}, rulesForCardsByValue);
            dealer.PutCardFaceUp(player, cardToPlay);
            var game = dealer.StartGameWithPlayPile(player, new[] { Card.SevenOfClubs });
            
            var outcome = game.PlayFaceUpCards(player, cardToPlay).ResultOutcome;
            
            outcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Playing_Card_Lower_In_Value_Is_Valid()
        {
            var cardToPlay = Card.SixOfClubs;
            var player = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = DealerHelper.TestDealerWithRules(new[]{player}, rulesForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player, new[] { Card.SevenOfClubs });
            
            var outcome = game.PlayInHandCards(player, cardToPlay).ResultOutcome;

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
            game.PlayInHandCards(player2, Card.JackOfClubs);

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
            game.PlayInHandCards(player2, Card.JackOfClubs);
            game.PlayInHandCards(player1, Card.AceOfClubs);

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
            game.PlayInHandCards(player2, Card.EightOfClubs);

            game.CurrentPlayer.Should().Be(player1);
        }
    }

    [TestFixture]
    public class WithTenAsBurnCard
    {
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

            game.PlayInHandCards(player1, Card.TenOfClubs);

            game.PlayPile.Count.Should().Be(0);
        }

        [Test]
        public void Can_Be_Played_Over_Cards_Of_Higher_Value()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TenOfClubs);
            var cardInPile = new List<Card>() { Card.JackOfClubs };
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player1, cardInPile);

            var outcome = game.PlayInHandCards(player1, Card.TenOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void After_Playing_Burn_Card_Player_Gets_Another_Turn()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TenOfClubs);
            var player2 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, rulesForCardsByValue);
            var game = dealer.StartGame(player1);

            game.PlayInHandCards(player1, Card.TenOfClubs);

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

            game.PlayInHandCards(player1, Card.EightOfClubs);

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

            game.PlayInHandCards(player2, Card.EightOfClubs);

            game.CurrentPlayer.Should().Be(player1);
        }

        [Test]
        public void Playing_Two_Skip_Cards_Skips_Two_Players()
        {
            var cardsToPlay = new[] { Card.EightOfClubs, Card.EightOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay, "Ed");
            var player2 = PlayerHelper.CreatePlayer();
            var player3 = PlayerHelper.CreatePlayer();
            var player4 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3, player4 }, ruleForCardsByValue);
            var game = dealer.StartGame();

            game.PlayInHandCards(player1, cardsToPlay);

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
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.SevenOfClubs });

            var outcome = game.PlayInHandCards(player1, Card.FiveOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void After_Playing_Card_Next_Player_Cannot_Play_Card_Lower_Than_Previous_Card()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.FiveOfClubs);
            var player2 = PlayerHelper.CreatePlayer(Card.SixOfClubs);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, ruleForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.SevenOfClubs });

            game.PlayInHandCards(player1, Card.FiveOfClubs);
            var outcome = game.PlayInHandCards(player2, Card.SixOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Fail);

        }

        [Test]
        public void After_Two_Players_Play_See_Through_Card_Next_Player_Cannot_Play_Lower_Card_Than_Previous_Card()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.FiveOfClubs);
            var player2 = PlayerHelper.CreatePlayer(Card.FiveOfClubs);
            var player3 = PlayerHelper.CreatePlayer(Card.SixOfClubs);
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, ruleForCardsByValue);
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.SevenOfClubs });
            game.PlayInHandCards(player1, Card.FiveOfClubs);
            game.PlayInHandCards(player2, Card.FiveOfClubs);

            var outcome = game.PlayInHandCards(player3, Card.SixOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Fail);
        }
    }

    [TestFixture]
    public class PlayingFourOfAKindInOneTurn
    {
        [Test]
        public void Burns_The_Play_Pile()
        {
            var cardsToPlay = new[] { Card.FiveOfClubs, Card.FiveOfClubs, Card.FiveOfClubs, Card.FiveOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.TwoOfClubs, Card.ThreeOfClubs });

            var outcome = game.PlayInHandCards(player1, cardsToPlay);

            game.PlayPile.Count.Should().Be(0);

        }

        [Test]
        public void Player_Gets_Another_Turn()
        {
            var cardsToPlay = new[] { Card.FiveOfClubs, Card.FiveOfClubs, Card.FiveOfClubs, Card.FiveOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay);
            var player2 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.TwoOfClubs, Card.ThreeOfClubs });

            var outcome = game.PlayInHandCards(player1, cardsToPlay);

            game.CurrentPlayer.Should().Be(player1);
        }
    }

    [TestFixture]
    public class PlayingFourOfAKindOverTwoTurns
    {
        [Test]
        public void Burns_The_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.AceOfClubs });
            var player2 = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.AceOfClubs });
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var game = dealer.StartGame();
            game.PlayInHandCards(player1, new[] { Card.AceOfClubs, Card.AceOfClubs });
            game.PlayInHandCards(player2, new[] { Card.AceOfClubs, Card.AceOfClubs });

            game.PlayPile.Count.Should().Be(0);
        }
    }

    [TestFixture]
    public class PlayingFourOfAKindOverThreeTurns
    {
        [Test]
        public void Burns_The_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.AceOfClubs, Card.AceOfClubs });
            var player2 = PlayerHelper.CreatePlayer(Card.AceOfClubs);
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var game = dealer.StartGame();

            game.PlayInHandCards(player1, new[] { Card.AceOfClubs, Card.AceOfClubs });
            game.PlayInHandCards(player2, Card.AceOfClubs);
            game.PlayInHandCards(player1, Card.AceOfClubs);

            game.PlayPile.Count.Should().Be(0);
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

            player1.CardsInHand.Count.Should().Be(2);
        }

        [Test]
        public void Play_Pile_Is_Cleared()
        {
            var player1 = PlayerHelper.CreatePlayer();
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var game = dealer.StartGameWithPlayPile(player1, new[] { Card.EightOfClubs, Card.FourOfSpades, Card.SixOfClubs });

            game.PlayerCannotPlayCards(player1);

            game.PlayPile.Count.Should().Be(0);
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

}
