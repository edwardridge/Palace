using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace
{
    public class GameOverResult : Result
    {
        private readonly Player winner;

        public GameOverResult(Player player, Player winner) : base(player, new GameState())
        {
            this.winner = winner;
        }

        public override ResultOutcome ResultOutcome
        {
            get { return ResultOutcome.GameOver; }
        }

        public Player Winner { get { return winner; } }
    }
}
