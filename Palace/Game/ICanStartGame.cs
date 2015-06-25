using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public interface ICanStartGame
	{
		bool IsReady(ICollection<Player> players);
	}

    public class DefaultStartGameRules : ICanStartGame
    {
        public bool IsReady(ICollection<Player> players)
        {
            var playersHaveThreeCardsFaceUp = !(players.Any(p => p.CardsFaceUp.Count != 3));
            //return true;
            return playersHaveThreeCardsFaceUp;
        }
    }
}

