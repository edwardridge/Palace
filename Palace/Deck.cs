using System.Collections.Generic;

namespace Palace
{
	public class Deck
	{
		private IList<Card> cards;

	    public Deck()
	    {
			cards = new List<Card>();
			for (int i = 0; i < 52; i++) {
				cards.Add (new Card (i));
			}
	    }

		public IEnumerable<Card> GetCards (int count)
		{
			var returnCards = new List<Card>() ;
			for(int i = 0 ;i < count; i++){
				returnCards.Add (cards [i]);
			}

			for(int i = 0 ;i < count; i++){
				cards.Remove (cards [i]);
			}
		
			return returnCards;
		}

		public int GetCount ()
		{
			return cards.Count;
		}
	}

}

