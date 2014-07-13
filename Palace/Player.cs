using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Player
	{
		public string Name {
			get;
			private set;
		}

		public int NumFaceUpCards {
			get {return cards.Count; }
		}

		public Player(string name){
			Name = name;
			cards = new List<Card> ();
		}

		public void AddCards (IEnumerable<Card> cardsToBeAdded)
		{
			foreach (Card addedCard in cardsToBeAdded) {
				this.cards.Add (addedCard);
			}
		}

		private ICollection<Card> cards;
	}

}

