using System;
namespace Palace.Repository
{
    using Raven.Client;

    public class GameRepository
    {
        private IDocumentSession documentSession;

        public GameRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public void Save(Game game)
        {
            documentSession.Store(game);
            documentSession.SaveChanges();
        }

        public Game Open(string p)
        {
            var game = documentSession
                .Load<Game>("games/" + p);
            return game;
        }
    }
}
