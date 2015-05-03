namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    using Palace;

    using UnitTests.Helpers;

    public class DummyCanStartGame : ICanStartGame
    {
        public bool GameIsReadyToStart(ICollection<IPlayer> players)
        {
            return true;
        }
    }

    // [TestFixture]
    // public static class GameInitialiseTests
    // {
    [TestFixture]
    public class SetupGame
    {
        private Dealer dealer;

        private Game game;

        private IPlayer player1;

        private IPlayer player2;

        [SetUp]
        public void Setup()
        {
            player1 = new StubReadyPlayer();
            player2 = new StubReadyPlayer();
            var deck = new StandardDeck();
            dealer = new Dealer(deck, new DummyCanStartGame());
            dealer.DealIntialCards(new List<IPlayer>() { player1, player2 });
            game = dealer.StartGame(new List<IPlayer>() { player1, player2 });
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
            player1.Cards.Count(card => card.CardOrientation == CardOrientation.InHand).Should().Be(6);
        }

        [Test]
        public void Player_2_Has_6_In_Hand_Cards()
        {
            player2.Cards.Count(card => card.CardOrientation == CardOrientation.InHand).Should().Be(6);
        }

        [Test]
        public void Player_1_Has_3_Face_Down_Cards()
        {
            player1.Cards.Count(card => card.CardOrientation == CardOrientation.FaceDown).Should().Be(3);
        }
    }

    [TestFixture]
    public class GameStart
    {
        [Test]
        public void Cannot_Start_When_Both_Players_Not_Ready()
        {
            var player1 = new MockPlayerBuilder().WithState(PlayerState.Setup).Build();
            var player2 = new MockPlayerBuilder().WithState(PlayerState.Ready).Build();

            var dealer = new Dealer(new StandardDeck(), new CanStartGame());

            Action outcome = () => dealer.StartGame(new[] { player1, player2 });

            outcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Can_Start_When_Both_Players_Are_Ready()
        {
            var player1 = new StubReadyPlayer();
            var player2 = new StubReadyPlayer();

            Action outcome = () => DealerHelper.TestDealer().StartGame(new[] { player1, player2 });

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
            dealer = new Dealer(new StandardDeck(), new CanStartGame());
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
            player1.AddCards(new[] { new Card(CardValue.Ace, Suit.Club, CardOrientation.FaceUp), new Card(CardValue.Ace, Suit.Club, CardOrientation.FaceUp) });

            Action outcome = () => dealer.StartGame(new[] { player1 });

            outcome.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Player_Can_Be_Ready_When_Three_Cards_Are_Face_Up()
        {
            player1.AddCards(
                new[]
                    {
                        new Card(CardValue.Ace, Suit.Club, CardOrientation.FaceUp), 
                        new Card(CardValue.Ace, Suit.Club, CardOrientation.FaceUp), 
                        new Card(CardValue.Ace, Suit.Club, CardOrientation.FaceUp)
                    });

            var outcome = player1.Ready();

            outcome.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Player_Cannot_Put_Fourth_Card_Face_Up()
        {
            player1.AddCards(
                new[]
                    {
                        new Card(CardValue.Ace, Suit.Club, CardOrientation.FaceUp), 
                        new Card(CardValue.Ace, Suit.Club, CardOrientation.FaceUp), 
                        new Card(CardValue.Ace, Suit.Club, CardOrientation.FaceUp)
                    });

            var outcome = player1.PutCardFaceUp(new Card(CardValue.Ace, Suit.Club, CardOrientation.FaceUp));

            outcome.Should().Be(ResultOutcome.Fail);
        }
    }

    [TestFixture]
    public class FirstMoveWhenAllPlayersAreReady
    {
        private IPlayer player1;

        private IPlayer player2;

        private IPlayer player3;

        private Game game;

        private Dealer dealer;

        [SetUp]
        public void Setup()
        {
            // player1Builder = new MockPlayerBuilder ().WithState (PlayerState.Ready).WithName ("Ed");
            player1 = new StubReadyPlayer();
            player2 = new StubReadyPlayer();
            player3 = new StubReadyPlayer();
            dealer = DealerHelper.TestDealer();
        }

        [Test]
        public void P1_Has_Lowest_Card_P1_Goes_First()
        {
            player1.AddCards(new[] { Card.TwoOfClubs });
            player2.AddCards(new[] { Card.ThreeOfClubs });

            game = dealer.StartGame(new[] { player1, player2 });

            game.CurrentPlayer.Should().Be(player1);
        }

        [Test]
        public void P2_Has_Lowest_Card_P2_Goes_First()
        {
            player1.AddCards(new[] { Card.ThreeOfClubs });
            player2.AddCards(new[] { Card.TwoOfClubs });

            game = dealer.StartGame(new[] { player1, player2 });

            game.CurrentPlayer.Should().Be(player2);
        }

        [Test]
        public void P3_Has_Lowest_Card_P3_Goes_First()
        {
            player1.AddCards(new[] { Card.ThreeOfClubs });
            player2.AddCards(new[] { Card.ThreeOfClubs });
            player3.AddCards(new[] { Card.TwoOfClubs });

            game = dealer.StartGame(new[] { player1, player2, player3 });

            game.CurrentPlayer.Should().Be(player3);
        }
    }
}

//}