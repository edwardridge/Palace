namespace Palace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Raven.Imports.Newtonsoft.Json;
    using Rules;

    public enum PlayerCardType
    {
        InHand = 1,
        FaceUp = 2,
        FaceDown = 3
    }

    public class GameState
    {
        public GameState()
        {
            this._playPileStack = new Stack<Card>();
            this._players = new LinkedList<Player>();
            this._deck = new Deck();
        }
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
                return _playPileStack.ToList().AsReadOnly();
            }
        } 

        internal Stack<Card> PlayPileStack
        {
            get
            {
                return this._playPileStack;
            }
            set
            {
                this._playPileStack = new Stack<Card>(value);
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

        public string CurrentPlayerName
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

        //Todo: Remove!
        //[JsonIgnore]
        public Player GetCurrentPlayer()
        {
           return this.Players.First(f => f.Name == _currentPlayer); 
        }

        [JsonIgnore]
        internal LinkedListNode<Player> CurrentPlayerLinkedListNode
        {
            get
            {
                return _players.Find(this.Players.First(f => f.Name == this.CurrentPlayerName));
            }
        }

        //public Card LastCardPlayed

        public Deck Deck
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

        public int NumberOfValdMoves { get; internal set; }

        private string _currentPlayer;

        private OrderOfPlay _orderOfPlay;

        private Stack<Card> _playPileStack;

        private LinkedList<Player> _players;

        private Deck _deck;

        public static GameState SetUpInitialState(IEnumerable<Player> players, Deck deck, Guid id, Player startingPlayer = null, IEnumerable<Card> cardsInPile = null)
        {
            players = players.ToList();
            return new GameState()
            {
                OrderOfPlay = OrderOfPlay.Forward,
                CurrentPlayerName = startingPlayer?.Name ?? players.First()?.Name,
                Players = new LinkedList<Player>(players),
                //Id = id,
                GameOver = false,
                PlayPileStack = new Stack<Card>(cardsInPile ?? new List<Card>()),
                Deck = deck
            };
        }
    }
    
    public class Game
    {
        public Guid Id { get; set; }

        protected bool Equals(Game other)
        {
            return Equals(this.rulesProcessorGenerator, other.rulesProcessorGenerator) && Equals(this._state, other._state);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.rulesProcessorGenerator != null ? this.rulesProcessorGenerator.GetHashCode() : 0) * 397) ^ (this.State != null ? this.State.GetHashCode() : 0);
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

        public Game(GameState gameState, RulesProcessorGenerator rulesProcessorGenerator)
        {
            this._state = gameState;
            this.RulesProcessorGenerator = rulesProcessorGenerator;
        }

        public Result PlayInHandCards(string playerName, ICollection<Card> cards)
        {
            var player = this.FindPlayer(playerName);
            var preCheck = IfArgumentsAreInvalidThenThrow(player, cards, player.CardsInHand);
            if (preCheck.Any()) return new Result(player, this.State, preCheck.ToArray()[0]);

            return PlayCardAndChooseNextPlayer(player, cards, PlayerCardType.InHand);
        }

        public Result PlayInHandCards(string playerName, Card card)
        {
            return this.PlayInHandCards(playerName, new[] { card });
        }

        public Result PlayFaceUpCards(string playerName, Card card)
        {
            return PlayFaceUpCards(playerName, new[] { card });
        }

        public Result PlayFaceUpCards(string playerName, ICollection<Card> cards)
        {
            var player = this.FindPlayer(playerName);
            var preCheck =IfArgumentsAreInvalidThenThrow(player, cards, player.CardsFaceUp);
            if (preCheck.Any()) return new Result(player, this.State, preCheck.ToArray()[0]);

            if (player.CardsInHand.Count >= 3) return new Result(player, this.State, "Cannot play face up card when you have cards in hand");
            return PlayCardAndChooseNextPlayer(player, cards, PlayerCardType.FaceUp);
        }

        public Result PlayFaceDownCards(string playerName, Card card)
        {
            var player = this.FindPlayer(playerName);

            if (player.CardsInHand.Count != 0) return new Result(player, this.State, "Cannot play face down card when you have cards in hand");
            if (player.CardsFaceUp.Count != 0) return new Result(player, this.State, "Cannot play face down card when you have face up cards");
            IfArgumentsAreInvalidThenThrow(player, new[]{card}, player.CardsFaceDown);

            var ruleProcessor = RulesProcessorGenerator.GetRuleProcessor(_state, new[] { card });
            if (!ruleProcessor.CardCanBePlayed())
            {
                ruleProcessor.GetNextStateWhenPlayingFaceDownCard();
                return new Result(player, _state, "That face down card is invalid!");
            }

            return PlayCardAndChooseNextPlayer(player, new[] { card }, PlayerCardType.FaceDown); ;
        }

        public Result PlayerCannotPlayCards(string playerName)
        {
            var player = FindPlayer(playerName);
            if (_state.CurrentPlayerName.Equals(playerName) == false) return new Result(player, this.State, "It isn't your turn!");
            var ruleProcessor = this.rulesProcessorGenerator.GetRuleProcessor(_state, null);
            this._state = ruleProcessor.GetNextStateWhenCardCannotBePlayed(player);

            return new Result(player, this.State);
        }

        private Player FindPlayer(string playerName)
        {
            return _state.Players.First(f => f.Name == playerName);
        }

        internal void Start(Player startingPlayer)
        {
            _state.CurrentPlayerName = _state.Players.Find(startingPlayer).Value.Name;
        }

        private IEnumerable<string> IfArgumentsAreInvalidThenThrow(Player player, ICollection<Card> cards, IEnumerable<Card> cardsToCheck)
        {
            if (cards.Except(cardsToCheck).Any())
                return new[] { "You cannot play cards you don't have!" } ;

            if (cards.Select(card => card.Value).Distinct().Count() != 1)
                return new[] { "You cannot play more than one type of card" };
            return new List<string>();
        }

        private Result PlayCardAndChooseNextPlayer(Player player, ICollection<Card> cards, PlayerCardType playerCardType)
        {
            if (this._state.GameOver) return new GameOverResult(player, _state.GetCurrentPlayer(), _state);
            if (_state.GetCurrentPlayer().Equals(player) == false) return new Result(player, this.State, "It isn't your turn!");

            var ruleProcessor = this.rulesProcessorGenerator.GetRuleProcessor(_state, cards);

            if (!ruleProcessor.CardCanBePlayed())
                return new Result(player, this.State, "This card is invalid to play");

            this._state = ruleProcessor.GetNextState(playerCardType);
            if (this._state.GameOver)
                return new GameOverResult(player, _state.GetCurrentPlayer(), _state);
            return new Result(player, this.State);
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

        public RulesForGame Rules
        {
            get
            {
                RulesProcessorGenerator rulesGenerator = RulesProcessorGenerator == null ? new RulesProcessorGenerator(this.Id, new RulesForGame()) : RulesProcessorGenerator;
                return rulesGenerator.RulesForCardsByValue;
            }
        }

        internal RulesProcessorGenerator RulesProcessorGenerator
        {
            get
            {
                return this.rulesProcessorGenerator;
            }
            private set
            {
                this.rulesProcessorGenerator = value;
            }
        }
        private RulesProcessorGenerator rulesProcessorGenerator;

        private GameState _state;
    }

    
}