using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Pack
	{
		public List<Card> Cards { get; private set; }

		public Pack() : this(52){

		}

		public Pack(int numCards){
			var fullPack = new List<Card> ();
			fullPack.AddRange(SetupSuiteOfCards(Suite.Club));
			fullPack.AddRange(SetupSuiteOfCards(Suite.Diamond));
			fullPack.AddRange(SetupSuiteOfCards(Suite.Heart));
			fullPack.AddRange(SetupSuiteOfCards(Suite.Spade));

			Cards = new List<Card> ();

			for (int i = 0; i < numCards; i++) {
				Cards.Add (fullPack [i]);
			}
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
	}


}

