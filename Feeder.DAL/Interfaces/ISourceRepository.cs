using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface ISourceRepository
    {
        Source GetSource(string Name);
        Source AddSource(string Name, string Url);
        List<Source> GetSources();
        void Save();
    }
}
