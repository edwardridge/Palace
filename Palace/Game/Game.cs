using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class GameInProgress : Game{
		public GameInProgress(ICollection<IPlayer> players, Deck deck)
			: this(players, deck, new Dictionary<CardValue, CardType> (),new Stack<Card>()){

		}

		public GameInProgress(ICollection<IPlayer> players, Deck deck, IEnumerable<Card> playPile)
			: this(players, deck, new Dictionary<CardValue, CardType> (), playPile){

		}
		 
		public GameInProgress (ICollection<IPlayer> players, Deck deck, Dictionary<CardValue, CardType> cardTypes, IEnumerable<Card> playPile)
			: base (players, deck, cardTypes){
			this._gameState = GameState.GameStarted;
			this._playPile = new Stack<Card>(playPile);
		}

		public override sealed void Setup ()
		{

		}

		public override sealed ResultOutcome Start ()
		{
			return ResultOutcome.Success;
		}

	}
	public class Game
	{
		public Game(ICollection<IPlayer> players, Deck deck)
			: this(players, deck, new Dictionary<CardValue, CardType> ()){

		}

		public Game(ICollection<IPlayer> players, Deck deck, Dictionary<CardValue, CardType> cardTypes){
			this._deck = deck;
			this._players = new LinkedList<IPlayer>(players);
			this._gameState = GameState.GameInSetup;
			this._playPile = new Stack<Card>();
			this._cardTypes = cardTypes;

			_currentPlayer = _players.First;
		}

		public virtual ResultOutcome Start ()
		{
			bool allPlayersReady = _players.All(player => player.State == PlayerState.Ready);

			if(!allPlayersReady) return ResultOutcome.Fail;

			var startingPlayer = _players.First ();

			var playersWithCards = _players.Where (p => p.Cards != null && p.Cards.Count > 0);

			foreach (var player in playersWithCards) {
				if (LowestCard(player.Cards).Value < LowestCard(startingPlayer.Cards).Value)
					startingPlayer = player;
			}

			_currentPlayer = _players.Find(startingPlayer);
			_gameState = GameState.GameStarted;
			return ResultOutcome.Success;
		}

		public virtual void Setup ()
		{
			foreach (IPlayer player in _players) {
				player.AddCards (this._deck.TakeCards (3, CardOrientation.FaceDown));
				player.AddCards (this._deck.TakeCards (6, CardOrientation.InHand));
			}
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


			_currentPlayer = _currentPlayer.Next != null ? _currentPlayer.Next : _players.First ;

			return ResultOutcome.Success;
		}

		private bool cardsPassRules (IPlayer player, ICollection<Card> cards)
		{
			var distinctValues = cards.Select (card => card.Value).Distinct ();

			if (distinctValues.Count() != 1)
				return false;

			if (cards.Except (player.Cards).Any ())
				return false;

			var lastCardPlayed = _playPile.Count > 0 ? _playPile.Peek () : null;
			var playersCard = cards.First ();

			if (lastCardPlayed != null) {
				if (getCardTypeFromCardValue(lastCardPlayed.Value) == CardType.Standard && playersCard.Value < lastCardPlayed.Value)
					return false;
				if (getCardTypeFromCardValue(lastCardPlayed.Value) == CardType.LowerThan && playersCard.Value > lastCardPlayed.Value)
					return false;
			}

			return true;
		}

		private CardType getCardTypeFromCardValue(CardValue cardValue){
			CardType cardType;
			_cardTypes.TryGetValue(cardValue, out cardType);
			return cardType == 0 ? CardType.Standard : cardType;
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
			get{ return _currentPlayer.Value; }
		}

		private Card LowestCard(ICollection<Card> cards){
			return cards.OrderBy (o => o.Value).FirstOrDefault (); 
		}

		private LinkedListNode<IPlayer> _currentPlayer;
		private LinkedList<IPlayer> _players;
		protected Stack<Card> _playPile;
		private Deck _deck;
		protected GameState _gameState;
		private Dictionary<CardValue, CardType> _cardTypes;
	}

}

