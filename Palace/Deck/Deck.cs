using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Deck : ICardDealer
	{
		private ICollection<Card> cards;

		public Deck(IShuffler shuffler) : this(shuffler, new Pack()){
		}

		public Deck(IShuffler shuffler, Pack pack)
	    {
			cards = pack.Cards;

			cards = shuffler.ShuffleCards (cards);
		}

		//Aside from setup we can assume we want in hand cards
		public IEnumerable<Card> DealCards (int count){
			return this.DealCards (count, CardOrientation.InHand);
		}

        public IEnumerable<Card> DealCards(int count, CardOrientation cardOrientation)
		{
			var returnCards = new List<Card>() ;

			for(int i = 0 ;(i < count) && (cards.Count > 0); i++){
				var card = GetNextCard (cardOrientation);
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

		private Card GetNextCard(CardOrientation cardOrientation){
			var cardToReturn = cards.First();
			cardToReturn.CardOrientation = cardOrientation;

			return cardToReturn;
		}

		private void RemoveCard(Card card){
			cards.Remove (card);
		}
	}

}

