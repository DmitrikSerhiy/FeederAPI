using AutoMapper;
using Feeder.DAL;
using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using Freeder.BLL.DTOs;
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

        public SourceDTO GetSource(string Name)
        {
            return Mapper.Map<SourceDTO>(sourceRepository.GetSource(Name));
        }

        public List<SourceDTO> GetSources()
        {
            return Mapper.Map<List<Source>, List<SourceDTO>>(sourceRepository.GetSources());
        }
    }
}
