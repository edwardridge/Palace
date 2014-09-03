using Palace;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class DeckTests
    {

		[TestFixture]
		public class StandardPack{
			Deck deck;

			[SetUp]
			public void Setup(){
				var nonShuffler = new NonShuffler ();
				Pack pack = new Pack ();
				deck = new Deck (nonShuffler, pack);
			}

			[Test]
			public void Has_13_Hearts(){
				var heartCount = deck.CardsOfSuite (Suit.Heart);

				Assert.AreEqual (13, heartCount);
			}

			[Test]
			public void Has_13_Clubs(){
				var clubCount = deck.CardsOfSuite (Suit.Club);

				Assert.AreEqual (13, clubCount);
			}

			[Test]
			public void Has_13_Spades(){
				var spadeCount = deck.CardsOfSuite (Suit.Spade);

				Assert.AreEqual (13, spadeCount);
			}

			[Test]
			public void Has_13_Diamonds(){
				var diamondCount = deck.CardsOfSuite (Suit.Diamond);

				Assert.AreEqual (13, diamondCount);
			}

			[Test]
			public void Has_4_Aces(){
				var aceCount = deck.CardsOfType (CardValue.Ace);

				Assert.AreEqual (4, aceCount);
			}

			[Test]
			public void Has_4_Jacks(){
				var jackCount = deck.CardsOfType (CardValue.Jack);

				Assert.AreEqual (4, jackCount);
			}

			[Test]
			public void Has_4_Queens(){
				var queenCount = deck.CardsOfType (CardValue.Queen);

				Assert.AreEqual (4, queenCount);
			}

			[Test]
			public void Has_4_Kings(){
				var kingCount = deck.CardsOfType (CardValue.King);

				Assert.AreEqual (4, kingCount);
			}

			[Test]
			public void Has_4_NumberTwo_Cards(){
				int numberTwoCards = deck.CardsOfType (CardValue.Two);

				Assert.AreEqual (4, numberTwoCards);
			}
		}

		[TestFixture]
		public class GetCardsWithPackOf52{
			Deck deck;
			int preDeckCount;

			[SetUp]
			public void Setup(){
				var order = CardHelpers.GetCardsFromValues(new List<int>(){ 7,2,10,3,5,2,5,6,4,8,12,51});
				var predeterminedShuffler = new PredeterminedShuffler (order);
				Pack pack = new Pack ();
				deck = new Deck (predeterminedShuffler, pack);
				preDeckCount = deck.CardCount ();
			}

			[Test]
			public void Gets_Single_Card(){
				var expectedCardValue = CardValue.Seven;

				var cardFromDeckValue = deck.TakeCards (1).Select (s => s.Value).First();

				Assert.AreEqual (expectedCardValue, cardFromDeckValue);
			}

			[Test]
			public void Gets_Single_Card_Card_Is_Removed_From_Deck(){
				deck.TakeCards (1);
				var postDeckCount = deck.CardCount ();

				Assert.AreEqual (preDeckCount - 1, postDeckCount);
			}

			[Test]
			public void Gets_Two_Cards(){
				var expectedCardValues = new List<CardValue> (new []{CardValue.Seven,CardValue.Two});

				var cardsFromDeckValues = deck.TakeCards(2).Select(s=>s.Value);

				Assert.AreEqual (expectedCardValues, cardsFromDeckValues);
			}

			[Test]
			public void Gets_Two_Cards_Two_Cards_Removed_From_Deck(){
				deck.TakeCards (2);
				var postDeckCount = deck.CardCount ();

				Assert.AreEqual (preDeckCount - 2, postDeckCount);
			}
		}

		[TestFixture]
		public class GetCardsWithPackOf2
		{
			Deck deck;
			[SetUp]
			public void Setup(){
				IShuffler randomiser = new NonShuffler ();
				Pack pack = new Pack (2);
				deck = new Deck (randomiser, pack);
			}

			[Test]
			public void Gets_Three_Cards_Returns_Two_Cards(){
				var cards = deck.TakeCards (3);

				Assert.AreEqual (2, cards.Count());
			}
		}
    }
}

