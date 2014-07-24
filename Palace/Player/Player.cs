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
			get { return state; }
			set { state = value; }
		}

		public ICollection<Card> Cards {
			get { return cards; }
		}

		public Player(string name){
			Name = name;
			cards = new List<Card> ();
			state = PlayerState.Setup;
		}

		public void SetupPlayerForTest(ICollection<Card> cards){
			state = PlayerState.Ready;
			this.cards = cards;
		}

		public void AddCards (ICollection<Card> cardsToBeAdded)
		{
			foreach (Card addedCard in cardsToBeAdded) {
				this.cards.Add (addedCard);
			}
		}

		public Result PutCardFaceUp (Card cardToPutFaceUp)
		{
			if(this.NumCards(CardOrientation.FaceUp) >= 3)
				return new Result(ResultOutcome.Fail);

			cardToPutFaceUp.CardOrientation = CardOrientation.FaceUp;
			return new Result (ResultOutcome.Success);
		}

		public int NumCards(CardOrientation cardLocation){
			return cards.Where (card => card.CardOrientation == cardLocation).Count ();
		}

		public Result Ready ()
		{
			if (this.NumCards (CardOrientation.FaceUp) != 3)
				return new Result (ResultOutcome.Fail);

			this.State = PlayerState.Ready;

			return new Result (ResultOutcome.Success);
		}

		private ICollection<Card> cards;

		private PlayerState state;
	}

}

