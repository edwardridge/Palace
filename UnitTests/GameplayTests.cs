﻿using System;
using NUnit.Framework;
using Palace;
using System.Linq;
using FluentAssertions;
using System.Collections.Generic;

namespace UnitTests
{
	[TestFixture]
	public class GameplayTests
	{
		[Test]
		public void Cannot_Play_A_Card_When_Game_Not_Started(){
			var cardsPlayerHas = new List<Card>(){ new Card (CardValue.Four, Suit.Club) };
			var player1 = new StubReadyPlayer (cardsPlayerHas);

			var game = new Game (new []{player1}, NonShufflingDeck());
			//Dont' start game 
			var result = game.PlayCards (player1, player1.Cards.First());

			result.Should ().Be (ResultOutcome.Fail);
		}

		[Test]
		public void Player_Cannot_Play_Card_Player_Doesnt_Have(){
			var cardsPlayerHas = new List<Card>(){ new Card (CardValue.Four, Suit.Club) };
			var player1 = new StubReadyPlayer (cardsPlayerHas);

			var cardsPlayerPlays = new Card (CardValue.Ace, Suit.Club);
			var game = new Game (new []{player1}, NonShufflingDeck(), GameState.GameStarted);
			var playingCardsPlayerHasOutcome = game.PlayCards (player1, cardsPlayerPlays);

			playingCardsPlayerHasOutcome.Should ().Be (ResultOutcome.Fail);
		}

		[Test]
		public void Player_Can_Play_Card_Player_Has(){
			var cardsPlayerHas = new List<Card>(){ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Four, Suit.Club) };
			var player1 = new StubReadyPlayer (cardsPlayerHas);

			var game = new Game (new []{player1}, NonShufflingDeck(), GameState.GameStarted);
			var playingCardsPlayerHasOutcome = game.PlayCards (player1, cardsPlayerHas[0]);

			playingCardsPlayerHasOutcome.Should ().Be (ResultOutcome.Success);
		}

		[Test]
		public void When_Playing_Multiple_Cards_Player_Cannot_PLay_Card_They_Dont_Have(){
			var cardsPlayerHas = new List<Card>(){ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Four, Suit.Club) };
			var player1 = new StubReadyPlayer (cardsPlayerHas);

