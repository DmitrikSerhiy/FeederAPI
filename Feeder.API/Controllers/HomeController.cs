using System;
using Freeder.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Feeder.Controllers
{
    // [Produces("application/json")]
    [Route("api/feed")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private RssService rssService;
        private SourceService sourceService;
        private readonly ILogger logger;

        public HomeController(RssService RssService, SourceService SourceService,
            ILoggerFactory LoggerFactory)
        {
            rssService = RssService;
            sourceService = SourceService;
            logger = LoggerFactory.CreateLogger<HomeController>();

        }

        [HttpGet("{Id}", Name = "GetFeed")]
        public ActionResult GetRssFeed(int Id)
        {
             var feed = rssService.GetFeed(Id);

            if (feed != null) return Ok(feed);
            return NotFound();
        }

        [HttpGet(Name = "GetFeeds")]
        public ActionResult GetRssFeeds()
        {
            var feed = rssService.GetFeeds();

            if (feed != null) return Ok(feed);
            return NotFound();
        }

    }
}
