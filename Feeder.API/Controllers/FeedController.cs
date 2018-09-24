﻿using System;
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
        ///     Get all feeds for specific source
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
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

        /// <summary>
        ///     Fill specific source with feeds
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="type">RSS or Atom</param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
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
