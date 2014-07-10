using NUnit.Framework;
using Palace;
using System;

namespace UnitTests
{
	[TestFixture]
	public class DeckTests
	{
		[TestFixture]
		public class GetCard
		{
			[Test]
			public void Gets_Card ()
			{
				var deck = new Deck();
				var expectedCard = new Card();

				var cardFromDeck = deck.GetNextCard();

				Assert.AreEqual (cardFromDeck, expectedCard);
			}
		}

	}
}

