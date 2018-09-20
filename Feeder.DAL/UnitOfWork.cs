using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private FeedContext feedContext;
        private IRssRepository rssRepository;

        public UnitOfWork(FeedContext FeedContext, IRssRepository IRssRepository)
        {
            feedContext = FeedContext;
            rssRepository = IRssRepository;
        }

        public IRssRepository Rsses => rssRepository == null ? new RssRepository(feedContext) : rssRepository;

        public void Save()
        {
            feedContext.SaveChanges();
        }
        public void Dispose()
        {
            feedContext.Dispose();
        }
    }
}
