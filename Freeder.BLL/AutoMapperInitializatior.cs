using AutoMapper;
using Feeder.DAL.Models;
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
                cfg.CreateMap<RssDTO, Rss>().ReverseMap();
            });
        }
    }
}
