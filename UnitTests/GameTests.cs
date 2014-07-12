using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests
{
	[TestFixture]
	public class GameTests
	{
		[TestFixture]
		public class SetupGame
		{
			[Test]
			public void Start_With_Two_Players_Two_Players_Are_In_Game(){
				var game = new Game();
				var players = new List<Player> ();
				players.Add (new Player ());
				players.Add (new Player ());

				game.Start (players);

				var playerCount = game.NumberOfPlayers;
				Assert.AreEqual (2, playerCount);
			}
		}
	}
}

