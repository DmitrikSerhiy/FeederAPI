using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Freeder.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Feeder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private SourceService sourceService;
        private readonly ILogger logger;

        public SourceController(SourceService SourceService, ILoggerFactory LoggerFactory)
        {
            sourceService = SourceService;
            logger = LoggerFactory.CreateLogger<SourceController>();
        }

        [HttpGet("{Name}", Name = "GetSource")]
        public ActionResult GetSource(string Name)
        {
            var source = sourceService.GetSource(Name);

            logger.LogInformation($"Source: {source?.Name}");

            if (source != null) return Ok(source);
            return NotFound();
        }

        [HttpGet(Name = "GetSources")]
        public ActionResult GetSources()
        {
            var sources = sourceService.GetSources();

            logger.LogInformation($"Sources: {string.Join(", ", sources.Select(s => s.Name))}");

            if (sources != null) return Ok(sources);
            return NotFound();
        }

        [HttpPost(Name = "AddSource")]
        public ActionResult AddSource(string Name, string Url)
        {
            var newSource = sourceService.AddSource(Name, Url);

            if (newSource == null)
            {
                logger.LogInformation($"Source {newSource?.Name} is already in db");
                return Conflict("Already in db");
            }
            logger.LogInformation($"Source {newSource?.Name} has been added");
            return CreatedAtRoute("GetSources", new { Name }, newSource);
        }
    }
}