using Padrao.Domain.Interfaces;
using Padrao.Domain.Virtual;
using System.Linq;

namespace Padrao.Service.Services
{
    public abstract class BaseService
    {
        private readonly IResponse _response;
        protected BaseService (IResponse response)
        {
            _response = response;
        }
        protected void AddError(string message)
        {
            var errors  = _response.GetErrors();
            if(!errors.Where(w => w.Message.Equals(message)).Any())
                _response.AddError(new ErrorResponse(message));
        }
        protected void UpdateMessage(string message)
        {
            _response.UpdateMessage(message);
        }
    }
}
