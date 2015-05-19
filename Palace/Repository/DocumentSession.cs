using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Repository
{
    using Raven.Client;
    using Raven.Client.Document;

    public static class DocumentSession
    {
        public static IDocumentSession GetDocumentSession()
        {
            var documentStore = new DocumentStore()
            {
                Url = "http://localhost:8080"
            };
            documentStore.Initialize();
            return documentStore.OpenSession("Palace");
        }
    }
}
