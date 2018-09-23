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

            if (source != null)
            {
                logger.LogInformation($"Source: {source?.Name}");
                return Ok(source);
            }
            return NotFound();
        }

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

        [HttpPost(Name = "AddSource")]
        public ActionResult AddSource(string Name, string Url)
        {
            if (sourceService.IsSourceNameValid(Name)) return Conflict($"Source {Name} is already created");

            var newSource = sourceService.AddSource(Name, Url);

            if (newSource != null)
            {
                logger.LogInformation($"Source {newSource?.Name} has been added");
                return CreatedAtRoute("GetSources", new { Name }, newSource);
            }
            return BadRequest();
        }
    }
}