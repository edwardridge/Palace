using System.Collections.Generic;

namespace Palace
{
    interface IRulesValidator
    {
        bool CardsPassRules(IEnumerable<Card> cardsToPlay, Card lastCardPlayed);
    }
}
