using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Models;

namespace MyBlog.Data
{
    public class BlogDbContext : DbContext
    {
        public const string CONNECTION_STRING = "Data Source=(localdb)\\mssqllocaldb;Integrated Security=True;Database=BlogAppDb";
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<UserSub> UserSubs { get; set; }
        public DbSet<NewsLike> NewsLikes { get; set; }

        public BlogDbContext() { }
        public BlogDbContext(DbContextOptions<BlogDbContext> contextOptions) : base(contextOptions) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(CONNECTION_STRING);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.News)
                .WithOne(x => x.Author)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
