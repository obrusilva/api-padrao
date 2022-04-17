using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padrao.Domain.Virtual
{
    public class AppToken
    {
        public string Secret { get; set; }
        public int ExpireMinutes { get; set; }
        public string Issuer { get; set; }
    }
}
