using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public interface ICanStartGame
	{
		bool GameIsReadyToStart(ICollection<Player> players);
	}

    public class CanStartGame : ICanStartGame
    {
        public bool GameIsReadyToStart(ICollection<Player> players)
        {
            var playersHaveThreeCardsFaceUp = !(players.Any(p => p.NumCardsFaceUp != 3));
            //return true;
            return playersHaveThreeCardsFaceUp;
        }
    }
}

