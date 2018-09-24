﻿using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feeder.DAL.Repositories
{
    public class SourceRepository : ISourceRepository
    {
        private FeedContext feedContext;
        public SourceRepository(FeedContext FeedContext)
        {
            feedContext = FeedContext;
        }

        public Source AddSource(string Name, string Url)
        {
            return feedContext.Sources.Add(new Source { Name = Name, Url = Url }).Entity;
        }

        public Source GetSource(string Name)
        {
            return feedContext.Sources.FirstOrDefault(s => s.Name == Name);
        }

        public List<Source> GetSources()
        {
            return feedContext.Sources.ToList();
        }

        public bool IsExist(string sourceName)
        {
            return feedContext.Sources.Any(s => s.Name == sourceName);
        }

        public void Save()
        {
            feedContext.SaveChanges();
        }

        public void Dispose()
        {
            feedContext.Dispose();
        }
    }
}
