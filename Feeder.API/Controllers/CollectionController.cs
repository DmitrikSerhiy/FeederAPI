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
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private CollectionService collectionService;
        private SourceService sourceService;
        private readonly ILogger logger;
        public CollectionController(CollectionService CollectionService, SourceService SourceService, ILoggerFactory LoggerFactory)
        {
            collectionService = CollectionService;
            sourceService = SourceService;
            logger = LoggerFactory.CreateLogger<CollectionController>();
        }

        [HttpGet("{CollectionName}", Name = "GetCollection")]
        public ActionResult GetCollection(string CollectionName)
        {
            if (!collectionService.IsCollectionNameValid(CollectionName)) return Conflict("There is no such collection in db");
            var col = collectionService.GetCollection(CollectionName);

            if (col != null)
            {
                logger.LogInformation($"Collection: {col.Name}");
                return Ok(col);
            }
            return NotFound();
        }

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




        [HttpPost("{CollectionName}", Name = "AddCollection")]
        public ActionResult AddCollection(string CollectionName)
        {
            if (collectionService.IsCollectionNameValid(CollectionName)) return Conflict($"{CollectionName} is already created");
            var collection = collectionService.AddCollection(CollectionName);

            if (collection != null)
            {
                logger.LogInformation($"Collection created: {collection.Name}");
                return CreatedAtRoute("GetCollection", new { CollectionName }, collection);
            }
            return NotFound();
        }

        [HttpPut(Name ="AddSourceToCollection")]
        public ActionResult AddSourceToCollection(string collectionName, string sourceName)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict($"There is no {collectionName} collection");
            if(!sourceService.IsSourceNameValid(sourceName)) return Conflict($"There is no {sourceName} source");

            var collection = collectionService.AddSourceToCollection(sourceName, collectionName);

            if(collection != null)
            {
                logger.LogInformation($"Source {sourceName} added to the collection {collectionName}");
                return CreatedAtRoute("GetCollection", new { CollectionName = collectionName }, collection);
            }
            return BadRequest();
        }


        [HttpPut("{collectionName}", Name = "UpdateCollectionName")]
        public ActionResult UpdateCollectionName(string collectionName, string newName)
        {
            if (!collectionService.IsCollectionNameValid(collectionName)) return Conflict($"There is no {collectionName} collection");

            if (collectionService.IsCollectionNameValid(newName)) return Conflict($"Collection {newName} is already created");

            var updatedCollection = collectionService.EditCollectionName(collectionName, newName);

            if(updatedCollection != null)
            {
                logger.LogInformation($"Collection {collectionName} is changed to {newName}");
                return CreatedAtRoute("GetCollection", new { CollectionName = collectionName }, updatedCollection);
            }
            return BadRequest();
        }

        [HttpDelete("{CollectionName}", Name="DeleteCollection")]
        public ActionResult DeleteCollection(string CollectionName)
        {
            if (!collectionService.IsCollectionNameValid(CollectionName)) return Conflict($"There is no {CollectionName} collection");

            var isDeleted = collectionService.DeleteCollection(CollectionName);

            if (isDeleted)
            {
                logger.LogInformation($"Deleted collection : {CollectionName}");
                return NoContent();
            }
            return BadRequest();
        }
    }
}