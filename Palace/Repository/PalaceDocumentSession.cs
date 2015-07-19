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

    public class PalaceDocumentSession
    {
        DocumentStore documentStore;
        public PalaceDocumentSession()
        {
            documentStore = new DocumentStore()
            {
                Url = "http://localhost:8080"
            };
            documentStore.Initialize();
        }
        public virtual IDocumentSession GetDocumentSession()
        {
            return documentStore.OpenSession("Palace");
        }
    }
}
