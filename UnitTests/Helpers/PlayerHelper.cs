using Palace;
using System.Collections.Generic;

namespace UnitTests
{
    public static class PlayerHelper
    {
        public static Player CreatePlayer()
        {
            return CreatePlayer(new List<Card>(), "Test");
        }

        public static Player CreatePlayer(string name)
        {
            return CreatePlayer(new List<Card>(), name);
        }

        public static Player CreatePlayer(Card card)
        {
            return CreatePlayer(new List<Card>() { card }, "Test");
        }

        public static Player CreatePlayer(Card card, string name)
        {
            return CreatePlayer(new List<Card>() { card }, name);
        }

        public static Player CreatePlayer(IEnumerable<Card> cards)
        {
            return CreatePlayer(cards, "Test");
        }

        public static Player CreatePlayer(IEnumerable<Card> cards, string name)
        {
            var player = new Player(name, cards);
            //player.AddCardsToInHandPile(cards);
            return player;
        }
    }
}

