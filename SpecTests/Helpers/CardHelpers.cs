namespace SpecTests.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Palace;

    public static class CardHelpers
    {
        public static ICollection<Card> ConvertIntegersToCardsWithSuitClub(ICollection<int> values)
        {
            return values.Select(s => new Card((CardValue)s, Suit.Club)).ToList();
        }
    }
}