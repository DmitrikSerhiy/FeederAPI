using System;
using System.Linq;
using Freeder.BLL.CacheManagers;
using Freeder.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Feeder.Controllers
{
    /// <summary>
    ///     Handle all http requests with feeds
    /// </summary>
    [Route("api/feeds")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private FeedService feedService;
        private readonly ILogger logger;
        private const int cacheExpiration = 300;

        /// <summary>
        ///     Used consructor injection to get all needed services 
        /// </summary>
        /// <param name="FeedService"></param>
        /// <param name="LoggerFactory"></param>
        /// <param name="CacheManager"></param>
        public FeedController(FeedService FeedService,
            ILoggerFactory LoggerFactory, ICacheManager CacheManager)
        {
            feedService = FeedService;
            logger = LoggerFactory.CreateLogger<FeedController>();
            CacheManager.DurationInSeconds = cacheExpiration;
        }

        /// <summary>
        ///     Get specific feed by name and publish date
        /// </summary>
        /// <param name="feedTitle"></param>
        /// <param name="feedPublishDate"></param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
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

        /// <summary>
        ///     Get specific feed by Id
        /// </summary>
        /// <param name="feedId"></param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
        [HttpGet("{feedId}", Name = "GetFeedById")]
        public ActionResult GetFeed(int feedId)
        {
            var feed = feedService.GetFeed(feedId);

            if (feed != null)
            {
                logger.LogInformation($"Feed: {feed.Title}");
                return Ok(feed);
            }
            return NotFound();
        }

        /// <summary>
        ///     Get all existed feeds
        /// </summary>
        /// <returns></returns>
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet(Name = "GetFeeds")]
        public ActionResult GetFeeds()
        {
            var feeds = feedService.GetFeeds();

            if (feeds.Count != 0)
            {
                logger.LogInformation($"Feeds: {string.Join(", ", feeds.Select(s => s.Title))}");
                return Ok(feeds);
            }
            return NotFound();
        }
    }
}
