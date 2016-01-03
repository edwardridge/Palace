using Palace.Repository;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Graph;

namespace Palace.Api.DependencyInjection
{
    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
            For<IDocumentStore>().Use(CreateDocumentStore()).Singleton();
            For<IPalaceDocumentSessionFactory>().Use<PalaceDocumentSessionFactory>().Ctor<string>("database").Is("Palace");
        }

        private DocumentStore CreateDocumentStore()
        {
            var documentStore = new DocumentStore();
            documentStore.ConnectionStringName = "Server";
            return documentStore;
        }

        #endregion
    }
}