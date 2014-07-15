using System;
using Palace;
using System.Linq;

namespace UnitTests
{
	public static class PlayerHelper
	{
		public static void PutSomeCardsFaceUp (Player player, int count)
		{
			var cards = player.Cards;

			for (var i = 0; i < count; i++) {
				player.PutCardFaceUp (cards.ToArray () [i]);
			}
		}
	}
}

