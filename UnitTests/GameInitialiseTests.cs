namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FluentAssertions;

    using NUnit.Framework;

    using Palace;

    using UnitTests.Helpers;

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
            dealer.DealIntialCards(new List<Player>() { player1, player2 });
            game = dealer.StartGame(new List<Player>() { player1, player2 });
        }

        [Test]
        public void Setup_With_Two_Players_Two_Players_Are_In_Game()
        {
            var playerCount = game.NumberOfPlayers;

            playerCount.Should().Be(2);
        }

        [Test]
        public void Player_1_Has_6_In_Hand_Cards()
        {
            player1.NumCardsInHand.Should().Be(6);
        }

        [Test]
        public void Player_2_Has_6_In_Hand_Cards()
        {
            player2.NumCardsInHand.Should().Be(6);
        }

        [Test]
        public void Player_1_Has_3_Face_Down_Cards()
        {
            player1.NumCardsFaceDown.Should().Be(3);
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
            player2.Ready();

            var dealer = new Dealer(new[] { player1, player2 }, new StandardDeck(), new DefaultStartGameRules());

            Action outcome = () => dealer.StartGame(new[] { player1, player2 });

            outcome.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Can_Start_When_Both_Players_Are_Ready()
        {
            var player1 = PlayerHelper.CreatePlayer();
            var player2 = PlayerHelper.CreatePlayer();

            Action outcome = () => DealerHelper.TestDealer(new[]{player1, player2} ).StartGame(new[] { player1, player2 });

            outcome.ShouldNotThrow();
        }
    }

    [TestFixture]
    public class PlayerReady
    {
        private Player player1;

        private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            player1 = new Player("Ed");
            dealer = new Dealer(new[]{player1}, new StandardDeck(), new DefaultStartGameRules());
        }

        [Test]
        public void Player_Cannot_Be_Ready_With_No_Face_Up_Cards()
        {
            // Don't put any cards face up
            Action outcome = () => dealer.StartGame(new[] { player1 });

            outcome.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Player_Cannot_Be_Ready_With_Two_Face_Up_Cards()
        {
            player1.AddCardsToInHandPile(new[] { new Card(CardValue.Ace, Suit.Club), new Card(CardValue.Ace, Suit.Club) });

            Action outcome = () => dealer.StartGame(new[] { player1 });

            outcome.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Player_Can_Be_Ready_When_Three_Cards_Are_Face_Up()
        {
            player1.AddCardsToInHandPile(
                new[]
                    {
                        new Card(CardValue.Ace, Suit.Club), 
                        new Card(CardValue.Ace, Suit.Club), 
                        new Card(CardValue.Ace, Suit.Club)
                    });

            Action outcome = () => player1.Ready();

            outcome.ShouldNotThrow();
        }

        [Test]
        public void Player_Cannot_Put_Fourth_Card_Face_Up()
        {
            player1.AddCardsToInHandPile(
                new[]
                    {
                        Card.AceOfClubs, 
                        Card.EightOfClubs, 
                        Card.FourOfClubs,
                        Card.JackOfClubs
                    });

            player1.PutCardFaceUp(Card.AceOfClubs);
            player1.PutCardFaceUp(Card.EightOfClubs);
            player1.PutCardFaceUp(Card.FourOfClubs);

            var outcome = player1.PutCardFaceUp(Card.JackOfClubs);

            outcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Player_Cannot_Put_Card_They_Dont_Have_Face_Up()
        {
            player1.AddCardsToInHandPile(new[] { new Card(CardValue.Ace, Suit.Club) });

            Action outcome = () => player1.PutCardFaceUp(Card.EightOfClubs);

            outcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void If_Player_Has_Same_Card_Face_Down_And_In_Hand_The_In_Hand_Card_Is_Put_Face_Up()
        {
            player1.AddCardsToInHandPile(new[] { new Card(CardValue.Ace, Suit.Club), new Card(CardValue.Ace, Suit.Club) });

            player1.PutCardFaceUp(Card.AceOfClubs);

            player1.NumCardsFaceUp.Should().Be(1);
            player1.NumCardsInHand.Should().Be(1);
        }
    }

    [TestFixture]
    public class FirstMoveWhenAllPlayersAreReady
    {
        private Player player1;

        private Player player2;

        private Player player3;

        private Game game;

        private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            // player1Builder = new MockPlayerBuilder ().WithState (PlayerState.Ready).WithName ("Ed");
            player1 = PlayerHelper.CreatePlayer();
            player2 = PlayerHelper.CreatePlayer();
            player3 = PlayerHelper.CreatePlayer();
            dealer = DealerHelper.TestDealer(new[]{player1, player2, player3});
        }

        [Test]
        public void P1_Has_Lowest_Card_P1_Goes_First()
        {
            player1.AddCardsToInHandPile(new[] { Card.TwoOfClubs });
            player2.AddCardsToInHandPile(new[] { Card.ThreeOfClubs });

            game = dealer.StartGame(new[] { player1, player2 });

            game.CurrentPlayer.Should().Be(player1);
        }

        [Test]
        public void P2_Has_Lowest_Card_P2_Goes_First()
        {
            player1.AddCardsToInHandPile(new[] { Card.ThreeOfClubs });
            player2.AddCardsToInHandPile(new[] { Card.TwoOfClubs });

            game = dealer.StartGame(new[] { player1, player2 });

            game.CurrentPlayer.Should().Be(player2);
        }

        [Test]
        public void P3_Has_Lowest_Card_P3_Goes_First()
        {
            player1.AddCardsToInHandPile(new[] { Card.ThreeOfClubs });
            player2.AddCardsToInHandPile(new[] { Card.ThreeOfClubs });
            player3.AddCardsToInHandPile(new[] { Card.TwoOfClubs });

            game = dealer.StartGame(new[] { player1, player2, player3 });

            game.CurrentPlayer.Should().Be(player3);
        }
    }
}

 }