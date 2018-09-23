using Feeder.DAL;
using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freeder.BLL.Services
{
    public class SourceService
    {
        private ISourceRepository sourceRepository;
        public SourceService(ISourceRepository SourceRepository)
        {
            sourceRepository = SourceRepository;
        }

        public Source AddSource(string Name, string Url)
        {
            var source = sourceRepository.AddSource(Name, Url);
            sourceRepository.Save();
            return source;
        }

        public bool IsSourceNameValid(string sourceName)
        {
            return sourceRepository.IsExist(sourceName);
        }

        public Source GetSource(string Name)
        {
            return sourceRepository.GetSource(Name);
        }

        public List<Source> GetSources()
        {
            return sourceRepository.GetSources();
        }
    }
}
