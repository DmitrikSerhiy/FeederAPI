using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feeder.DAL.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private FeedContext context;
        public CollectionRepository(FeedContext Context)
        {
            context = Context;
        }

        public Collection GetCollection(string Name, bool withIncludes)
        {
            if(withIncludes) return context.Collections
                .Include(c => c.Sources)
                    .ThenInclude(s => s.Feeds)
                .FirstOrDefault(c => c.Name == Name);

            return context.Collections.FirstOrDefault(c => c.Name == Name);
        }


        public Collection GetCollection(int Id, bool withIncludes)
        {
            if (withIncludes) return context.Collections
                    .Include(c => c.Sources)
                        .FirstOrDefault(c => c.Id == Id);

            return context.Collections.FirstOrDefault(c => c.Id == Id);
        }


        public Collection AddCollection(string Name)
        {
            return context.Collections.Add(new Collection() { Name = Name }).Entity;
        }

        public bool IsExist(string Name)
        {
            return context.Collections.Any(c => c.Name == Name);
        }

        public IEnumerable<Collection> GetCollections(bool withIncludes)
        {
            if (withIncludes) return context.Collections
                     .Include(c => c.Sources)
                         .ThenInclude(s => s.Feeds)
                     .ToList();

            return context.Collections.ToList();
        }

        public void DeleteCollection(string Name)
        {
            context.Collections.Remove(context.Collections.First(c => c.Name == Name));;
        }

        public void EditCollectionName(string collectionName, string newName)
        {
            context.Collections.FirstOrDefault(c => c.Name == collectionName).Name = newName;
        }

        public Collection AddSourceToCollection(Collection collection, int sourceId)
        {
            context.Sources.FirstOrDefault(s => s.Id == sourceId).CollectionId = collection.Id;

            return collection;
        }

        public bool HasSource(string collectionName, int sourceId)
        {
            var collection = context.Collections.Include(c => c.Sources).First(c => c.Name == collectionName);

            return collection.Sources.Any(s => s.Id == sourceId);
        }

        public void DeleteSourceFromCollection(string collectionName, int sourceId)
        {
            var collection = context.Collections.Include(c => c.Sources).First(c => c.Name == collectionName);
            collection.Sources.First(s => s.Id == sourceId).CollectionId = null;

        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
