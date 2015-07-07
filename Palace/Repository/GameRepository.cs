using System;
namespace Palace.Repository
{
    using System.Linq;

    using Raven.Client;
    using Raven.Client.Linq;

    public class GameRepository
    {
        private IDocumentSession documentSession;

        public GameRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public void Save(Game game)
        {
            game.State.DateSaved = DateTime.Now;
            documentSession.Store(game.State);
            documentSession.Store(game.RulesProcessesor);
            documentSession.SaveChanges();
        }

        public Game Open(string p)
        {
            var gameId = Guid.Parse(p);
            var gameStateList = documentSession.Query<GameState>().ToList();
            var gameState = documentSession.Query<GameState>().Where(state => state.GameId == gameId).OrderByDescending(o => o.DateSaved).First();

            var rules = documentSession.Query<RulesProcessesor>().First(rp => rp.GameId == gameId);

            return new Game(gameState, rules);
        }
    }
}
