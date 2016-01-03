using System;
namespace Palace.Repository
{
    using System.Linq;

    using Raven.Client;
    using Raven.Client.Linq;

    public class GameRepository
    {
        private IPalaceDocumentSessionFactory palaceSession;

        public GameRepository(IPalaceDocumentSessionFactory palaceSession)
        {
            this.palaceSession = palaceSession;
        }

        public void Save(Game game)
        {
            game.State.DateSaved = DateTime.Now;
            using(var documentSession = palaceSession.GetDocumentSession())
            {
                documentSession.Store(game);
                documentSession.SaveChanges();
            }
        }

        public Game Open(string p)
        {
            var gameId = Guid.Parse(p);
            using (var documentSession = palaceSession.GetDocumentSession())
            {
                var game = documentSession.Load<Game>(gameId);
                return game;
            }
        }
    }
}
