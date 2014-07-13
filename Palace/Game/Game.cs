using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Game
	{
		private ICollection<Player> players;
		private Deck deck;

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

			player.SetState (PlayerState.Ready);

			return new Result (ResultOutcome.Success);

		}

		public Result Start ()
		{
			bool allPlayersReady = true;
			foreach (Player player in players) {
				if (player.State != PlayerState.Ready)
					allPlayersReady = false;
			}

			if(allPlayersReady)
				return new Result (ResultOutcome.Success);

			return new Result (ResultOutcome.Fail);
		}

		public int NumberOfPlayers {
			get{ return players.Count; }
		}

		public Player CurrentTurn(){
			return players.ToArray()[0];
		}
	}

}

