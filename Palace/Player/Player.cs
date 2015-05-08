using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Player
	{
		public string Name {
			get{ return _name; }
		}

		public PlayerState State {
			get {return _state;}
		}

		public IEnumerable<Card> CardsInHand {
			get { return this._cardsInHand; }
		}

        public IEnumerable<Card> CardsFaceUp
        {
            get { return this._cardsFaceUp; }
        }

        public IEnumerable<Card> CardsFaceDown
        {
            get { return this._cardsFaceDown; }
        }

		public Player(string name){
			this._name = name;
		    this._cardsFaceDown = new List<Card>();//.ToList();
            this._cardsInHand = new List<Card>(); 
            this._cardsFaceUp = new List<Card>(); // cardsFaceUp.ToList();
			_state = PlayerState.Setup;
		}

	    public void AddCardsToInHandPile(IEnumerable<Card> cardsToBeAdded)
	    {
            foreach (Card addedCard in cardsToBeAdded)
            {
                this._cardsInHand.Add(addedCard);
            }
	    }

	    public void AddCardToFaceDownPile(IEnumerable<Card> cardsToBeAdded)
	    {
            foreach (Card addedCard in cardsToBeAdded)
            {
                this._cardsFaceDown.Add(addedCard);
            }
	    }

        public ResultOutcome PutCardFaceUp(Card cardToPutFaceUp, Card faceUpCardToSwap = null)
        {
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
		        return this._cardsFaceUp.Union(_cardsInHand).ToList ().OrderBy (o => o.Value).FirstOrDefault ();
		    }
		}

	    private string _name;

        private List<Card> _cardsFaceDown;

        private List<Card> _cardsInHand;

        private List<Card> _cardsFaceUp;

		private PlayerState _state;
	}

}

