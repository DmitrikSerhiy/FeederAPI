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
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IRssRepository, RssRepository>();
            serviceCollection.AddScoped<RssService>();
        }
    }
}
