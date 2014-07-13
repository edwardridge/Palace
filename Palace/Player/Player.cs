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

		public PlayerState State {
			get;
			private set;
		}

		public int NumCards(CardOrientation cardLocation){
			return cards.Where (card => card.CardOrientation == cardLocation).Count ();
		}

		public Player(string name){
			Name = name;
			cards = new List<Card> ();
			State = PlayerState.Setup;
		}

		public void AddCards (IEnumerable<Card> cardsToBeAdded)
		{
			foreach (Card addedCard in cardsToBeAdded) {
				this.cards.Add (addedCard);
			}
		}

		public void SetState (PlayerState state)
		{
			this.State = state;
		}

		public ICollection<Card> GetCards ()
		{
			return cards;
		}

		public Result PutCardFaceUp (Card cardToPutFaceUp)
		{
			if(this.NumCards(CardOrientation.FaceUp) >= 3)
				return new Result(ResultOutcome.Fail);

			cardToPutFaceUp.CardOrientation = CardOrientation.FaceUp;
			return new Result (ResultOutcome.Success);
		}

		private ICollection<Card> cards;
	}

}

