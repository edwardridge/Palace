using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Game
	{
        internal Game(IEnumerable<IPlayer> players, ICanLayCards canLayCards)
            : this(players, canLayCards, new List<Card>())
        {

		}

        internal Game(IEnumerable<IPlayer> players, ICanLayCards canLayCards, IEnumerable<Card> cardsInPile)
        {
            this.canLayCards = canLayCards;
            this._players = new LinkedList<IPlayer>(players);
            this._playPile = new Stack<Card>(cardsInPile);

            _currentPlayerNode = _players.First;
        }

		public ResultOutcome PlayCards (IPlayer player, ICollection<Card> cards)
		{
		    if (cards.Except(player.Cards).Any())
                throw new ArgumentException("You cannot play cards you don't have!");

            if (cards.Select(card => card.Value).Distinct().Count() != 1)
                throw new ArgumentException("You cannot play more than one type of card");

            if (!this.canLayCards.CardsPassRules(cards, _playPile.Any() ? _playPile.Peek() : null)) 
				return ResultOutcome.Fail;
				
			player.RemoveCards (cards);
			foreach (Card card in cards) {
				_playPile.Push (card);
			}

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
            bool allPlayersReady = _players.All(player => player.State == PlayerState.Ready);

            if (!allPlayersReady) throw new ArgumentException("Not all players are ready");

            _currentPlayerNode = _players.Find(startingPlayer);
        }
			
		private LinkedListNode<IPlayer> _currentPlayerNode;
		private LinkedList<IPlayer> _players;
		protected Stack<Card> _playPile;

	    private ICanLayCards canLayCards;
	}

}

