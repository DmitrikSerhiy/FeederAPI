using AutoMapper;
using Feeder.DAL;
using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using Freeder.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Freeder.BLL
{
    public class FeedService
    {
        private IFeedRepository feedRepository;
        private ISourceRepository sourceRepository;
        public FeedService(IUnitOfWork UnitOfWork)
        {
            feedRepository = UnitOfWork.feedRepository;
            sourceRepository = UnitOfWork.sourceRepository;
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
            return Mapper.Map<SourceDTO>(feedRepository.GetFeeds(sourceName));
        }

        public FeedDTO GetFeed(string title, string publishDate)
        {
            return Mapper.Map<FeedDTO>(feedRepository.GetFeed(title, publishDate));
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
