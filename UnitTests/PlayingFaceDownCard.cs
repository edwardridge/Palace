using FluentAssertions;
using NUnit.Framework;
using Palace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHelpers;

namespace UnitTests
{
    [TestFixture]
    public class PlayingFaceDownCardThatIsntValid
    {
        Player player1;
        Player player2;
        Game game;
        [SetUp]
        public void Setup()
        {
            //Extra card face down is needed to avoid a game over result
            player1 = PlayerHelper.CreatePlayer(new Card[0], "Ed", null, new[] { Card.ThreeOfClubs, Card.FourOfClubs});
            player2 = PlayerHelper.CreatePlayer("Liam");
            game = DealerHelper.TestDealer(new[] { player1, player2 })
                .CreateGameInitialisation()
                .StartGameWithPlayPile(player1, new[] { Card.FourOfClubs });

        }

        [Test]
        public void It_Is_Next_Players_Turn()
        {
            var result = game.PlayFaceDownCards("Ed", Card.ThreeOfClubs);
            game.State.GetCurrentPlayer().Should().Be(player2);
        }
        
        [Test]
        public void Player_Gets_Play_Pile_And_Card_Played()
        {
            var result = game.PlayFaceDownCards("Ed", Card.ThreeOfClubs);
            player1.CardsInHand.Count().Should().Be(2);
        }

        [Test]
        public void Play_Pile_Is_Cleared()
        {
            var result = game.PlayFaceDownCards("Ed", Card.ThreeOfClubs);
            game.State.PlayPile.Count().Should().Be(0);
        }
        
        [Test]
        public void Valid_Moves_Increases()
        {
            game.State.NumberOfValdMoves.Should().Be(0);
            var result = game.PlayFaceDownCards("Ed", Card.ThreeOfClubs);
            game.State.NumberOfValdMoves.Should().Be(1);
        }

        [Test]
        public void Card_Is_Removed_From_Face_Down_Cards()
        {
            var result = game.PlayFaceDownCards("Ed", Card.ThreeOfClubs);
            player1.CardsFaceDown.Count().Should().Be(1);
        }
    }
}
