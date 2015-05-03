using System.Collections.Generic;

namespace Palace
{
    interface IRulesProcessesor
    {
        bool ProcessRulesForGame(IReverseOrderOfPlay orderOfPlay, IEnumerable<Card> cardsToPlay, Card lastCardPlayed);
    }
}
