using Konekti.BD;
using Microsoft.Extensions.Configuration;

namespace Padrao.Infra.Repository
{
    public class BaseRepository
    {
        protected readonly DataContext DataContext;
        public BaseRepository(IConfiguration configuration)
        {
            DataContext = new(configuration);
        }
    }
}
