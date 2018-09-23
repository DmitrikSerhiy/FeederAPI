﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Freeder.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Feeder.API.Controllers
{
    [Route("api/sources")]
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