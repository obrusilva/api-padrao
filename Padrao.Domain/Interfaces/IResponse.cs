using Padrao.Domain.Virtual;
using System.Collections.Generic;

namespace Padrao.Domain.Interfaces
{
    public interface IResponse
    {
        bool ContainsError();
        List<ErrorResponse> GetErrors();
        void AddError(ErrorResponse error);
        void UpdateMessage(string message);
        string GetMessage();

    }
}
