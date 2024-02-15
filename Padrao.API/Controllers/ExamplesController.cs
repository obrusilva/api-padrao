using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Padrao.APi.Controllers;
using Padrao.Domain.Interfaces;
using System.Threading;

namespace Padrao.Api.Controllers
{
    public class ExamplesController(IResponse response) : BaseController(response)
    {
        [HttpGet]
        [EnableRateLimiting("ip")]
        [Route("rate-limiting")]
        public IActionResult RateLimitingIP()
        {
            Thread.Sleep(10000);
            return JsonResponse();
        }
        
        [HttpPost]
        [EnableRateLimiting("concurrency")]
        [Route("rate-limiting-post")]
        public IActionResult ReteLimitingPost()
        {
            Thread.Sleep(10000);
            return JsonResponse();
        }
    }
}
