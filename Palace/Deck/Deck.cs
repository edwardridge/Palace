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
			//Pack pack = new Pack ();
			cards = new List<Card> ();

			cards = pack.Cards;

			cards = shuffler.ShuffleCards (cards);
		}

		private ICollection<Card> SetupSuiteOfCards(Suite suite){
			List<Card> suiteOfCards = new List<Card> ();

			for(int i = 2; i <= 10; i++){
				suiteOfCards.Add(new Card(i, CardType.Number, suite));
			}
			suiteOfCards.Add(new Card(11, CardType.Other, suite));
			suiteOfCards.Add(new Card(12, CardType.Other, suite));
			suiteOfCards.Add(new Card(13, CardType.Other, suite));
			suiteOfCards.Add(new Card(14, CardType.Ace, suite));

			return suiteOfCards;
		}

		//Aside from setup we can assume we want in hand cards
		public ICollection<Card> GetCards (int count){
			return this.GetCards (count, CardOrientation.InHand);
		}

		public ICollection<Card> GetCards (int count, CardOrientation cardOrientation)
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

		public int CardsOfType (CardType type)
		{
			return cards.Where (card => card.Type == type).Count ();
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

