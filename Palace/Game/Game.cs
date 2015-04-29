using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class GameStartValidator : IGameStartValidator{

		public bool GameIsValid (ICollection<IPlayer> players)
		{
			var returnVal = !(players.Any (p => p.Cards.Count (c => c.CardOrientation == CardOrientation.FaceUp) != 3));
			return returnVal;
		}
	}

	public class Dealer{
		public Dealer(Deck deck, IGameStartValidator gameStartValidator) : this(deck, gameStartValidator, new Dictionary<CardValue, RuleForCard>()){

		}

		public Dealer(Deck deck,  IGameStartValidator gameStartValidator, Dictionary<CardValue, RuleForCard> rulesForCardsByValue){
			this._deck = deck;
			this._rulesForCardsByValue = rulesForCardsByValue;
			this._gameStartValidator = gameStartValidator;
		}

		public void DealIntialCards(ICollection<IPlayer> players){
			foreach (IPlayer player in players) {
				player.AddCards (_deck.TakeCards (3, CardOrientation.FaceDown));
				player.AddCards (_deck.TakeCards (6, CardOrientation.InHand));
			}
		}

		public Game StartGame(ICollection<IPlayer> players){
			if(_gameStartValidator.GameIsValid(players))
				return ResumeGame (players);

			throw new InvalidOperationException ();
		}

		public Game ResumeGame(ICollection<IPlayer> players){
			var startingPlayer = players.First ();

			foreach (var player in players) {
				if (player.Cards == null || player.Cards.Count == 0)
					continue;
				if (player.LowestCardInValue.Value < startingPlayer.LowestCardInValue.Value)
					startingPlayer = player;
			}
			Game game = new Game(players, _deck, _rulesForCardsByValue);
			game.Start (startingPlayer);
			return game;
		}

		private Deck _deck;
		private Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;
		private IGameStartValidator _gameStartValidator;
	}

	public class GameInProgress : Game{
		public GameInProgress(ICollection<IPlayer> players, Deck deck)
			: this(players, deck, new Dictionary<CardValue, RuleForCard> (),new Stack<Card>()){

		}

		public GameInProgress(ICollection<IPlayer> players, Deck deck, IEnumerable<Card> playPile)
			: this(players, deck, new Dictionary<CardValue, RuleForCard> (), playPile){

		}
		 
		public GameInProgress (ICollection<IPlayer> players, Deck deck, Dictionary<CardValue, RuleForCard> rulesForCardsByValue, IEnumerable<Card> playPile)
			: base (players, deck, rulesForCardsByValue){
			this._gameState = GameState.GameStarted;
			this._playPile = new Stack<Card>(playPile);

		}
	}

	public class Game
	{
		public Game(ICollection<IPlayer> players, Deck deck)
			: this(players, deck, new Dictionary<CardValue, RuleForCard> ()){

		}

		public Game(ICollection<IPlayer> players, Deck deck, Dictionary<CardValue, RuleForCard> rulesForCardsByValue){
			this._deck = deck;
			this._players = new LinkedList<IPlayer>(players);
			this._gameState = GameState.GameInSetup;
			this._playPile = new Stack<Card>();
			this.rulesForCardsByValue = rulesForCardsByValue;

			_currentPlayerNode = _players.First;
		}

		public void Start(IPlayer startingPlayer){
			bool allPlayersReady = _players.All(player => player.State == PlayerState.Ready);

			if(!allPlayersReady) throw new ArgumentException("Not all players are ready");

			_currentPlayerNode = _players.Find (startingPlayer);
			_gameState = GameState.GameStarted;
		}

		public ResultOutcome PlayCards (IPlayer player, ICollection<Card> cards)
		{
			if (_gameState != GameState.GameStarted)
				return ResultOutcome.Fail;

			if (!cardsPassRules (player, cards)) 
				return ResultOutcome.Fail;
				
			player.RemoveCards (cards);
			foreach (Card card in cards) {
				_playPile.Push (card);
			}

			_currentPlayerNode = _currentPlayerNode.Next ?? _players.First ;

			return ResultOutcome.Success;
		}

		private bool cardsPassRules (IPlayer player, ICollection<Card> cards)
		{
			if (cards.Select (card => card.Value).Distinct ().Count() != 1)
				return false;

			if (cards.Except (player.Cards).Any ())
				throw new ArgumentException ("You cannot play cards you don't have!");

			if (_playPile.Count == 0)
				return true;

			var lastCardPlayed = _playPile.Peek();
			var playersCard = cards.First ();
			var ruleForCard = getRuleForCardFromCardValue (lastCardPlayed.Value);

			if (ruleForCard == RuleForCard.Standard && playersCard.Value < lastCardPlayed.Value)
				return false;
			if (ruleForCard == RuleForCard.LowerThan && playersCard.Value > lastCardPlayed.Value)
				return false;

			return true;
		}

		private RuleForCard getRuleForCardFromCardValue(CardValue cardValue){
			RuleForCard ruleForCard;
			rulesForCardsByValue.TryGetValue(cardValue, out ruleForCard);
			return ruleForCard == 0 ? RuleForCard.Standard : ruleForCard;
		}

		public ResultOutcome PlayCards(IPlayer player, Card card){
			return PlayCards (player, new[]{ card });
		}

		public int PlayPileCardCount(){
			return _playPile.Count;
		}

		public int NumberOfPlayers {
			get{ return _players.Count; }
		}

		public IPlayer CurrentPlayer {
			get{ return _currentPlayerNode.Value; }
		}

		public GameState GameState{
			get { return _gameState; }
		}

		private LinkedListNode<IPlayer> _currentPlayerNode;
		private LinkedList<IPlayer> _players;
		protected Stack<Card> _playPile;
		private Deck _deck;
		protected GameState _gameState;
		private Dictionary<CardValue, RuleForCard> rulesForCardsByValue;
	}

}

