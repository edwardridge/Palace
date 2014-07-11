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
        public class GetCard
        {
            [Test]
            public void Gets_Card()
            {
                var deck = new Deck();
                var expectedCard = new Card(0);

                var cardFromDeck = deck.GetNextCard();

                Assert.AreEqual(cardFromDeck.Value, expectedCard.Value);
            }

			[Test]
			public void Card_Is_Removed_From_Deck()
			{
				var deck = new Deck ();
				var preDeckCount = deck.GetCount();

				deck.GetNextCard ();
				var postDeckCount = deck.GetCount();

				Assert.AreEqual (preDeckCount - 1, postDeckCount);
			}
        }

		[TestFixture]
		public class GetCards{

			[Test]
			public void Gets_Single_Card(){
				var deck = new Deck ();
				var expectedCardValue = 0;

				var cardFromDeckValue = deck.GetCards (1).Select (s => s.Value).First();

				Assert.AreEqual (expectedCardValue, cardFromDeckValue);
			}

			[Test]
			public void Gets_Two_Cards(){
				var deck = new Deck ();
				var expectedCardValues = new List<int> (new []{0,1});

				var cardsFromDeckValues = deck.GetCards(2).Select(s=>s.Value).OrderBy(o=>o).ToList();

				Assert.AreEqual (expectedCardValues, cardsFromDeckValues);
			}
		}

    }
}

