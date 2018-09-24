using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;
using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL.CacheManagers
{
    public abstract class CacheManager<T> : ICacheManager where T : IModel

    {
        internal IMemoryCache cache { get; set; }
        public int DurationInSeconds
        {
            get => (int)casheExpiration.TotalSeconds;
            set => casheExpiration = new TimeSpan(0, 0, value);
        }
        internal TimeSpan casheExpiration { get;set; } = new TimeSpan(0, 0, 300);
        internal abstract string Determinant { get; set; }

        public CacheManager(IMemoryCache MemoryCache)
        {
            cache = MemoryCache;
        }

        internal abstract T Get(string TName, bool withIncludes = false);

        internal abstract T Set(string TName, T entityToCache, bool withIncludes = false);

        internal abstract void Remove(string TName, bool withIncludes = false);
    }
}
