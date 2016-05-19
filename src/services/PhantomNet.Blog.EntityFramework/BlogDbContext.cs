using System;
using Microsoft.EntityFrameworkCore;

namespace PhantomNet.Blog.EntityFramework
{
    public class BlogDbContext : BlogDbContext<Article, Category, Blogger>
    {
        public BlogDbContext(DbContextOptions options) : base(options) { }

        protected BlogDbContext() { }
    }

    public class BlogDbContext<TArticle, TCategory, TBlogger> : BlogDbContext<TArticle, TCategory, TBlogger, int>
        where TArticle : Article<TArticle, TCategory, TBlogger, int>
        where TCategory : Category<TArticle, TCategory, TBlogger, int>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, int>
    {
        public BlogDbContext(DbContextOptions options) : base(options) { }

        protected BlogDbContext() { }
    }

    public class BlogDbContext<TArticle, TCategory, TBlogger, TKey> : DbContext
        where TArticle : Article<TArticle, TCategory, TBlogger, TKey>
        where TCategory : Category<TArticle, TCategory, TBlogger, TKey>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, TKey>
        where TKey : IEquatable<TKey>
    {
        public BlogDbContext(DbContextOptions options) : base(options) { }

        protected BlogDbContext() { }

        public string DefaultSchema { get; }
            = nameof(BlogDbContext<TArticle, TCategory, TBlogger, TKey>).Replace("DbContext", string.Empty);

        public DbSet<TArticle> Articles { get; set; }

        public DbSet<TCategory> Categories { get; set; }

        public DbSet<TBlogger> Bloggers { get; set; }

        // TODO:: String length
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);

            modelBuilder.Entity<TArticle>(b => {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.UrlFriendlyTitle).HasName("UrlFriendlyTitleIndex").IsUnique();
                b.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(x => x.UrlFriendlyTitle).HasMaxLength(256);

                b.Ignore(x => x.ProcessedShortContent)
                 .Ignore(x => x.PlainShortContent)
                 .Ignore(x => x.ProcessedContent);
            });

            modelBuilder.Entity<TCategory>(b => {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.UrlFriendlyName).HasName("UrlFriendlyNameIndex").IsUnique();
                b.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(x => x.UrlFriendlyName).HasMaxLength(256);
            });

            modelBuilder.Entity<TBlogger>(b => {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.UrlFriendlyPenName).HasName("UrlFriendlyPenNameIndex").IsUnique();
                b.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(x => x.UrlFriendlyPenName).HasMaxLength(256);
            });
        }
    }
}
