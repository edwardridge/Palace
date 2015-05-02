using System;
using System.Collections.Generic;

namespace Palace
{
    public interface IPlayer
	{
		ICollection<Card> Cards { get; }

		PlayerState State { get; }

		string Name { get; }

		void AddCards(IEnumerable<Card> cards);

		void RemoveCards(ICollection<Card> cards);

		Card LowestCardInValue { get; }
	}
}

