using Padrao.Domain.Virtual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padrao.Domain.Interfaces
{
    public interface IUser
    {
        int Id { get;}
        string Name { get; }
        List<ClaimUser> Claims { get; }
        bool ContainsRole(string role);
        bool Authenticated();
        List<ClaimUser> GetClaims();
    }
}
