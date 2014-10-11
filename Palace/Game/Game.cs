using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Game
	{
		public Game(ICollection<IPlayer> players, Deck deck)
			: this(players, deck, GameState.GameInSetup, new List<Card>(), new Dictionary<CardValue, CardType> ()){

		}

		public Game(ICollection<IPlayer> players, Deck deck, GameState gameState)
			: this(players, deck, gameState, new List<Card>(), new Dictionary<CardValue, CardType> ()){

		}

		public Game (ICollection<IPlayer> players, Deck deck, GameState gameState, ICollection<Card> playPile)
			: this (players, deck, gameState, playPile, new Dictionary<CardValue, CardType> ()){
		}

		public Game(ICollection<IPlayer> players, Deck deck, GameState gameState, ICollection<Card> playPile, Dictionary<CardValue, CardType> cardTypes){
			this._deck = deck;
			this._players = players;
			this._gameState = gameState;
			this._playPile = playPile;
			this._cardTypes = cardTypes;

			_currentPlayer = players.First ();
		}

		public ResultOutcome Start ()
		{
			bool allPlayersReady = _players.All(player => player.State == PlayerState.Ready);

			if(!allPlayersReady) return ResultOutcome.Fail;

			var startingPlayer = _players.First ();

			var playersWithCards = _players.Where (p => p.Cards != null && p.Cards.Count > 0);

			foreach (var player in playersWithCards) {
				if (LowestCard(player.Cards).Value < LowestCard(startingPlayer.Cards).Value)
					startingPlayer = player;
			}

			_currentPlayer = startingPlayer;
			_gameState = GameState.GameStarted;
			return ResultOutcome.Success;
		}

		public void Setup ()
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
				_playPile.Add (card);
			}

			return ResultOutcome.Success;
		}

		private bool cardsPassRules (IPlayer player, ICollection<Card> cards)
		{
			var distinctValues = cards.Select (card => card.Value).Distinct ();

			if (distinctValues.Count() != 1)
				return false;

			if (cards.Except (player.Cards).Any ())
				return false;

			var lastCardPlayed = _playPile.LastOrDefault ();
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
			get{ return _currentPlayer; }
		}

		private Card LowestCard(ICollection<Card> cards){
			return cards.OrderBy (o => o.Value).FirstOrDefault (); 
		}

		private IPlayer _currentPlayer;
		private ICollection<IPlayer> _players;
		private ICollection<Card> _playPile;
		private Deck _deck;
		private GameState _gameState;
		private Dictionary<CardValue, CardType> _cardTypes;
	}

}

