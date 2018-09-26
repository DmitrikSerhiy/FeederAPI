using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Feeder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        /// <summary>
        ///     Welcome Page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("welcome")]
        public ActionResult<string> Welcome()
        {
            return Ok("Use http://localhost:44399/swagger to get info about http methods");
        }
    }
}