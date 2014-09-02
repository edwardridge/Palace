using NUnit.Framework;
using System;
using System.Collections.Generic;
using Palace;
using System.Collections;
using System.Linq;
using Moq;

namespace UnitTests
{
    [TestFixture]
	public static class GameInitialiseTests
	{
		[TestFixture]
		public class SetupGame
		{
			Game game;
			Player[] players;

			[SetUp]
			public void Setup(){
				players = new []{new Player ("Ed"), new Player("Liam")};
				game = GameHelper.CreateGameWithPlayers (players);
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
				game = GameHelper.CreateGameWithPlayers (players);
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
			public class FirstMoveWhenAllPlayersAreReady{

				MockPlayerBuilder player1Builder;
				MockPlayerBuilder player2Builder;
				MockPlayerBuilder player3Builder;
				Game game;

				[SetUp]
				public void Setup(){
					player1Builder = new MockPlayerBuilder ().WithState (PlayerState.Ready).WithName ("Ed");
					player2Builder = new MockPlayerBuilder ().WithState (PlayerState.Ready).WithName ("Liam");
					player3Builder = new MockPlayerBuilder ().WithState (PlayerState.Ready).WithName ("Jess");
				}

				[Test]
				public void P1_Has_Lowest_Card_P1_Goes_First(){
					var player1 = player1Builder.WithCards (new []{ 2 }).Build();
					var player2 = player2Builder.WithCards (new []{ 3 }).Build();

					game = new Game (new[]{ player1, player2 }, new Deck (new NonShuffler ()));
					game.Start ();
					Assert.AreEqual (player1.Name, game.CurrentPlayer.Name);
				}

				[Test]
				public void P2_Has_Lowest_Card_P2_Goes_First(){
					var player1 = player1Builder.WithCards (new []{ 3 }).Build();
					var player2 = player2Builder.WithCards (new []{ 2 }).Build();

					game = new Game (new[]{ player1, player2 }, new Deck (new NonShuffler ()));
					game.Start ();
					Assert.AreEqual (player2.Name, game.CurrentPlayer.Name);
				}

				[Test] 
				public void P3_Has_Lowest_Card_P3_Goes_First(){
					var player1 = player1Builder.WithCards (new []{ 3 }).Build();
					var player2 = player2Builder.WithCards (new []{ 3 }).Build();
					var player3 = player3Builder.WithCards (new []{ 2 }).Build();

					game = new Game (new[]{ player1, player2, player3 }, new Deck (new NonShuffler ()));
					game.Start ();
					Assert.AreEqual (player3.Name, game.CurrentPlayer.Name);
				}

//				static Mock<IPlayer> MockReadyPlayer (string name)
//				{
//					var player1 = new Mock<IPlayer> ();
//					player1.SetupGet (prop => prop.Name).Returns (name);
//					player1.SetupGet (prop => prop.Cards).Returns (CardHelpers.GetCardsFromValues (new[] {
//						3
//					}));
//					player1.SetupGet (prop => prop.State).Returns (PlayerState.Ready);
//					return player1;
//				}
			}
		}
	}
}

