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

    public class PalaceDocumentSession : IPalaceDocumentSessionFactory
    {
        IDocumentStore documentStore;
        public PalaceDocumentSession(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
            documentStore.Initialize();
        }

        public IDocumentSession GetDocumentSession()
        {
            return documentStore.OpenSession("Palace");
        }
    }
}
