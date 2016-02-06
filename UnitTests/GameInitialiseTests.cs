namespace UnitTests
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

        private GameInitialisation gameInitialisation;
        
        private Game game;

        private Player player1;

        private Player player2;

        [SetUp]
        public void Setup()
        {
            player1 = PlayerHelper.CreatePlayer("Ed");
            player2 = PlayerHelper.CreatePlayer("Liam");
            var deck = StandardDeck.CreateDeck();
            dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            gameInitialisation = dealer.CreateGameInitialisation();
            gameInitialisation.DealInitialCards();
            
            game = gameInitialisation.StartGame();
        }

        [Test]
        public void Setup_With_Two_Players_Two_Players_Are_In_Game()
        {
            var playerCount = game.State.Players.Count;

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
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            
            var dealer = new Dealer(StandardDeck.CreateDeck(), new DefaultStartGameRules());
            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PlayerReady(player2);
            Action outcome = () => gameInit.StartGame();

            outcome.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Cannot_Have_Duplicate_Player_Name()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var player2 = PlayerHelper.CreatePlayer("Ed");
            var dealer = new Dealer(StandardDeck.CreateDeck(), new DefaultStartGameRules());
            dealer.AddPlayer(player1);
            var outcome = dealer.AddPlayer(player2);
            outcome.Should().Be(false);
        }

        [Test]
        public void Can_Start_When_Both_Players_Are_Ready()
        {
            var player1 = PlayerHelper.CreatePlayer("Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var gameInit = DealerHelper.TestDealer(new[] { player1, player2 }).CreateGameInitialisation();
            Action outcome = () => gameInit.StartGame();

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
            var player = PlayerHelper.CreatePlayer("Ed");
            var dealer = new Dealer(StandardDeck.CreateDeck(), new DefaultStartGameRules());
            dealer.AddPlayer(player);
            Action outcome = () => dealer.CreateGameInitialisation().StartGame();

            outcome.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Player_Cannot_Be_Ready_With_Two_Face_Up_Cards()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs }, "Ed");
            var dealerForThisTest = new Dealer(StandardDeck.CreateDeck(), new DefaultStartGameRules());
            dealerForThisTest.AddPlayer(player);
            var gameInit = dealerForThisTest.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, Card.AceOfClubs);
            gameInit.PutCardFaceUp(player, Card.EightOfClubs);

            Action outcome = () => gameInit.StartGame();

            outcome.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Player_Can_Be_Ready_When_Three_Cards_Are_Face_Up()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs }, "Ed");
            var dealerForThisTest = new Dealer(StandardDeck.CreateDeck(), new DefaultStartGameRules());
            dealerForThisTest.AddPlayer(player);
            var gameInit = dealerForThisTest.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, Card.AceOfClubs);
            gameInit.PutCardFaceUp(player, Card.EightOfClubs);
            gameInit.PutCardFaceUp(player, Card.FiveOfClubs);

            Action outcome = () => gameInit.StartGame();

            outcome.ShouldNotThrow();
        }

        [Test]
        public void Player_Cannot_Put_Fourth_Card_Face_Up()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs,Card.JackOfClubs }, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, Card.AceOfClubs);
            gameInit.PutCardFaceUp(player, Card.EightOfClubs);
            gameInit.PutCardFaceUp(player, Card.FiveOfClubs);

            var outcome = gameInit.PutCardFaceUp(player, Card.JackOfClubs).ResultOutcome;

            outcome.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void Player_Cannot_Put_Card_They_Dont_Have_Face_Up()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs }, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            Action outcome = () => gameInit.PutCardFaceUp(player, Card.EightOfClubs);

            outcome.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Player_Can_Swap_Face_Up_Card_For_In_Hand_Card()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs }, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();

            gameInit.PutCardFaceUp(player, Card.AceOfClubs);
            gameInit.PutCardFaceUp(player, Card.EightOfClubs, Card.AceOfClubs);

            player.CardsFaceUp.Should().Contain(Card.EightOfClubs);
            player.CardsFaceUp.Should().NotContain(Card.AceOfClubs);
        }

        [Test]
        public void When_Swapping_Face_Up_Card_Face_Up_Card_Is_Added_To_In_Hand_Pile()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs }, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, Card.AceOfClubs);
            gameInit.PutCardFaceUp(player, Card.EightOfClubs, Card.AceOfClubs);

            player.CardsInHand.Should().Contain(Card.AceOfClubs);
        }

        [Test]
        public void Cannot_Swap_Face_Up_Card_Player_Doesnt_Have()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs }, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            Action action = () => gameInit.PutCardFaceUp(player, Card.AceOfClubs, Card.SevenOfClubs);

            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Can_Swap_Card_When_Player_Has_Three_Face_Up_Cards()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.EightOfClubs, Card.FiveOfClubs, Card.FourOfClubs }, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, Card.AceOfClubs);
            gameInit.PutCardFaceUp(player, Card.EightOfClubs);
            gameInit.PutCardFaceUp(player, Card.FiveOfClubs);

            var result = gameInit.PutCardFaceUp(player, Card.FourOfClubs, Card.AceOfClubs).ResultOutcome;

            result.Should().Be(ResultOutcome.Success);
        }

        [Test]
        public void Cannot_Swap_Cards_When_Player_Is_Ready()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.EightOfClubs }, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PlayerReady(player);

            var result = gameInit.PutCardFaceUp(player, Card.EightOfClubs).ResultOutcome;

            result.Should().Be(ResultOutcome.Fail);
        }

        [Test]
        public void If_Player_Has_Same_Card_Face_Down_And_In_Hand_The_In_Hand_Card_Is_Put_Face_Up()
        {
            var player = PlayerHelper.CreatePlayer(new[] { Card.AceOfClubs, Card.AceOfClubs }, "Ed");
            var dealer = DealerHelper.TestDealer(new[] { player });
            var gameInit = dealer.CreateGameInitialisation();
            gameInit.PutCardFaceUp(player, Card.AceOfClubs);

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
            var player1 = PlayerHelper.CreatePlayer(Card.TwoOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.ThreeOfClubs, "Liam");
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            game = dealer.CreateGameInitialisation().StartGame();

            game.State.GetCurrentPlayer().Should().Be(player1);
        }

        [Test]
        public void P2_Has_Lowest_Card_P2_Goes_First()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.ThreeOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.TwoOfClubs, "Liam");
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });

            game = dealer.CreateGameInitialisation().StartGame();

            game.State.GetCurrentPlayer().Should().Be(player2);
        }

        [Test]
        public void P3_Has_Lowest_Card_P3_Goes_First()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.ThreeOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.ThreeOfClubs, "Liam");
            var player3 = PlayerHelper.CreatePlayer(Card.TwoOfClubs, "David");
            var dealer = DealerHelper.TestDealer(new[] { player1, player2, player3 });

            game = dealer.CreateGameInitialisation().StartGame();

            game.State.GetCurrentPlayer().Should().Be(player3);
        }
    }
}

 }