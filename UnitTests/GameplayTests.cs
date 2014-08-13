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
		public void Cannot_Play_A_Card_WHen_Game_Not_Started(){

			var players = new [] { new Player ("Ed"), new Player ("Liam") };

			var game = GameHelper.CreateGameWithPlayers (players);

			var result = game.PlayCards (players [0], players [0].Cards.First());

			Assert.AreEqual (ResultOutcome.Fail, result);
		}
	}
}

