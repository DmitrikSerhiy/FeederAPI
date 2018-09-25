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


        ///// <summary>
        /////     Get specific source by name and url
        ///// </summary>
        ///// <param name="sourceName"></param>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //[ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
        //[HttpGet("{sourceName}/{url}", Name = "GetSource")]
        //public ActionResult GetSource(string sourceName, string url)
        //{
        //    var source = sourceService.GetSource(sourceName, url);

        //    if (source != null)
        //    {
        //        logger.LogInformation($"Source: {sourceName}");
        //        return Ok(source);
        //    }
        //    return NotFound();
        //}

        /// <summary>
        ///     Get Source without feeds by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
        [HttpGet("{id}", Name = "GetSourceById")]
        public ActionResult GetSourceById(int id)
        {
            var source = sourceService.GetSource(id);

            if (source != null)
            {
                logger.LogInformation($"Source: {source.Name}");
                return Ok(source);
            }
            return NotFound();
        }

        /// <summary>
        ///     Get all the sources
        /// </summary>
        /// <param name="withIncludes">Get sources with included feeds or not</param>
        /// <returns></returns>
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet(Name = "GetSources")]
        public ActionResult GetSources(bool withIncludes)
        {
            var sources = sourceService.GetSources(withIncludes);

            if (sources != null)
            {
                logger.LogInformation($"Sources: {string.Join(", ", sources.Select(s => s.Name))}");
                return Ok(sources);
            }
            return NotFound();
        }

        /// <summary>
        ///     Download and add feeds for existed source
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="type">RSS or Atom</param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
        [HttpPut("{sourceId}/{type}", Name = "AddFeedsToSource")]
        public ActionResult AddFeedsToSource(int sourceId, string type)
        {
            if (!sourceService.IsSourceValid(sourceId)) return Conflict("There is no such source");
            if (!sourceService.IsFeedTypeValid(type)) return Conflict($"{type} - nvalid feed type. Try RSS or Atom");

            var source = sourceService.AddFeeds(sourceId, type);
            if(source != null)
                return CreatedAtRoute("GetSourceById", new { id = sourceId }, source);
            return BadRequest();
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
            if (sourceService.IsSourceValid(sourceName, url)) return Conflict($"Source {sourceName} with url {url} is already created");

            var newSource = sourceService.AddSource(sourceName, url);

            if (newSource != null)
            {
                logger.LogInformation($"Source {newSource?.Name} with url {url} has been added");
                return CreatedAtRoute("GetSourceById", new { id = newSource.Id }, newSource);
            }
            return BadRequest();
        }

        ///// <summary>
        /////     Remove source by name and url
        ///// </summary>
        ///// <param name="sourceName"></param>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        //[HttpDelete("{sourceName}/{url}", Name = "DeleteSource")]
        //public ActionResult DeleteSource(string sourceName, string url)
        //{
        //    if (!sourceService.IsSourceValid(sourceName, url)) return Conflict($"There is no {sourceName} with url {url}");

        //    sourceService.DeleteSource(sourceName, url);

        //    if (!sourceService.IsSourceValid(sourceName, url))
        //    {
        //        logger.LogInformation($"Deleted source: {sourceName} with url {url}");
        //        return NoContent();
        //    }
        //    return BadRequest();
        //}

        /// <summary>
        ///     Remove source by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpDelete("{Id}", Name = "DeleteSourceById")]
        public ActionResult DeleteSourceById(int Id)
        {
            if (!sourceService.IsSourceValid(Id)) return Conflict($"There is no such source");

            sourceService.DeleteSource(Id);

            if (!sourceService.IsSourceValid(Id))
                return NoContent();
            return BadRequest();
        }
    }
}