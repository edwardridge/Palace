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
			var suiteOfCards = new List<Card> ();

			foreach (CardValue value in (CardValue[]) Enum.GetValues(typeof(CardValue)))
				suiteOfCards.Add(new Card(value, suit));

			return suiteOfCards;
		}
	}


}

