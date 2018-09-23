using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface ICollectionRepository : IDisposable
    {
        Collection AddCollection(string Name);
        Collection GetCollection(string Name);
        IEnumerable<Collection> GetCollections();
        void DeleteCollection(string Name);
        void EditCollectionName(string collectionName, string newName);
        void AddSourceToCollection(Source source, Collection collection);
        bool IsCollectionContainSource(Collection collection, Source source);
        void DeleteSourceFromCollection(string collectionName, string sourceName);
        bool IsExist(string Name);
        void Save();
    }
}
