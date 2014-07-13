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
				cards.Add (new Card (i, false));
			}
	    }

		public IEnumerable<Card> GetCards (int count, bool faceUp)
		{
			var returnCards = new List<Card>() ;

			for(int i = 0 ;i < count; i++){
				returnCards.Add(GetCard(faceUp));
			}
		
			return returnCards;
		}

		private Card GetCard(bool faceUp){
			var index = randomiser.GetRandom (cards.Count);

			var cardToReturn = cards [index];
			cardToReturn.FaceUp = faceUp;

			cards.Remove (cardToReturn);

			return cardToReturn;
		}

		public int GetCount ()
		{
			return cards.Count;
		}
	}

}

