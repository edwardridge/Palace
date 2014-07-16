using System;
using System.Collections.Generic;

namespace Palace
{
	public interface IShuffler
	{
		ICollection<Card> ShuffleCards(ICollection<Card> preShuffledDeck);
	}
}

