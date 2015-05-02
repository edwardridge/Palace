using System;
using Palace;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public class StubReadyPlayer : IPlayer
	{
		public StubReadyPlayer () : this(new List<Card>(), "No Name")
		{
		}

		public StubReadyPlayer (ICollection<Card> cards) : this (cards, "No Name"){
		}

		public StubReadyPlayer (ICollection<Card> cards, string name)
		{
			_cards = cards;
			this._state = PlayerState.Ready;
			this._name = name;
		}

		public StubReadyPlayer(string name) : this(new List<Card>(), name){

		}

		public StubReadyPlayer (Card card) : this(new List<Card>(){card})
		{
		}

		public StubReadyPlayer (Card card, string name) : this(new List<Card>(){card}, name)
		{
		}

		#region IPlayer implementation

		public void AddCards (Card card)
		{
			_cards.Add (card);
		}

		public void AddCards (IEnumerable<Card> cards)
		{
			foreach (Card card in cards) {
				_cards.Add (card);
			}
		}

		public void RemoveCards (ICollection<Card> cards)
		{
			foreach (Card card in cards.ToList()) {
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
				return _name;
			}
			private set{_name = value;}
		}

		public Card LowestCardInValue{
			get { return this._cards.ToList ().OrderBy (o => o.Value).FirstOrDefault (); }
		}

		#endregion

		private ICollection<Card> _cards;
		private PlayerState _state;
		private string _name;
	}
}

