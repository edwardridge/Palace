using Palace;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class DeckTests
    {
		public static Deck SetupDeckWithStandardPack(){
			var order = new []{ new Card(7, Suite.Club),new Card(1, Suite.Club),new Card(10, Suite.Club),new Card(4, Suite.Club),new Card(5, Suite.Club),new Card(6, Suite.Club),new Card(7, Suite.Club),new Card(8, Suite.Club),new Card(9, Suite.Club)};
			var randomiser = new StubShuffler (order);
			return new Deck (randomiser, 52);
		}

		[TestFixture]
		public class StandardPack{
			Deck deck;

			[SetUp]
			public void Setup(){
				deck = DeckTests.SetupDeckWithStandardPack();
			}

			[Test]
			public void Has_13_Hearts(){
				var heartCount = deck.CardsOfSuite (Suite.Heart);

				Assert.AreEqual (13, heartCount);
			}

			[Test]
			public void Has_13_Clubs(){
				var clubCount = deck.CardsOfSuite (Suite.Club);

				Assert.AreEqual (13, clubCount);
			}

			[Test]
			public void Has_13_Spades(){
				var clubCount = deck.CardsOfSuite (Suite.Spade);

				Assert.AreEqual (13, clubCount);
			}

			[Test]
			public void Has_13_Diamonds(){
				var clubCount = deck.CardsOfSuite (Suite.Diamond);

				Assert.AreEqual (13, clubCount);
			}
		}

		[TestFixture]
		public class GetCardsWithPackOf52{
			Deck deck;
			int preDeckCount;

			[SetUp]
			public void Setup(){
				deck = DeckTests.SetupDeckWithStandardPack();
				preDeckCount = deck.GetCount ();
			}

			[Test]
			public void Gets_Single_Card(){
				var expectedCardValue = 7;

				var cardFromDeckValue = deck.GetCards (1).Select (s => s.Value).First();

				Assert.AreEqual (expectedCardValue, cardFromDeckValue);
			}

			[Test]
			public void Gets_Single_Card_Card_Is_Removed_From_Deck(){
				deck.GetCards (1);
				var postDeckCount = deck.GetCount ();

				Assert.AreEqual (preDeckCount - 1, postDeckCount);
			}

			[Test]
			public void Gets_Two_Cards(){
				var expectedCardValues = new List<int> (new []{7,1});

				var cardsFromDeckValues = deck.GetCards(2).Select(s=>s.Value);

				Assert.AreEqual (expectedCardValues, cardsFromDeckValues);
			}

			[Test]
			public void Gets_Two_Cards_Two_Cards_Removed_From_Deck(){
				deck.GetCards (2);
				var postDeckCount = deck.GetCount ();

				Assert.AreEqual (preDeckCount - 2, postDeckCount);
			}
		}

		[TestFixture]
		public class GetCardsWithPackOf2
		{
			Deck deck;
			[SetUp]
			public void Setup(){
				IShuffler randomiser = new StubShuffler ();
				deck = new Deck (randomiser, 2);
			}

			[Test]
			public void Gets_Three_Cards_Returns_Two_Cards(){
				var cards = deck.GetCards (3);

				Assert.AreEqual (2, cards.Count());
			}
		}
    }
}

