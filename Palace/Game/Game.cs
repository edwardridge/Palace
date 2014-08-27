using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public class Game
	{
		public Game(ICollection<IPlayer> players, Deck deck){
			this.deck = deck;
			this.players = players;

			currentPlayer = players.First ();
		}

		public Result Start ()
		{
			bool allPlayersReady = players.All(player => player.State == PlayerState.Ready);

			if(!allPlayersReady) return new Result (ResultOutcome.Fail);

			var startingPlayer = players.First ();

			foreach (var player in players) {
				if (LowestCard(player.Cards).Value < LowestCard(startingPlayer.Cards).Value)
					startingPlayer = player;
			}

			currentPlayer = startingPlayer;
			return new Result (ResultOutcome.Success);
		}

		public void Setup ()
		{
			foreach (Player player in players) {
				player.AddCards (this.deck.TakeCards (3, CardOrientation.FaceDown));
				player.AddCards (this.deck.TakeCards (6, CardOrientation.InHand));
			}
		}

		public ResultOutcome PlayCards (IPlayer player, Card card)
		{
			player.RemoveCards (new []{card});
			return ResultOutcome.Fail;
		}

		public int NumberOfPlayers {
			get{ return players.Count; }
		}

		public IPlayer CurrentPlayer {
			get{ return currentPlayer; }
		}

		private Card LowestCard(ICollection<Card> cards){
			return cards.OrderBy (o => o.Value).First (); 
		}

		private IPlayer currentPlayer;
		private ICollection<IPlayer> players;
		private Deck deck;
	}

}

