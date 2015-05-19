namespace Palace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Palace.Repository;

    using Raven.Client.Document;

    public enum OrderOfPlay
    {
        Forward = 1,
        Backward = 2
    }

    public class Game
    {

        internal Game()
        {
            
        }

        internal Game(SavedGame savedGame)
        {
            this._rulesProcessesor = savedGame.RulesProcessesor;
            this._players = new LinkedList<Player>(savedGame.Players);
            
            
        }
        
        internal Game(IEnumerable<Player> players, RulesProcessesor rulesProcessesor, ICardDealer cardDealer)
            : this(players, rulesProcessesor, cardDealer, new List<Card>())
        {
        }

        internal Game(IEnumerable<Player> players, RulesProcessesor rulesProcessesor, ICardDealer cardDealer, IEnumerable<Card> cardsInPile)
        {
            this._rulesProcessesor = rulesProcessesor;
            this._players = new LinkedList<Player>(players);
            this._playPile = new Stack<Card>(cardsInPile);
            this._cardDealer = cardDealer;
            this._orderOfPlay = OrderOfPlay.Forward;

            this._currentPlayer = _players.First;
            
        }

        public ResultOutcome PlayCards(Player player, ICollection<Card> cards)
        {
            if (cards.Except(player.CardsInHand).Any())
                throw new ArgumentException("You cannot play cards you don't have!");

            if (cards.Select(card => card.Value).Distinct().Count() != 1)
                throw new ArgumentException("You cannot play more than one type of card");

            var cardToPlay = cards.First();
            
            if (!this._rulesProcessesor.CardCanBePlayed(cardToPlay, _playPile))
                return ResultOutcome.Fail;

            _orderOfPlay = this._rulesProcessesor.ChooseOrderOfPlay(_orderOfPlay, cardToPlay);
            if(this._rulesProcessesor.PlayPileShouldBeCleared(cards))
                this._playPile.Clear();
            else
                foreach (Card card in cards)            
                    _playPile.Push(card);
            

            player.RemoveCardsFromInHand(cards);

            while(player.NumCardsInHand < 3 && _cardDealer.CardsRemaining)
                player.AddCardsToInHandPile(_cardDealer.DealCards(1));
            
            this._currentPlayer = this._rulesProcessesor.ChooseNextPlayer(cards, _players, this._currentPlayer, _orderOfPlay);

            return ResultOutcome.Success;
        }

        public ResultOutcome PlayCards(Player player, Card card)
        {
            return PlayCards(player, new[] { card });
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

        internal LinkedList<Player> Players
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

        public void PlayerCannotPlayCards(Player player)
        {
            player.AddCardsToInHandPile(_playPile);
            _playPile.Clear();
            _currentPlayer = _rulesProcessesor.ChooseNextPlayer(null, _players, _currentPlayer, _orderOfPlay);
        }
    }
}