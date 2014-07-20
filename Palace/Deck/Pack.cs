using System.Collections.Generic;
using System.Linq;
using System;

namespace Palace
{
	public class Pack
	{
		public List<Card> Cards { get; private set; }

		public Pack() : this(52){

		}

		public Pack(int numCards){
			var fullPack = new List<Card> ();
			foreach (Suit suite in(Suit[]) Enum.GetValues(typeof(Suit))) {
				fullPack.AddRange(SetupSuiteOfCards(suite));
			}

			Cards = new List<Card> ();

			for (int i = 0; i < numCards; i++) {
				Cards.Add (fullPack [i]);
			}
		}

		private ICollection<Card> SetupSuiteOfCards(Suit suit){
			List<Card> suiteOfCards = new List<Card> ();

			for(int i = 2; i <= 10; i++){
				suiteOfCards.Add(new Card(i, CardType.Number, suit));
			}
			suiteOfCards.Add(new Card(11, CardType.Other, suit));
			suiteOfCards.Add(new Card(12, CardType.Other, suit));
			suiteOfCards.Add(new Card(13, CardType.Other, suit));
			suiteOfCards.Add(new Card(14, CardType.Ace, suit));

			return suiteOfCards;
		}
	}


}

