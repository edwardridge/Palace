using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Player : IPlayer
	{
		public string Name {
			get{ return name; }
		}

		public PlayerState State {
			get {return state;}
		}

		public ICollection<Card> Cards {
			get { return cards; }
		}

		public Player(string name){
			this.name = name;
			cards = new List<Card> ();
			state = PlayerState.Setup;
		}

		public void AddCards (ICollection<Card> cardsToBeAdded)
		{
			foreach (Card addedCard in cardsToBeAdded) {
				this.cards.Add (addedCard);
			}
		}

		public void RemoveCards(ICollection<Card> cardsToBeRemoved){
			foreach (Card removedCard in cardsToBeRemoved) {
				this.cards.Remove (removedCard);
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

			state = PlayerState.Ready;

			return new Result (ResultOutcome.Success);
		}

		private string name;

		private ICollection<Card> cards;

		private PlayerState state;
	}

}

