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
        Collection AddSourceToCollection(string sourceName, Collection collection);
        bool IsCollectionContainSource(string collectionName, string sourceName);
        void DeleteSourceFromCollection(string collectionName, string sourceName);
        Collection ViewCollection(string collectionName);
        bool IsExist(string Name);
        void Save();
    }
}
