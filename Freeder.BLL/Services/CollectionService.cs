using AutoMapper;
using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
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
        public CollectionService(IUnitOfWork UnitOfWork)
        {
            collectionRepository = UnitOfWork.CollectionRepository;
            sourceRepository = UnitOfWork.SourceRepository;
        }

        public CollectionDTO GetCollection(string Name)
        {
            return Mapper.Map<CollectionDTO>(collectionRepository.GetCollection(Name));
        }

        public IEnumerable<CollectionDTO> GetCollections()
        {
            return Mapper.Map<List<Collection>, List<CollectionDTO>>(collectionRepository.GetCollections().ToList());
        }

        public CollectionDTO AddCollection(string Name)
        {

            var col = collectionRepository.AddCollection(Name);
            collectionRepository.Save();
            return Mapper.Map<CollectionDTO>(col);
        }

        public CollectionDTO EditCollectionName(string collectionName, string newName)
        {
            collectionRepository.EditCollectionName(collectionName, newName);
            collectionRepository.Save();
            return Mapper.Map<CollectionDTO>(collectionRepository.GetCollection(newName));
        }

        public CollectionDTO AddSourceToCollection(string sourceName, string collectionName)
        {
            var source = sourceRepository.GetSource(sourceName);
            var collection = collectionRepository.GetCollection(collectionName);
            collectionRepository.AddSourceToCollection(source, collection);
            collectionRepository.Save();
            return Mapper.Map<CollectionDTO>(collectionRepository.GetCollection(collectionName));
        }

        public bool IsCollectionContainSource(string collectionName, string sourceName)
        {
            var collection = collectionRepository.GetCollection(collectionName);
            var source = sourceRepository.GetSource(sourceName);

            return collectionRepository.IsCollectionContainSource(collection, source);
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

        public bool DeleteCollection(string Name)
        {
            collectionRepository.DeleteCollection(Name);
            collectionRepository.Save();
            return true;
        }

        public bool IsCollectionNameValid(string collectionName)
        {
            return collectionRepository.IsExist(collectionName);
        }
    }
}
