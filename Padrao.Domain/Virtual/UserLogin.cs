using System.Collections.Generic;

namespace Padrao.Domain.Virtual
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public double ExpireIn { get; set; }
        public List<ClaimUser> Claims {get;set;}
    }
}
