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
    ///     Handle all http requests with collections 
    /// </summary>
    [Route("api/collections")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private CollectionService collectionService;
        private SourceService sourceService;
        private readonly ILogger logger;
        private const int cacheExpiration = 350;

        /// <summary>
        ///     Used consructor injection to get all needed services 
        /// </summary>
        /// <param name="CollectionService"></param>
        /// <param name="SourceService"></param>
        /// <param name="LoggerFactory"></param>
        /// <param name="CacheManager"></param>
        public CollectionController(CollectionService CollectionService, SourceService SourceService,
            ILoggerFactory LoggerFactory, ICacheManager CacheManager)
        {
            collectionService = CollectionService;
            sourceService = SourceService;
            logger = LoggerFactory.CreateLogger<CollectionController>();
            CacheManager.DurationInSeconds = cacheExpiration;
        }

        /// <summary>
        ///     Get specific collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="withIncludes"></param>
        /// <returns></returns>
        /// 
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
        [HttpGet("{collectionName}", Name = "GetCollection")]
        public ActionResult GetCollection(string collectionName, bool withIncludes)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict("There is no such collection in db");
            var col = collectionService.GetCollection(collectionName, withIncludes);

            if (col != null)
            {
                logger.LogInformation($"Collection: {collectionName}");
                return Ok(col);
            }
            return NotFound();
        }

        /// <summary>
        ///     Get all collections
        /// </summary>
        /// <param name="withIncludes">Return collection with included sources and feeds or not</param>
        /// <returns></returns>
        [ResponseCache(NoStore =true, Location =ResponseCacheLocation.None)]
        [HttpGet(Name = "GetCollections")]
        public ActionResult GetCollections(bool withIncludes)
        {
            var collections = collectionService.GetCollections(withIncludes);

            if (collections.Count() != 0)
            {
                logger.LogInformation($"Collections: {string.Join(", ", collections.Select(s => s.Name))}");
                return Ok(collections);
            }
            return NotFound();
        }


        /// <summary>
        ///     Add new collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
        [HttpPost(Name = "AddCollection")]
        public ActionResult AddCollection(string collectionName)
        {
            if (collectionService.IsCollectionNameValid(collectionName)) return Conflict($"{collectionName} is already created");
            var collection = collectionService.AddCollection(collectionName);

            if (collection != null)
            {
                logger.LogInformation($"Collection created: {collection.Name}");
                return CreatedAtRoute("GetCollection", new { collectionName }, collection);
            }
            return NotFound();
        }

        /// <summary>
        ///     Add existed source to collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
        [HttpPut("{collectionName}/{sourceId}", Name = "AddSourceToCollection")]
        public ActionResult AddSourceToCollection(string collectionName, int sourceId)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict($"There is no {collectionName} collection");
            if (!sourceService.IsSourceValid(sourceId)) return Conflict($"There is no such source");

            var collection = collectionService.AddSourceToCollection(collectionName, sourceId);

            if (collection != null)
                return CreatedAtRoute("GetCollection", new { collectionName, withIncludes = true }, collection);
            return BadRequest();
        }

        /// <summary>
        ///     Delete source from collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpDelete(Name = "DeleteSourceFromCollection")]
        public ActionResult DeleteSourceFromCollection(string collectionName, int sourceId)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict($"There is no {collectionName}");
            if (!collectionService.IsCollectionContainSource(collectionName, sourceId)) return Conflict($"There is no such in {collectionName}");

            var isDeleted = collectionService.DeleteSourceFromCollection(collectionName, sourceId);

            if (isDeleted)
            {
                logger.LogInformation($"Source {sourceId} deleted from {collectionName}");
                return NoContent();
            }
            return NotFound();

        }

        /// <summary>
        ///     Update collection name
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = cacheExpiration)]
        [HttpPut(Name = "UpdateCollectionName")]
        public ActionResult UpdateCollectionName(string collectionName, string newName)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict($"There is no {collectionName} collection");

            if (collectionService.IsCollectionNameValid(newName)) return Conflict($"Collection {newName} is already created");

            var updatedCollection = collectionService.EditCollectionName(collectionName, newName);

            if (updatedCollection != null)
            {
                logger.LogInformation($"Collection {collectionName} is changed to {newName}");
                return CreatedAtRoute("GetCollection", new { collectionName, withIncludes = true }, updatedCollection);
            }
            return BadRequest();
        }

        /// <summary>
        ///     Delete specific collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpDelete("{collectionName}", Name = "DeleteCollection")]
        public ActionResult DeleteCollection(string collectionName)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict($"There is no {collectionName} collection");

            var isDeleted = collectionService.DeleteCollection(collectionName);

            if (isDeleted)
            {
                logger.LogInformation($"Deleted collection : {collectionName}");
                return NoContent();
            }
            return BadRequest();
        }
    }
}