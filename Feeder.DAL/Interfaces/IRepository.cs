using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetFeeds();
        T GetFeed(int Id);
        void Save();
    }
}
