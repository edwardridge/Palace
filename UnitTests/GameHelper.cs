using System;
using Palace;
using System.Collections.Generic;

namespace UnitTests
{
	public static class GameHelper
	{
		public static Game CreateGameWithPlayers(IList<Player> players){
			var game = new Game ();

			var randomiser = new NonShuffler ();
			var deck = new Deck (randomiser);

			game.Setup (players, deck);

			return game;
		}

		public static void StartGameForTest(Game game, IList<Player> players, IList<IList<int>> cards){
			for (int i = 0; i < players.Count; i++) {
				if (i < cards.Count)
					players [i].ReplaceCardsAndSetReady (CardHelpers.GetCardsFromValues (cards [i]));
				else 
					players [i].ReplaceCardsAndSetReady (CardHelpers.GetCardsFromValues (new[]{ 15}));
			}

			game.ReplacePlayers (players);

			game.Start ();
		}
	}
}

