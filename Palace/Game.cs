using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Game
	{
		private ICollection<Player> players;

		public void Start (ICollection<Player> players)
		{
			this.players = players;
		}

		public int NumberOfPlayers {
			get{ return players.Count; }
		}

		public Player CurrentTurn(){
			return players.ToArray()[0];
		}
	}

}

