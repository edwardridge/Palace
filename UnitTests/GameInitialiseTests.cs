﻿namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FluentAssertions;

    using NUnit.Framework;

    using Palace;
    using Palace.Rules;

    using TestHelpers;

    public class DummyCanStartGame : ICanStartGame
    {
        public bool IsReady(ICollection<Player> players)
        {
            return true;
        }
    }

     [TestFixture]
     public static class GameInitialiseTests
     {
    [TestFixture]
    public class SetupGame
    {
        private Dealer dealer;
        
        private Game game;

        private Player player1;

        private Player player2;

        [SetUp]
        public void Setup()
        {
            player1 = PlayerHelper.CreatePlayer();
            player2 = PlayerHelper.CreatePlayer();
            var deck = new StandardDeck();
            dealer = new Dealer(new []{player1, player2}, deck, new DummyCanStartGame());
            dealer.DealIntialCards();
            game = dealer.StartGame();
        }

        [Test]
        public void Setup_With_Two_Players_Two_Players_Are_In_Game()
        {
            var playerCount = game.Players.Count;

            playerCount.Should().Be(2);
        }

        [Test]
        public void Player_1_Has_6_In_Hand_Cards()
        {
            player1.CardsInHand.Count.Should().Be(6);
        }

        [Test]
        public void Player_2_Has_6_In_Hand_Cards()
        {
            player2.CardsInHand.Count.Should().Be(6);
        }

        [Test]
        public void Player_1_Has_3_Face_Down_Cards()
        {
            player1.CardsFaceDown.Count.Should().Be(3);
        }
    }

    [TestFixture]
    public class GameStart
    {
        [Test]
        public void Cannot_Start_When_Both_Players_Not_Ready()
        {
            var player1 = PlayerHelper.CreatePlayer();
            var player2 = PlayerHelper.CreatePlayer();
            
            var dealer = new Dealer(new[] { player1, player2 }, new StandardDeck(), new DefaultStartGameRules());

            dealer.PlayerReady(player2);
            Action outcome = () => dealer.StartGame();

            outcome.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Can_Start_When_Both_Players_Are_Ready()
        {
            var player1 = PlayerHelper.CreatePlayer();
            var player2 = PlayerHelper.CreatePlayer();

            Action outcome = () => DealerHelper.TestDealer(new[]{player1, player2} ).StartGame();

            outcome.ShouldNotThrow();
        }
    }

    [TestFixture]
    public class PlayerReady
    {
        //private Player player1;

        //private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            //player1 = new Player("Ed");
            //dealer = new Dealer(new[]{player1}, new StandardDeck(), new DefaultStartGameRules());
        }

        [Test]
        public void Player_Cannot_Be_Ready_With_No_Face_Up_Cards()
        {
            var player = PlayerHelper.CreatePlayer();
            var dealer = new Dealer(new[] { player }, new StandardDeck(), new DefaultStartGameRules());
            Action outcome = () => dealer.StartGame();

            outcome.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Player_Cannot_Be_Ready_With_Two_Face_Up_Cards()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs });
            var dealerForThisTest = new Dealer(new[] { player }, new StandardDeck(), new DefaultStartGameRules());
            dealerForThisTest.PutCardFaceUp(player, Card.AceOfClubs);
            dealerForThisTest.PutCardFaceUp(player, Card.EightOfClubs);

            Action outcome = () => dealerForThisTest.StartGame();

            outcome.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Player_Can_Be_Ready_When_Three_Cards_Are_Face_Up()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs });
            var dealerForThisTest = new Dealer(new[] { player }, new StandardDeck(), new DefaultStartGameRules());
            dealerForThisTest.PutCardFaceUp(player, Card.AceOfClubs);
            dealerForThisTest.PutCardFaceUp(player, Card.EightOfClubs);
            dealerForThisTest.PutCardFaceUp(player, Card.FiveOfClubs);

            Action outcome = () => dealerForThisTest.StartGame();

            outcome.ShouldNotThrow();
        }

        [Test]
        public void Player_Cannot_Put_Fourth_Card_Face_Up()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs,Card.JackOfClubs });
            var dealer = DealerHelper.TestDealer(new[] { player });
            dealer.PutCardFaceUp(player, Card.AceOfClubs);
            dealer.PutCardFaceUp(player, Card.EightOfClubs);
            dealer.PutCardFaceUp(player, Card.FiveOfClubs);

            var outcome = dealer.PutCardFaceUp(player, Card.JackOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Player_Cannot_Put_Card_They_Dont_Have_Face_Up()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs });
            var dealer = DealerHelper.TestDealer(new[] { player });
            Action outcome = () => dealer.PutCardFaceUp(player, Card.EightOfClubs);

            outcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Player_Can_Swap_Face_Up_Card_For_In_Hand_Card()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs });
            var dealer = DealerHelper.TestDealer(new[] { player });
            
            dealer.PutCardFaceUp(player, Card.AceOfClubs);
            dealer.PutCardFaceUp(player, Card.EightOfClubs, Card.AceOfClubs);

            player.CardsFaceUp.Should().Contain(Card.EightOfClubs);
            player.CardsFaceUp.Should().NotContain(Card.AceOfClubs);
        }

        [Test]
        public void When_Swapping_Face_Up_Card_Face_Up_Card_Is_Added_To_In_Hand_Pile()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs });
            var dealer = DealerHelper.TestDealer(new[] { player });
            dealer.PutCardFaceUp(player, Card.AceOfClubs);
            dealer.PutCardFaceUp(player, Card.EightOfClubs, Card.AceOfClubs);

            player.CardsInHand.Should().Contain(Card.AceOfClubs);
        }

        [Test]
        public void Cannot_Swap_Face_Up_Card_Player_Doesnt_Have()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs });
            var dealer = DealerHelper.TestDealer(new[] { player });
            Action action = () => dealer.PutCardFaceUp(player, Card.AceOfClubs, Card.SevenOfClubs);

            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Can_Swap_Card_When_Player_Has_Three_Face_Up_Cards()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs, Card.FourOfClubs });
            var dealer = DealerHelper.TestDealer(new[] { player });
            dealer.PutCardFaceUp(player, Card.AceOfClubs);
            dealer.PutCardFaceUp(player, Card.EightOfClubs);
            dealer.PutCardFaceUp(player, Card.FiveOfClubs);

            var result = dealer.PutCardFaceUp(player, Card.FourOfClubs, Card.AceOfClubs).ResultOutcome;

            result.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Cannot_Swap_Cards_When_Player_Is_Ready()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.EightOfClubs });
            var dealer = DealerHelper.TestDealer(new[] { player });
            dealer.PlayerReady(player);

            var result = dealer.PutCardFaceUp(player, Card.EightOfClubs).ResultOutcome;

            result.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void If_Player_Has_Same_Card_Face_Down_And_In_Hand_The_In_Hand_Card_Is_Put_Face_Up()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.AceOfClubs });
            var dealer = DealerHelper.TestDealer(new[] { player });
            dealer.PutCardFaceUp(player, Card.AceOfClubs);

            player.CardsFaceUp.Count.Should().Be(1);
            player.CardsInHand.Count.Should().Be(1);
        }
    }

    [TestFixture]
    public class FirstMoveWhenAllPlayersAreReady
    {
        //private Player player1;

        //private Player player2;

        //private Player player3;

        private Game game;

        //private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            // player1Builder = new MockPlayerBuilder ().WithState (PlayerState.Ready).WithName ("Ed");
            //player1 = PlayerHelper.CreatePlayer();
            //player2 = PlayerHelper.CreatePlayer();
            //player3 = PlayerHelper.CreatePlayer();
            //dealer = DealerHelper.TestDealer(new[]{player1, player2, player3});
        }

        [Test]
        public void P1_Has_Lowest_Card_P1_Goes_First()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TwoOfClubs);
            var player2 = PlayerHelper.CreatePlayer(Card.ThreeOfClubs);
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            game = dealer.StartGame();

            game.CurrentPlayer.Should().Be(player1);
        }

        [Test]
        public void P2_Has_Lowest_Card_P2_Goes_First()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.ThreeOfClubs);
            var player2 = PlayerHelper.CreatePlayer(Card.TwoOfClubs);
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });

            game = dealer.StartGame();

            game.CurrentPlayer.Should().Be(player2);
        }

        [Test]
        public void P3_Has_Lowest_Card_P3_Goes_First()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.ThreeOfClubs);
            var player2 = PlayerHelper.CreatePlayer(Card.ThreeOfClubs);
            var player3 = PlayerHelper.CreatePlayer(Card.TwoOfClubs);
            var dealer = DealerHelper.TestDealer(new[] { player1, player2, player3 });

            game = dealer.StartGame();

            game.CurrentPlayer.Should().Be(player3);
        }
    }
}

 }