using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IFeedRepository feedRepository { get; }
        ISourceRepository sourceRepository { get; }
    }
}
