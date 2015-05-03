using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Helpers
{
    using Palace;

    public static class DealerHelper
    {
        public static Dealer TestDealer()
        {
            return new Dealer(new StandardDeck(), new DummyCanStartGame());
        }

        public static Dealer TestDealerWithRules(Dictionary<CardValue, RuleForCard> rulesForCardByValue)
        {
            return new Dealer(new StandardDeck(), new DummyCanStartGame(), rulesForCardByValue);
        }
    }
}
