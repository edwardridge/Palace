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
				suiteOfCards.Add(new Card((CardType) i, suit));
			}

			suiteOfCards.Add(new Card(CardType.Jack, suit));
			suiteOfCards.Add(new Card(CardType.Queen, suit));
			suiteOfCards.Add(new Card(CardType.King, suit));
			suiteOfCards.Add(new Card(CardType.Ace, suit));

			return suiteOfCards;
		}
	}


}

