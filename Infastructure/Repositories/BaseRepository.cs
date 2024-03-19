using Infastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infastructure.Repositories
{
    public class BaseRepository<T> where T : class
    {
        private readonly AppDbContext context;
        private readonly DbSet<T> table;
        public BaseRepository(AppDbContext context)
        {
            this.context = context;
            this.table = context.Set<T>();
        }
    }
}
