using Feeder.DAL.Interfaces;
using System;
using System.Collections.Generic;

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
            //map
            return new RssDTO()
            {
                Title = "test feed1",
                Content = "some content1",
                Link = "some url1",
                Description = "some desc1"
            };
        }

        public IEnumerable<RssDTO> GetFeeds()
        {
            var rss = rssRepo.GetFeeds();
            //map
            var rssDTO = new List<RssDTO>();
            rssDTO.Add(new RssDTO()
            {
                Title = "test feed1",
                Content = "some content1",
                Link = "some url1",
                Description = "some desc1"
            });
            rssDTO.Add(new RssDTO()
            {
                Title = "test feed2",
                Content = "some content2",
                Link = "some url2",
                Description = "some desc2"
            });

            return rssDTO;
        }

    }
}
