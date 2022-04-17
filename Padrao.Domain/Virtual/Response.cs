using Padrao.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Padrao.Domain.Virtual
{
    public class Response : IResponse
    {
        protected List<ErrorResponse> _errors;
        protected bool _isError;
        protected bool _notAcess;
        protected string _message;
        public Response()
        {
            _errors = new List<ErrorResponse>();
            _isError = false;
            _notAcess = false;
            _message = string.Empty;
        }
        public void AddError(ErrorResponse error)
        {
            _isError = true;
            _errors.Add(error);
        }
        public bool ContainsError() => _isError;
        public List<ErrorResponse> GetErrors() => _errors;
        public string GetMessage()
        {
            if (!string.IsNullOrWhiteSpace(_message))
                return _message;

            if (ContainsError())
                return GetErrors().Select(s => s.Message).FirstOrDefault();

            return "Sucesso";
        }
        public void UpdateMessage(string message) => _message = message;
      
    }
}