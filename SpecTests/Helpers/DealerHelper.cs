namespace SpecTests.Helpers
{
    using System.Collections.Generic;

    using Palace;

    public static class DealerHelper
    {
        public static Dealer TestDealer(IEnumerable<Player> players)
        {
            return new Dealer(players, new StandardDeck(), new DummyCanStartGame());
        }

        public static Dealer TestDealerWithRules(IEnumerable<Player> players, Dictionary<CardValue, RuleForCard> rulesForCardByValue)
        {
            return new Dealer(players, new StandardDeck(), new DummyCanStartGame(), rulesForCardByValue);
        }
    }

    public class DummyCanStartGame : ICanStartGame
    {
        public bool IsReady(ICollection<Player> players)
        {
            return true;
        }
    }
}