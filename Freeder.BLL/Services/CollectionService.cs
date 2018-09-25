using AutoMapper;
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
    public class CollectionService
    {
        private ICollectionRepository collectionRepository;
        private ISourceRepository sourceRepository;
        private CollectionCacheManager collectionCacheManager;
        private SourceCacheManager sourceCacheManager;
        public CollectionService(IUnitOfWork UnitOfWork, CacheManager<Collection> CollectionCacheManager,
            CacheManager<Source> SourceCacheManager)
        {
            collectionRepository = UnitOfWork.CollectionRepository;
            sourceRepository = UnitOfWork.SourceRepository;
            collectionCacheManager = (CollectionCacheManager)CollectionCacheManager;
            sourceCacheManager = (SourceCacheManager)SourceCacheManager;
        }

        public CollectionDTO GetCollection(string collectionName, bool withIncludes)
        {
            Collection collection = collectionCacheManager.Get(collectionName, withIncludes);
            if(collection == null)
            {
                collection = collectionRepository.GetCollection(collectionName, withIncludes);
                collectionCacheManager.Set(collectionName, collection, withIncludes);
            }
            return Mapper.Map<CollectionDTO>(collection);
        }

        public IEnumerable<CollectionDTO> GetCollections(bool withIncludes)
        {
            return Mapper.Map<List<Collection>, List<CollectionDTO>>(collectionRepository.GetCollections(withIncludes).ToList());
        }

        public CollectionDTO AddCollection(string collectionName)
        {
            var collection = collectionRepository.AddCollection(collectionName);
            collectionRepository.Save();

            collectionCacheManager.Set(collectionName, collection, false);

            return Mapper.Map<CollectionDTO>(collection);
        }

        public CollectionDTO EditCollectionName(string collectionName, string newName)
        {
            collectionRepository.EditCollectionName(collectionName, newName);
            collectionRepository.Save();

            var collection = collectionRepository.GetCollection(newName, true);
            collectionCacheManager.Set(newName, collection, true);

            return Mapper.Map<CollectionDTO>(collection);
        }

        public CollectionDTO AddSourceToCollection(string collectionName, int sourceId)
        {
            var source = sourceRepository.GetSource(sourceId);
            var collection = collectionRepository.GetCollection(collectionName, true);

            collection = collectionRepository.AddSourceToCollection(collection, sourceId);
            collectionRepository.Save();

            sourceCacheManager.Set(source.ToString(), source);
            collectionCacheManager.Set(collectionName, collection, true);

            return Mapper.Map<CollectionDTO>(collection);
        }

        public bool IsCollectionContainSource(string collectionName, int sourceId)
        {
            return collectionRepository.HasSource(collectionName, sourceId);
        }

        public bool DeleteSourceFromCollection(string collectionName, int sourceId)
        {
            collectionRepository.DeleteSourceFromCollection(collectionName, sourceId);
            collectionRepository.Save();
            return true;
        }

        public bool DeleteCollection(string collectionName)
        {
            collectionRepository.DeleteCollection(collectionName);
            collectionRepository.Save();

            collectionCacheManager.Remove(collectionName, true);
            collectionCacheManager.Remove(collectionName, false);
            return true;
        }

        public bool IsCollectionNameValid(string collectionName)
        {
            return collectionRepository.IsExist(collectionName);
        }
    }
}
