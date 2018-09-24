using Feeder.DAL.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL.CacheManagers
{
    public class FeedCacheManager : CacheManager<Feed>
    {
        protected override string Determinant { get => "Feed"; set => Determinant = value; }
        public FeedCacheManager(IMemoryCache MemoryCache) : base(MemoryCache)
        { }

        public override Feed Get(string feedTitle, bool withIncludes=false)
        {
            if (cache.TryGetValue(Determinant + feedTitle, out Feed feed))
                return feed;
            return null;
        }

        public override Feed Set(string feedTitle, Feed feed, bool withIncludes = false)
        {
            return cache.Set(Determinant + feedTitle, feed,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(casheExpiration));
        }

        public override void Remove(string feedTitle, bool withIncludes = false)
        {
            cache.Remove(Determinant + feedTitle);
        }
    }
}
