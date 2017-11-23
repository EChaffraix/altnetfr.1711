using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using common;
using Microsoft.AspNetCore.Mvc;

namespace service.Controllers
{
    [Route("[controller]")]
    public class PingController : Controller
    {
        [HttpGet]
        public IActionResult Version()
        {
            var message = new VersionMessage();
            message.Version = "1.0";
            message.Timestamp = DateTime.UtcNow;
            return Ok(message);
        }
    }

}
