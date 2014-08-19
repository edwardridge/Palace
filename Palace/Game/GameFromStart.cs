using System;
using System.Linq;
using System.Collections.Generic;

namespace Palace
{
	public class GameFromStart : Game
	{
		public override Result Start ()
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

		public override void Setup (ICollection<Player> players, Deck deck)
		{
			this.deck = deck;
			this.players = players;

			foreach (Player player in players) {
				player.AddCards (this.deck.TakeCards (3, CardOrientation.FaceDown));
				player.AddCards (this.deck.TakeCards (6, CardOrientation.InHand));
			}
		}
	}
}

