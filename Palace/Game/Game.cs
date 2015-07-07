namespace Palace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Raven.Imports.Newtonsoft.Json;

    internal enum PlayerCardTypes
    {
        InHand = 1,
        FaceUp = 2,
        FaceDown = 3
    }

    public class GameState
    {
        public Guid GameId { get; set; }

        public DateTime DateSaved { get; set; }

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

        [JsonIgnore]
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
                return this._currentPlayer;
            }
            set
            {
                this._currentPlayer = value;
            }
        }

        [JsonIgnore]
        internal LinkedListNode<Player> CurrentPlayerLinkedListNode
        {
            get
            {
                return _players.Find(_currentPlayer);
            }
        }

        internal Deck Deck
        {
            get
            {
                return this._deck;
            }
            set
            {
                this._deck = value;
            }
        }

        private Player _currentPlayer;

        private OrderOfPlay _orderOfPlay;

        private Stack<Card> playPileStack;

        private LinkedList<Player> _players;

        private Deck _deck;

        public static GameState SetUpInitialState(IEnumerable<Player> players, Deck deck, Guid id, Player startingPlayer = null, IEnumerable<Card> cardsInPile = null)
        {
            players = players.ToList();
            return new GameState()
            {
                OrderOfPlay = OrderOfPlay.Forward,
                CurrentPlayer = startingPlayer ?? players.First(),
                Players = new LinkedList<Player>(players),
                GameId = id,
                GameOver = false,
                PlayPileStack = new Stack<Card>(cardsInPile ?? new List<Card>()),
                Deck = deck
            };
        }
    }
    
    public class Game
    {
        protected bool Equals(Game other)
        {
            return Equals(this._rulesProcessesor, other._rulesProcessesor) && Equals(this.State, other.State);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this._rulesProcessesor != null ? this._rulesProcessesor.GetHashCode() : 0) * 397) ^ (this.State != null ? this.State.GetHashCode() : 0);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Game)obj);
        }


        internal Game()
        {
            //Used for Rhino only
        }

        public Game(GameState gameState, RulesProcessesor rulesProcessesor)
        {
            this.State = gameState;
            this.RulesProcessesor = rulesProcessesor;
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
            var ruleChecker = _rulesProcessesor.GetRuleChecker(State, null);
            player.AddCardsToInHandPile(State.PlayPileStack);
            State.PlayPileStack.Clear();
            State.CurrentPlayer = ruleChecker.SetNextPlayer();
        }

        internal void Start(Player startingPlayer)
        {
            State.CurrentPlayer= State.Players.Find(startingPlayer).Value;
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

            var ruleChecker = _rulesProcessesor.GetRuleChecker(State, cards);

            if (!ruleChecker.CardCanBePlayed())
                return new Result("This card is invalid to play");

            this.RemoveCardsFromPlayer(State.CurrentPlayer, cards, playerCardType);

            foreach (Card card in cards)
                State.PlayPileStack.Push(card);

            State.OrderOfPlay = ruleChecker.GetOrderOfPlay();

            if (!State.CurrentPlayer.HasNoMoreCards())
            {
                State.CurrentPlayer = ruleChecker.SetNextPlayer();

                if (ruleChecker.PlayPileShouldBeCleared())
                    this.State.PlayPileStack.Clear();
                
            }
            else
            {
                this.State.GameOver = true;
                return new GameOverResult(State.CurrentPlayer);
            }
            return new Result();
        }

        private void RemoveCardsFromPlayer(Player player, ICollection<Card> cards, PlayerCardTypes playerCardTypes)
        {
            if (playerCardTypes == PlayerCardTypes.InHand)
            {
                player.RemoveCardsFromInHand(cards);

                while (player.CardsInHand.Count < 3 && this.State.Deck.CardsRemaining)
                {
                    player.AddCardsToInHandPile(this.State.Deck.DealCards(1));
                }
            }
            if (playerCardTypes == PlayerCardTypes.FaceDown)
                player.RemoveCardsFromFaceDown(cards);
            if (playerCardTypes == PlayerCardTypes.FaceUp)
                player.RemoveCardsFromFaceUp(cards);
        }

        public GameState State
        {
            get
            {
                return this._state;
            }
            private set
            {
                this._state = value;
            }
        }

        internal RulesProcessesor RulesProcessesor
        {
            get
            {
                return _rulesProcessesor;
            }
            private set
            {
                _rulesProcessesor = value;
            }
        }
        private RulesProcessesor _rulesProcessesor;

        private GameState _state;
    }

    
}