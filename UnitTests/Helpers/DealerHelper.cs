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
            return new Dealer(new Deck(new NonShuffler()), new DummyGameStartValidator());
        }
    }
}
