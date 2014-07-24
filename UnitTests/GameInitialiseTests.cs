using NUnit.Framework;
using System;
using System.Collections.Generic;
using Palace;
using System.Collections;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
	public static class GameInitialiseTests
	{
		public static Game CreateGameWithTwoPlayers(ref Player player1, ref Player player2){
			var game = new Game ();

			var randomiser = new NonShuffler ();
			var deck = new Deck (randomiser);

			player1 = new Player ("Ed");
			player2 = new Player ("Liam");
			var players = new List<Player> { player1, player2 };

		    game.Setup (players, deck);

			return game;
		}

		[TestFixture]
		public class SetupGame
		{
			Game game;
			Player player1;
			Player player2;

			[SetUp]
			public void Setup(){
				game = CreateGameWithTwoPlayers (ref player1, ref player2);
			}

			[Test]
			public void Setup_With_Two_Players_Two_Players_Are_In_Game(){
				var playerCount = game.NumberOfPlayers;

				Assert.AreEqual (2, playerCount);
			}

			[Test]
			public void Player_1_Has_6_In_Hand_Cards(){
				Assert.AreEqual (6, player1.NumCards(CardOrientation.InHand));
			}

			[Test]
			public void Player_2_Has_6_In_Hand_Cards(){
				Assert.AreEqual (6, player2.NumCards(CardOrientation.InHand));
			}

			[Test]
			public void Player_1_Has_3_Face_Down_Cards(){
				Assert.AreEqual (3, player1.NumCards(CardOrientation.FaceDown));
			}
		}

		[TestFixture]
		public class StartGame
		{
			Game game;
			Player player1;
			Player player2;

			[SetUp]
			public void Setup(){
				game = CreateGameWithTwoPlayers (ref player1, ref player2);
			}

			[Test]
			public void Game_Cannot_Start_When_Both_Players_Not_Ready(){
				var result = game.Start ();

				Assert.AreEqual (ResultOutcome.Fail, result.ResultOutcome);
			}

			[Test]
			public void Game_Can_Start_When_Both_Players_Are_Ready(){
				PlayerHelper.PutSomeCardsFaceUp (player1, 3);
				PlayerHelper.PutSomeCardsFaceUp (player2, 3);

				player1.Ready ();
				player2.Ready ();
				var result = game.Start ();

				Assert.AreEqual (ResultOutcome.Success, result.ResultOutcome);
			}

			[Test]
			public void Player_Cannot_Be_Ready_With_No_Face_Up_Cards(){
				var result = player1.Ready();

				Assert.AreEqual (ResultOutcome.Fail, result.ResultOutcome);
			}

			[Test]
			public void Player_Cannot_Be_Ready_With_Two_Face_Up_Cards(){
				PlayerHelper.PutSomeCardsFaceUp (player1, 2);

				var result = player1.Ready ();

				Assert.AreEqual (ResultOutcome.Fail, result.ResultOutcome);
			}

			[Test]
			public void Player_Can_Be_Ready_When_Three_Cards_Are_Face_Up(){
				PlayerHelper.PutSomeCardsFaceUp (player1, 3);

				var result = player1.Ready ();

				Assert.AreEqual (ResultOutcome.Success, result.ResultOutcome);
			}

			[Test]
			public void Player_Cannot_Put_Fourth_Card_Face_Up(){
				PlayerHelper.PutSomeCardsFaceUp (player1, 3);

				var result = player1.PutCardFaceUp(player1.Cards.ToArray()[3]);

				Assert.AreEqual (ResultOutcome.Fail, result.ResultOutcome);
			}

			[TestFixture]
			public class FirstMoveTests{

				[Test]
				public void P1_Has_2_As_Lowest_Card_P2_Has_3_As_Lowest_Card_P1_Goes_First(){
					var p1 = new Player ("Ed");
					var p2 = new Player ("Liam");
					p1.SetupPlayerForTest (CardHelpers.GetCardsFromValues (new []{2,5,6}));
					p2.SetupPlayerForTest (CardHelpers.GetCardsFromValues (new []{3, 7, 8}));
					var game = new Game ();
					game.SetupGameForTest (new []{p1,p2});

					game.Start ();

					Assert.AreEqual (p1.Name, game.CurrentPlayer.Name);
				}

				[Test]
				public void P1_Has_3_As_Lowest_Card_P2_Has_2_As_Lowest_Card_P2_Goes_First(){
					var p1 = new Player ("Ed");
					var p2 = new Player ("Liam");
					p1.SetupPlayerForTest (CardHelpers.GetCardsFromValues (new[]{ 3, 5, 6 }));
					p2.SetupPlayerForTest (CardHelpers.GetCardsFromValues (new[]{ 2, 5, 6 }));
					var game = new Game ();
					game.SetupGameForTest (new[]{ p1, p2 });

					game.Start ();
				
					Assert.AreEqual (p2.Name, game.CurrentPlayer.Name);
				}


			}
		}
	}
}

