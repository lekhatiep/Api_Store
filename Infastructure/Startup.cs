using Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infacstructure
{
    public static class Startup
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString) =>
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString)); // will be created in web project root
    }
}
