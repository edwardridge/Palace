using System;
using Palace;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public class StubReadyPlayer : IPlayer
	{
		public StubReadyPlayer ()
		{
			_cards = new List<Card> ();
			this._state = PlayerState.Ready;
		}

		#region IPlayer implementation

		public void AddCards (ICollection<Card> cards)
		{
			foreach (Card card in cards) {
				_cards.Add (card);
			}
		}

		public void RemoveCards (ICollection<Card> cards)
		{
			foreach (Card card in cards) {
				_cards.Remove (card);
			}
		}

		public ICollection<Card> Cards {
			get {
				return this._cards;
			}
		}

		public PlayerState State {
			get { return _state;}
			set { _state = value;}
		}

		public string Name {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion

		private ICollection<Card> _cards;
		private PlayerState _state;
	}
}

