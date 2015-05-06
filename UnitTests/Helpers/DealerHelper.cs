using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Helpers
{
    using Palace;

    public static class DealerHelper
    {
        public static Dealer TestDealer(IEnumerable<Player> players)
        {
            return new Dealer(players, new StandardDeck(), new DummyCanStartGame());
        }

        public static Dealer TestDealerWithRules(IEnumerable<Player> players,Dictionary<CardValue, RuleForCard> rulesForCardByValue)
        {
            return new Dealer(players, new StandardDeck(), new DummyCanStartGame(), rulesForCardByValue);
        }
    }
}
