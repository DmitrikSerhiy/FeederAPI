using Feeder.DAL.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL.CacheManagers
{
    public class FeedCacheManager : CacheManager<Feed>
    {
        internal override string Determinant { get; set; } = "Feed";
        public FeedCacheManager(IMemoryCache MemoryCache) : base(MemoryCache)
        { }

        internal override Feed Get(string feedTitle, bool withIncludes=false)
        {
            if (cache.TryGetValue(Determinant + feedTitle, out Feed feed))
                return feed;
            return null;
        }

        internal override Feed Set(string feedTitle, Feed feed, bool withIncludes = false)
        {
            return cache.Set(Determinant + feedTitle, feed,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(casheExpiration));
        }

        internal override void Remove(string feedTitle, bool withIncludes = false)
        {
            cache.Remove(Determinant + feedTitle);
        }
    }
}
