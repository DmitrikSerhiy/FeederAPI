using Feeder.DAL;
using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using Feeder.DAL.Repositories;
using Freeder.BLL;
using Freeder.BLL.CacheManagers;
using Freeder.BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Feeder.Infrastructure
{
    public class DependencyResolver
    {
        public DependencyResolver(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<FeedContext>();

            serviceCollection.AddScoped<IFeedRepository, FeedRepository>();
            serviceCollection.AddScoped<ISourceRepository, SourceRepository>();
            serviceCollection.AddScoped<ICollectionRepository, CollectionRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

            serviceCollection.AddScoped<FeedService>();
            serviceCollection.AddScoped<SourceService>();
            serviceCollection.AddScoped<CollectionService>();

            serviceCollection.AddScoped<CacheManager<Collection>, CollectionCacheManager>();
            serviceCollection.AddScoped<CacheManager<Source>, SourceCacheManager>();
            serviceCollection.AddScoped<CacheManager<Feed>, FeedCacheManager>();

            serviceCollection.AddScoped<ICacheManager, CollectionCacheManager>();
            serviceCollection.AddScoped<ICacheManager, SourceCacheManager>();
            serviceCollection.AddScoped<ICacheManager, FeedCacheManager>();
        }
    }
}
