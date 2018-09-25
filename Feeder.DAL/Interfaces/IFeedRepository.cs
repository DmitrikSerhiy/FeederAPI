using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface IFeedRepository : IDisposable
    {
        IEnumerable<Feed> GetFeeds();
        Feed GetFeed(int Id);
        Feed GetFeed(string title, string publishDate);
        bool IsExist(string title, string publishDate);
        void Save();

    }
}
