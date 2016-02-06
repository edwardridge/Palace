using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Repository
{
    using System.Runtime.InteropServices;

    using Raven.Client;
    using Raven.Client.Document;

    public interface IPalaceDocumentSessionFactory
    {
        IDocumentSession GetDocumentSession();
    }

    public class PalaceDocumentSessionFactory : IPalaceDocumentSessionFactory
    {
        private IDocumentStore documentStore;
        private string database;

        public PalaceDocumentSessionFactory(IDocumentStore documentStore, string database)
        {
            this.documentStore = documentStore;
            this.database = database;
            documentStore.Initialize();
        }

        public IDocumentSession GetDocumentSession()
        {
            return documentStore.OpenSession(database);
        }
    }
}
