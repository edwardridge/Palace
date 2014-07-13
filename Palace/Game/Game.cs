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
				player.AddCards (this.deck.GetCards (6, CardOrientation.InHand));
				player.AddCards (this.deck.GetCards (3, CardOrientation.FaceDown));
			}
		}

		public Result PlayerReady (Player player)
		{
			if (player.NumCards (CardOrientation.FaceUp) != 3)
				return new Result (ResultOutcome.Fail);

			player.State = PlayerState.Ready;

			return new Result (ResultOutcome.Success);
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

