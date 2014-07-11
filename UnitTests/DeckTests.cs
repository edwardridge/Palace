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
                var expectedCard = new Card(1);

                var cardFromDeck = deck.GetNextCard();

                Assert.AreEqual(cardFromDeck.Value, expectedCard.Value);
            }
        }

    }
}

