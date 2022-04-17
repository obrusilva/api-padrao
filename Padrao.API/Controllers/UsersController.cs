using Padrao.Domain.Interfaces;
using Padrao.Domain.Request;
using Padrao.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Padrao.APi.Controllers
{
    public class UsersController : BaseController
    {
        private readonly UsersService _userService;
        public UsersController(IConfiguration configuration, IResponse response) : base(response)
        {
            _userService = new(configuration, response);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> NewUser(NewUserRequest jsonRequest) => JsonResponse(await _userService.New(jsonRequest));
    }
}
