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

		public State State {
			get;
			private set;
		}

		public int NumFaceUpCards {
			get {return cards.Where(card=>card.FaceOrientation == FaceOrientation.FaceUp).Count(); }
		}

		public int NumFaceDownCards {
			get { return cards.Where(card=>card.FaceOrientation == FaceOrientation.FaceDown).Count(); }
		}

		public Player(string name){
			Name = name;
			cards = new List<Card> ();
			State = State.Setup;
		}

		public void AddCards (IEnumerable<Card> cardsToBeAdded)
		{
			foreach (Card addedCard in cardsToBeAdded) {
				this.cards.Add (addedCard);
			}
		}

		public void SetState (State state)
		{
			this.State = state;
		}

		private ICollection<Card> cards;
	}

}

