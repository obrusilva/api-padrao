using Padrao.Domain.Virtual;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Padrao.Domain.Interfaces
{
    public interface IRequest
    {
        Task<ResponseApi<T>> GetAsync<T>(string json, string uri, bool returnObject, bool returnList = true, List<Parameters> parameters = null, List<Headers> headers = null) where T : new();
        //Task<T> PostAsync<T>(string url);
    }
}
