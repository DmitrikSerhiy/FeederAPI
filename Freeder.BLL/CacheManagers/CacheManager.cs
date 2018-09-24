using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;
using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL.CacheManagers
{
    public abstract class CacheManager<T> where T : class
    {
        protected IMemoryCache cache { get; set; }
        protected TimeSpan casheExpiration { get; } = TimeSpan.FromMinutes(5);
        protected abstract string Determinant { get; set; }

        public CacheManager(IMemoryCache MemoryCache)
        {
            cache = MemoryCache;
        }

        public abstract T Get(string TName, bool withIncludes = false);

        public abstract T Set(string TName, T entityToCache, bool withIncludes = false);

        public abstract void Remove(string TName, bool withIncludes = false);
    }
}
