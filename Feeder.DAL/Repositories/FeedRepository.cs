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

        public void AddFeed(Feed feed)
        {
            feedContext.Feeds.Add(feed);
        }

        public Source GetFeeds(string sourceName)
        {
            return feedContext.Sources
                .Include(s => s.Feeds)
                .FirstOrDefault(s => s.Name == sourceName);
        }

        public Feed GetFeed(string title, string publishDate)
        {
            return feedContext.Feeds.FirstOrDefault(f => f.Title == title && f.PublishDate == publishDate);
        }

        public bool IsFeedInSource(Feed feed, Source source)
        {
            return feedContext.Sources
                .Include(s => s.Feeds)
                .First(s => s.Id == source.Id)
                    .Feeds.Any(f => f.PublishDate == feed.PublishDate && f.Title == feed.Title);
        }

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
