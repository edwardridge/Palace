using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace
{
    public class Result
    {
        private List<string> _errorMessages;

        public List<string> Errors { get { return _errorMessages; } }

        public GameStatusForPlayer GameStatusForPlayer { get; }

        public Result(Player player, GameState gameState) : this(player, gameState, new List<string>())
        {
        }

        public Result(Player player, GameState gameState, string error) : this (player, gameState, new List<string>(new[] { error }))
        {
        }

        private Result(Player player, GameState gameState, List<string> errorList)
        {
            this._errorMessages = errorList;
            this.GameStatusForPlayer = SetupGameStatusForPlayer(player, gameState);
        }

        private GameStatusForPlayer SetupGameStatusForPlayer(Player player, GameState gameState)
        {
            var otherPlayers = gameState
                                            .Players?
                                            .Where(w => w.Name != player.Name)
                                            .Select(s => new GameStatusForOpponent
                                            {
                                                Name = s.Name,
                                                CardsInHandNum = s.CardsInHand.Count(),
                                                CardsFaceDownNum = s.CardsFaceDown.Count(),
                                                CardsFaceUp = s.CardsFaceUp
                                            });

            return new GameStatusForPlayer
            {
                Name = player.Name,
                CardsInHand = player.CardsInHand.OrderBy(o => o.Value),
                CardsFaceDownNum = player.CardsFaceDown.Count(),
                CardsFaceUp = player.CardsFaceUp,
                CardsInDeckNum = gameState?.Deck?.Cards?.Count() ?? 0,
                GameStatusForOpponents = otherPlayers,
                PlayPile = gameState?.PlayPile?.ToArray(),
                CurrentPlayer = gameState?.CurrentPlayerName,
                NumberOfValidMoves = gameState?.NumberOfValdMoves ?? 0
            };
        }

        public void AddErrorMessage(string message)
        {
            this._errorMessages.Add(message);
        }

        public virtual ResultOutcome ResultOutcome
        {
            get
            {
                return this._errorMessages.Any() ? ResultOutcome.Fail : ResultOutcome.Success;
            }
        }
    }

    public class GameStatusForPlayer
    {
        public int CardsFaceDownNum { get; set; }
        public int CardsInDeckNum { get; set; }
        public IEnumerable<Card> CardsInHand { get; set; }
        public IEnumerable<Card> CardsFaceUp { get; set; }
        public string Name { get; set; }
        public IEnumerable<GameStatusForOpponent> GameStatusForOpponents { get; set; }
        public IEnumerable<Card> PlayPile { get; set; }
        public string CurrentPlayer { get; set; }
        public int NumberOfValidMoves { get; set; }
    }

    public class GameStatusForOpponent
    {
        public string Name { get; set; }
        public int CardsInHandNum { get; set; }
        public int CardsFaceDownNum { get; set; }
        public IEnumerable<Card> CardsFaceUp { get; set; }
    }
}
