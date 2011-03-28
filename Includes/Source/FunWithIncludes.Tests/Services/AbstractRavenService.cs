using System;
using System.Web;
using log4net;
using Raven.Client;

namespace FunWithIncludes.Tests.Services
{
    public abstract class AbstractRavenService
    {
        protected static ILog log;

        protected AbstractRavenService()
        {
            log = LogManager.GetLogger(GetType());
        }

        public static IDocumentSession GetCurrentDocumentSession()
        {
            if (HttpContext.Current == null)
            {
                throw new Exception("No HttpContext found");
            }

            var session = HttpContext.Current.Items["RavenDBSession"] as IDocumentSession;

            if (session == null)
            {
                try
                {
                    session = RavenDBConfiguration.Instance().OpenNewSession();
                }
                catch (Exception ex)
                {
                    log.Warn("First attempt to open raven session failed.  Will give it one more try", ex);
                    //NOTE:there is an IIS 7.5 problem with webdev extensions that I am still trying to figure out.
                    //on the first hit of the site after it has been shut down IIS will gie us a 405- method not allowed error
                    try
                    {
                        session = RavenDBConfiguration.Instance().OpenNewSession();
                    }
                    catch (Exception secondEx)
                    {
                        log.Error(secondEx);
                    }
                }

                if (session != null)
                {
                    HttpContext.Current.Items["RavenDBSession"] = session;
                }
            }
            return session;
        }
    }
}