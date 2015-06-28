using System.Collections.Generic;

namespace Palace
{
    public interface ICanStartGame
    {
        bool IsReady(ICollection<Player> players);
    }
}

