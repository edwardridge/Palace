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
		public static Game CreateGameWithTwoPlayers(IList<Player> players){
			var game = new Game ();

			var randomiser = new NonShuffler ();
			var deck = new Deck (randomiser);

			game.Setup (players, deck);

			return game;
		}

		[TestFixture]
		public class SetupGame
		{
			Game game;
			Player[] players;

			[SetUp]
			public void Setup(){
				players = new []{new Player ("Ed"), new Player("Liam")};
				game = CreateGameWithTwoPlayers (players);
			}

			[Test]
			public void Setup_With_Two_Players_Two_Players_Are_In_Game(){
				var playerCount = game.NumberOfPlayers;

				Assert.AreEqual (2, playerCount);
			}

			[Test]
			public void Player_1_Has_6_In_Hand_Cards(){
				Assert.AreEqual (6, players[0].NumCards(CardOrientation.InHand));
			}

			[Test]
			public void Player_2_Has_6_In_Hand_Cards(){
				Assert.AreEqual (6, players[1].NumCards(CardOrientation.InHand));
			}

			[Test]
			public void Player_1_Has_3_Face_Down_Cards(){
				Assert.AreEqual (3, players[0].NumCards(CardOrientation.FaceDown));
			}
		}

		[TestFixture]
		public class StartGame
		{
			Game game;
			Player[] players;

			[SetUp]
			public void Setup(){
				players = new []{new Player ("Ed"), new Player("Liam")};
				game = CreateGameWithTwoPlayers (players);
			}

			[Test]
			public void Game_Cannot_Start_When_Both_Players_Not_Ready(){
				var result = game.Start ();

				Assert.AreEqual (ResultOutcome.Fail, result.ResultOutcome);
			}

			[Test]
			public void Game_Can_Start_When_Both_Players_Are_Ready(){
				PlayerHelper.PutSomeCardsFaceUp (players[0], 3);
				PlayerHelper.PutSomeCardsFaceUp (players[1], 3);

				players[0].Ready ();
				players[1].Ready ();
				var result = game.Start ();

				Assert.AreEqual (ResultOutcome.Success, result.ResultOutcome);
			}

			[Test]
			public void Player_Cannot_Be_Ready_With_No_Face_Up_Cards(){
				var result = players[0].Ready();

				Assert.AreEqual (ResultOutcome.Fail, result.ResultOutcome);
			}

			[Test]
			public void Player_Cannot_Be_Ready_With_Two_Face_Up_Cards(){
				PlayerHelper.PutSomeCardsFaceUp (players[0], 2);

				var result = players[0].Ready ();

				Assert.AreEqual (ResultOutcome.Fail, result.ResultOutcome);
			}

			[Test]
			public void Player_Can_Be_Ready_When_Three_Cards_Are_Face_Up(){
				PlayerHelper.PutSomeCardsFaceUp (players[0], 3);

				var result = players[0].Ready ();

				Assert.AreEqual (ResultOutcome.Success, result.ResultOutcome);
			}

			[Test]
			public void Player_Cannot_Put_Fourth_Card_Face_Up(){
				PlayerHelper.PutSomeCardsFaceUp (players[0], 3);

				var result = players[0].PutCardFaceUp(players[0].Cards.ToArray()[3]);

				Assert.AreEqual (ResultOutcome.Fail, result.ResultOutcome);
			}

			[TestFixture]
			public class FirstMoveTests{

				Player[] players;
				Game game;

				[SetUp]
				public void Setup(){
					game = new Game ();
					players = new []{new Player ("Ed"), new Player("Liam")};
				}

				[Test]
				public void P1_Has_2_As_Lowest_Card_P2_Has_3_As_Lowest_Card_P1_Goes_First(){
					SetupGameForTest (game, players, new [] { new []{ 2, 5, 6}, new[]{3, 5, 6 } });

					Assert.AreEqual (players[0].Name, game.CurrentPlayer.Name);
				}

				[Test]
				public void P1_Has_3_As_Lowest_Card_P2_Has_2_As_Lowest_Card_P2_Goes_First(){
					SetupGameForTest (game, players, new [] { new []{ 3, 5, 6}, new[]{2, 5, 6 } });

					Assert.AreEqual (players[1].Name, game.CurrentPlayer.Name);
				}

				public void SetupGameForTest(Game game, IList<Player> players, IList<IList<int>> cards){
					for (int i = 0; i < players.Count; i++) {
						players [i].SetupPlayerForTest (CardHelpers.GetCardsFromValues (cards [i]));
					}

					game.SetupGameForTest (players);

					game.Start ();
				}
			}
		}
	}
}

