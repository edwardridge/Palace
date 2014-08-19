using System;
using System.Collections.Generic;

namespace Palace
{
	public class GameInProgress : Game
	{
		public override void Setup (ICollection<Player> players, Deck deck)
		{
			this.players = players;
			this.deck = deck;

			foreach (Player player in players)
				player.State = PlayerState.Ready;
		}

		public override Result Start ()
		{
			return new Result (ResultOutcome.Success);
		}
	}
}

