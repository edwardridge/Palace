using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palace
{
    public class PredeterminedDeck : Deck
    {
        public PredeterminedDeck(IEnumerable<Card> cards) : base(cards) { }

        public void PadCardsToCount(int count)
        {
            for (int i = cards.Count; i < count; i++)
            {
                cards.Add(Card.ThreeOfClubs);
            }
        }
    }
}
