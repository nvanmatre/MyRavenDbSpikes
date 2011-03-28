using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using log4net;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace FunWithIncludes.Tests.Services
{
    public class RavenDBConfiguration
    {
        private static readonly object SyncLock = new object();
        private static RavenDBConfiguration instance;
        private static ILog log;

        private IDocumentStore documentStore;

        protected RavenDBConfiguration()
        {
            if (log == null)
            {
                log = LogManager.GetLogger(typeof (RavenDBConfiguration));
                log.Debug("Logger created for RavenDBConfiguration");
            }
        }

        public void CreateIndex(Assembly assemblyOfIndex)
        {
            lock (SyncLock)
            {
                if (documentStore == null)
                    CreateDocumentStore();

                IndexCreation.CreateIndexes(assemblyOfIndex, documentStore);
            }
        }

        public virtual IDocumentStore GetDocumentStore()
        {
            if (ConfigurationManager.ConnectionStrings["RavenDB"] == null ||
                string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["RavenDB"].ConnectionString))
                throw new Exception("RavenDB connection string missing");

            string connectionString = ConfigurationManager.ConnectionStrings["RavenDB"].ConnectionString;


            ConnectionStringSettings ravenDBTenant = ConfigurationManager.ConnectionStrings["RavenDBTenant"];
            string database = string.Empty;
            if (ravenDBTenant != null)
            {
                log.DebugFormat("Tenant:{0} found", database);
                database = ravenDBTenant.ConnectionString;
            }

            return new DocumentStore {Url = connectionString, DefaultDatabase = database};
        }

        public void Init()
        {
            lock (SyncLock)
            {
                try
                {
                    CreateDocumentStore();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creation sessionFactory", ex);
                }
            }
        }

        public static RavenDBConfiguration Instance()
        {
            lock (SyncLock)
            {
                if (instance == null)
                {
                    instance = new RavenDBConfiguration();
                }
            }
            return instance;
        }

        public static RavenDBConfiguration Instance(RavenDBConfiguration ravenInstance)
        {
            lock (SyncLock)
            {
                instance = ravenInstance;
            }
            return instance;
        }

        public IDocumentSession OpenNewSession()
        {
            lock (SyncLock)
            {
                log.DebugFormat("Open new session from session factory for user: {0}",
                                HttpContext.Current.User.Identity.Name);

                try
                {
                    if (documentStore == null)
                        CreateDocumentStore();

                    ConnectionStringSettings ravenDBTenant = ConfigurationManager.ConnectionStrings["RavenDBTenant"];
                    if (ravenDBTenant != null)
                    {
                        string database = ravenDBTenant.ConnectionString;
                        log.DebugFormat("Tenant:{0} found", database);

                        //   documentStore.DatabaseCommands.EnsureDatabaseExists(database);
                        return documentStore.OpenSession(database);
                    }

                    return documentStore.OpenSession();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating session", ex);
                }
            }
        }

        private void CreateDocumentStore()
        {
            documentStore = GetDocumentStore();

            InitDocumentStore(documentStore);
        }

        private void InitDocumentStore(IDocumentStore docStore)
        {
            docStore.Initialize();

            CreateIndex(typeof (RavenDBConfiguration).Assembly);
        }
    }
}