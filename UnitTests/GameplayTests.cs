namespace UnitTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using NUnit.Framework;

    using Palace;

    using TestHelpers;

    [TestFixture]
    public class GameplayChangingGameStateTests
    {
        [Test]
        public void When_Player_Plays_Card_Card_Is_Added_To_PlayPile()
        {
            var cardToPlay = Card.FourOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay);
            var dealer = new Dealer(new[] { player1 }, new StandardDeck(), new DummyCanStartGame());
            var game = dealer.StartGame(player1);
            game.PlayInHandCards(player1, cardToPlay);

            game.PlayPile.Count.Should().Be(1);
        }

        [Test]
        public void When_Player_Plays_Card_Its_Next_Players_Turn()
        {
            var cardToPlay = Card.AceOfClubs;
            var player1 = PlayerHelper.CreatePlayer(cardToPlay, "Ed");
            var player2 = PlayerHelper.CreatePlayer("Liam");
            var dealer = new Dealer(new[] { player1, player2 }, new StandardDeck(), new DummyCanStartGame());
            var game = dealer.StartGame(player1);
            game.PlayInHandCards(player1, cardToPlay);

            game.CurrentPlayer.Should().Be(player2);
        }
    }

}