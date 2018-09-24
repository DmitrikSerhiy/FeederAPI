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

        public Source AddSource(string Name, string Url)
        {
            var source = sourceRepository.AddSource(Name, Url);
            sourceRepository.Save();
            sourceCacheManager.Set(Name, source);
            return source;
        }

        public bool IsSourceNameValid(string sourceName)
        {
            return sourceRepository.IsExist(sourceName); 
        }

        public SourceDTO GetSource(string sourceName)
        {
            var source = sourceCacheManager.Get(sourceName);
            if (source == null)
            {
                source = sourceRepository.GetSource(sourceName);
                sourceCacheManager.Set(sourceName, source);
            }
            return Mapper.Map<SourceDTO>(source);
        }

        public List<SourceDTO> GetSources()
        {
            return Mapper.Map<List<Source>, List<SourceDTO>>(sourceRepository.GetSources());
        }
    }
}
