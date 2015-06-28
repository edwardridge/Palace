namespace Palace
{
    using System;
    using System.Collections.Generic;

    public class StandardDeck : Deck
    {
        public StandardDeck()
            : base(new Pack().Cards)
        {
            var shuffledCards = Shuffle(new List<Card>(this.cards));
            this.cards = new List<Card>(shuffledCards);
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
    }
}
