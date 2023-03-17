using Padrao.Domain.Interfaces;
using Padrao.Domain.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Padrao.Service.Interface;

namespace Padrao.APi.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService _userService;
        public UsersController(IResponse response, IUsersService usersService) : base(response)
        {
            _userService = usersService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> NewUser(NewUserRequest jsonRequest) => JsonResponse(await _userService.New(jsonRequest));
    }
}
