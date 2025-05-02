using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using textforum.data.classes;
using textforum.data.contexts;
using textforum.data.repositories;

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

            services.AddTransient<ITextForumRepository<User>,  TextForumRepository<User>>();
            services.AddTransient<ITextForumRepository<Post>,  TextForumRepository<Post>>();
            services.AddTransient<ITextForumRepository<PostLike>,  TextForumRepository<PostLike>>();
            services.AddTransient<ITextForumRepository<PostComment>,  TextForumRepository<PostComment>>();
            services.AddTransient<ITextForumRepository<PostTag>,  TextForumRepository<PostTag>>();
        }
    }
}
