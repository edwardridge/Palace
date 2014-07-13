using System;
using Palace;
using System.Collections.Generic;

namespace UnitTests
{
	public class StubRandomiser : IShuffler
	{
		public StubRandomiser ()
		{
		}

		public ICollection<Card> ShuffleCards (ICollection<Card> preShuffledDeck)
		{
			List<Card> cards = new List<Card> ();
			for (int i = 0; i < preShuffledDeck.Count; i++) {
				cards.Add (new Card (i));
			}
			return cards;
		}
	}
}

