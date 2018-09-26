using AutoMapper;
using Feeder.DAL;
using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using Freeder.BLL.CacheManagers;
using Freeder.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Freeder.BLL.Services
{
    public class SourceService
    {
        private ISourceRepository sourceRepository;
        private SourceCacheManager sourceCacheManager;
        public SourceService(ISourceRepository SourceRepository, CacheManager<Source> CacheManager)
        {
            sourceRepository = SourceRepository;
            sourceCacheManager = (SourceCacheManager)CacheManager;
        }

        public SourceDTO AddFeeds(int Id, string Type)
        {
            var source = sourceRepository.GetSource(Id);

            if (source != null)
            {
                XDocument feedXML = XDocument.Load(source.Url);

                var feeds = FeedParcer.Parce(feedXML, Type);
                if (feeds != null)
                {
                    foreach (var feed in feeds)
                    {
                        if (!sourceRepository.HasFeed(Id, feed.Title))
                        {
                            feed.SourceId = Id;
                            sourceRepository.AddFeed(feed);
                        }
                    }
                    sourceRepository.Save();
                    return Mapper.Map<SourceDTO>(sourceRepository.GetSource(Id));
                }
            }
            return null;
        }

        public SourceDTO AddSource(string Name, string Url)
        {
            var source = sourceRepository.AddSource(Name, Url);
            sourceRepository.Save();
            sourceCacheManager.Set(Name, source);
            sourceCacheManager.Set(source.Id.ToString(), source);
            return Mapper.Map<SourceDTO>(source);
        }

        public bool IsSourceValid(int sourceId)
        {
            return sourceRepository.IsExist(sourceId);
        }

        public bool IsSourceValid(string sourceName, string url)
        {
            return sourceRepository.IsExist(sourceName, url);
        }

        public SourceDTO GetSource(int Id, bool withIncludes)
        {
            var source = sourceCacheManager.Get(Id.ToString(), withIncludes);
            if (source == null)
            {
                source = sourceRepository.GetSource(Id);
                sourceCacheManager.Set(Id.ToString(), source, withIncludes);
            }
            return Mapper.Map<SourceDTO>(source);
        }

        public List<SourceDTO> GetSources(bool withIncludes)
        {
            return Mapper.Map<List<Source>, List<SourceDTO>>(sourceRepository.GetSources(withIncludes));
        }

        public void DeleteSource(int Id)
        {
            sourceRepository.DeleteSource(Id);
            sourceRepository.Save();
            sourceCacheManager.Remove(Id.ToString());
        }

        public bool IsFeedTypeValid(string Type)
        {
            return Enum.IsDefined(typeof(FeedType), Type);
        }
    }
}
