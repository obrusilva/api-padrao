using Padrao.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Padrao.Service.Services;
using Padrao.Domain.Request;
using Microsoft.Extensions.Options;
using Padrao.Domain.Virtual;

namespace Padrao.APi.Controllers
{
    public class AuthController : BaseController
    {
        private readonly AuthService _authService;

        public AuthController(IConfiguration configuration, IResponse response, IOptions<AppToken> appToken) : base(response)
        {
            _authService = new(configuration, response, appToken);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest jsonRequest) => JsonResponse(await _authService.Login(jsonRequest));

    }
}
