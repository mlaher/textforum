using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using textforum.data.contexts;

namespace textforum.data
{
    public static class ServiceRegistration
    {
        public static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TextForumDatabaseContext>(o =>
            {
                o.UseSqlServer(configuration["ConnectionStrings:SQLConnection"]);
            });
        }
    }
}
