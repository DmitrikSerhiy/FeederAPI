using AutoMapper;
using Feeder.DAL;
using Feeder.DAL.Models;
using Freeder.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL
{
    public class AutoMapperInitializatior
    {
        public AutoMapperInitializatior()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Feed, FeedDTO>().ReverseMap(); ;
                // .ForMember(dto => dto.Type, feed => feed.MapFrom(f => f.Type));
                //.ForMember(feed => feed.Type, f => f.MapFrom(feed => Enum.GetName(typeof(FeedType), feed.Type)));

                cfg.CreateMap<Source, SourceDTO>().ReverseMap();
                // .ForMember(dto => dto.Feeds, source => source.MapFrom(s => s.Feeds))

                cfg.CreateMap<Collection, CollectionDTO>().ReverseMap();
            });
        }
    }
}
