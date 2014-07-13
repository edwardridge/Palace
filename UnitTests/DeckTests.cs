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
		public class GetCardsWithPackOf52{
			Deck deck;
			int preDeckCount;

			[SetUp]
			public void Setup(){
				StubShuffler randomiser = new StubShuffler ();
				deck = new Deck (randomiser, 52);
				preDeckCount = deck.GetCount ();
			}

			[Test]
			public void Gets_Single_Card(){
				var expectedCardValue = 0;

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
				var expectedCardValues = new List<int> (new []{0,1});

				var cardsFromDeckValues = deck.GetCards(2).Select(s=>s.Value).OrderBy(o=>o).ToList();

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

