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

    public class Game
    {
        internal Game(IEnumerable<IPlayer> players, RulesProcessesor rulesProcessesor, ICardDealer cardDealer)
            : this(players, rulesProcessesor, cardDealer, new List<Card>())
        {
        }

        internal Game(IEnumerable<IPlayer> players, RulesProcessesor rulesProcessesor, ICardDealer cardDealer, IEnumerable<Card> cardsInPile)
        {
            this._rulesProcessesor = rulesProcessesor;
            this._players = new LinkedList<IPlayer>(players);
            this._playPile = new Stack<Card>(cardsInPile);
            this._cardDealer = cardDealer;
            this._orderOfPlay = OrderOfPlay.Forward;

            this._currentPlayer = _players.First;
        }

        public ResultOutcome PlayCards(IPlayer player, ICollection<Card> cards)
        {
            if (cards.Except(player.Cards).Any())
                throw new ArgumentException("You cannot play cards you don't have!");

            if (cards.Select(card => card.Value).Distinct().Count() != 1)
                throw new ArgumentException("You cannot play more than one type of card");

            var cardToPlay = cards.First();
            var lastCardPlayed = _playPile.Any() ? _playPile.Peek() : null;

            if (!this._rulesProcessesor.CardCanBePlayed(cardToPlay, lastCardPlayed))
                return ResultOutcome.Fail;

            _orderOfPlay = this._rulesProcessesor.ChooseOrderOfPlay(_orderOfPlay, cardToPlay);
            if(this._rulesProcessesor.PlayPileShouldBeCleared(cardToPlay))
                this._playPile.Clear();
            else
                foreach (Card card in cards)            
                    _playPile.Push(card);
            

            player.RemoveCards(cards);
            player.AddCards(_cardDealer.DealCards(cards.Count));

            this._currentPlayer = this._rulesProcessesor.ChooseNextPlayer(cardToPlay, _players, this._currentPlayer, _orderOfPlay);

            return ResultOutcome.Success;
        }

        public ResultOutcome PlayCards(IPlayer player, Card card)
        {
            return PlayCards(player, new[] { card });
        }

        public int PlayPileCardCount
        {
            get
            {
                return _playPile.Count;
            }
        }

        public int NumberOfPlayers
        {
            get
            {
                return _players.Count;
            }
        }

        public IPlayer CurrentPlayer
        {
            get
            {
                return this._currentPlayer.Value;
            }
        }

        internal void Start(IPlayer startingPlayer)
        {
            this._currentPlayer = _players.Find(startingPlayer);
        }

        private LinkedListNode<IPlayer> _currentPlayer;

        private LinkedList<IPlayer> _players;

        private Stack<Card> _playPile;

        private RulesProcessesor _rulesProcessesor;

        private ICardDealer _cardDealer;

        private OrderOfPlay _orderOfPlay;
    }
}