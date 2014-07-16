using System;
using Palace;
using System.Collections.Generic;

namespace UnitTests
{
	public class NonShuffler : IShuffler
	{
		public NonShuffler ()
		{
		}

		public ICollection<Card> ShuffleCards (ICollection<Card> preShuffledDeck)
		{
			return preShuffledDeck;
		}
	}
}

