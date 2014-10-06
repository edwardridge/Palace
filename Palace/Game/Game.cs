using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Game
	{
		public Game(ICollection<IPlayer> players, Deck deck)
			: this(players, deck, GameState.GameInSetup, new List<Card>()){

		}

		public Game(ICollection<IPlayer> players, Deck deck, GameState gameState)
			: this(players, deck, gameState, new List<Card>()){

		}

		public Game(ICollection<IPlayer> players, Deck deck, GameState gameState, ICollection<Card> playPile){
			this.deck = deck;
			this.players = players;
			this.gameState = gameState;
			this.playPile = playPile;

			currentPlayer = players.First ();
		}

		public Result Start ()
		{
			bool allPlayersReady = players.All(player => player.State == PlayerState.Ready);

			if(!allPlayersReady) return new Result (ResultOutcome.Fail);

			var startingPlayer = players.First ();

			var playersWithCards = players.Where (p => p.Cards != null && p.Cards.Count > 0);

			foreach (var player in playersWithCards) {
				if (LowestCard(player.Cards).Value < LowestCard(startingPlayer.Cards).Value)
					startingPlayer = player;
			}

			currentPlayer = startingPlayer;
			gameState = GameState.GameStarted;
			return new Result (ResultOutcome.Success);
		}

		public void Setup ()
		{
			foreach (IPlayer player in players) {
				player.AddCards (this.deck.TakeCards (3, CardOrientation.FaceDown));
				player.AddCards (this.deck.TakeCards (6, CardOrientation.InHand));
			}
		}

		public ResultOutcome PlayCards (IPlayer player, ICollection<Card> cards)
		{
			if (gameState != GameState.GameStarted)
				return ResultOutcome.Fail;

			var distinctValues = cards.Select (card => card.Value).Distinct ();

			if (distinctValues.Count() != 1)
				return ResultOutcome.Fail;

			if (cards.Except (player.Cards).Any ())
				return ResultOutcome.Fail;

			var lastCardPlayed = playPile.LastOrDefault ();

			if (lastCardPlayed != null && distinctValues.First () < lastCardPlayed.Value)
				return ResultOutcome.Fail;

			player.RemoveCards (cards);
			foreach (Card card in cards) {
				playPile.Add (card);
			}

			return ResultOutcome.Success;
		}

		public ResultOutcome PlayCards(IPlayer player, Card card){
			return PlayCards (player, new[]{ card });
		}

		public int PlayPileCardCount(){
			return playPile.Count;
		}

		public int NumberOfPlayers {
			get{ return players.Count; }
		}

		public IPlayer CurrentPlayer {
			get{ return currentPlayer; }
		}

		private Card LowestCard(ICollection<Card> cards){
			return cards.OrderBy (o => o.Value).FirstOrDefault (); 
		}

		private IPlayer currentPlayer;
		private ICollection<IPlayer> players;
		private ICollection<Card> playPile;
		private Deck deck;
		private GameState gameState;
	}

}

