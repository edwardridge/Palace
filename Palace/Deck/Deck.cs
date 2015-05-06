using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public abstract class Deck : ICardDealer
	{
		protected ICollection<Card> cards;

		internal Deck(IEnumerable<Card> cards)
		{
		    this.cards = new List<Card>(cards);
		}

        public IEnumerable<Card> DealCards(int count)
		{
			var returnCards = new List<Card>() ;

			for(int i = 0 ;(i < count) && (cards.Count > 0); i++){
				var card = GetNextCard ();
				returnCards.Add(card);
				RemoveCard (card);
			}
		
			return returnCards;
		}

		public int CardsOfSuite (Suit suit)
		{
			return cards.Count (card => card.Suit == suit);
		}

		public int CardsOfType (CardValue type)
		{
			return cards.Count (card => card.Value == type);
		}
			
		public int CardCount ()
		{
			return cards.Count;
		}

		private Card GetNextCard(){
			var cardToReturn = cards.First();
			//cardToReturn.CardOrientation = cardOrientation;

			return cardToReturn;
		}

		private void RemoveCard(Card card){
			cards.Remove (card);
		}
	}

}

