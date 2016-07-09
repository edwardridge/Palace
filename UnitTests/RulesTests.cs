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
    using Palace.Rules;
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
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardInPile = new Stack<Card>();
            cardInPile.Push(Card.TwoOfClubs);
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayInHandCards(player1.Name, cardToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Playing_Card_Lower_In_Value_Isnt_Valid()
        {
            var cardToPlay = Card.ThreeOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardInPile = Card.FiveOfClubs;
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, new[] { cardInPile });
            var outcome = game.PlayInHandCards(player1.Name, cardToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Play_Card_Of_Same_Value_Is_Valid()
        {
            var cardToPlay = Card.FourOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var cardInPile = new List<Card>(new[] { Card.FourOfClubs });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayInHandCards(player1.Name, cardToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }
    }

    [TestFixture]
    public class WithSevenAsLowerThanCard
    {
        private RulesForGame cardTypes;

        //private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            cardTypes = new RulesForGame();
            cardTypes.Add(new Rule(CardValue.Seven, RuleForCard.LowerThan));

        }

        [Test]
        public void Playing_Card_Higher_In_Value_Isnt_Valid()
        {
            var cardToPlay = Card.EightOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, cardTypes);
            var cardInPile = new List<Card>(new[] { Card.SevenOfClubs });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayInHandCards(player1.Name, cardToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Playing_Card_Lower_In_Value_Is_Valid()
        {
            var cardToPlay = Card.SixOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, cardTypes);
            var cardInPile = new List<Card>(new[] { Card.SevenOfClubs });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayInHandCards(player1.Name, cardToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Playing_Card_Of_Same_Value_Is_Valid()
        {
            var cardToPlay = Card.SevenOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, cardTypes);
            var cardInPile = new List<Card>(new[] { Card.SevenOfClubs });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayInHandCards(player1.Name, cardToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }
    }

    [TestFixture]
    public class WithTwoAsResetCard
    {
        private RulesForGame rulesForCardsByValue;

        //private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            rulesForCardsByValue = new RulesForGame();
            //rulesForCardsByValue.Add( new Rule(CardValue.Two, RuleForCard.Reset));
            rulesForCardsByValue.AddIRule(new ResetRule(CardValue.Two));
        }

        [Test]
        public void After_Playing_Reset_Card_Any_Card_IsValid()
        {
            var cardToPlay = Card.SixOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var cardInPile = new List<Card>(new[] { Card.TwoOfClubs });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, cardInPile);
            var outcome = game.PlayInHandCards(player1.Name, cardToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Resets_The_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer(new List<Card>() { Card.SixOfClubs, Card.SevenOfClubs }, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.TwoOfClubs, "Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, rulesForCardsByValue);
            var game = dealer.CreateGameInitialisation().StartGame(player1);
            game.PlayInHandCards(player1.Name, Card.SevenOfClubs);
            game.PlayInHandCards(player2.Name, Card.TwoOfClubs);

            // Playing this card without the reset card would not be valid
            var outcome = game.PlayInHandCards(player1.Name, Card.SixOfClubs);

            outcome.ResultOutcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Can_Be_Played_Over_Cards_Of_Higher_Value()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TwoOfClubs, "Ed");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, new List<Card>() { Card.EightOfClubs });
            var outcome = game.PlayInHandCards(player1.Name, Card.TwoOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }
    }

    [TestFixture]
    public class WithSevenAsLowerThanCardWhenPlayerPlaysFaceUpCard
    {
        private RulesForGame rulesForCardsByValue;

        //private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            rulesForCardsByValue = new RulesForGame();
            rulesForCardsByValue.Add(new Rule(CardValue.Seven, RuleForCard.LowerThan));
        }

        [Test]
        public void Playing_Card_Higher_In_Value_Isnt_Valid()
        {
            var cardToPlay = Card.EightOfClubs;
            var player = PlayerHelper.CreatePlayer(new[]{cardToPlay}, "Ed");
            
            var dealer = DealerHelper.TestDealerWithRules(new[]{player}, rulesForCardsByValue);
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, cardToPlay);
            var game = gameInit.StartGameWithPlayPile(player, new[] { Card.SevenOfClubs });
            
            var outcome = game.PlayFaceUpCards(player.Name, cardToPlay).ResultOutcome;
            
            outcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Playing_Card_Lower_In_Value_Is_Valid()
        {
            var cardToPlay = Card.SixOfClubs;
            var player = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var dealer = DealerHelper.TestDealerWithRules(new[]{player}, rulesForCardsByValue);
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player, new[] { Card.SevenOfClubs });
            
            var outcome = game.PlayInHandCards(player.Name, cardToPlay).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }
    }

    [TestFixture]
    public class WithJackAsReverseOrderOfPlayCard
    {
        private RulesForGame rulesForCardByValue;
        [SetUp]
        public void Setup()
        {
            rulesForCardByValue = new RulesForGame();
            rulesForCardByValue.Add(new Rule(CardValue.Jack, RuleForCard.ReverseOrderOfPlay));

        }

        [Test]
        public void After_Playing_Reverse_Order_Card_Order_Is_Reversed_For_One_Turn()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.JackOfClubs, "Dave");
            var player3 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, rulesForCardByValue);
            var game = dealer.CreateGameInitialisation().StartGame(player2);
            game.PlayInHandCards(player2.Name, Card.JackOfClubs);

            game.State.CurrentPlayer.Should().Be(player1);
        }

        [Test]
        public void After_Playing_Reverse_Order_Card_Order_Is_Reversed_For_Two_Turns()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.AceOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.JackOfClubs, "Dave");
            var player3 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, rulesForCardByValue);
            var game = dealer.CreateGameInitialisation().StartGame(player2);
            game.PlayInHandCards(player2.Name, Card.JackOfClubs);
            game.PlayInHandCards(player1.Name, Card.AceOfClubs);

            game.State.CurrentPlayer.Should().Be(player3);
        }

    }

    [TestFixture]
    public class WithEightAsReverseOrderOfPlayCard
    {
        private RulesForGame rulesForCardsByValue;
        [SetUp]
        public void Setup()
        {
            rulesForCardsByValue = new RulesForGame();
            rulesForCardsByValue.Add(new Rule(CardValue.Eight, RuleForCard.ReverseOrderOfPlay));
        }

        [Test]
        public void Playing_Reverse_Order_Of_Play_Card_Reverses_Order_Of_Play()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.JackOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.EightOfClubs, "Dave");
            var player3 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, rulesForCardsByValue);
            var game = dealer.CreateGameInitialisation().StartGame(player2);
            game.PlayInHandCards(player2.Name, Card.EightOfClubs);

            game.State.CurrentPlayer.Should().Be(player1);
        }
    }

    [TestFixture]
    public class WithTenAsBurnCard
    {
        private RulesForGame rulesForCardsByValue;
        [SetUp]
        public void Setup()
        {
            rulesForCardsByValue = new RulesForGame();
            //rulesForCardsByValue.Add(new Rule(CardValue.Ten, RuleForCard.Burn));
            rulesForCardsByValue.AddIRule(new BurnRule(CardValue.Ten));

        }

        [Test]
        public void Clears_The_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TenOfClubs, "Ed");
            var cardsInPile = new List<Card>() { Card.TwoOfClubs, Card.ThreeOfClubs };
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, cardsInPile);

            game.PlayInHandCards(player1.Name, Card.TenOfClubs);

            game.State.PlayPile.Count().Should().Be(0);
        }

        [Test]
        public void Can_Be_Played_Over_Cards_Of_Higher_Value()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TenOfClubs, "Ed");
            var cardInPile = new List<Card>() { Card.JackOfClubs };
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1 }, rulesForCardsByValue);
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, cardInPile);

            var outcome = game.PlayInHandCards(player1.Name, Card.TenOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void After_Playing_Burn_Card_Player_Gets_Another_Turn()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TenOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, rulesForCardsByValue);
            var game = dealer.CreateGameInitialisation().StartGame(player1);

            game.PlayInHandCards(player1.Name, Card.TenOfClubs);

            game.State.CurrentPlayer.Should().Be(player1);
        }
    }

    [TestFixture]
    public class WithEightAsSkipPlayerCard
    {
        private RulesForGame ruleForCardsByValue;

        [SetUp]
        public void Setup()
        {
            ruleForCardsByValue = new RulesForGame();
            //ruleForCardsByValue.Add(new Rule(CardValue.Eight, RuleForCard.SkipPlayer));
            ruleForCardsByValue.AddIRule(new SkipRule(CardValue.Eight));
        }

        [Test]
        public void After_Playing_Skips_Next_Player()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.EightOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var player3 = PlayerHelper.CreatePlayer("Dave");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, ruleForCardsByValue);
            var game = dealer.CreateGameInitialisation().StartGame();

            game.PlayInHandCards(player1.Name, Card.EightOfClubs);

            game.State.CurrentPlayer.Should().Be(player3);
        }

        [Test]
        public void With_Second_Player_As_Current_Player_After_Playing_Skips_Next_Player()
        {
            var player1 = PlayerHelper.CreatePlayer("Dave");
            var player2 = PlayerHelper.CreatePlayer(Card.EightOfClubs, "Ed");
            var player3 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, ruleForCardsByValue);
            var game = dealer.CreateGameInitialisation().StartGame(player2);

            game.PlayInHandCards(player2.Name, Card.EightOfClubs);

            game.State.CurrentPlayer.Should().Be(player1);
        }

        [Test]
        public void Playing_Two_Skip_Cards_Skips_Two_Players()
        {
            var cardsToPlay = new[] { Card.EightOfClubs, Card.EightOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Dave");
            var player3 = PlayerHelper.CreatePlayer("Soph");
            var player4 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3, player4 }, ruleForCardsByValue);
            var game = dealer.CreateGameInitialisation().StartGame();

            game.PlayInHandCards(player1.Name, cardsToPlay);

            game.State.CurrentPlayer.Should().Be(player4);
        }
    }

    [TestFixture]
    public class WithFiveAsSeeThroughCard
    {
        private RulesForGame ruleForCardsByValue;

        [SetUp]
        public void Setup()
        {
            ruleForCardsByValue = new RulesForGame();
            ruleForCardsByValue.Add(new Rule(CardValue.Five, RuleForCard.SeeThrough));
        }

        [Test]
        public void Can_Be_Played_Over_Card_Of_Higher_Value()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.FiveOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Dave");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, ruleForCardsByValue);
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, new[] { Card.SevenOfClubs });

            var outcome = game.PlayInHandCards(player1.Name, Card.FiveOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void After_Playing_Card_Next_Player_Cannot_Play_Card_Lower_Than_Previous_Card()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.FiveOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.SixOfClubs, "Dave");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2 }, ruleForCardsByValue);
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, new[] { Card.SevenOfClubs });

            game.PlayInHandCards(player1.Name, Card.FiveOfClubs);
            var outcome = game.PlayInHandCards(player2.Name, Card.SixOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Fail);

        }

        [Test]
        public void After_Two_Players_Play_See_Through_Card_Next_Player_Cannot_Play_Lower_Card_Than_Previous_Card()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.FiveOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.FiveOfClubs, "Dave");
            var player3 = PlayerHelper.CreatePlayer(Card.SixOfClubs, "Liam");
            var dealer = DealerHelper.TestDealerWithRules(new[] { player1, player2, player3 }, ruleForCardsByValue);
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, new[] { Card.SevenOfClubs });
            game.PlayInHandCards(player1.Name, Card.FiveOfClubs);
            game.PlayInHandCards(player2.Name, Card.FiveOfClubs);

            var outcome = game.PlayInHandCards(player3.Name, Card.SixOfClubs).ResultOutcome;

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
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, new[] { Card.TwoOfClubs, Card.ThreeOfClubs });

            var outcome = game.PlayInHandCards(player1.Name, cardsToPlay);

            game.State.PlayPile.Count().Should().Be(0);

        }

        [Test]
        public void Player_Gets_Another_Turn()
        {
            var cardsToPlay = new[] { Card.FiveOfClubs, Card.FiveOfClubs, Card.FiveOfClubs, Card.FiveOfClubs };
            var player1 = PlayerHelper.CreatePlayer(cardsToPlay, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, new[] { Card.TwoOfClubs, Card.ThreeOfClubs });

            var outcome = game.PlayInHandCards(player1.Name, cardsToPlay);

            game.State.CurrentPlayer.Should().Be(player1);
        }
    }

    [TestFixture]
    public class PlayingFourOfAKindOverTwoTurns
    {
        [Test]
        public void Burns_The_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.AceOfClubs }, "Ed");
            var player2 = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.AceOfClubs }, "Liam");
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var game = dealer.CreateGameInitialisation().StartGame();
            game.PlayInHandCards(player1.Name, new[] { Card.AceOfClubs, Card.AceOfClubs });
            game.PlayInHandCards(player2.Name, new[] { Card.AceOfClubs, Card.AceOfClubs });

            game.State.PlayPile.Count().Should().Be(0);
        }
    }

    [TestFixture]
    public class PlayingFourOfAKindOverThreeTurns
    {
        [Test]
        public void Burns_The_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.AceOfClubs, Card.AceOfClubs }, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.AceOfClubs, "Liam");
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var game = dealer.CreateGameInitialisation().StartGame();

            game.PlayInHandCards(player1.Name, new[] { Card.AceOfClubs, Card.AceOfClubs });
            game.PlayInHandCards(player2.Name, Card.AceOfClubs);
            game.PlayInHandCards(player1.Name, Card.AceOfClubs);

            game.State.PlayPile.Count().Should().Be(0);
        }
    }

    [TestFixture]
    public class WhenPlayerCannotPlayCards
    {
        [Test]
        public void Player_Gets_All_Cards_In_Play_Pile()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, new[] { Card.FiveOfClubs, Card.FourOfClubs });

            game.PlayerCannotPlayCards(player1.Name);

            player1.CardsInHand.Count.Should().Be(2);
        }

        [Test]
        public void Play_Pile_Is_Cleared()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var dealer = DealerHelper.TestDealer(new[] { player1 });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, new[] { Card.EightOfClubs, Card.FourOfSpades, Card.SixOfClubs });

            game.PlayerCannotPlayCards(player1.Name);

            game.State.PlayPile.Count().Should().Be(0);
        }

        [Test]
        public void Its_Next_Players_Turn()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGameWithPlayPile(player1, new[] { Card.AceOfClubs, Card.FiveOfClubs });

            game.PlayerCannotPlayCards(player1.Name);

            game.State.CurrentPlayer.Should().Be(player2);
        }

        [Test]
        public void If_It_Isnt_Players_Turn_Nothing_Happens()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.EightOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var gameInit = dealer.CreateGameInitialisation();
            var game = gameInit.StartGame(player1);

            game.PlayInHandCards("Ed", Card.EightOfClubs);
            game.PlayerCannotPlayCards(player1.Name);

            game.State.CurrentPlayerName.Should().Be(player2.Name);
        }
    }

    
}
