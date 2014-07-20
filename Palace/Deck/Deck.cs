using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Deck
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
		public ICollection<Card> GetCards (int count){
			return this.GetCards (count, CardOrientation.InHand);
		}

		public ICollection<Card> GetCards (int count, CardOrientation cardOrientation)
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
			return cards.Where (card => card.Suit == suit).Count ();
		}

		public int CardsOfType (CardType type)
		{
			return cards.Where (card => card.Type == type).Count ();
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

