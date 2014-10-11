using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Player : IPlayer
	{
		public string Name {
			get{ return _name; }
		}

		public PlayerState State {
			get {return _state;}
		}

		public ICollection<Card> Cards {
			get { return _cards; }
		}

		public Player(string name){
			this._name = name;
			_cards = new List<Card> ();
			_state = PlayerState.Setup;
		}

		public void AddCards (ICollection<Card> cardsToBeAdded)
		{
			foreach (Card addedCard in cardsToBeAdded) {
				this._cards.Add (addedCard);
			}
		}

		public void RemoveCards(ICollection<Card> cardsToBeRemoved){
			foreach (Card removedCard in cardsToBeRemoved) {
				this._cards.Remove (removedCard);
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
			return _cards.Where (card => card.CardOrientation == cardLocation).Count ();
		}

		public Result Ready ()
		{
			if (this.NumCards (CardOrientation.FaceUp) != 3)
				return new Result (ResultOutcome.Fail);

			_state = PlayerState.Ready;

			return new Result (ResultOutcome.Success);
		}

		private string _name;

		private ICollection<Card> _cards;

		private PlayerState _state;
	}

}

