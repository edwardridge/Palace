using System.Collections.Generic;

namespace Palace
{
    interface ICanLayCards
    {
        bool CardsPassRules(IEnumerable<Card> cardsToPlay, Card lastCardPlayed);
    }
}
