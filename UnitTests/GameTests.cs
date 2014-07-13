﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using Palace;
using System.Collections;
using System.Linq;

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

				IRandomiser randomiser = new StubRandomiser ();
				Deck deck = new Deck (randomiser);

				player1 = new Player ("Ed");
				player2 = new Player ("Liam");
				players = new List<Player> ();

				players.Add (player1);
				players.Add (player2);

				game.Start (players, deck);
			}

			[Test]
			public void Start_With_Two_Players_Two_Players_Are_In_Game(){
				var playerCount = game.NumberOfPlayers;
				Assert.AreEqual (2, playerCount);
			}

			[Test]
			public void Player_1_Has_First_Go(){
				Assert.AreEqual (player1.Name, game.CurrentTurn().Name);
			}

			[Test]
			public void Player_1_Has_6_Face_Up_Cards(){
				Assert.AreEqual (6, player1.NumFaceUpCards);
			}

			[Test]
			public void Player_2_Has_6_Face_Up_Cards(){
				Assert.AreEqual (6, player2.NumFaceUpCards);
			}
		}
	}
}

