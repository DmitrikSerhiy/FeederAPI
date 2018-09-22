using AutoMapper;
using Feeder.DAL.Interfaces;
using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Freeder.BLL
{
    public class FeedService
    {
        private IFeedRepository feedRepository;
        public FeedService(IFeedRepository FeedRepository)
        {
            feedRepository = FeedRepository;
        }


    }
}
