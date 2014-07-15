using System;
using Palace;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public class StubShuffler : IShuffler
	{
		int[] cardOrder;

		public StubShuffler () {
			this.cardOrder = GetSequentialOrder ();
		}

		public StubShuffler (int[] cardOrder)
		{
			var NewThing = cardOrder.ToList ();

			for (int i = 0; i < (52 - cardOrder.Count()); i++) {
				NewThing.Add (i + cardOrder.Count());
			}

			this.cardOrder = NewThing.ToArray ();
		}

		public ICollection<Card> ShuffleCards (ICollection<Card> preShuffledDeck)
		{
			List<Card> cards = new List<Card> ();
			var newOrder = cardOrder.Where (i=>i < preShuffledDeck.Count);

			foreach(int order in newOrder){
				cards.Add(preShuffledDeck.ToArray()[order]);
			}

			return cards;
		}

		public int[] GetSequentialOrder ()
		{
			List<int> order = new List<int> ();
			for (int i = 0; i < 52; i++) {
				order.Add (i);
			}

			return order.ToArray ();
		}
	}
}

