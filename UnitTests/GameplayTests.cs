using System;
using NUnit.Framework;
using Palace;
using System.Linq;

namespace UnitTests
{
	[TestFixture]
	public class GameplayTests
	{
		MockPlayerBuilder player1Builder;
		MockPlayerBuilder player2Builder;

		[SetUp]
		public void Setup(){
			player1Builder = new MockPlayerBuilder ().WithName ("Ed").WithState (PlayerState.Ready);
			player2Builder = new MockPlayerBuilder ().WithName ("Liam").WithState (PlayerState.Ready);
		}

		[Test]
		public void Cannot_Play_A_Card_When_Game_Not_Started(){
			var player1 = player1Builder.WithCards (new []{ 2, 3, 4 }).Build ();
			var player2 = player2Builder.WithCards (new []{ 2, 3, 4 }).Build ();

			var game = new Game (new []{player1, player2},new Deck(new NonShuffler()));
			//Dont' start game 
			var result = game.PlayCards (player1, player1.Cards.First());

			Assert.AreEqual (ResultOutcome.Fail, result);
		}

		[Test]
		public void Player_Cannot_Play_Card_Player_Doesnt_Have(){
			var player1 = player1Builder.WithCards (new []{ 2, 3, 4 }).Build ();
			var player2 = player2Builder.WithCards (new []{ 2, 3, 4 }).Build ();

			var game = new Game (new []{player1, player2},new Deck(new NonShuffler()));
			game.Start ();
			var result = game.PlayCards (player1, new Card(CardValue.Five,Suit.Club));

			Assert.AreEqual (ResultOutcome.Fail, result);
		}

		[Test]
		public void Player_Can_Play_Card_Player_Has(){
			var player1 = player1Builder.WithCards (new []{ 2, 3, 4 }).Build ();
			var player2 = player2Builder.WithCards (new []{ 2, 3, 4 }).Build ();

			var game = new Game (new []{player1, player2},new Deck(new NonShuffler()));
			game.Start ();

			var result = game.PlayCards (player1, new Card(CardValue.Two,Suit.Club));

			Assert.AreEqual (ResultOutcome.Success, result);
		}
	}
}

