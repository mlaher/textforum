using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using textforum.data.classes;

namespace textforum.data.contexts
{
    public class TextForumDatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        public TextForumDatabaseContext(DbContextOptions<TextForumDatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
