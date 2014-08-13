using System;
using NUnit.Framework;
using Palace;
using System.Linq;

namespace UnitTests
{
	[TestFixture]
	public class GameplayTests
	{
		[Test]
		public void Cannot_Play_A_Card_When_Game_Not_Started(){
			var players = new [] { new Player ("Ed"), new Player ("Liam") };
			var game = GameHelper.CreateGameWithPlayers (players);

			var result = game.PlayCards (players [0], players [0].Cards.First());

			Assert.AreEqual (ResultOutcome.Fail, result);
		}

		[Test]
		public void Plays_Card_Card_Is_Removed_From_Players_Hand(){
			var players = new [] { new Player ("Ed"), new Player ("Liam") };
			var player1Cards = new [] { 2, 4, 5 };
			var player2Cards = new [] { 3, 5, 6 };

			var game = new Game ();
			game.Setup (players, new Deck(new PredeterminedShuffler(CardHelpers.GetCardsFromValues(new []{10}))));

			game.StartGameForTest (players, new [] { player1Cards, player2Cards });
			var lowestCard = players [0].LowestCard;

			game.PlayCards (players [0], lowestCard);
			Assert.IsTrue(!players[0].Cards.Any(card=>card.Value == lowestCard.Value));
		}
	}
}

