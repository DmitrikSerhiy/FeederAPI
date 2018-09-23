using System;
using System.Linq;
using Freeder.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Feeder.Controllers
{
    [Route("api/feeds")]
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

        [HttpGet("{feedTitle}/{feedPublishDate}", Name = "GetFeed")]
        public ActionResult GetFeed(string feedTitle, string feedPublishDate)
        {
            var feed = feedService.GetFeed(feedTitle, feedPublishDate);

            if (feed != null) 
            {
                logger.LogInformation($"Feed: {feed.Title}");
                return Ok(feed);
            }
            return NotFound($"There is no feed with title: {feedTitle}");
        }

        [HttpGet("{sourceName}", Name = "GetFeeds")]
        public ActionResult GetFeeds(string sourceName)
        {
            if (!feedService.IsSourceNameValid(sourceName)) return Conflict("There is no such source in db");

            var sourceDTO = feedService.GetFeeds(sourceName);
           
            if (sourceDTO != null)
            {
                logger.LogInformation($"Feeds from {sourceName}: {string.Join(", ", sourceDTO.Feeds.Select(s => s.Title))}");
                return Ok(sourceDTO);
            }
            return NotFound();
        }

        [HttpPost(Name = "AddFeeds")]
        public ActionResult AddFeeds(string sourceName, string type)
        {
            if (!feedService.IsFeedTypeValid(type)) return Conflict("Invalid feed type. Use RSS or Atom");

            if (!feedService.IsSourceNameValid(sourceName)) return Conflict("There is no such source in db");

            var sourceDTO = feedService.AddFeeds(sourceName, type);

            if (sourceDTO != null)
            {
                logger.LogInformation($"Updated feeds for {sourceName}: {string.Join(", ", sourceDTO.Feeds.Select(s => s.Title))}");
                return CreatedAtRoute("GetFeeds", new { sourceName }, sourceDTO);
            }
            return BadRequest();
        }

    }
}
