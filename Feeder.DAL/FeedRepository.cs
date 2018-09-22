using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feeder.DAL
{
    public class FeedRepository : IFeedRepository
    {
        private FeedContext feedContext;
        public FeedRepository(FeedContext FeedContext)
        {
            feedContext = FeedContext;
        }

        public void AddRssToDb(Feed feed)
        {
          //  feedContext.Rsses.Add(rss);
        }

        public Feed GetFeed(int Id)
        {
            return null;// feedContext.Rsses.FirstOrDefault(rss => rss.Id == Id);
        }

        public IEnumerable<Feed> GetFeeds()
        {
            return null;//feedContext.Rsses.ToList();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
