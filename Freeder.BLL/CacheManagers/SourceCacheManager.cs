using Feeder.DAL.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL.CacheManagers
{
    public class SourceCacheManager : CacheManager<Source>
    {
        internal override string Determinant { get; set; } = "Source";

        public SourceCacheManager(IMemoryCache MemoryCache) : base(MemoryCache)
        {}

        internal override Source Get(string sourceName, bool withIncludes=false)
        {
            if (cache.TryGetValue(Determinant + sourceName, out Source source))
                return source;
            return null;
        }

        internal override Source Set(string sourceName, Source Source, bool withIncludes=false)
        {
            return cache.Set(Determinant + sourceName, Source,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(casheExpiration));
        }

        internal override void Remove(string sourceName, bool withIncludes=false)
        {
            cache.Remove(Determinant + sourceName);
        }
    }
}
