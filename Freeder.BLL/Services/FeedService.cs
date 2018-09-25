using AutoMapper;
using Feeder.DAL;
using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using Freeder.BLL.CacheManagers;
using Freeder.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Freeder.BLL.Services
{
    public class FeedService
    {
        private IFeedRepository feedRepository;
        private FeedCacheManager feedCacheManager;
        private SourceCacheManager sourceCacheManager;
        public FeedService(IUnitOfWork UnitOfWork, CacheManager<Feed> FeedCacheManager, CacheManager<Source> SourceCacheManager)
        {
            feedRepository = UnitOfWork.FeedRepository;
            feedCacheManager = (FeedCacheManager)FeedCacheManager;
            sourceCacheManager = (SourceCacheManager)SourceCacheManager;
        }

        public List<FeedDTO> GetFeeds()
        {
            return Mapper.Map<List<Feed>, List<FeedDTO> >(feedRepository.GetFeeds().ToList());
        }

        public FeedDTO GetFeed(string title, string publishDate)
        {
            var feed = feedCacheManager.Get(title);
            if(feed == null)
            {
                feed = feedRepository.GetFeed(title, publishDate);
                feedCacheManager.Set(title, feed);
            }
            return Mapper.Map<FeedDTO>(feed);
        }

        public FeedDTO GetFeed(int Id)
        {
            var feed = feedCacheManager.Get(Id.ToString());
            if (feed == null)
            {
                feed = feedRepository.GetFeed(Id);
                feedCacheManager.Set(Id.ToString(), feed);
            }
            return Mapper.Map<FeedDTO>(feed);
        }

        public bool IsFeedValid(string title, string publishDate)
        {
            return feedRepository.IsExist(title, publishDate);
        }
    }
}
