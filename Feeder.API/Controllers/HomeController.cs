using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Freeder.BLL;
using Microsoft.AspNetCore.Mvc;

namespace Feeder.Controllers
{
    // [Produces("application/json")]
    [Route("api/feed")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private RssService rssService;
        public HomeController(RssService RssService)
        {
            rssService = RssService;
        }

        [HttpGet("{Id}", Name = "GetFeed")]
        public ActionResult GetRssFeed(int Id)
        {
            var feed = rssService.GetFeed(Id);

            if (feed != null) return Ok(feed);
            return NotFound();
        }

        [HttpGet(Name = "GetFeeds")]
        public ActionResult GetRssFeeds()
        {
            var feed = rssService.GetFeeds();

            if (feed != null) return Ok(feed);
            return NotFound();
        }

    }
}
