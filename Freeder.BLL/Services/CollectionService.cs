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

        public CollectionDTO GetCollection(string collectionName)
        {
            Collection collection = collectionCacheManager.Get(collectionName, true);
            if(collection == null)
            {
                collection = collectionRepository.GetCollection(collectionName);
                collectionCacheManager.Set(collectionName, collection, true);
            }
            return Mapper.Map<CollectionDTO>(collection);
        }

        public IEnumerable<CollectionDTO> GetCollections()
        {
            return Mapper.Map<List<Collection>, List<CollectionDTO>>(collectionRepository.GetCollections().ToList());
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

            var collection = collectionRepository.GetCollection(newName);
            collectionCacheManager.Set(newName, collection, true);

            return Mapper.Map<CollectionDTO>(collection);
        }

        public CollectionDTO AddSourceToCollection(string sourceName, string collectionName)
        {
            var source = sourceCacheManager.Get(sourceName);
            var collection = collectionCacheManager.Get(collectionName, true);

            if (source == null)
            {
                source = sourceRepository.GetSource(sourceName);
                sourceCacheManager.Set(sourceName, source);
            }
            if (collection == null)
                collection = collectionRepository.GetCollection(collectionName);


            collection = collectionRepository.AddSourceToCollection(sourceName, collection);
            collectionRepository.Save();

            sourceCacheManager.Remove(sourceName);
            collectionCacheManager.Remove(collectionName, true);

            collectionCacheManager.Set(collectionName, collection, true);

            return Mapper.Map<CollectionDTO>(collection);
        }

        public bool IsCollectionContainSource(string collectionName, string sourceName)
        {
            return collectionRepository.IsCollectionContainSource(collectionName, sourceName);
        }

        public CollectionDTO ViewCollection(string collectionName)
        {
            return Mapper.Map<CollectionDTO>(collectionRepository.ViewCollection(collectionName));
        }

        public bool DeleteSourceFromCollection(string collectionName, string sourceName)
        {
            collectionRepository.DeleteSourceFromCollection(collectionName, sourceName);
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
