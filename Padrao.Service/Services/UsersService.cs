using Padrao.Domain.Entities;
using Padrao.Domain.Interfaces;
using Padrao.Domain.Request;
using Padrao.Infra.Repository;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Padrao.Service.Interface;
using kriptografo;


namespace Padrao.Service.Services
{
    public class UsersService : BaseService, IUsersService
    {
        private readonly UsersRepository _dalUser;

        private readonly Cryptography _cryptography;
        public UsersService(IConfiguration configuration, IResponse response) : base(response)
        {
            _dalUser = new(configuration);
            _cryptography = new Cryptography(configuration);
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

            newUser.PassWord = _cryptography.Encrypt(newUser.PassWord);
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
