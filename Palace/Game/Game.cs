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
				player.AddCards (this.deck.TakeCards (6, CardOrientation.InHand));
				player.AddCards (this.deck.TakeCards (3, CardOrientation.FaceDown));
			}
		}

		public Result Start ()
		{
			bool allPlayersReady = players.All(player => player.State == PlayerState.Ready);

			if(allPlayersReady) return new Result (ResultOutcome.Success);

			return new Result (ResultOutcome.Fail);
		}

		public int NumberOfPlayers {
			get{ return players.Count; }
		}

		private ICollection<Player> players;
		private Deck deck;
	}

}

