using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
    using System.Runtime.InteropServices.ComTypes;

    public class Player
	{
        internal Player()
        {
            
        }

		public string Name {
			get{ return _name; }
            internal set
            {
                _name = value; 
            }
		}

		public PlayerState State {
			get {return _state;}
            internal set
            {
                _state = value;
            }
		}

        public List<Card> CardsInHand
        {
			get { return this._cardsInHand; }
            internal set { this._cardsInHand = value; }
		}

        public List<Card> CardsFaceUp
        {
            get { return this._cardsFaceUp; }
            internal set { this._cardsFaceUp = value; }
        }

        public List<Card> CardsFaceDown
        {
            get { return this._cardsFaceDown; }
            internal set { this._cardsFaceDown = value; }
        }

        public Player(string name, IEnumerable<Card> cardsInHand, IEnumerable<Card> cardsFaceDown)
        {
            this._name = name;
            this._cardsFaceDown = new List<Card>(cardsFaceDown);//.ToList();
            this._cardsInHand = new List<Card>(cardsInHand);
            this._cardsFaceUp = new List<Card>(); // cardsFaceUp.ToList();
            _state = PlayerState.Setup;
        }

	    public Player(string name, IEnumerable<Card> cardsInHand): this(name, cardsInHand, new List<Card>())
	    {
	    }

		public Player(string name) : this(name, new List<Card>()) {
			
		}

	    internal void AddCardsToInHandPile(IEnumerable<Card> cardsToBeAdded)
	    {
            foreach (Card addedCard in cardsToBeAdded)
            {
                this._cardsInHand.Add(addedCard);
            }
	    }

	    internal void AddCardToFaceDownPile(IEnumerable<Card> cardsToBeAdded)
	    {
            foreach (Card addedCard in cardsToBeAdded)
            {
                this._cardsFaceDown.Add(addedCard);
            }
	    }

        public ResultOutcome PutCardFaceUp(Card cardToPutFaceUp, Card faceUpCardToSwap = null)
        {
            if(_state != PlayerState.Setup) return ResultOutcome.Fail;
            
            if (faceUpCardToSwap != null)
                this.MoveCardToNewPile(faceUpCardToSwap, _cardsInHand, _cardsFaceUp);
            
            if (this.NumCardsFaceUp >= 3)
                return ResultOutcome.Fail;

            this.MoveCardToNewPile(cardToPutFaceUp, _cardsFaceUp, _cardsInHand);

            return ResultOutcome.Success;
        }

	    private void MoveCardToNewPile(Card cardToPutFaceUp, ICollection<Card> pileToAddTo, ICollection<Card> pileToRemoveFrom)
	    {
            var cardIsInPlayersHand = pileToRemoveFrom.Any(card => card.Equals(cardToPutFaceUp));

	        if (!cardIsInPlayersHand)
	            throw new ArgumentException();

            pileToAddTo.Add(cardToPutFaceUp);
            pileToRemoveFrom.Remove(cardToPutFaceUp);
	    }

	    internal void RemoveCardsFromInHand(ICollection<Card> cardsToBeRemoved){
			foreach (Card removedCard in cardsToBeRemoved) {
				this._cardsInHand.Remove (removedCard);
			}
		}

        public int NumCardsFaceUp { get { return _cardsFaceUp.Count; } }
        public int NumCardsFaceDown { get { return _cardsFaceDown.Count; } }
        public int NumCardsInHand { get { return _cardsInHand.Count; } }

		public void Ready ()
		{
			_state = PlayerState.Ready;

			//return ResultOutcome.Success;
		}

		public Card LowestCardInValue{
		    get
		    {
		        if (_cardsFaceUp == null && _cardsInHand == null)
		            return null;
		        return this._cardsFaceUp.Union(_cardsInHand).ToList ().OrderBy (o => o.Value).FirstOrDefault ();
		    }
		}

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var objAsPlayer = obj as Player;
            if (objAsPlayer == null)
                return false;
            return this._name.Equals(objAsPlayer._name);
        }

        private string _name;

        private List<Card> _cardsFaceDown;

        private List<Card> _cardsInHand;

        private List<Card> _cardsFaceUp;

		private PlayerState _state;

        
	}

}

