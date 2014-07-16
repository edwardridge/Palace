using System;
using Palace;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public class PredeterminedShuffler : IShuffler
	{
		Card[] cardOrder;

		public PredeterminedShuffler () {
			this.cardOrder = GetSequentialOrder ();
		}

		public PredeterminedShuffler (IEnumerable<Card> cardOrder)
		{
			var NewThing = cardOrder.ToList ();

			for (int i = 0; i < (52 - cardOrder.Count()); i++) {
				NewThing.Add (new Card(i + cardOrder.Count(), Suite.Club));
			}

			this.cardOrder = NewThing.ToArray ();
		}

		public ICollection<Card> ShuffleCards (ICollection<Card> preShuffledDeck)
		{
			List<Card> cards = new List<Card> ();
			for (int i = 0; i < preShuffledDeck.Count; i++) {
				if (i < this.cardOrder.Count())
					cards.Add (this.cardOrder [i]);
				else
					cards.Add(new Card(i, Suite.Club));
			}

			return cards;
		}

		public Card[] GetSequentialOrder ()
		{
			var order = new List<Card> ();
			for (int i = 0; i < 52; i++) {
				order.Add (new Card(i, Suite.Club));
			}

			return order.ToArray ();
		}
	}
}

