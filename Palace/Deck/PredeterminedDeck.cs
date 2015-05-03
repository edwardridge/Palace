using System.Collections.Generic;

namespace Palace
{
    public class PredeterminedDeck : Deck
    {
        public PredeterminedDeck(IEnumerable<Card> cards) : base(cards) { }
    }
}
