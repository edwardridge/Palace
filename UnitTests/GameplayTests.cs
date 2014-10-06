using System;
using NUnit.Framework;
using Palace;
using System.Linq;
using FluentAssertions;

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

			var game = new Game (new []{player1},new Deck(new NonShuffler()));
			//Dont' start game 
			var result = game.PlayCards (player1, player1.Cards.First());

			result.Should ().Be (ResultOutcome.Fail);
		}

		[Test]
		public void Player_Cannot_Play_Card_Player_Doesnt_Have(){
			var player1 = player1Builder.WithCards (new []{ 2, 3, 4 }).Build ();

			var game = new Game (new []{player1},new Deck(new NonShuffler()));
			game.Start ();
			var playingCardsPlayerDoesntHaveOutcome = game.PlayCards (player1, new Card(CardValue.Five,Suit.Club));

			playingCardsPlayerDoesntHaveOutcome.Should ().Be (ResultOutcome.Fail);
		}

		[Test]
		public void Player_Can_Play_Card_Player_Has(){
			var cardsPlayerHas = new []{ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Four, Suit.Club) };
			var player1 = new StubReadyPlayer ();
			player1.AddCards (cardsPlayerHas);

			var game = new Game (new []{player1},new Deck(new NonShuffler()));
			game.Start ();

			var playingCardsPlayerHasOutcome = game.PlayCards (player1, cardsPlayerHas[0]);

			playingCardsPlayerHasOutcome.Should ().Be (ResultOutcome.Success);
		}

		[Test]
		public void When_Playing_Multiple_Cards_Player_Cannot_PLay_Card_They_Dont_Have(){
			var cardsPlayerHas = new []{ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Four, Suit.Club) };
			var player1 = new StubReadyPlayer ();
			player1.AddCards (cardsPlayerHas);

			var cardsPlayerPlays = new []{ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Four, Suit.Spade) };
			var game = new Game (new[]{ player1 }, new Deck (new NonShuffler ()));
			game.Start ();
			var result = game.PlayCards (player1, cardsPlayerPlays);

			result.Should ().Be (ResultOutcome.Fail);
			
		}

		[Test]
		public void When_Player_Plays_Card_Card_Is_Removed_From_Hand(){
			var cardToPlay = new Card (CardValue.Four, Suit.Club);
			var player1 = new StubReadyPlayer ();
			player1.AddCards (new []{ cardToPlay });

			var game = new Game(new []{player1}, new Deck(new NonShuffler()));
			game.Start ();
			game.PlayCards (player1, cardToPlay);

			player1.Cards.Count.Should ().Be (0);
		}



		[Test]
		public void Can_Play_Multiple_Cards_Of_Same_Value(){
			var cardsToPlay = new []{ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Four, Suit.Club) };
			var player1 = new StubReadyPlayer ();
			player1.AddCards (cardsToPlay);

			var game = new Game (new[]{player1}, new Deck (new NonShuffler ()));
			game.Start ();
			var playerPlaysMultipleCardsOfSameValueOutcome = game.PlayCards (player1, cardsToPlay);

			playerPlaysMultipleCardsOfSameValueOutcome.Should ().Be (ResultOutcome.Success);
		}

		[Test]
		public void When_PLayer_Plays_Multiple_Cards_Cards_Are_Removed_From_Hand(){
			var cardsToPlay = new []{ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Four, Suit.Club) };
			var player1 = new StubReadyPlayer ();
			player1.AddCards (cardsToPlay);

			var game = new Game (new[]{player1}, new Deck (new NonShuffler ()));
			game.Start ();
			game.PlayCards (player1, cardsToPlay);

			player1.Cards.Count ().Should ().Be (0);
		}

		[Test]
		public void Cannot_Play_Multiple_Cards_Of_Different_Value(){
			var cardsToPlay = new []{ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Ace, Suit.Club) };
			var player1 = new StubReadyPlayer ();
			player1.AddCards (cardsToPlay);

			var game = new Game (new[]{ player1 }, new Deck (new NonShuffler ()));
			game.Start ();
			var playerPlaysMultipleCardsOfDifferentValueOutcome = game.PlayCards (player1, cardsToPlay);

			playerPlaysMultipleCardsOfDifferentValueOutcome.Should ().Be (ResultOutcome.Fail);
		}

		[Test]
		public void When_PLayer_Plays_Multiple_Of_Different_Value_No_Cards_Are_Removed_From_Hand(){
			var cardsToPlay = new []{ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Ace, Suit.Club) };
			var player1 = new StubReadyPlayer ();
			player1.AddCards (cardsToPlay);

			var game = new Game (new[]{ player1 }, new Deck (new NonShuffler ()));
			game.Start ();
			game.PlayCards (player1, cardsToPlay);

			player1.Cards.Count().Should ().Be (cardsToPlay.Count());
		}


		[Test]
		public void When_Player_Plays_Card_Card_Is_Added_To_PlayPile(){
			var cardToPlay = new Card (CardValue.Four, Suit.Club);
			var player1 = new StubReadyPlayer ();
			player1.AddCards (new []{cardToPlay});

			var game = new Game (new[]{ player1 }, new Deck (new NonShuffler ()));
			game.Start ();
			game.PlayCards (player1, cardToPlay);

			game.PlayPileCardCount().Should ().Be (1);
		}

		[TestFixture]
		public class RulesTests{
			[Test]

			public void Playing_Card_High_In_Value_Is_Valid(){
				var cardToPlay = new Card (CardValue.Three, Suit.Club);
				var player1 = new StubReadyPlayer ();
				player1.AddCards (new []{ cardToPlay });

				var game = new Game (new []{ player1 }, new Deck (new NonShuffler ()));
				game.Start ();
				var playingCardWithHigherValueOutcome = game.PlayCards (player1, cardToPlay);

				playingCardWithHigherValueOutcome.Should ().Be (ResultOutcome.Success);
			}
		}
	}
}

