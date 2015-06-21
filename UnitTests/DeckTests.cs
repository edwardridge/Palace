using Palace;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace UnitTests
{
    using TestHelpers;

    [TestFixture]
    public class DeckTests
    {

		[TestFixture]
		public class StandardPack{
			Deck deck;

			[SetUp]
			public void Setup(){
				deck = new StandardDeck();
			}

			[Test]
			public void Has_13_Hearts(){
				var heartCount = deck.CardsOfSuite (Suit.Heart);

				heartCount.Should ().Be (13);
			}

			[Test]
			public void Has_13_Clubs(){
				var clubCount = deck.CardsOfSuite (Suit.Club);

				clubCount.Should ().Be (13);
			}

			[Test]
			public void Has_13_Spades(){
				var spadeCount = deck.CardsOfSuite (Suit.Spade);

				spadeCount.Should ().Be (13);
			}

			[Test]
			public void Has_13_Diamonds(){
				var diamondCount = deck.CardsOfSuite (Suit.Diamond);

				diamondCount.Should ().Be (13);
			}

			[Test]
			public void Has_4_Aces(){
				var aceCount = deck.CardsOfType (CardValue.Ace);

				aceCount.Should ().Be (4);
			}

			[Test]
			public void Has_4_Jacks(){
				var jackCount = deck.CardsOfType (CardValue.Jack);

				jackCount.Should ().Be (4);
			}

			[Test]
			public void Has_4_Queens(){
				var queenCount = deck.CardsOfType (CardValue.Queen);

				queenCount.Should ().Be (4);
			}

			[Test]
			public void Has_4_Kings(){
				var kingCount = deck.CardsOfType (CardValue.King);

				kingCount.Should ().Be (4);
			}

			[Test]
			public void Has_4_NumberTwo_Cards(){
				int numberTwoCards = deck.CardsOfType (CardValue.Two);

				numberTwoCards.Should().Be(4);
			}
		}

		[TestFixture]
		public class GetCardsWithPackOf52{
			Deck deck;
			int preDeckCount;

			[SetUp]
			public void Setup(){
				var order = CardHelpers.ConvertIntegersToCardsWithSuitClub(new List<int>(){ 7,2,10,3,5,2,5,6,4,8,12,51});
				//var predeterminedShuffler = new PredeterminedShuffler (order);
				//Pack pack = new Pack ();
                deck = new PredeterminedDeck(order);
				preDeckCount = deck.CardCount ();
			}

			[Test]
			public void Gets_Single_Card(){
				var cardFromDeckValue = deck.DealCards (1).Select (s => s.Value).First();

				cardFromDeckValue.Should ().Be (CardValue.Seven);
			}

			[Test]
			public void Gets_Single_Card_Card_Is_Removed_From_Deck(){
				deck.DealCards (1);
				var postDeckCount = deck.CardCount ();

				postDeckCount.Should ().Be (preDeckCount - 1);
			}

			[Test]
			public void Gets_Two_Cards(){
				var expectedCardValues = new List<CardValue> (new []{CardValue.Seven,CardValue.Two});

				var cardsFromDeckValues = deck.DealCards(2).Select(s=>s.Value);

				cardsFromDeckValues.ShouldAllBeEquivalentTo (expectedCardValues);
			}

			[Test]
			public void Gets_Two_Cards_Two_Cards_Removed_From_Deck(){
				deck.DealCards (2);
				var postDeckCount = deck.CardCount ();

				postDeckCount.Should ().Be (preDeckCount - 2);
			}
		}

		[TestFixture]
		public class GetCardsWithPackOf2
		{
			Deck deck;
			[SetUp]
			public void Setup(){
                //Deck with two random cards
				deck = new PredeterminedDeck(new []{Card.AceOfClubs, Card.AceOfClubs});
			}

			[Test]
			public void Gets_Three_Cards_Returns_Two_Cards(){
				var cards = deck.DealCards (3);

				cards.Count ().Should ().Be (2);
			}
		}
    }
}

