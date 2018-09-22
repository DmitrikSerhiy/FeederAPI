using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface IFeedRepository //: IRepository<Feed>
    {
        void AddFeed(Feed feed);
        bool IsFeedInSource(Feed feed, Source Id);
        Source GetFeeds(string sourceName);
        Feed GetFeed(string title, string publishDate);
        void Save();

    }
}
