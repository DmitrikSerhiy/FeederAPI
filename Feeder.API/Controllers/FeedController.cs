using System;
using System.Linq;
using Freeder.BLL.Services;
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
        private readonly ILogger logger;

        public FeedController(FeedService FeedService,
            ILoggerFactory LoggerFactory)
        {
            feedService = FeedService;
            logger = LoggerFactory.CreateLogger<FeedController>();

        }

        [HttpGet(Name = "GetFeed")]
        public ActionResult GetFeed(string title, string publishDate)
        {
            var feed = feedService.GetFeed(title, publishDate);

            if (feed != null) 
            {
                logger.LogInformation($"Feed: {feed.Title}");
                return Ok(feed);
            }
            return NotFound($"There is no feed with title: {title}");
        }

        [HttpGet("{source}", Name = "GetFeeds")]
        public ActionResult GetFeeds(string source)
        {
            if (!feedService.IsSourceNameValid(source)) return Conflict("There is no such source in db");

            var sourceDTO = feedService.GetFeeds(source);
           
            if (sourceDTO != null)
            {
                logger.LogInformation($"Feeds from {source}: {string.Join(", ", sourceDTO.Feeds.Select(s => s.Title))}");
                return Ok(sourceDTO);
            }
            return NotFound();
        }

        [HttpPost(Name = "AddFeeds")]
        public ActionResult AddFeed(string sourceName, string Type)
        {
            if (!feedService.IsFeedTypeValid(Type)) return Conflict("Invalid feed type. Use RSS or Atom");

            if (!feedService.IsSourceNameValid(sourceName)) return Conflict("There is no such source in db");

            var sourceDTO = feedService.AddFeeds(sourceName, Type);

            if (sourceDTO != null)
            {
                logger.LogInformation($"Updated feeds for {sourceName}: {string.Join(", ", sourceDTO.Feeds.Select(s => s.Title))}");
                return CreatedAtRoute("GetFeeds", new { source = sourceName }, sourceDTO);
            }
            return BadRequest();
        }

    }
}
