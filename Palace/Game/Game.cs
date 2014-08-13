using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Game
	{
		public void Setup (ICollection<Player> players, Deck deck)
		{
			this.deck = deck;
			this.players = players;

			foreach (Player player in players) {
				player.AddCards (this.deck.TakeCards (3, CardOrientation.FaceDown));
				player.AddCards (this.deck.TakeCards (6, CardOrientation.InHand));
			}
		}

		public void ReplacePlayers(ICollection<Player> players)
		{
			this.players = players;
		}

		public Result Start ()
		{
			bool allPlayersReady = players.All(player => player.State == PlayerState.Ready);

			if(!allPlayersReady) return new Result (ResultOutcome.Fail);

			var startingPlayer = players.First ();

			foreach (Player player in players) {
				if (player.LowestCard.Value < startingPlayer.LowestCard.Value)
					startingPlayer = player;
			}

			currentPlayer = startingPlayer;
			return new Result (ResultOutcome.Success);
		}

		public ResultOutcome PlayCards (Player player, Card card)
		{
			return ResultOutcome.Fail;
		}

		public int NumberOfPlayers {
			get{ return players.Count; }
		}

		public Player CurrentPlayer {
			get{ return currentPlayer; }
		}

		private Player currentPlayer;
		private ICollection<Player> players;
		private Deck deck;
	}

}

