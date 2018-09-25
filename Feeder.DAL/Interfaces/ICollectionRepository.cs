using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface ICollectionRepository : IDisposable
    {
        Collection AddCollection(string Name);
        Collection GetCollection(string Name, bool withIncludes);
        Collection GetCollection(int Id, bool withIncludes);
        IEnumerable<Collection> GetCollections(bool withIncludes);
        void DeleteCollection(string Name);
        void EditCollectionName(string collectionName, string newName);
        Collection AddSourceToCollection(Collection collection, int sourceId);
        bool HasSource(string collectionName, int sourceId);
        void DeleteSourceFromCollection(string collectionName, int sourceId);
        bool IsExist(string Name);
        void Save();
    }
}
