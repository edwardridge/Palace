using NUnit.Framework;
using System;
using System.Collections.Generic;
using Palace;
using System.Collections;
using System.Linq;
using Moq;
using FluentAssertions;

namespace UnitTests
{
    [TestFixture]
	public static class GameInitialiseTests
	{
		[TestFixture]
		public class SetupGame
		{
			Game game;
			IPlayer player1;
			IPlayer player2;

			[SetUp]
			public void Setup(){
				player1 = new StubReadyPlayer ();
				player2 = new StubReadyPlayer ();
				var deck = new Deck (new NonShuffler ());
				game = new Game (new []{player1, player2}, deck);
				game.Setup ();
			}

			[Test]
			public void Setup_With_Two_Players_Two_Players_Are_In_Game(){
				var playerCount = game.NumberOfPlayers;

				playerCount.Should ().Be (2);
			}

			[Test]
			public void Player_1_Has_6_In_Hand_Cards(){
				player1.Cards.Count (card => card.CardOrientation == CardOrientation.InHand).Should ().Be (6);
			}

			[Test]
			public void Player_2_Has_6_In_Hand_Cards(){
				player2.Cards.Count (card => card.CardOrientation == CardOrientation.InHand).Should ().Be (6);
			}

			[Test]
			public void Player_1_Has_3_Face_Down_Cards(){
				player1.Cards.Count (card => card.CardOrientation == CardOrientation.FaceDown).Should ().Be (3);
			}
		}

		[TestFixture]
		public class GameStart
		{
			[Test]
			public void Cannot_Start_When_Both_Players_Not_Ready(){
				var player1 = new MockPlayerBuilder ().WithState (PlayerState.Setup).Build();
				var player2 = new MockPlayerBuilder ().WithState (PlayerState.Ready).Build();
				var game = new Game (new []{ player1, player2 }, new Deck (new NonShuffler ()));
				var outcome = game.Start ();

				outcome.Should ().Be (ResultOutcome.Fail);
			}

			[Test]
			public void Can_Start_When_Both_Players_Are_Ready(){
				var player1 = new MockPlayerBuilder ().WithState (PlayerState.Ready).Build();
				var player2 = new MockPlayerBuilder ().WithState (PlayerState.Ready).Build();
				var game = new Game (new []{ player1, player2 }, new Deck (new NonShuffler ()));
				var outcome = game.Start ();

				outcome.Should ().Be (ResultOutcome.Success);
			}

		}

		[TestFixture]
		public class PlayerReady{
			Player player;

			[SetUp]
			public void Setup(){
				player = new Player ("Ed");
			}

			[Test]
			public void Player_Cannot_Be_Ready_With_No_Face_Up_Cards(){
				//Don't put any cards face up
				var outcome = player.Ready ();

				outcome.Should ().Be (ResultOutcome.Fail);
			}

			[Test]
			public void Player_Cannot_Be_Ready_With_Two_Face_Up_Cards(){
				player.AddCards(new []{new Card(CardValue.Ace, Suit.Club,CardOrientation.FaceUp), 
					new Card(CardValue.Ace, Suit.Club,CardOrientation.FaceUp)});

				var outcome = player.Ready ();

				outcome.Should ().Be (ResultOutcome.Fail);
			}

			[Test]
			public void Player_Can_Be_Ready_When_Three_Cards_Are_Face_Up(){
				player.AddCards(new []{new Card(CardValue.Ace, Suit.Club,CardOrientation.FaceUp), 
					new Card(CardValue.Ace, Suit.Club,CardOrientation.FaceUp),
					new Card(CardValue.Ace, Suit.Club,CardOrientation.FaceUp)});

				var outcome = player.Ready ();

				outcome.Should ().Be (ResultOutcome.Success);
			}

			[Test]
			public void Player_Cannot_Put_Fourth_Card_Face_Up ()
			{
				player.AddCards (new [] {new Card (CardValue.Ace, Suit.Club, CardOrientation.FaceUp), 
					new Card (CardValue.Ace, Suit.Club, CardOrientation.FaceUp),
					new Card (CardValue.Ace, Suit.Club, CardOrientation.FaceUp)
				});

				var outcome = player.PutCardFaceUp (new Card (CardValue.Ace, Suit.Club, CardOrientation.FaceUp));

				outcome.Should ().Be (ResultOutcome.Fail);
			}

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

					game.CurrentPlayer.Should().Be(player1);
				}

				[Test]
				public void P2_Has_Lowest_Card_P2_Goes_First(){
					var player1 = player1Builder.WithCards (new []{ 3 }).Build();
					var player2 = player2Builder.WithCards (new []{ 2 }).Build();

					game = new Game (new[]{ player1, player2 }, new Deck (new NonShuffler ()));
					game.Start ();
					
					game.CurrentPlayer.Should().Be(player2);
				}

				[Test] 
				public void P3_Has_Lowest_Card_P3_Goes_First(){
					var player1 = player1Builder.WithCards (new []{ 3 }).Build();
					var player2 = player2Builder.WithCards (new []{ 3 }).Build();
					var player3 = player3Builder.WithCards (new []{ 2 }).Build();

					game = new Game (new[]{ player1, player2, player3 }, new Deck (new NonShuffler ()));
					game.Start ();

					game.CurrentPlayer.Should().Be(player3);
				}
			}
		}
	}


