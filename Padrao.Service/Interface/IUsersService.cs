using Padrao.Domain.Entities;
using Padrao.Domain.Request;
using System.Threading.Tasks;

namespace Padrao.Service.Interface
{
    public interface IUsersService
    {
        Task<Users> GetByEmailAsync(string email);
        Task<Users> GetByUserAsync(string user);
        Task<NewUserRequest> New(NewUserRequest newUser);
    }
}