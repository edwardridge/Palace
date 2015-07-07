using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
    public class Player
    {
        internal Player()
        {

        }

        public string Name
        {
            get
            {
                return _name;
            }
            internal set
            {
                _name = value;
            }
        }

        public PlayerState State
        {
            get
            {
                return _state;
            }
            internal set
            {
                _state = value;
            }
        }

        public IReadOnlyCollection<Card> CardsInHand
        {
            get
            {
                return this._cardsInHand == null ? new List<Card>().AsReadOnly() : this._cardsInHand.AsReadOnly();
            }
            internal set
            {
                this._cardsInHand = new List<Card>(value);
            }
        }

        public IReadOnlyCollection<Card> CardsFaceUp
        {
            get
            {
                return this._cardsFaceUp;
            }
            internal set
            {
                this._cardsFaceUp = new List<Card>(value);
            }
        }

        public IReadOnlyCollection<Card> CardsFaceDown
        {
            get
            {
                return this._cardsFaceDown;
            }
            internal set
            {
                this._cardsFaceDown = new List<Card>(value);
            }
        }

        public Player(string name, IEnumerable<Card> cardsInHand, IEnumerable<Card> cardsFaceUp, IEnumerable<Card> cardsFaceDown)
        {
            this._name = name;
            this._cardsFaceDown = new List<Card>(cardsFaceDown); //.ToList();
            this._cardsInHand = new List<Card>(cardsInHand);
            this._cardsFaceUp = new List<Card>(cardsFaceUp); // cardsFaceUp.ToList();
            _state = PlayerState.Setup;
        }

        public Player(string name, IEnumerable<Card> cardsInHand, IEnumerable<Card> cardsFaceUp)
            : this(name, cardsInHand, cardsFaceUp, new List<Card>())
        {
            
        }

        public Player(string name, IEnumerable<Card> cardsInHand)
            : this(name, cardsInHand, new List<Card>(), new List<Card>())
        {
        }

        public Player(string name)
            : this(name, new List<Card>())
        {

        }

        internal Result PutCardFaceUp(Card cardToPutFaceUp, Card faceUpCardToSwap = null)
        {
            if (_state != PlayerState.Setup)
                return new Result("Cannot put card face up");

            if (faceUpCardToSwap != null)
                this.MoveCardToNewPile(faceUpCardToSwap, _cardsInHand, _cardsFaceUp);

            if (this.CardsFaceUp.Count >= 3)
                return new Result("Cannot put more than 3 cards face up");

            this.MoveCardToNewPile(cardToPutFaceUp, _cardsFaceUp, _cardsInHand);

            return new Result();
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

        internal void RemoveCardsFromFaceDown(ICollection<Card> cards)
        {
            foreach (var card in cards)
                this._cardsFaceDown.Remove(card);
        }

        internal void RemoveCardsFromFaceUp(ICollection<Card> cards)
        {
            foreach (var card in cards)
                this._cardsFaceUp.Remove(card);

        }

        internal void RemoveCardsFromInHand(ICollection<Card> cardsToBeRemoved)
        {
            foreach (Card removedCard in cardsToBeRemoved)
            {
                this._cardsInHand.Remove(removedCard);
            }
        }

        internal void Ready()
        {
            _state = PlayerState.Ready;

            //return ResultOutcome.Success;
        }

        internal bool HasNoMoreCards()
        {
            return this.CardsFaceDown.Count == 0 && this.CardsFaceUp.Count == 0 && this.CardsInHand.Count == 0;
        }

        private void MoveCardToNewPile(Card cardToMove, ICollection<Card> pileToAddTo, ICollection<Card> pileToRemoveFrom)
        {
            var cardIsInPileToRemoveFrom = pileToRemoveFrom.Any(card => card.Equals(cardToMove));

            if (!cardIsInPileToRemoveFrom)
                throw new ArgumentException();

            pileToAddTo.Add(cardToMove);
            pileToRemoveFrom.Remove(cardToMove);
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



