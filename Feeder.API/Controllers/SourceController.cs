using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Freeder.BLL.CacheManagers;
using Freeder.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Feeder.API.Controllers
{
    /// <summary>
    ///     Just for developers: becauce all the http methods returns Source entity without included feeeds
    ///     Use this to fill database with new sources
    /// </summary>
    [Route("api/sources")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private SourceService sourceService;
        private readonly ILogger logger;
        private const int cacheExpiration = 300;

        /// <summary>
        ///     Used consructor injection to get all needed services 
        /// </summary>
        /// <param name="SourceService"></param>
        /// <param name="LoggerFactory"></param>
        /// <param name="CacheManager"></param>
        public SourceController(SourceService SourceService, ILoggerFactory LoggerFactory,
            ICacheManager CacheManager)
        {
            sourceService = SourceService;
            logger = LoggerFactory.CreateLogger<SourceController>();
            CacheManager.DurationInSeconds = cacheExpiration;
        }


        /// <summary>
        ///     Get specific source without feeds
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
        [HttpGet("{sourceName}", Name = "GetSource")]
        public ActionResult GetSource(string sourceName)
        {
            var source = sourceService.GetSource(sourceName); 

            if (source != null)
            {
                logger.LogInformation($"Source: {sourceName}");
                return Ok(source);
            }
            return NotFound();
        }

        /// <summary>
        ///     Get all sources without feeds
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetSources")]
        public ActionResult GetSources()
        {
            var sources = sourceService.GetSources();

            if (sources != null)
            {
                logger.LogInformation($"Sources: {string.Join(", ", sources.Select(s => s.Name))}");
                return Ok(sources);
            }
            return NotFound();
        }

        /// <summary>
        ///     Add new source
        /// </summary>
        /// <param name="sourceName">Name of the real site</param>
        /// <param name="url">Link to the real site</param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
        [HttpPost(Name = "AddSource")]
        public ActionResult AddSource(string sourceName, string url)
        {
            if (sourceService.IsSourceNameValid(sourceName)) return Conflict($"Source {sourceName} is already created");

            var newSource = sourceService.AddSource(sourceName, url);

            if (newSource != null)
            {
                logger.LogInformation($"Source {newSource?.Name} has been added");
                return CreatedAtRoute("GetSource", new { sourceName }, newSource);
            }
            return BadRequest();
        }
    }
}