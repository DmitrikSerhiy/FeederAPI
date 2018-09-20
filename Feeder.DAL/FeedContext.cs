using Feeder.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Feeder.DAL
{
    public class FeedContext : DbContext
    {
        ////  private IConfiguration configuration;
        //  public FeedContext(IConfiguration Configuration)
        //  {
        //      //var builder = new ConfigurationBuilder()
        //      //    .SetBasePath(Directory.GetCurrentDirectory())
        //      //    .AddJsonFile("appsettings.json");

        //      //configuration = builder.Build();

        //      Database.EnsureCreated();
        //  }

        public DbSet<Rss> Rsses { get; set; }
        public DbSet<Source> Sources { get; set; }
        public FeedContext(DbContextOptions<FeedContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.
        //        //.UseLoggerFactory(loggerFactory)
        //        //.EnableSensitiveDataLogging(true)
        //        .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        //}
    }
}
