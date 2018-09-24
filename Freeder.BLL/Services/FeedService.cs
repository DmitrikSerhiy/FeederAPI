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
        private ISourceRepository sourceRepository;
        private FeedCacheManager feedCacheManager;
        private SourceCacheManager sourceCacheManager;
        public FeedService(IUnitOfWork UnitOfWork, CacheManager<Feed> FeedCacheManager, CacheManager<Source> SourceCacheManager)
        {
            feedRepository = UnitOfWork.FeedRepository;
            sourceRepository = UnitOfWork.SourceRepository;
            feedCacheManager = (FeedCacheManager)FeedCacheManager;
            sourceCacheManager = (SourceCacheManager)SourceCacheManager;
        }

        public SourceDTO AddFeeds(string sourceName, string Type)
        {
            var source = sourceRepository.GetSource(sourceName);

            var url = sourceRepository.GetSource(sourceName)?.Url;
            if (url != null)
            {
                XDocument feedXML = XDocument.Load(url);

                foreach (var feed in FeedParcer.Parce(feedXML, Type))
                {
                    if(!feedRepository.IsFeedInSource(feed, source))
                    {
                        feed.SourceId = source.Id;
                        feedRepository.AddFeed(feed);
                    }
                }
                feedRepository.Save();
                return Mapper.Map < SourceDTO > (feedRepository.GetFeeds(sourceName));
            }
            return null;
        }

        public SourceDTO GetFeeds(string sourceName)
        {
            var source = sourceCacheManager.Get(sourceName, true);
            if(source == null)
            {
                source = feedRepository.GetFeeds(sourceName);
                sourceCacheManager.Set(sourceName, source, true);
            }
            return Mapper.Map<SourceDTO>(source);
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

        public bool IsSourceNameValid(string SourceName)
        {
            return sourceRepository.GetSource(SourceName) != null;
        }

        public bool IsFeedTypeValid(string Type)
        {
            return Enum.IsDefined(typeof(FeedType), Type);
        }


    }
}
