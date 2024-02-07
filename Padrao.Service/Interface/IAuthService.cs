using Padrao.Domain.Request;
using Padrao.Domain.Virtual;
using System.Threading.Tasks;

namespace Padrao.Service.Interface
{
    public interface IAuthService
    {
        string Criptografa(string texto);
        Task<UserLogin> Login(LoginRequest login);
    }
}