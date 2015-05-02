using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Game
	{
        internal Game(IEnumerable<IPlayer> players, IRulesValidator rulesValidator, ICardDealer cardDealer)
            : this(players, rulesValidator, cardDealer, new List<Card>())
        {

		}

        internal Game(IEnumerable<IPlayer> players, IRulesValidator rulesValidator, ICardDealer cardDealer, IEnumerable<Card> cardsInPile)
        {
            this.rulesValidator = rulesValidator;
            this._players = new LinkedList<IPlayer>(players);
            this._playPile = new Stack<Card>(cardsInPile);
            this._cardDealer = cardDealer;

            _currentPlayerNode = _players.First;
        }

		public ResultOutcome PlayCards (IPlayer player, ICollection<Card> cards)
		{
		    if (cards.Except(player.Cards).Any())
                throw new ArgumentException("You cannot play cards you don't have!");

            if (cards.Select(card => card.Value).Distinct().Count() != 1)
                throw new ArgumentException("You cannot play more than one type of card");

            if (!this.rulesValidator.CardsPassRules(cards, _playPile.Any() ? _playPile.Peek() : null)) 
				return ResultOutcome.Fail;
				
			player.RemoveCards (cards);
			foreach (Card card in cards) {
				_playPile.Push (card);
			}

            player.AddCards(_cardDealer.DealCards(cards.Count));

			_currentPlayerNode = _currentPlayerNode.Next ?? _players.First ;

			return ResultOutcome.Success;
		}

		public ResultOutcome PlayCards(IPlayer player, Card card){
			return PlayCards (player, new[]{ card });
		}

		public int PlayPileCardCount{
		    get { return _playPile.Count; }
		}

		public int NumberOfPlayers {
			get{ return _players.Count; }
		}

		public IPlayer CurrentPlayer {
			get{ return _currentPlayerNode.Value; }
		}

        internal void Start(IPlayer startingPlayer)
        {
            _currentPlayerNode = _players.Find(startingPlayer);
        }
			
		private LinkedListNode<IPlayer> _currentPlayerNode;
		private LinkedList<IPlayer> _players;
		private Stack<Card> _playPile;

	    private IRulesValidator rulesValidator;
        private ICardDealer _cardDealer;
	}

}

