using Padrao.Domain.Interfaces;
using Padrao.Domain.Virtual;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Padrao.APi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        private readonly IResponse _response;
     
        protected BaseController(IResponse response)
        {
            _response = response;
        }
        protected bool ValidRespose()
        {
            return !_response.ContainsError();
        }
        protected ActionResult JsonResponse(object result = null)
        {
            if (ValidRespose())
                return Ok(new ResultJson(_response.GetMessage(), result));

            return BadRequest(new ResultJson(_response.GetMessage(), _response.GetErrors().Select(n => n.Message).ToList()));
        }
        protected ActionResult JsonResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) AddErrorsModelInvalid(modelState);
            return JsonResponse();
        }
        protected void AddErrorsModelInvalid(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                var errorMessage = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                AddErrors(errorMessage);
            }
        }
        protected void AddErrors(string message)
        {
            _response.AddError(new ErrorResponse(message));
        }
        protected void UpdateMessage(string message)
        {
            _response.UpdateMessage(message);
        }
    }
}
