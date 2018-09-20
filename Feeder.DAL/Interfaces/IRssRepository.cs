using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface IRssRepository : IRepository<Rss>
    {
        void AddRssToDb(Rss rss);
    }
}
