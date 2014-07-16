using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Deck
	{
		private ICollection<Card> cards;

		public Deck(IShuffler shuffler) : this(shuffler, 52){
		}

		public Deck(IShuffler shuffler, int numCards)
	    {
			cards = new List<Card>();
			for (int i = 0; i < numCards; i++) {
				if(i<13)
					cards.Add (new Card (i, Suite.Heart));
				else if(i<26)
					cards.Add (new Card (i, Suite.Club));
				else
					cards.Add (new Card (i, Suite.Other));
			}

			cards = shuffler.ShuffleCards (cards);
		}

		//Aside from setup we can assume we want in hand cards
		public IEnumerable<Card> GetCards (int count){
			return this.GetCards (count, CardOrientation.InHand);
		}

		public IEnumerable<Card> GetCards (int count, CardOrientation cardOrientation)
		{
			var returnCards = new List<Card>() ;

			for(int i = 0 ;(i < count) && (cards.Count > 0); i++){
				var card = GetCard (cardOrientation);
				returnCards.Add(card);
				RemoveCard (card);
			}
		
			return returnCards;
		}

		public int CardsOfSuite (Suite suite)
		{
			return cards.Where (card => card.Suite == suite).Count ();
		}
			
		public int GetCount ()
		{
			return cards.Count;
		}

		private Card GetCard(CardOrientation cardOrientation){
			var cardToReturn = cards.First();
			cardToReturn.CardOrientation = cardOrientation;

			return cardToReturn;
		}

		private void RemoveCard(Card card){
			cards.Remove (card);
		}
	}

}

