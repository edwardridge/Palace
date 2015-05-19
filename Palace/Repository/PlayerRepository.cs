using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Repository
{
    using Raven.Client;
    using Raven.Client.Document;

    public class PlayerRepository
    {
        private readonly IDocumentSession documentSession;
        public PlayerRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public void Save(Player player)
        {
            documentSession.Store(player);
            documentSession.SaveChanges();
        }

        public Player Load(int index)
        {
            return documentSession
                //.Include<Player>(p => p.CardsInHand)
                //.Include<Player>(p => p.CardsFaceUp)
                //.Include<Player>(p => p.CardsFaceDown)
                //.Include<Player>(p=>p.LowestCardInValue)
                .Load<Player>("players/" + index);
        }
    }
}
