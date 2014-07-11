using Palace;
using NUnit.Framework;

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

    }
}

