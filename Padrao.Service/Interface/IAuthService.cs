using Padrao.Domain.Request;
using Padrao.Domain.Virtual;
using System.Threading.Tasks;

namespace Padrao.Service.Interface
{
    public interface IAuthService
    {
        Task<UserLogin> Login(LoginRequest login);
    }
}