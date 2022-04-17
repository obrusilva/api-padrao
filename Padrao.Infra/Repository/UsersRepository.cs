using Functions.Util.Data;
using Padrao.Domain.Request;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padrao.Infra.Repository
{
    public class UsersRepository : BaseRepository
    {
        public UsersRepository(IConfiguration configuration) :base (configuration)
        {
        }
        public async Task<Domain.Entities.Users> GetAsync(string user, string email = null)
        {
            StringBuilder query = new();
            List <ParametersScript> parameters = new();
            query.Append("SELECT Id, UserName, Email, PassWord, Name from Users where 1=1");
            if (!string.IsNullOrWhiteSpace(user))
            {
                query.Append(" and UserName = @UserName");
                parameters.Add(new ParametersScript { Name = "@UserName", Value = user });
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query.Append(" and email = @email");
                parameters.Add(new ParametersScript { Name = "@email", Value = email });
            }

            var result = await DataContext.ExecuteQueryAsync<Domain.Entities.Users>(query.ToString(), parameters);
            return result.FirstOrDefault();

        }
        public async Task NewAsync (NewUserRequest login)
        {

            StringBuilder query = new();
            query.Append("INSERT INTO Users (UserName, email,password, name)");
            query.Append("  Values(@UserName, @Email, @PassWord,@name)");
            List<ParametersScript> parameters = new();
            parameters.Add(new ParametersScript { Name = "@UserName", Value = login.UserName, });
            parameters.Add(new ParametersScript { Name = "@Email", Value = login.Email, });
            parameters.Add(new ParametersScript { Name = "@PassWord", Value = login.PassWord, });
            parameters.Add(new ParametersScript { Name = "@name", Value = login.Name, });
            await DataContext.ExecuteScriptAsync(query.ToString(), parameters);

        }
    }
}
