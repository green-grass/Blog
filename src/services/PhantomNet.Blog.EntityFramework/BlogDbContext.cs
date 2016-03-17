using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace PhantomNet.Blog.EntityFramework
{
    public class BlogDbContext : BlogDbContext<Article> { }

    public class BlogDbContext<TArticle> : BlogDbContext<TArticle, string>
        where TArticle : Article<string>
    { }

    public class BlogDbContext<TArticle, TKey> : DbContext
        where TArticle : Article<TKey>
        where TKey : IEquatable<TKey>
    {
        public BlogDbContext(DbContextOptions options) : base(options) { }

        public BlogDbContext(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public BlogDbContext(IServiceProvider serviceProvider, DbContextOptions options) : base(serviceProvider, options) { }

        public BlogDbContext() { }

        public string DefaultSchema { get; }
            = nameof(BlogDbContext<TArticle, TKey>).Replace("DbContext", string.Empty);

        public DbSet<TArticle> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);

            modelBuilder.Entity<TArticle>(b => {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.UrlFriendlyTitle).HasName("UrlFriendlyTitleIndex");
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.Title).HasMaxLength(256);
                b.Property(u => u.UrlFriendlyTitle).HasMaxLength(256);
            });
        }
    }
}
