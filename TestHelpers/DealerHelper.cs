namespace TestHelpers
{
    using System.Collections.Generic;

    using Palace;

    public static class DealerHelper
    {
        public static Dealer TestDealer(IEnumerable<Player> players)
        {
            var dealer = new Dealer(StandardDeck.CreateDeck(), new DummyCanStartGame());
            foreach(var player in players)
                dealer.AddPlayer(player);
            
            return dealer;
        }

        public static Dealer TestDealerWithRules(IEnumerable<Player> players, Dictionary<CardValue, RuleForCard> rulesForCardByValue)
        {
            var dealer = new Dealer(StandardDeck.CreateDeck(), new DummyCanStartGame(), rulesForCardByValue);
            foreach (var player in players)
                dealer.AddPlayer(player);

            return dealer;
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