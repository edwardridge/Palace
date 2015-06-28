namespace Palace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal enum PlayerCardTypes
    {
        InHand = 1,
        FaceUp = 2,
        FaceDown = 3
    }

    public class GameState
    {
        internal bool GameOver { get; set; }

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

        public IReadOnlyCollection<Card> PlayPile
        {
            get
            {
                return playPileStack.ToList().AsReadOnly();
            }
        } 

        internal Stack<Card> PlayPileStack
        {
            get
            {
                return this.playPileStack;
            }
            set
            {
                this.playPileStack = new Stack<Card>(value);
            }
        }

        public LinkedList<Player> Players
        {
            get
            {
                return _players;
            }
            internal set
            {
                this._players = value;
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

        internal LinkedListNode<Player> CurrentPlayerLinkedListNode
        {
            get
            {
                return this._currentPlayer;
            }
            set
            {
                this._currentPlayer = value;
            }
        } 

        private LinkedListNode<Player> _currentPlayer;

        private OrderOfPlay _orderOfPlay;

        private Stack<Card> playPileStack;

        private LinkedList<Player> _players;
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
            //this._players = new LinkedList<Player>(players);
            //this._playPile = new Stack<Card>(cardsInPile);
            this._cardDealer = cardDealer;

            this.State = new GameState
                             {
                                 GameOver = false, OrderOfPlay = OrderOfPlay.Forward, PlayPileStack = new Stack<Card>(cardsInPile),
                                Players = new LinkedList<Player>(players)
                             };

            State.CurrentPlayer = State.Players.First.Value;
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

            if (player.CardsInHand.Count >= 3) return new Result("Cannot play face up card when you have cards in hand");
            return PlayCardAndChooseNextPlayer(player, cards, PlayerCardTypes.FaceUp);
        }

        public Result PlayFaceDownCards(Player player, Card card)
        {
            if (player.CardsInHand.Count != 0) return new Result("Cannot play face down card when you have cards in hand");
            if (player.CardsFaceUp.Count != 0) return new Result("Cannot play face down card when you have face up cards");
            IfArgumentsAreInvalidThenThrow(player, new[]{card}, player.CardsFaceDown);
            return PlayCardAndChooseNextPlayer(player, new[] { card }, PlayerCardTypes.FaceDown);
        }

        public void PlayerCannotPlayCards(Player player)
        {
            player.AddCardsToInHandPile(State.PlayPileStack);
            State.PlayPileStack.Clear();
            _rulesProcessesor.SetNextPlayer(null, State);
        }

        internal void Start(Player startingPlayer)
        {
            State.CurrentPlayerLinkedListNode = State.Players.Find(startingPlayer);
        }

        private void IfArgumentsAreInvalidThenThrow(Player player, ICollection<Card> cards, IEnumerable<Card> cardsToCheck)
        {
            if (cards.Except(cardsToCheck).Any())
                throw new ArgumentException("You cannot play cards you don't have!");

            if (cards.Select(card => card.Value).Distinct().Count() != 1)
                throw new ArgumentException("You cannot play more than one type of card");
        }

        private Result PlayCardAndChooseNextPlayer(Player player, ICollection<Card> cards, PlayerCardTypes playerCardType)
        {
            if (this.State.GameOver) return new GameOverResult(State.CurrentPlayer);
            if (State.CurrentPlayer.Equals(player) == false) return new Result("It isn't your turn!");
            
            var cardToPlay = cards.First();

            if (!this._rulesProcessesor.CardCanBePlayed(cardToPlay, State))
                return new Result("This card is invalid to play");

            this._rulesProcessesor.SetOrderOfPlay(this.State, cardToPlay);
            
            this.RemoveCardsFromPlayer(player, cards, playerCardType);

            if (player.CardsFaceDown.Count == 0 && player.CardsFaceUp.Count == 0 & player.CardsInHand.Count == 0)
            {
                this.State.GameOver = true;
                return new GameOverResult(player);
            }

            foreach (Card card in cards)
                State.PlayPileStack.Push(card);

            this._rulesProcessesor.SetNextPlayer(cards, State);

            if (this._rulesProcessesor.PlayPileShouldBeCleared(State))
                this.State.PlayPileStack.Clear();
            
            return new Result();
        }

        private void RemoveCardsFromPlayer(Player player, ICollection<Card> cards, PlayerCardTypes playerCardTypes)
        {
            if (playerCardTypes == PlayerCardTypes.InHand)
            {
                player.RemoveCardsFromInHand(cards);

                while (player.CardsInHand.Count < 3 && this._cardDealer.CardsRemaining)
                {
                    player.AddCardsToInHandPile(this._cardDealer.DealCards(1));
                }
            }
            if (playerCardTypes == PlayerCardTypes.FaceDown)
                player.RemoveCardsFromFaceDown(cards);
            if (playerCardTypes == PlayerCardTypes.FaceUp)
                player.RemoveCardsFromFaceUp(cards);
        }

        public GameState State { get; internal set; }

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

        private RulesProcessesor _rulesProcessesor;

        private ICardDealer _cardDealer;
    }

    
}