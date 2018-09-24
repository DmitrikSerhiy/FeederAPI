using Feeder.DAL.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL.CacheManagers
{
    public class SourceCacheManager : CacheManager<Source>
    {
        protected override string Determinant { get => "Source"; set => Determinant = value; }

        public SourceCacheManager(IMemoryCache MemoryCache) : base(MemoryCache)
        {}

        public override Source Get(string sourceName, bool withIncludes=false)
        {
            if (cache.TryGetValue(Determinant + sourceName, out Source source))
                return source;
            return null;
        }

        public override Source Set(string sourceName, Source Source, bool withIncludes=false)
        {
            return cache.Set(Determinant + sourceName, Source,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(casheExpiration));
        }

        public override void Remove(string sourceName, bool withIncludes=false)
        {
            cache.Remove(Determinant + sourceName);
        }
    }
}
