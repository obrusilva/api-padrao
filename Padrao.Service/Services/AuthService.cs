using Functions.Util.Cryptography;
using Functions.Util.Functions;
using Padrao.Domain.Interfaces;
using Padrao.Domain.Request;
using Padrao.Domain.Virtual;
using Padrao.Infra.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Padrao.Service.Interface;

namespace Padrao.Service.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AppToken _appToken;
        private readonly IUsersService _usersService;
        public AuthService(IResponse response, IOptions<AppToken> appToken, IUsersService usersService) : base(response)
        {
            _appToken = appToken.Value;
            _usersService = usersService;
        }
        public async Task<UserLogin> Login(LoginRequest login)
        {
            
            var user = await _usersService.GetByUserAsync(login.UserName);
            if (user == null)
            {
                user = await _usersService.GetByEmailAsync(login.UserName);
                if (user == null)
                {
                    AddError("Usuario ou senha inválidos!");
                    return null;
                }
            }
            var passwordCripty = Cryptography.Encrypts(login.PassWord);
            if (!FunctionsString.EqualsString(passwordCripty, user.PassWord))
            {
                AddError("Usuario ou senha inválidos!");
                return null;
            }
            return await NewTokenAsync(user.UserName, _appToken);
        }
        private async Task<UserLogin> NewTokenAsync(string user, AppToken appToken)
        {
            List<Claim> claims = new();
            var userValid = await _usersService.GetByUserAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userValid.Id.ToString()));
            claims.Add(new Claim("user", userValid.UserName));
            claims.Add(new Claim("name", userValid.Name));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.Now).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64));

            var identity = new ClaimsIdentity();
            identity.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appToken.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = appToken.Issuer,
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(appToken.ExpireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var tokenEncode = tokenHandler.WriteToken(token);
            UpdateMessage("Login Efetuado");
            return new UserLogin { Id = userValid.Id, User = userValid.UserName, Email = userValid.Email, Name = userValid.Name, Token = tokenEncode, ExpireIn = ToUnixEpochDate(DateTime.Now) };
        }
        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }
}
