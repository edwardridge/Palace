using System;
using System.Collections.Generic;

namespace Palace
{
    using System.Security.Cryptography.X509Certificates;

    public interface IPlayer
	{
		ICollection<Card> Cards { get; }

		PlayerState State { get; }

		string Name { get; }

		void AddCards(ICollection<Card> cards);

		void RemoveCards(ICollection<Card> cards);

		Card LowestCardInValue { get; }
	}
}

