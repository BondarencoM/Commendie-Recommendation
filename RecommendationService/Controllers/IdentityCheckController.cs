using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RecommendationService.Controllers
{
    [Route("identity-check")]
    [ApiController]
    [Authorize]
    public class IdentityCheckController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetInterests()
        {
            return "hello world";
        }
    }
}
