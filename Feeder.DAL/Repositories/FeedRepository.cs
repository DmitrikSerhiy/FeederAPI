using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feeder.DAL.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private FeedContext feedContext;
        public FeedRepository(FeedContext FeedContext)
        {
            feedContext = FeedContext;
        }

        public IEnumerable<Feed> GetFeeds()
        {
            return feedContext.Feeds.ToList();
        }

        public Feed GetFeed(string title, string publishDate)
        {
            return feedContext.Feeds.FirstOrDefault(f => f.Title == title && f.PublishDate == publishDate);
        }

        public Feed GetFeed(int Id)
        {
            return feedContext.Feeds.FirstOrDefault(f => f.Id == Id);
        }

        public void Save()
        {
            feedContext.SaveChanges();
        }

        public void Dispose()
        {
            feedContext.Dispose();
        }

        public bool IsExist(string title, string publishDate)
        {
            return feedContext.Feeds.Any(f => f.Title == title && f.PublishDate == publishDate);
        }
    }
}
