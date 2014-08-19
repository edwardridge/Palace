using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public abstract class Game
	{
		public abstract void Setup (ICollection<Player> players, Deck deck);

		public void ReplacePlayers(ICollection<Player> players)
		{
			this.players = players;
		}

		public abstract Result Start ();

		public ResultOutcome PlayCards (Player player, Card card)
		{
			player.Cards.Remove (card);
			return ResultOutcome.Fail;
		}

		public int NumberOfPlayers {
			get{ return players.Count; }
		}

		public Player CurrentPlayer {
			get{ return currentPlayer; }
		}

		protected Player currentPlayer;
		protected ICollection<Player> players;
		protected Deck deck;
	}

}

