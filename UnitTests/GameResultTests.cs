
using FluentAssertions;
using NUnit.Framework;
using Palace;
using Palace.Repository;
using Palace.Rules;
using System.Collections.Generic;
using System.Linq;
using TestHelpers;

namespace UnitTests
{
    [TestFixture]
    public class GameResultTestsWhenValidCardPlayed
    {
        [Test]
        public void Player_Name_Should_Be_Last_Player()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.EightOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var players = new List<Player>(new[] { player1, player2 });
            var dealer = DealerHelper.TestDealer(players);
            var game = dealer.CreateGameInitialisation().StartGame();

            var result = game.PlayInHandCards("Ed", Card.EightOfClubs);
            result.GameStatusForPlayer.Name.Should().Be("Ed");
        }

        [Test]
        public void Player_Can_See_Own_Cards_In_Hand()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.EightOfClubs, Card.AceOfClubs, Card.FiveOfClubs }, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var players = new List<Player>(new[] { player1, player2 });
            var dealer = new Dealer(new PredeterminedDeck(new List<Card>()), new DummyCanStartGame());
            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);
            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            var result = game.PlayInHandCards("Ed", Card.EightOfClubs);
            result.GameStatusForPlayer.CardsInHand.Should().BeEquivalentTo(new[] { Card.AceOfClubs, Card.FiveOfClubs });
        }

        [Test]
        public void In_Hand_Cards_Are_Ordered_By_Value()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.EightOfClubs, Card.AceOfClubs, Card.FiveOfClubs }, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var players = new List<Player>(new[] { player1, player2 });
            var dealer = new Dealer(new PredeterminedDeck(new List<Card>()), new DummyCanStartGame());
            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);
            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            var result = game.PlayInHandCards("Ed", Card.EightOfClubs);
            result.GameStatusForPlayer.CardsInHand.ToArray()[0].Should().Be(Card.FiveOfClubs);
                //
        }

        [Test]
        public void Player_Can_See_Number_Of_Cards_Face_Down()
        {
            var player1 = new Player("Ed", new[] { Card.EightOfClubs }, new Card[0], new[] { Card.FiveOfClubs, Card.FourOfClubs });

            var player2 = PlayerHelper.CreatePlayer("Liam");
            var players = new List<Player>(new[] { player1, player2 });
            var dealer = new Dealer(new PredeterminedDeck(new List<Card>()), new DummyCanStartGame());
            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);
            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            var result = game.PlayInHandCards("Ed", Card.EightOfClubs);
            result.GameStatusForPlayer.CardsFaceDownNum.Should().Be(2);
        }

        [Test]
        public void Player_Can_See_Face_Up_Cards()
        {
            var player1 = new Player("Ed", new[] { Card.EightOfClubs }, new[] { Card.SevenOfClubs, Card.FourOfClubs });

            var player2 = PlayerHelper.CreatePlayer("Liam");
            var players = new List<Player>(new[] { player1, player2 });
            var dealer = new Dealer(new PredeterminedDeck(new List<Card>()), new DummyCanStartGame());
            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);
            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            var result = game.PlayInHandCards("Ed", Card.EightOfClubs);
            result.GameStatusForPlayer.CardsFaceUp.Should().BeEquivalentTo(new[] { Card.SevenOfClubs, Card.FourOfClubs });
        }

        [Test]
        public void Player_Can_See_Number_Of_Cards_In_Deck()
        {
            var player1 = new Player("Ed", new[] { Card.EightOfClubs, Card.AceOfClubs, Card.FiveOfClubs });

            var player2 = PlayerHelper.CreatePlayer("Liam");
            var players = new List<Player>(new[] { player1, player2 });
            var dealer = new Dealer(new PredeterminedDeck(new List<Card>(new[] { Card.FourOfClubs, Card.FiveOfClubs, Card.FourOfSpades })), new DummyCanStartGame());
            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);
            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            var result = game.PlayInHandCards("Ed", Card.EightOfClubs);
            result.GameStatusForPlayer.CardsInDeckNum.Should().Be(2);
        }

        [Test]
        public void Player_Can_See_Cards_In_Play_Pile()
        {
            var player1 = new Player("Ed", new[] { Card.EightOfClubs, Card.AceOfClubs, Card.FiveOfClubs });

            var player2 = PlayerHelper.CreatePlayer(Card.AceOfClubs, "Liam");
            var players = new List<Player>(new[] { player1, player2 });
            var dealer = new Dealer(new PredeterminedDeck(new List<Card>(new[] { Card.FourOfClubs, Card.FiveOfClubs, Card.FourOfSpades })), new DummyCanStartGame());
            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);
            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            var result = game.PlayInHandCards("Ed", Card.EightOfClubs);
            result.GameStatusForPlayer.CurrentPlayer.Should().Be("Liam");
        }

        [Test]
        public void Player_Can_See_Current_Players_Name()
        {
            var player1 = new Player("Ed", new[] { Card.EightOfClubs, Card.AceOfClubs, Card.FiveOfClubs });

            var player2 = PlayerHelper.CreatePlayer(Card.AceOfClubs, "Liam");
            var players = new List<Player>(new[] { player1, player2 });
            var dealer = new Dealer(new PredeterminedDeck(new List<Card>(new[] { Card.FourOfClubs, Card.FiveOfClubs, Card.FourOfSpades })), new DummyCanStartGame());
            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);
            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            var result = game.PlayInHandCards("Ed", Card.EightOfClubs);
            result.GameStatusForPlayer.PlayPile.ShouldBeEquivalentTo(new[] { Card.EightOfClubs });
        }

        [Test]
        public void After_First_Valid_Move_Number_Of_Valid_Moves_Is_One()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.AceOfClubs, "Ed");
            var game = DealerHelper.TestDealer(new[] { player1 }).CreateGameInitialisation().StartGame();
            var result = game.PlayInHandCards("Ed", Card.AceOfClubs);

            result.GameStatusForPlayer.NumberOfValidMoves.Should().Be(1);
        }

        [Test]
        public void After_Second_Valid_Move_Number_Of_Valid_Moves_Is_Two()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.EightOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.AceOfClubs, "Liam");
            var game = DealerHelper.TestDealer(new[] { player1, player2 }).CreateGameInitialisation().StartGame();
            game.PlayInHandCards("Ed", Card.EightOfClubs);
            var result = game.PlayInHandCards("Liam", Card.AceOfClubs);

            result.GameStatusForPlayer.NumberOfValidMoves.Should().Be(2);
        }

        [Test]
        public void When_Player_Cant_Go_Valid_Move_Number_Is_Increased()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.EightOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.AceOfClubs, "Liam");
            var game = DealerHelper.TestDealer(new[] { player1, player2 }).CreateGameInitialisation().StartGame();
            var result = game.PlayerCannotPlayCards("Ed");

            result.GameStatusForPlayer.NumberOfValidMoves.Should().Be(1);
        }
    }

    [TestFixture]
    public class GameResultTestsWhenInvalidCardPlayed
    {
        [Test]
        public void Player_Name_Should_Be_Last_Player()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.EightOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.SevenOfClubs, "Liam");
            var players = new List<Player>(new[] { player1, player2 });
            var dealer = DealerHelper.TestDealer(players);
            var game = dealer.CreateGameInitialisation().StartGame();

            game.PlayInHandCards("Ed", Card.EightOfClubs);
            var result = game.PlayInHandCards("Liam", Card.SevenOfClubs);
            result.GameStatusForPlayer.Name.Should().Be("Liam");
        }

        [Test]
        public void Player_Can_See_Own_Cards_In_Hand()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.EightOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(new[] { Card.SevenOfClubs, Card.AceOfClubs, Card.FiveOfClubs }, "Liam");

            var players = new List<Player>(new[] { player1, player2 });
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            game.PlayInHandCards("Ed", Card.EightOfClubs);
            var result = game.PlayInHandCards("Liam", Card.SevenOfClubs);
            result.GameStatusForPlayer.CardsInHand.Should().BeEquivalentTo(new[] { Card.SevenOfClubs, Card.AceOfClubs, Card.FiveOfClubs });
        }

        [Test]
        public void Player_Can_See_Number_Of_Cards_Face_Down()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.EightOfClubs }, "Ed");
            var player2 = new Player("Liam", new[] { Card.SevenOfClubs }, new Card[0], new[] { Card.FiveOfClubs, Card.FourOfClubs });


            var players = new List<Player>(new[] { player1, player2 });
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            game.PlayInHandCards("Ed", Card.EightOfClubs);
            var result = game.PlayInHandCards("Liam", Card.SevenOfClubs);
            result.GameStatusForPlayer.CardsFaceDownNum.Should().Be(2);
        }

        [Test]
        public void Player_Can_See_Face_Up_Cards()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.EightOfClubs }, "Ed");
            var player2 = new Player("Liam", new[] { Card.SevenOfClubs }, new[] { Card.FiveOfClubs, Card.FourOfClubs });

            var players = new List<Player>(new[] { player1, player2 });
            var dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            game.PlayInHandCards("Ed", Card.EightOfClubs);
            var result = game.PlayInHandCards("Liam", Card.SevenOfClubs);
            result.GameStatusForPlayer.CardsFaceUp.ShouldBeEquivalentTo(new[] { Card.FiveOfClubs, Card.FourOfClubs });
        }

        [Test]
        public void Player_Can_See_Number_Of_Cards_In_Deck()
        {
            var player1 = PlayerHelper.CreatePlayer(new[] { Card.EightOfClubs, Card.EightOfClubs, Card.EightOfClubs }, "Ed");
            var player2 = new Player("Liam", new[] { Card.SevenOfClubs, Card.SevenOfClubs, Card.SevenOfClubs }, new[] { Card.FiveOfClubs, Card.FourOfClubs });

            var players = new List<Player>(new[] { player1, player2 });
            var dealer = new Dealer(new PredeterminedDeck(new List<Card>(new[] { Card.FourOfClubs, Card.FiveOfClubs, Card.FourOfSpades, Card.FourOfSpades })), new DummyCanStartGame());
            dealer.AddPlayer(player1);
            dealer.AddPlayer(player2);

            var game = dealer.CreateGameInitialisation().StartGameWithPlayPile(player1, new List<Card>());

            //One card will be removed by this valid move
            game.PlayInHandCards("Ed", Card.EightOfClubs);
            var result = game.PlayInHandCards("Liam", Card.SevenOfClubs);
            result.GameStatusForPlayer.CardsInDeckNum.Should().Be(3);
        }
    }

    [TestFixture]
    public class GameStatusOfOtherPlayers
    {
        [Test]
        public void Other_Players_Names_Are_Included()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TwoOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.ThreeOfClubs, "Liam");
            var player3 = PlayerHelper.CreatePlayer(Card.FourOfClubs, "Dave");

            var dealer = DealerHelper.TestDealer(new[] { player1, player2, player3 });
            var game = dealer.CreateGameInitialisation().StartGame(player1);

            var result = game.PlayInHandCards("Ed", Card.TwoOfClubs);

            result.GameStatusForPlayer.GameStatusForOpponents.Select(s => s.Name).ShouldBeEquivalentTo(new[] { "Liam", "Dave" });
        }

        [Test]
        public void Can_See_Num_In_Han_Cards()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TwoOfClubs, "Ed");
            var player2 = PlayerHelper.CreatePlayer(Card.ThreeOfClubs, "Liam");
            var player3 = PlayerHelper.CreatePlayer(new[] { Card.FourOfClubs, Card.JackOfClubs }, "Dave");

            var dealer = DealerHelper.TestDealer(new[] { player1, player2, player3 });
            var game = dealer.CreateGameInitialisation().StartGame(player1);


            var result = game.PlayInHandCards("Ed", Card.TwoOfClubs);

            result.GameStatusForPlayer.GameStatusForOpponents.First(s => s.Name == "Liam").CardsInHandNum.Should().Be(1);
            result.GameStatusForPlayer.GameStatusForOpponents.First(s => s.Name == "Dave").CardsInHandNum.Should().Be(2);
        }

        [Test]
        public void Can_See_Num_Face_Down_Cards()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TwoOfClubs, "Ed");
            var player2 = new Player("Liam", new Card[0], new Card[0], new[] { Card.JackOfClubs, Card.KingOfClubs });
            var player3 = new Player("Dave", new Card[0], new Card[0], new[] { Card.JackOfClubs, Card.KingOfClubs, Card.SevenOfClubs });

            var dealer = DealerHelper.TestDealer(new[] { player1, player2, player3 });
            var game = dealer.CreateGameInitialisation().StartGame(player1);

            var result = game.PlayInHandCards("Ed", Card.TwoOfClubs);

            result.GameStatusForPlayer.GameStatusForOpponents.First(s => s.Name == "Liam").CardsFaceDownNum.Should().Be(2);
            result.GameStatusForPlayer.GameStatusForOpponents.First(s => s.Name == "Dave").CardsFaceDownNum.Should().Be(3);
        }

        [Test]
        public void Can_See_Face_Up_Cards()
        {
            var player1 = PlayerHelper.CreatePlayer(Card.TwoOfClubs, "Ed");
            var player2 = new Player("Liam", new Card[0], new[] { Card.JackOfClubs, Card.KingOfClubs, Card.TwoOfClubs });
            var player3 = new Player("Dave", new Card[0], new[] { Card.JackOfClubs });

            var dealer = DealerHelper.TestDealer(new[] { player1, player2, player3 });
            var game = dealer.CreateGameInitialisation().StartGame(player1);

            var result = game.PlayInHandCards("Ed", Card.TwoOfClubs);

            result.GameStatusForPlayer.GameStatusForOpponents.First(s => s.Name == "Liam").CardsFaceUp.ShouldBeEquivalentTo(new[] { Card.JackOfClubs, Card.KingOfClubs, Card.TwoOfClubs });
            result.GameStatusForPlayer.GameStatusForOpponents.First(s => s.Name == "Dave").CardsFaceUp.ShouldBeEquivalentTo(new[] { Card.JackOfClubs });
        }
    }

    
}
