using BlogApplication_From_scratch_.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication_From_scratch_.Data
{
    public class BloggDbContext: DbContext
    {
        public BloggDbContext(DbContextOptions<BloggDbContext> options):base(options) { }

        public DbSet<BlogPost> Posts { get; set; }
        public DbSet<PostTag> Tags { get; set; }   
    }
}
