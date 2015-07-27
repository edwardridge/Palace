namespace TestHelpers
{
    using System.Collections.Generic;

    using Palace;
    using System;

    public static class PlayerHelper
    {

        public static Player CreatePlayer(string name)
        {
            return CreatePlayer(new List<Card>(), name);
        }

        public static Player CreatePlayer(Card card, string name)
        {
            return CreatePlayer(new List<Card>() { card }, name);
        }

        public static Player CreatePlayer(IEnumerable<Card> cardsInHand, string name,  IEnumerable<Card> cardsFaceUp = null, IEnumerable<Card> cardsFaceDown = null)
        {
            var cardsFaceDownToAdd = cardsFaceDown ?? new List<Card>();
            var cardsFaceUpToAdd = cardsFaceUp ?? new List<Card>();
            var player = new Player(name, cardsInHand, cardsFaceUpToAdd, cardsFaceDownToAdd);

            // player.AddCardsToInHandPile(cards);
            return player;
        }
    }
}