using System.Collections.Generic;
using System.Linq;

namespace Palace.Rules
{
    public class DefaultStartGameRules : ICanStartGame
    {
        public bool IsReady(ICollection<Player> players)
        {
            var playersHaveThreeCardsFaceUp = !(players.Any(p => p.CardsFaceUp.Count != 3));
            return playersHaveThreeCardsFaceUp;
        }
    }
}
