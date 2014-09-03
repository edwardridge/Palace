using System;
using System.Collections.Generic;
using Palace;
using System.Linq;

namespace UnitTests
{
	public static class CardHelpers
	{
		public static ICollection<Card> GetCardsFromValues(ICollection<int> values){
			return values.Select(s => new Card((CardType) s, Suit.Club)).ToList();
		}
	}
}

