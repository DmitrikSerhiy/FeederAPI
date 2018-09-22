using Feeder.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Feeder.DAL
{
    public class FeedContext : DbContext
    {
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Collection> Collections { get; set; }


        public FeedContext(DbContextOptions<FeedContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
    }
}
