namespace SpecTests.Helpers
{
    using System.Collections.Generic;

    using Palace;

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

        public static Player CreatePlayer(IEnumerable<Card> cardsInHand, string name, IEnumerable<Card> cardsFaceDown = null)
        {
            var cardsFaceDownToAdd = cardsFaceDown ?? new List<Card>();
            var player = new Player(name, cardsInHand, cardsFaceDownToAdd);

            // player.AddCardsToInHandPile(cards);
            return player;
        }
    }
}