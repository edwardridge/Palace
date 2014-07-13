using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Deck
	{
		private ICollection<Card> cards;
		private IShuffler shuffler;

		public Deck(IShuffler randomiser) : this(randomiser, 52){
		}

		public Deck(IShuffler shuffler, int numCards)
	    {
			this.shuffler = shuffler;

			cards = new List<Card>();
			for (int i = 0; i < numCards; i++) {
				cards.Add (new Card (i, CardOrientation.FaceUp));
			}

			cards = shuffler.ShuffleCards (cards);
		}

		//Aside from setup we can assume we want face up cards
		public IEnumerable<Card> GetCards (int count){
			return this.GetCards (count, CardOrientation.FaceUp);
		}

		public IEnumerable<Card> GetCards (int count, CardOrientation faceOrientation)
		{
			var returnCards = new List<Card>() ;

			for(int i = 0 ;(i < count) && (cards.Count > 0); i++){
				var card = GetCard (faceOrientation);
				returnCards.Add(card);
				RemoveCard (card);
			}
		
			return returnCards;
		}

		private Card GetCard(CardOrientation faceUp){
			var cardToReturn = cards.First();
			cardToReturn.CardOrientation = faceUp;

			return cardToReturn;
		}

		private void RemoveCard(Card card){
			cards.Remove (card);
		}

		public int GetCount ()
		{
			return cards.Count;
		}
	}

}

