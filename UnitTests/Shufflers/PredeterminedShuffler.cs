using System;
using Palace;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public class PredeterminedShuffler : IShuffler
	{
		Card[] predeterminedCards;

		public PredeterminedShuffler (ICollection<Card> cardOrder)
		{
			this.predeterminedCards = cardOrder.ToArray();
		}

		public ICollection<Card> ShuffleCards (ICollection<Card> preShuffledDeck)
		{
			var cards = new List<Card> (predeterminedCards);

			for (int i = predeterminedCards.Count(); i < preShuffledDeck.Count(); i++) {
				cards.Add(new Card((CardType) i, Suit.Club));
			}

			return cards;
		}
	}
}

