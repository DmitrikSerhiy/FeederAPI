﻿using Feeder.DAL.Interfaces;
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
            return context.Collections.Include(c => c.Sources).FirstOrDefault(c => c.Name == Name);
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

        public void AddSourceToCollection(Source source, Collection collection)
        {
            context.Sources.FirstOrDefault(s => s.Id == source.Id).CollectionId = collection.Id; 
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}