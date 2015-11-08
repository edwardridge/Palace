namespace Palace
{
    using System;
    using System.Collections.Generic;

    public class StandardDeck : Deck
    {
        private StandardDeck()
        {
            
        }

        private IList<Card> Shuffle(IList<Card> cardsToShuffle)
        {
            Random rng = new Random();
            int n = cardsToShuffle.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = cardsToShuffle[k];
                cardsToShuffle[k] = cardsToShuffle[n];
                cardsToShuffle[n] = value;
            }
            return cardsToShuffle;
        }

        public static StandardDeck CreateDeck()
        {
            StandardDeck standardDeck = new StandardDeck();
            standardDeck.Cards = new Pack().Cards;
            var shuffledCards = standardDeck.Shuffle(new List<Card>(standardDeck.Cards));
            standardDeck.Cards = new List<Card>(shuffledCards);

            return standardDeck;
        }
    }
}
