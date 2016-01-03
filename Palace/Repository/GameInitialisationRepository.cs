namespace Palace.Repository
{
    public class GameInitialisationRepository
    {
        IPalaceDocumentSessionFactory documentSession;

        public GameInitialisationRepository(IPalaceDocumentSessionFactory documentSession)
        {
            this.documentSession = documentSession;
        }

        public void Save(GameInitialisation gameInitialisation)
        {
            using(var session = documentSession.GetDocumentSession())
            {
                session.Store(gameInitialisation);
            }
        }

        public GameInitialisation Load(string id)
        {
            using (var session = documentSession.GetDocumentSession())
            {
                return session.Load<GameInitialisation>(id);
            }
        }
    }
}
