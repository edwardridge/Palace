namespace Palace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public enum OrderOfPlay
    {
        Forward = 1,
        Backward = 2
    }

    public enum PlayerCardTypes
    {
        InHand = 1,
        FaceUp = 2,
        FaceDown = 3
    }

    public class Game
    {
        public Guid Id { get; internal set; }

        internal Game()
        {
            //Used for Rhino only
        }

        internal Game(IEnumerable<Player> players, RulesProcessesor rulesProcessesor, ICardDealer cardDealer, Guid id)
            : this(players, rulesProcessesor, cardDealer, new List<Card>(), id)
        {
        }

        internal Game(IEnumerable<Player> players, RulesProcessesor rulesProcessesor, ICardDealer cardDealer, IEnumerable<Card> cardsInPile, Guid id)
        {
            this.Id = id;
            this._rulesProcessesor = rulesProcessesor;
            this._players = new LinkedList<Player>(players);
            this._playPile = new Stack<Card>(cardsInPile);
            this._cardDealer = cardDealer;
            this._orderOfPlay = OrderOfPlay.Forward;

            this._currentPlayer = _players.First;
        }

        public Result PlayInHandCards(Player player, ICollection<Card> cards)
        {
            IfArgumentsAreInvalidThenThrow(player, cards, player.CardsInHand);

            return PlayCardAndChooseNextPlayer(player, cards, PlayerCardTypes.InHand);
        }

        public Result PlayInHandCards(Player player, Card card)
        {
            return this.PlayInHandCards(player, new[] { card });
        }

        public Result PlayFaceUpCards(Player player, Card card)
        {
            return PlayFaceUpCards(player, new[] { card });
        }

        public Result PlayFaceUpCards(Player player, ICollection<Card> cards)
        {
            IfArgumentsAreInvalidThenThrow(player, cards, player.CardsFaceUp);

            if (player.NumCardsInHand >= 3) return new Result(ResultOutcome.Fail);
            return PlayCardAndChooseNextPlayer(player, cards, PlayerCardTypes.FaceUp);
        }

        public Result PlayFaceDownCards(Player player, Card card)
        {
            if (player.NumCardsInHand != 0) return new Result(ResultOutcome.Fail);
            if (player.NumCardsFaceUp != 0) return new Result(ResultOutcome.Fail);
            IfArgumentsAreInvalidThenThrow(player, new[]{card}, player.CardsFaceDown);
            return PlayCardAndChooseNextPlayer(player, new[] { card }, PlayerCardTypes.FaceDown);
        }

        private void IfArgumentsAreInvalidThenThrow(Player player, ICollection<Card> cards, ICollection<Card> cardsToCheck)
        {
            if (cards.Except(cardsToCheck).Any())
                throw new ArgumentException("You cannot play cards you don't have!");

            if (cards.Select(card => card.Value).Distinct().Count() != 1)
                throw new ArgumentException("You cannot play more than one type of card");
        }

        public void PlayerCannotPlayCards(Player player)
        {
            player.AddCardsToInHandPile(_playPile);
            _playPile.Clear();
            _currentPlayer = _rulesProcessesor.ChooseNextPlayer(null, _players, _currentPlayer, _orderOfPlay);
        }

        private Result PlayCardAndChooseNextPlayer(Player player, ICollection<Card> cards, PlayerCardTypes playerCardType)
        {
            if(_currentPlayer.Value.Equals(player) == false) return new Result(ResultOutcome.Fail);
            ;
            var cardToPlay = cards.First();

            if (!this._rulesProcessesor.CardCanBePlayed(cardToPlay, _playPile))
                return new Result(ResultOutcome.Fail);

            _orderOfPlay = this._rulesProcessesor.ChooseOrderOfPlay(_orderOfPlay, cardToPlay);
            if (this._rulesProcessesor.PlayPileShouldBeCleared(cards, _playPile))
                this._playPile.Clear();
            else
                foreach (Card card in cards)
                    _playPile.Push(card);

            this.RemoveCardsFromPlayer(player, cards, playerCardType);

            this._currentPlayer = this._rulesProcessesor.ChooseNextPlayer(cards, _players, this._currentPlayer, _orderOfPlay);

            return new Result(ResultOutcome.Success);
        }

        private void RemoveCardsFromPlayer(Player player, ICollection<Card> cards, PlayerCardTypes playerCardTypes)
        {
            if (playerCardTypes == PlayerCardTypes.InHand)
            {
                player.RemoveCardsFromInHand(cards);

                while (player.NumCardsInHand < 3 && this._cardDealer.CardsRemaining)
                {
                    player.AddCardsToInHandPile(this._cardDealer.DealCards(1));
                }
            }
            if (playerCardTypes == PlayerCardTypes.FaceDown)
                player.RemoveCardsFromFaceDown(cards);
            if (playerCardTypes == PlayerCardTypes.FaceUp)
                player.RemoveCardsFromFaceUp(cards);
        }

        public int PlayPileCardCount
        {
            get
            {
                if (_playPile == null)
                    return 0;
                return _playPile.Count;
            }
            internal set
            {
                var test = value;
                //noop
            }
        }

        public int NumberOfPlayers
        {
            get
            {
                return _players.Count;
            }
        }

        public Card LastCardPlayed
        {
            get
            {
                if (_playPile == null)
                    return null;
                return _playPile.Count == 0 ? null : _playPile.Peek();
            }
            internal set
            {
                var p = value;
            }
        }

        internal void Start(Player startingPlayer)
        {
            this._currentPlayer = _players.Find(startingPlayer);
        }

        internal Stack<Card> PlayPile
        {
            get
            {
                return _playPile;
            }
            set
            {
                _playPile = value;
            }
        }

        public IReadOnlyCollection<Player> Players
        {
            get
            {
                return _players.ToList().AsReadOnly();
            }
        } 

        internal LinkedList<Player> PlayersLinkedList
        {
            get { return _players; }
            set
            {
                this._players = value;
            }
        }

        internal RulesProcessesor RulesProcessesor
        {
            get
            {
                return _rulesProcessesor;
            }
            set
            {
                _rulesProcessesor = value;
            }
        }

        internal ICardDealer CardDealer
        {
            get
            {
                return _cardDealer;
            }
            set
            {
                _cardDealer = value;
            }
        }

        internal OrderOfPlay OrderOfPlay
        {
            get
            {
                return _orderOfPlay;
            }
            set
            {
                _orderOfPlay = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return this._currentPlayer != null ? this._currentPlayer.Value : null;
            }
            internal set
            {
                this._currentPlayer = _players.Find(value);
            }
        }

        private LinkedListNode<Player> _currentPlayer;

        private LinkedList<Player> _players;

        private Stack<Card> _playPile;

        private RulesProcessesor _rulesProcessesor;

        private ICardDealer _cardDealer;

        private OrderOfPlay _orderOfPlay;

        
    }
}