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
            if (!collectionRepository.IsExist(Name))
            {
                var col = collectionRepository.AddCollection(Name);
                collectionRepository.Save();
                return Mapper.Map<CollectionDTO>(col);
            }
            return null;
        }

        public CollectionDTO EditCollectionName(string collectionName, string newName)
        {
            if (collectionRepository.IsExist(collectionName))
            {
                collectionRepository.EditCollectionName(collectionName, newName);
                collectionRepository.Save();
                return Mapper.Map<CollectionDTO>(collectionRepository.GetCollection(newName));
            }
            return null;
        }

        public CollectionDTO AddSourceToCollection(string sourceName, string collectionName)
        {
            if(sourceRepository.IsExist(sourceName) && collectionRepository.IsExist(collectionName))
            {
                var source = sourceRepository.GetSource(sourceName);
                var collection = collectionRepository.GetCollection(collectionName);
                collectionRepository.AddSourceToCollection(source, collection);
                collectionRepository.Save();
                return Mapper.Map<CollectionDTO>(collectionRepository.GetCollection(collectionName));
            }
            return null;
        }

        public bool DeleteCollection(string Name)
        {
            if (collectionRepository.IsExist(Name))
            {
                collectionRepository.DeleteCollection(Name);
                collectionRepository.Save();
                return true;
            }
            return false;
        }

        public bool IsCollectionNameValid(string collectionName)
        {
            return collectionRepository.IsExist(collectionName);
        }
    }
}
