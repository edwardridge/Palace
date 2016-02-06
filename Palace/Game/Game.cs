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
            private set
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
            private set
            {
                this._players = value;
            }
        }

        public string CurrentPlayerName
        {
            get
            {
                return this._currentPlayerName;
            }
            set
            {
                this._currentPlayerName = value;
            }
        }

        [JsonIgnore]
        public Player CurrentPlayer
        {
            get { return this.Players.First(f => f.Name == _currentPlayerName); }
        }
        
        public Player FindPlayer(string playerName)
        {
            return this.Players.First(f => f.Name == playerName);
        }

        [JsonIgnore]
        internal LinkedListNode<Player> CurrentPlayerLinkedListNode
        {
            get
            {
                return _players.Find(this.Players.First(f => f.Name == this.CurrentPlayerName));
            }
        }

        public Deck Deck
        {
            get
            {
                return this._deck;
            }
            private set
            {
                this._deck = value;
            }
        }

        public int NumberOfValdMoves { get; internal set; }

        private string _currentPlayerName;

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

        private Game()
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
            var player = _state.FindPlayer(playerName);
            var preCheck = IfArgumentsAreInvalidThenThrow(player, cards, player.CardsInHand);
            if (preCheck.Any()) return new Result(player, this.State, preCheck.ToArray()[0]);

            return PlayCards(player, cards, PlayerCardType.InHand);
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
            var player = _state.FindPlayer(playerName);
            var preCheck =IfArgumentsAreInvalidThenThrow(player, cards, player.CardsFaceUp);
            if (preCheck.Any()) return new Result(player, this.State, preCheck.ToArray()[0]);

            if (player.CardsInHand.Count >= 3) return new Result(player, this.State, "Cannot play face up card when you have cards in hand");
            return PlayCards(player, cards, PlayerCardType.FaceUp);
        }

        public Result PlayFaceDownCards(string playerName, Card card)
        {
            var player = _state.FindPlayer(playerName);

            if (player.CardsInHand.Count != 0) return new Result(player, this.State, "Cannot play face down card when you have cards in hand");
            if (player.CardsFaceUp.Count != 0) return new Result(player, this.State, "Cannot play face down card when you have face up cards");
            IfArgumentsAreInvalidThenThrow(player, new[]{card}, player.CardsFaceDown);

            var ruleProcessor = RulesProcessorGenerator.GetRuleProcessor(_state, new[] { card });
            if (!ruleProcessor.CardCanBePlayed())
            {
                ruleProcessor.GetNextStateWhenPlayingFaceDownCard();
                return new Result(player, _state, "That face down card is invalid!");
            }

            return PlayCards(player, new[] { card }, PlayerCardType.FaceDown); ;
        }

        public Result PlayerCannotPlayCards(string playerName)
        {
            var player = _state.FindPlayer(playerName);
            if (_state.CurrentPlayerName.Equals(playerName) == false) return new Result(player, _state, "It isn't your turn!");
            var ruleProcessor = this.rulesProcessorGenerator.GetRuleProcessor(_state, null);
            _state = ruleProcessor.GetNextStateWhenCardCannotBePlayed(player);

            return new Result(player, this.State);
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

        private Result PlayCards(Player player, ICollection<Card> cards, PlayerCardType playerCardType)
        {
            if (_state.GameOver) return new GameOverResult(player, _state.CurrentPlayer, _state);
            if (_state.CurrentPlayer.Equals(player) == false) return new Result(player, this.State, "It isn't your turn!");

            var ruleProcessor = this.rulesProcessorGenerator.GetRuleProcessor(_state, cards);

            if (!ruleProcessor.CardCanBePlayed())
                return new Result(player, this.State, "This card is invalid to play");

            _state = ruleProcessor.GetNextState(playerCardType);
            if (_state.GameOver)
                return new GameOverResult(player, _state.CurrentPlayer, _state);
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

        public RulesForGame GetRules()
        {
            return RulesProcessorGenerator.RulesForCardsByValue;
            
        }

        private RulesProcessorGenerator RulesProcessorGenerator
        {
            get
            {
                return this.rulesProcessorGenerator;
            }
            set
            {
                this.rulesProcessorGenerator = value;
            }
        }
        private RulesProcessorGenerator rulesProcessorGenerator;

        private GameState _state;
    }

    
}