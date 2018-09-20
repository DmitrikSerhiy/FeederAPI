using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feeder.DAL
{
    public class RssRepository : IRssRepository
    {
        private FeedContext feedContext;
        public RssRepository(FeedContext FeedContext)
        {
            feedContext = FeedContext;
        }

        public void AddRssToDb(Rss rss)
        {
            feedContext.Rsses.Add(rss);
        }

        public Rss GetFeed(int Id)
        {
            return feedContext.Rsses.FirstOrDefault(rss => rss.Id == Id);
        }

        public IEnumerable<Rss> GetFeeds()
        {
            return feedContext.Rsses.ToList();
        }

    }
}
