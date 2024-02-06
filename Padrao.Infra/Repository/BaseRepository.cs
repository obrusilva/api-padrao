using Konekti.BD;
using Microsoft.Extensions.Configuration;

namespace Padrao.Infra.Repository
{
    public class BaseRepository
    {
        private readonly IConfiguration _configuration;
        protected readonly DataContext DataContext;
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            DataContext = new(_configuration);
        }
    }
}
