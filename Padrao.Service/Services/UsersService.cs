using Functions.Util.Cryptography;
using Padrao.Domain.Entities;
using Padrao.Domain.Interfaces;
using Padrao.Domain.Request;
using Padrao.Domain.Virtual;
using Padrao.Infra.Repository;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Padrao.Service.Interface;

namespace Padrao.Service.Services
{
    public class UsersService : BaseService, IUsersService
    {
        private readonly UsersRepository _dalUser;
        public UsersService(IConfiguration configuration, IResponse response) : base(response)
        {
            _dalUser = new(configuration);
        }

        public async Task<NewUserRequest> New(NewUserRequest newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.UserName))
                newUser.UserName = newUser.Email;

            var user = await GetByUserAsync(newUser.UserName);
            if (user != null)
            {
                AddError("Usuario já existe!");
                return null;
            }

            user = await GetByEmailAsync(newUser.Email);
            if (user != null)
            {
                AddError("Esse e-mail já possui usuario cadastrado!");
                return null;
            }

            newUser.PassWord = Cryptography.Encrypts(newUser.PassWord);
            await _dalUser.NewAsync(newUser);
            UpdateMessage("Usuario cadastrado com sucesso!");
            return newUser;
        }

        public async Task<Users> GetByUserAsync(string user)
        {
            var userDb = await _dalUser.GetAsync(user);
            if (userDb == null)
                return null;

            return userDb;
        }
        public async Task<Users> GetByEmailAsync(string email)
        {
            var userDb = await _dalUser.GetAsync(null, email);
            if (userDb == null)
                return null;

            return userDb;
        }

    }
}
