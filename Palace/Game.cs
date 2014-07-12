using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Game
	{
		private ICollection<Player> players;
		private Deck deck;

		public void Start (ICollection<Player> players, Deck deck)
		{
			this.deck = deck;
			this.players = players;

			foreach (Player player in players) {
				player.AddCards (this.deck.GetCards (9));
			}
		}

		public int NumberOfPlayers {
			get{ return players.Count; }
		}

		public Player CurrentTurn(){
			return players.ToArray()[0];
		}
	}

}

