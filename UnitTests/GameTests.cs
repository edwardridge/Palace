using NUnit.Framework;
using System;
using System.Collections.Generic;
using Palace;
using System.Collections;

namespace UnitTests
{
	[TestFixture]
	public class GameTests
	{
		[TestFixture]
		public class SetupGame
		{
			Game game;
			Player player1;
			Player player2;
			ICollection<Player> players;

			[SetUp]
			public void Setup(){
				game = new Game ();
				player1 = new Player ("Ed");
				player2 = new Player ("Liam");
				players = new List<Player> ();

				players.Add (player1);
				players.Add (player2);
			}

			[Test]
			public void Start_With_Two_Players_Two_Players_Are_In_Game(){
				game.Start (players);

				var playerCount = game.NumberOfPlayers;
				Assert.AreEqual (2, playerCount);
			}

			[Test]
			public void Player_1_Has_First_Go(){
				game.Start (players);

				Assert.AreEqual (player1.Name, game.CurrentTurn().Name);
			}
		}
	}
}

