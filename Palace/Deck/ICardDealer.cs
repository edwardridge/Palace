using System.Collections.Generic;

namespace Palace
{
    interface ICardDealer
    {
        IEnumerable<Card> DealCards(int count);

        bool CardsRemaining { get; }
    }
}