			var cardsPlayerPlays = new []{ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Four, Suit.Spade) };
			var game = new Game (new[]{ player1 }, NonShufflingDeck(), GameState.GameStarted);
			var result = game.PlayCards (player1, cardsPlayerPlays);

			result.Should ().Be (ResultOutcome.Fail);
			
		}

		[Test]
		public void When_Player_Plays_Card_Card_Is_Removed_From_Hand(){
			var cardToPlay = new Card (CardValue.Four, Suit.Club);
			var player1 = new StubReadyPlayer (cardToPlay);

			var game = new Game(new []{player1}, NonShufflingDeck(), GameState.GameStarted);
			game.PlayCards (player1, cardToPlay);

			player1.Cards.Count.Should ().Be (0);
		}

		[Test]
		public void Can_Play_Multiple_Cards_Of_Same_Value(){
			var cardsToPlay = new List<Card>(){ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Four, Suit.Club) };
			var player1 = new StubReadyPlayer (cardsToPlay);

			var game = new Game (new[]{player1}, NonShufflingDeck(), GameState.GameStarted);
			var playerPlaysMultipleCardsOfSameValueOutcome = game.PlayCards (player1, cardsToPlay);

			playerPlaysMultipleCardsOfSameValueOutcome.Should ().Be (ResultOutcome.Success);
		}

		[Test]
		public void When_PLayer_Plays_Multiple_Cards_Cards_Are_Removed_From_Hand(){
			var cardsToPlay = new List<Card>(){ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Four, Suit.Club) };
			var player1 = new StubReadyPlayer (cardsToPlay);

			var game = new Game (new[]{player1}, NonShufflingDeck(), GameState.GameStarted);
			game.PlayCards (player1, cardsToPlay);

			player1.Cards.Count ().Should ().Be (0);
		}

		[Test]
		public void Cannot_Play_Multiple_Cards_Of_Different_Value(){
			var cardsToPlay = new List<Card>(){ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Ace, Suit.Club) };
			var player1 = new StubReadyPlayer (cardsToPlay);

			var game = new Game (new[]{ player1 }, NonShufflingDeck(), GameState.GameStarted);
			var playerPlaysMultipleCardsOfDifferentValueOutcome = game.PlayCards (player1, cardsToPlay);

			playerPlaysMultipleCardsOfDifferentValueOutcome.Should ().Be (ResultOutcome.Fail);
		}

		[Test]
		public void When_Player_Plays_Multiple_Cards_Of_Different_Value_No_Cards_Are_Removed_From_Hand(){
			var cardsToPlay = new List<Card>(){ new Card (CardValue.Four, Suit.Club), new Card (CardValue.Ace, Suit.Club) };
			var player1 = new StubReadyPlayer (cardsToPlay);

			var game = new Game (new[]{ player1 }, NonShufflingDeck(), GameState.GameStarted);
			game.PlayCards (player1, cardsToPlay);

			player1.Cards.Count().Should ().Be (cardsToPlay.Count());
		}


		[Test]
		public void When_Player_Plays_Card_Card_Is_Added_To_PlayPile(){
			var cardToPlay = new Card (CardValue.Four, Suit.Club);
			var player1 = new StubReadyPlayer (cardToPlay);

			var game = new Game (new[]{ player1 }, NonShufflingDeck(), GameState.GameStarted);
			game.PlayCards (player1, cardToPlay);

			game.PlayPileCardCount().Should ().Be (1);
		}

		[Test]
		public void When_Player_Plays_Card_Its_Next_Players_Turn(){
			var cardToPlay = new Card (CardValue.Ace, Suit.Club);
			var player1 = new StubReadyPlayer (cardToPlay, "Ed");
			var player2 = new StubReadyPlayer ("Liam");

			var game = new Game (new[]{ player1, player2 }, NonShufflingDeck (), GameState.GameStarted);
			game.PlayCards (player1, cardToPlay);

			game.CurrentPlayer.Should ().Be (player2);
		}

		[TestFixture]
		public class RulesTests{
			[TestFixture]
			public class StandardCard{
				[Test]
				public void Playing_Card_Higher_In_Value_Is_Valid(){
					var cardToPlay = new Card (CardValue.Three, Suit.Club);
					var player1 = new StubReadyPlayer (cardToPlay);

					var cardInPile = new Stack<Card>();
					cardInPile.Push (new Card (CardValue.Two, Suit.Club));
					var game = new Game (new []{ player1 }, NonShufflingDeck(), GameState.GameStarted, cardInPile);
					var outcome = game.PlayCards (player1, cardToPlay);

					outcome.Should ().Be (ResultOutcome.Success);
					player1.Cards.Count ().Should ().Be (0);
				}

				[Test]
				public void Playing_Card_Lower_In_Value_Isnt_Valid(){
					var cardToPlay = new Card (CardValue.Three, Suit.Club);
					var player1 = new StubReadyPlayer (cardToPlay);

					var cardInPile = new Card (CardValue.Five, Suit.Club);
					var game = new Game (new []{ player1 }, NonShufflingDeck(), GameState.GameStarted, new Stack<Card>(new []{cardInPile}));
					var outcome = game.PlayCards (player1, cardToPlay);

					outcome.Should ().Be (ResultOutcome.Fail);
					player1.Cards.Count ().Should ().Be (1);
				}

				[Test]
				public void Play_Card_Of_Same_Value_Is_Valid(){
					var cardToPlay = new Card (CardValue.Four, Suit.Club);
					var player1 = new StubReadyPlayer (cardToPlay);

					var cardInPile = new Stack<Card>(new []{new Card (CardValue.Four, Suit.Club)});
					var game = new Game (new[]{ player1 }, NonShufflingDeck (), GameState.GameStarted, cardInPile);
					var outcome = game.PlayCards (player1, cardToPlay);

					outcome.Should ().Be (ResultOutcome.Success);
					player1.Cards.Count ().Should ().Be (0);
				}
			}

			[TestFixture]
			public class WithSevenAsLowerThanCard{
				Dictionary<CardValue, CardType> cardTypes;
				[SetUp]
				public void Setup(){
					cardTypes = new Dictionary<CardValue, CardType> ();
					cardTypes.Add (CardValue.Seven, CardType.LowerThan);
				}

				[Test]
				public void Playing_Card_Higher_In_Value_Isnt_Valid(){
					var cardToPlay = new Card (CardValue.Eight, Suit.Club);
					var player1 = new StubReadyPlayer (cardToPlay);

					var cardInPile = new Stack<Card>(new []{new Card (CardValue.Seven, Suit.Club)});
					var game = new Game (new[]{ player1 }, NonShufflingDeck(), GameState.GameStarted, cardInPile, cardTypes);
					var outcome = game.PlayCards (player1, cardToPlay);

					outcome.Should ().Be (ResultOutcome.Fail);
					player1.Cards.Count ().Should ().Be (1);
				}

				[Test]
				public void Playing_Card_Lower_In_Value_Is_Valid(){
					var cardToPlay = new Card (CardValue.Six, Suit.Club);
					var player1 = new StubReadyPlayer (cardToPlay);

					var cardInPile = new Stack<Card> (new []{new Card (CardValue.Seven, Suit.Club) });
					var game = new Game (new[]{ player1 }, NonShufflingDeck(), GameState.GameStarted, cardInPile, cardTypes);
					var outcome = game.PlayCards (player1, cardToPlay);

					outcome.Should ().Be (ResultOutcome.Success);
					player1.Cards.Count ().Should ().Be (0);
				}

				[Test]
				public void Playing_Card_Of_Same_Value_Is_Valid(){
					var cardToPlay = new Card (CardValue.Seven, Suit.Club);
					var player1 = new StubReadyPlayer (cardToPlay);

					var cardInPile = new Stack<Card> ( new[]{ new Card(CardValue.Seven, Suit.Club)});
					var game = new Game (new []{ player1 }, NonShufflingDeck (), GameState.GameStarted, cardInPile, cardTypes);
					var outcome = game.PlayCards (player1, cardToPlay);

					outcome.Should ().Be (ResultOutcome.Success);
					player1.Cards.Count ().Should ().Be (0);
				}
			}
		}

		public static Deck NonShufflingDeck(){
			return new Deck (new NonShuffler ());
		}
	}
}

