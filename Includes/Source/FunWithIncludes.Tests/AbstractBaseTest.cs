using System;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Hosting;
using FunWithIncludes.Tests.Services;
using FunWithIncludes.Tests.Services.Indexes;
using Raven.Client;
using Raven.Client.Client;
using Raven.Client.Document;
using Raven.Storage.Esent;
using Rhino.Mocks;
using TransactionalStorage = Raven.Storage.Managed.TransactionalStorage;

namespace FunWithIncludes.Tests
{
   

    public abstract class AbstractBaseTest
    {
        public static void CreateHttpContext()
        {
            if (HttpContext.Current != null)
                return;

            TextWriter tw = new StringWriter();
            HttpWorkerRequest wr = new SimpleWorkerRequest("/webappt", "c:\\inetpub\\wwwroot\\webapp\\", "default.aspx",
                                                           "", tw);
            HttpContext.Current = new HttpContext(wr);
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("testuser"), null);
        }

        public TableColumnsCache DummyMethod()
        {
            return null;
        }

        public TransactionalStorage DummyMethod2()
        {
            return null;
        }

        


        public void InitEmeddedDocumentStore(string dataDirectory)
        {
            var mockRavenDBConfigurationInstance = MockRepository.GenerateStub<RavenDBConfiguration>();

            Func<IDocumentStore> setDocumentStore =
                () =>
                    {
                        var documentStore = new EmbeddableDocumentStore
                                                {
                                                    DataDirectory = dataDirectory
                                                };

                        return documentStore;
                    };


            mockRavenDBConfigurationInstance.Stub(x => x.GetDocumentStore()).Do(setDocumentStore);
            RavenDBConfiguration.Instance(mockRavenDBConfigurationInstance).Init();

            RavenDBConfiguration.Instance().CreateIndex(typeof (Package_CurrentInventory).Assembly);
        }

        

        public void InitEmeddedInMemoryDocumentStore()
        {
            var mockRavenDBConfigurationInstance = MockRepository.GenerateStub<RavenDBConfiguration>();

            Func<IDocumentStore> setDocumentStore =
                () =>
                    {
                        var documentStore = new EmbeddableDocumentStore
                                                {
                                                    RunInMemory = true
                                                };
                        return documentStore;
                    };


            mockRavenDBConfigurationInstance.Stub(x => x.GetDocumentStore()).Do(setDocumentStore);
            RavenDBConfiguration.Instance(mockRavenDBConfigurationInstance).Init();
            //if you don't want to use the embedded version then comment out the line above and uncomment out the 
            //line below
            //RavenDBConfiguration.Instance().Init();


            RavenDBConfiguration.Instance().CreateIndex(typeof (Package_CurrentInventory).Assembly);
        }
    }
}