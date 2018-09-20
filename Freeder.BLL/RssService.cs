using AutoMapper;
using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Freeder.BLL
{
    public class RssService
    {
        private IRssRepository rssRepo;
        public RssService(IUnitOfWork UnitOfWork)
        {
            rssRepo = UnitOfWork.Rsses;
        }

        public RssDTO GetFeed(int Id)
        {
            var rss = rssRepo.GetFeed(Id);
            var rssDto = Mapper.Map<Rss, RssDTO>(rss);
            return rssDto;
            //map
            //return new RssDTO()
            //{
            //    Title = "test feed1",
            //    Content = "some content1",
            //    Link = "some url1",
            //    Description = "some desc1"
            //};
        }

        public IEnumerable<RssDTO> GetFeeds()
        {
            var rss = rssRepo.GetFeeds().ToList();
            var rssDtos = Mapper.Map<Rss[], List<RssDTO>>(rss.ToArray());
            //map
            //var rssDTO = new List<RssDTO>();
            //rssDTO.Add(new RssDTO()
            //{
            //    Title = "test feed1",
            //    Content = "some content1",
            //    Link = "some url1",
            //    Description = "some desc1"
            //});
            //rssDTO.Add(new RssDTO()
            //{
            //    Title = "test feed2",
            //    Content = "some content2",
            //    Link = "some url2",
            //    Description = "some desc2"
            //});

            return rssDtos;
        }

    }
}
