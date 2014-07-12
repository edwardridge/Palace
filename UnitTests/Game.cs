using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests
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
	}

}

