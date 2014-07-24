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

		public void SetupGameForTest(ICollection<Player> players)
		{
			this.players = players;
		}

		public Result Start ()
		{
			bool allPlayersReady = players.All(player => player.State == PlayerState.Ready);

			if(!allPlayersReady) return new Result (ResultOutcome.Fail);

			var startingPlayer = players.First ();
			int startingPlayerLowestValue = startingPlayer.Cards.OrderBy (o => o.Value).Select (s => s.Value).First ();

			foreach (Player player in players) {
				int currentPlayerLowestValue = player.Cards.OrderBy (o => o.Value).Select (s => s.Value).First ();

				if (currentPlayerLowestValue < startingPlayerLowestValue) {
					startingPlayer = player;
					startingPlayerLowestValue = currentPlayerLowestValue;
				}
			}

			currentPlayer = startingPlayer;
			return new Result (ResultOutcome.Success);
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

