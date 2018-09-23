using Feeder.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IFeedRepository feedRepository;
        private readonly ISourceRepository sourceRepository;
        private readonly ICollectionRepository collectionRepository;

        public UnitOfWork(IFeedRepository FeedRepository, ISourceRepository SourceRepository, ICollectionRepository CollectionRepository)
        {
            feedRepository = FeedRepository;
            sourceRepository = SourceRepository;
            collectionRepository = CollectionRepository;
        }

        public IFeedRepository FeedRepository => feedRepository;

        public ISourceRepository SourceRepository => sourceRepository;

        public ICollectionRepository CollectionRepository => collectionRepository;

    }
}
