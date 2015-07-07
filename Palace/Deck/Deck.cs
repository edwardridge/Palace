using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public abstract class Deck
	{
	    private ICollection<Card> cards;

		protected Deck(IEnumerable<Card> cards)
		{
		    this.cards = new List<Card>(cards);
		}

        public IEnumerable<Card> DealCards(int count)
		{
			var returnCards = this.Cards.Take(count).ToList();
            foreach (var card in returnCards)
            {
                this.cards.Remove(card);
            }
		
			return returnCards;
		}

	    public bool CardsRemaining
	    {
	        get
	        {
	            return this.cards.Any();
	        }
	    }

	    internal ICollection<Card> Cards
	    {
	        get
	        {
	            return cards;
	        }
	        set
	        {
	            cards = value;
	        }
	    }

	    public int CardsOfSuite (Suit suit)
		{
			return this.cards.Count (card => card.Suit == suit);
		}

		public int CardsOfType (CardValue type)
		{
			return this.cards.Count (card => card.Value == type);
		}
			
		public int CardCount ()
		{
			return this.cards.Count;
		}
	}

}

