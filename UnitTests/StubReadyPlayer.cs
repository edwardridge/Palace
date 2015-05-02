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
		    _cards = new List<Card>(cards);
			this.State = PlayerState.Ready;
			this.Name = name;
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
			foreach (var card in cards) {
				_cards.Add (card);
			}
		}

		public void RemoveCards (ICollection<Card> cards)
		{
            foreach (var card in cards)
            {
				_cards.Remove (card);
			}
		}

		public ICollection<Card> Cards {
			get {
				return this._cards;
			}
		}

	    public PlayerState State { get; set; }

	    public string Name { get; private set; }

	    public Card LowestCardInValue{
			get { return this._cards.ToList ().OrderBy (o => o.Value).FirstOrDefault (); }
		}

		#endregion

		private ICollection<Card> _cards;
	}
}

