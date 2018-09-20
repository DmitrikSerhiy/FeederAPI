using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Freeder.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Feeder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private SourceService sourceService;
        public SourceController(SourceService SourceService)
        {
            sourceService = SourceService;
        }

        [HttpGet("{Name}", Name = "GetSource")]
        public ActionResult GetSource(string Name)
        {
            var source = sourceService.GetSource(Name);

            if (source != null) return Ok(source);
            return NotFound();
        }


        [HttpGet(Name = "GetSources")]
        public ActionResult GetSources()
        {
            var sources = sourceService.GetSources();

            if (sources != null) return Ok(sources);
            return NotFound();
        }


        // [HttpPost("{Name}/{Url}", Name ="AddSource")]
        [HttpPost(Name = "AddSource")]
        public ActionResult AddSource(string Name, string Url)
        {
            var newSource = sourceService.AddSource(Name, Url);
            if(newSource == null) return Conflict("Already in db");
            return CreatedAtRoute("GetSources", new { Name }, newSource);
        }
    }
}