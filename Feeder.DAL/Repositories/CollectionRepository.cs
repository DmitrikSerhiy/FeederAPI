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

        public Collection GetCollection(string Name)
        {
            return context.Collections
                .Include(c => c.Sources)
                    .ThenInclude(s => s.Feeds)
                .FirstOrDefault(c => c.Name == Name);
        }
        public Collection AddCollection(string Name)
        {
            return context.Collections.Add(new Collection() { Name = Name }).Entity;
        }

        public bool IsExist(string Name)
        {
            return context.Collections.Any(c => c.Name == Name);
        }

        public IEnumerable<Collection> GetCollections()
        {
            return context.Collections.Include(c => c.Sources).ToList();
        }

        public void DeleteCollection(string Name)
        {
            context.Collections.Remove(context.Collections.First(c => c.Name == Name));;
        }

        public void EditCollectionName(string collectionName, string newName)
        {
            context.Collections.FirstOrDefault(c => c.Name == collectionName).Name = newName;
        }

        public Collection AddSourceToCollection(string sourceName, Collection collection)
        {
            var source = context.Sources.FirstOrDefault(s => s.Name == sourceName).CollectionId = collection.Id; ;

            return collection;
        }

        public bool IsCollectionContainSource(string collectionName, string sourceName)
        {
            return context.Collections.Include(c => c.Sources).Any(c => c.Sources.Any(s => s.Name == sourceName));
        }

        public void DeleteSourceFromCollection(string collectionName, string sourceName)
        {
            var collection = context.Collections.Include(c => c.Sources).First(c => c.Name == collectionName);
            collection.Sources.First(s => s.Name == sourceName).CollectionId = null;

        }

        public Collection ViewCollection(string collectionName)
        {
            return context.Collections
                .Include(c => c.Sources)
                    .ThenInclude(s => s.Feeds)
                .FirstOrDefault(s => s.Name == collectionName);
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
