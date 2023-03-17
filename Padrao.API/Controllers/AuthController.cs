using Padrao.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Padrao.Domain.Request;
using Padrao.Service.Interface;

namespace Padrao.APi.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController( IResponse response, IAuthService  authService) : base(response)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest jsonRequest) => JsonResponse(await _authService.Login(jsonRequest));

    }
}
