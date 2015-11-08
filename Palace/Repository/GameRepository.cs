using System;
namespace Palace.Repository
{
    using System.Linq;

    using Raven.Client;
    using Raven.Client.Linq;

    public class GameRepository
    {
        private PalaceDocumentSession palaceSession;

        public GameRepository(PalaceDocumentSession palaceSession)
        {
            this.palaceSession = palaceSession;
        }

        public void Save(Game game)
        {
            game.State.DateSaved = DateTime.Now;
            using(var documentSession = palaceSession.GetDocumentSession())
            {
                //documentSession.Store(game.State);
                //documentSession.Store(game.RulesProcessorGenerator);

                documentSession.Store(game);
                //documentSession.Store(game.RulesProcessorGenerator);
                documentSession.SaveChanges();
            }
        }

        public Game Open(string p)
        {
            var gameId = Guid.Parse(p);
            using (var documentSession = palaceSession.GetDocumentSession())
            {
                //var gameState = documentSession.Query<GameState>().Where(state => state.GameId == gameId)
                //  .Customize(c => c.WaitForNonStaleResults())
                //.OrderByDescending(o => o.DateSaved).First();

                //var rules = documentSession.Query<RulesProcessorGenerator>().First(rp => rp.GameId == gameId);
                var game = documentSession.Load<Game>(gameId);
                return game;
            }
        }
    }
}
