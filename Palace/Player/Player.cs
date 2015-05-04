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

		public void AddCards (IEnumerable<Card> cardsToBeAdded)
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

		public ResultOutcome PutCardFaceUp (Card cardToPutFaceUp)
		{
			if(this.NumCards(CardOrientation.FaceUp) >= 3)
				return ResultOutcome.Fail;

			cardToPutFaceUp.CardOrientation = CardOrientation.FaceUp;
			return ResultOutcome.Success;
		}

		public int NumCards(CardOrientation cardLocation){
			return _cards.Where (card => card.CardOrientation == cardLocation).Count ();
		}

		public void Ready ()
		{
			_state = PlayerState.Ready;

			//return ResultOutcome.Success;
		}

		public Card LowestCardInValue{
			get{ return _cards.ToList ().OrderBy (o => o.Value).FirstOrDefault (); }
		}

	    private string _name;

		private ICollection<Card> _cards;

		private PlayerState _state;
	}

}

