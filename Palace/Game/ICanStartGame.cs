using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public interface ICanStartGame
	{
		bool GameIsReadyToStart(ICollection<IPlayer> players);
	}

    public class CanStartGame : ICanStartGame
    {
        public bool GameIsReadyToStart(ICollection<IPlayer> players)
        {
            var playersHaveThreeCardsFaceUp = !(players.Any(p => p.Cards.Count(c => c.CardOrientation == CardOrientation.FaceUp) != 3));

            return playersHaveThreeCardsFaceUp;
        }
    }
}

