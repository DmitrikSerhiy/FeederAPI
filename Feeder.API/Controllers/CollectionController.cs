using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        /// <summary>
        ///     Used consructor injection to get all needed services 
        /// </summary>
        /// <param name="CollectionService"></param>
        /// <param name="SourceService"></param>
        /// <param name="LoggerFactory"></param>
        public CollectionController(CollectionService CollectionService, SourceService SourceService,
            ILoggerFactory LoggerFactory)
        {
            collectionService = CollectionService;
            sourceService = SourceService;
            logger = LoggerFactory.CreateLogger<CollectionController>();
        }

        /// <summary>
        ///     Get specific collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        [HttpGet("{collectionName}", Name = "GetCollection")]
        public ActionResult GetCollection(string collectionName)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict("There is no such collection in db");
            var col = collectionService.GetCollection(collectionName);

            if (col != null)
            {
                logger.LogInformation($"Collection: {collectionName}");
                return Ok(col);
            }
            return NotFound();
        }

        /// <summary>
        ///     Get all collections without sources
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetCollections")]
        public ActionResult GetCollections()
        {
            var collections = collectionService.GetCollections();

            if (collections.Count() != 0)
            {
                logger.LogInformation($"Collections: {string.Join(", ", collections.Select(s => s.Name))}");
                return Ok(collections);
            }
            return NotFound();
        }

        /// <summary>
        ///     Get specific collecton with included sources and feeds
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        [HttpGet("view/{collectionName}", Name ="ViewCollection")]
        public ActionResult ViewCollection(string collectionName)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict($"There is no {collectionName} collection");

            var collection = collectionService.ViewCollection(collectionName);

            if(collection != null)
            {
                logger.LogInformation($"Collection {collectionName} with sources: {string.Join(", ", collection.Sources.Select(s => s.Name))}");
                return Ok(collection);
            }
            return BadRequest();
        }

        /// <summary>
        ///     Add new collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
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
        ///     Add existed source to the specific collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        [HttpPut("{collectionName}/{sourceName}", Name = "AddSourceToCollection")]
        public ActionResult AddSourceToCollection(string collectionName, string sourceName)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict($"There is no {collectionName} collection");
            if(!sourceService.IsSourceNameValid(sourceName)) return Conflict($"There is no {sourceName} source");

            var collection = collectionService.AddSourceToCollection(sourceName, collectionName);

            if(collection != null)
            {
                logger.LogInformation($"Source {sourceName} added to the collection {collectionName}");
                return CreatedAtRoute("GetCollection", new { collectionName }, collection);
            }
            return BadRequest();
        }

        /// <summary>
        ///     Delete source from collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteSourceFromCollection")]
        public ActionResult DeleteSourceFromCollection(string collectionName, string sourceName)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict($"There is no {collectionName}");
            if(!collectionService.IsCollectionContainSource(collectionName, sourceName)) return Conflict($"There is no {sourceName} in {collectionName}");

            var isDeleted = collectionService.DeleteSourceFromCollection(collectionName, sourceName);

            if (isDeleted)
            {
                logger.LogInformation($"Source {sourceName} deleted from {collectionName}");
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
        [HttpPut(Name = "UpdateCollectionName")]
        public ActionResult UpdateCollectionName(string collectionName, string newName)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict($"There is no {collectionName} collection");

            if (collectionService.IsCollectionNameValid(newName)) return Conflict($"Collection {newName} is already created");

            var updatedCollection = collectionService.EditCollectionName(collectionName, newName);

            if(updatedCollection != null)
            {
                logger.LogInformation($"Collection {collectionName} is changed to {newName}");
                return CreatedAtRoute("GetCollection", new { collectionName }, updatedCollection);
            }
            return BadRequest();
        }

        /// <summary>
        ///     Delete specific collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        [HttpDelete("{collectionName}", Name="DeleteCollection")]
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