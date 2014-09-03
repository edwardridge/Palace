using System;
using Palace;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public static class GameHelper
	{

		public static void StartGameForTest(this Game game, ICollection<IPlayer> players, IList<IList<int>> cards){
			game.Setup ();

			for (int i = 0; i < players.Count; i++) {
				if (i < cards.Count)
					players.ToArray() [i].AddCards (CardHelpers.GetCardsFromValues (cards [i]));
				else 
					players.ToArray() [i].AddCards (CardHelpers.GetCardsFromValues (new[]{ 15}));
			}

			game.Start ();
		}
	}
}

