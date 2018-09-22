using Feeder.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IFeedRepository FeedRepository;
        private readonly ISourceRepository SourceRepository;

        public UnitOfWork(IFeedRepository feedRepository, ISourceRepository sourceRepository)
        {
            FeedRepository = feedRepository;
            SourceRepository = sourceRepository;
        }

        public IFeedRepository feedRepository => FeedRepository;

        public ISourceRepository sourceRepository => SourceRepository;

    }
}
