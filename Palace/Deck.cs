using System.Collections.Generic;

namespace Palace
{
	public class Deck
	{
		private IList<Card> cards;
		private IRandomiser randomiser;

		public Deck(IRandomiser randomiser)
	    {
			this.randomiser = randomiser;
			cards = new List<Card>();
			for (int i = 0; i < 52; i++) {
				cards.Add (new Card (i));
			}
	    }

		public IEnumerable<Card> GetCards (int count)
		{
			var returnCards = new List<Card>() ;

			for(int i = 0 ;i < count; i++){
				returnCards.Add(GetCard());
			}
		
			return returnCards;
		}

		private Card GetCard(){
			var index = randomiser.GetRandom (cards.Count);
			var cardToReturn = cards [index];
			cards.Remove (cardToReturn);

			return cardToReturn;
		}

		public int GetCount ()
		{
			return cards.Count;
		}
	}

}

