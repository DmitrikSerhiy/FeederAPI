using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface ISourceRepository : IDisposable
    {
        Source GetSource(int Id);
        Source AddSource(string sourceName, string url);
        bool HasFeed(int sourceId, string feedTitle);
        void AddFeed(Feed feed);
        void DeleteSource(int sourceId);
        List<Source> GetSources(bool withIncludes);
        bool IsExist(string sourceName, string url);
        bool IsExist(int sourceId);
        void Save();
    }
}
