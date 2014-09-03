using System;
using Palace;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public class StubPlayer : IPlayer
	{
		public StubPlayer ()
		{
			_cards = new List<Card> ();
		}

		#region IPlayer implementation

		public void AddCards (ICollection<Card> cards)
		{
			foreach (Card card in cards) {
				_cards.Add (card);
			}
			//this._cards.ToList ().AddRange (cards);
		}

		public void RemoveCards (ICollection<Card> cards)
		{
			throw new NotImplementedException ();
		}

		public ICollection<Card> Cards {
			get {
				return this._cards;
			}
		}

		public PlayerState State {
			get {
				throw new NotImplementedException ();
			}
		}

		public string Name {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion

		private ICollection<Card> _cards;
	}
}

