using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class GameStartValidator : IGameStartValidator{

		public bool GameIsReadyToStart (ICollection<IPlayer> players)
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
			if(!_gameStartValidator.GameIsReadyToStart(players))
                throw new InvalidOperationException();

            var game = new Game(players, this);

            var startingPlayer = players.First();

            foreach (var player in players)
            {
                if (player.Cards == null || player.Cards.Count == 0)
                    continue;
                if (player.LowestCardInValue.Value < startingPlayer.LowestCardInValue.Value)
                    startingPlayer = player;
            }
		    
            game.Start(startingPlayer);
		    return game;
		}

        public Game ResumeGame(ICollection<IPlayer> players, IPlayer startingPlayer)
        {
            return ResumeGame(players, startingPlayer, new List<Card>());
        }

		public Game ResumeGame(ICollection<IPlayer> players, IPlayer startingPlayer, IEnumerable<Card> cardsInPile){
            var game = new Game(players, this);
		    game.Start(startingPlayer);
            return game;
		}

        public RuleForCard GetRuleForCardFromCardValue(CardValue cardValue)
        {
            RuleForCard ruleForCard;
            _rulesForCardsByValue.TryGetValue(cardValue, out ruleForCard);
            return ruleForCard == 0 ? RuleForCard.Standard : ruleForCard;
        }

		private Deck _deck;
		private Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;
		private IGameStartValidator _gameStartValidator;
	}

	public class Game
	{
        internal Game(ICollection<IPlayer> players, Dealer dealer)
			: this(players, dealer, new List<Card>()){

		}

        internal Game(ICollection<IPlayer> players, Dealer dealer, IEnumerable<Card> cardsInPile)
        {
            this._dealer = dealer;
            this._players = new LinkedList<IPlayer>(players);
            this._playPile = new Stack<Card>(cardsInPile);

            _currentPlayerNode = _players.First;
        }

		public void Start(IPlayer startingPlayer){
			bool allPlayersReady = _players.All(player => player.State == PlayerState.Ready);

			if(!allPlayersReady) throw new ArgumentException("Not all players are ready");

			_currentPlayerNode = _players.Find (startingPlayer);
		}

		public ResultOutcome PlayCards (IPlayer player, ICollection<Card> cards)
		{
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
		    var ruleForCard = _dealer.GetRuleForCardFromCardValue(playersCard.Value);

			if (ruleForCard == RuleForCard.Standard && playersCard.Value < lastCardPlayed.Value)
				return false;
			if (ruleForCard == RuleForCard.LowerThan && playersCard.Value > lastCardPlayed.Value)
				return false;

			return true;
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
			
		private LinkedListNode<IPlayer> _currentPlayerNode;
		private LinkedList<IPlayer> _players;
		protected Stack<Card> _playPile;

	    private Dealer _dealer;
	}

}

