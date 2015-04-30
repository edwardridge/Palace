using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
	public interface IGameStartValidator
	{
		bool GameIsReadyToStart(ICollection<IPlayer> players);
	}

    public class GameStartValidator : IGameStartValidator
    {
        public bool GameIsReadyToStart(ICollection<IPlayer> players)
        {
            var returnVal = !(players.Any(p => p.Cards.Count(c => c.CardOrientation == CardOrientation.FaceUp) != 3));
            return returnVal;
        }
    }
}

