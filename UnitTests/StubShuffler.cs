using System;
using Palace;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public class StubShuffler : IShuffler
	{
		Card[] cardOrder;

		public StubShuffler () {
			this.cardOrder = GetSequentialOrder ();
		}

		public StubShuffler (Card[] cardOrder)
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
			var newOrder = cardOrder.Where (i=>i.Value < preShuffledDeck.Count);

			foreach(Card order in newOrder){
				cards.Add(preShuffledDeck.ToArray()[order.Value]);
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

