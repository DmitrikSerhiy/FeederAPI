using Feeder.DAL;
using Feeder.DAL.Interfaces;
using Freeder.BLL;
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
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<FeedService>();
            serviceCollection.AddScoped<SourceService>();
        }
    }
}
