using Microsoft.AspNetCore.Http;
using Padrao.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Padrao.Domain.Virtual
{
    public class UserAuthenticated : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public UserAuthenticated(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public int Id => Convert.ToInt32(GetClaims().FirstOrDefault(a => a.Claim.Equals("sub"))?.Value);

        public string Name => GetClaims().FirstOrDefault(a => a.Claim.Equals("user"))?.Value;

        public List<ClaimUser> Claims => GetClaims();

        public bool Authenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool ContainsRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public List<ClaimUser> GetClaims()
        {
            var list = new List<ClaimUser>();
            foreach (var claim in _accessor.HttpContext.User.Claims)
            {
                list.Add(new ClaimUser { Claim = claim.Type, Value = claim.Value });
            }
            return list;

        }
    }
}
