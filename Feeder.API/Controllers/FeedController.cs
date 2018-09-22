using System;
using Freeder.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Feeder.Controllers
{
    // [Produces("application/json")]
    [Route("api/feed")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private FeedService feedService;
        private SourceService sourceService;
        private readonly ILogger logger;

        public FeedController(FeedService FeedService, SourceService SourceService,
            ILoggerFactory LoggerFactory)
        {
            feedService = FeedService;
            sourceService = SourceService;
            logger = LoggerFactory.CreateLogger<FeedController>();

        }

        //[HttpGet("{Id}", Name = "GetFeed")]
        //public ActionResult GetRssFeed(int Id)
        //{
        //     var feed = rssService.GetFeed(Id);

        //    if (feed != null) return Ok(feed);
        //    return NotFound();
        //}

        //[HttpGet(Name = "GetFeeds")]
        //public ActionResult GetRssFeeds()
        //{
        //    var feed = rssService.GetFeeds();

        //    if (feed != null) return Ok(feed);
        //    return NotFound();
        //}

    }
}
