using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RequestPathModifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            string response = "success";

            if (HttpContext.Items.ContainsKey("INTERNAL_CLIENT_ID"))
            {
                response += " " + HttpContext.Items["INTERNAL_CLIENT_ID"];
            }

            return Ok(response);
        }

    }
}
