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
			var player1 = new MockPlayerBuilder ().WithName ("Ed").WithState (PlayerState.Ready).WithCards (new []{ 2, 3, 4 }).Build ();
			var player2 = new MockPlayerBuilder ().WithName ("Ed").WithState (PlayerState.Ready).WithCards (new []{ 2, 3, 4 }).Build ();

			var game = new Game (new []{player1, player2},new Deck(new NonShuffler()));

			var result = game.PlayCards (player1, player1.Cards.First());

			Assert.AreEqual (ResultOutcome.Fail, result);
		}


	}
}

