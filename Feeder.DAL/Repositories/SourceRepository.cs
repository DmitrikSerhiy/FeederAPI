using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feeder.DAL.Repositories
{
    public class SourceRepository : ISourceRepository
    {
        private FeedContext feedContext;
        public SourceRepository(FeedContext FeedContext)
        {
            feedContext = FeedContext;
        }

        public Source AddSource(string sourceName, string Url)
        {
            return feedContext.Sources.Add(new Source { Name = sourceName, Url = Url }).Entity;
        }


        public List<Source> GetSources(bool withIncludes)
        {
            if(withIncludes) return feedContext.Sources.Include(s => s.Feeds).ToList();
            return feedContext.Sources.ToList();
        }

        public void AddFeed(Feed feed)
        {
            feedContext.Feeds.Add(feed);
        }

        public bool IsFeedInSource(Feed feed, Source source)
        {
            return feedContext.Sources.Include(s => s.Feeds).Any(s => s.Feeds.Contains(feed));
        }

        public bool IsExist(int sourceId)
        {
            return feedContext.Sources.Any(s => s.Id == sourceId);
        }
        public bool IsExist(string sourceName, string url)
        {
            return feedContext.Sources.Any(s => s.Name == sourceName && s.Url == url);
        }

        public void Save()
        {
            feedContext.SaveChanges();
        }

        public bool HasFeed(int sourceId, string feedTitle)
        {
            return feedContext.Feeds.First(c => c.Title == feedTitle).SourceId == sourceId;
        }
        
        public void Dispose()
        {
            feedContext.Dispose();
        }

        public void DeleteSource(int sourceId)
        {
            var source = feedContext.Sources.FirstOrDefault(s => s.Id == sourceId);
            feedContext.Sources.Remove(source);
        }

        public Source GetSource(int Id)
        {
            return feedContext.Sources.FirstOrDefault(s => s.Id == Id);
        }
    }
}
