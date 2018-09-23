using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IFeedRepository FeedRepository { get; }
        ISourceRepository SourceRepository { get; }
        ICollectionRepository CollectionRepository { get; }
    }
}
