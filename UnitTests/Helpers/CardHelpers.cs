using System;
using System.Collections.Generic;
using Palace;
using System.Linq;

namespace UnitTests
{
	public static class CardHelpers
	{
		public static ICollection<Card> ConvertIntegersToCardsWithSuitClub(ICollection<int> values){
			return values.Select(s => new Card((CardValue) s, Suit.Club)).ToList();
		}
	}
}

