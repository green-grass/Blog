using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PhantomNet;
using PhantomNet.Blog;
using PhantomNet.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlogServiceCollectionExtensions
    {
        public static BlogBuilder AddBlog<TArticle, TCategory, TBlogger>(
            this IServiceCollection services, Action<BlogOptions> setupAction = null)
            where TArticle : class
            where TCategory : class
            where TBlogger : class
        {
            return services.AddBlog<TArticle, TCategory, TBlogger, IBlogModuleMarker>(setupAction);
        }

        public static BlogBuilder AddBlog<TArticle, TCategory, TBlogger, TModuleMarker>(
            this IServiceCollection services, Action<BlogOptions> setupAction = null)
            where TArticle : class
            where TCategory : class
            where TBlogger : class
            where TModuleMarker : IBlogModuleMarker
        {
            // Services used by identity
            services.AddOptions();

            #region Blog services

            // Article
            services.TryAddScoped<ArticleManager<TArticle, TModuleMarker>>();
            services.TryAddScoped<IEntityValidator<TArticle, ArticleManager<TArticle, TModuleMarker>>, ArticleValidator<TArticle, ArticleManager<TArticle, TModuleMarker>>>();
            services.TryAddScoped<IEntityCodeGenerator<TArticle, ArticleManager<TArticle, TModuleMarker>>, UrlFriendlyCodeGenerator<TArticle, ArticleManager<TArticle, TModuleMarker>>>();

            // Category
            services.TryAddScoped<CategoryManager<TCategory, TModuleMarker>>();
            services.TryAddScoped<IEntityValidator<TCategory, CategoryManager<TCategory, TModuleMarker>>, CategoryValidator<TCategory, CategoryManager<TCategory, TModuleMarker>>>();
            services.TryAddScoped<IEntityCodeGenerator<TCategory, CategoryManager<TCategory, TModuleMarker>>, UrlFriendlyCodeGenerator<TCategory, CategoryManager<TCategory, TModuleMarker>>>();

            // Blogger
            services.TryAddScoped<BloggerManager<TBlogger, TModuleMarker>>();
            services.TryAddScoped<IEntityValidator<TBlogger, BloggerManager<TBlogger, TModuleMarker>>, BloggerValidator<TBlogger, BloggerManager<TBlogger, TModuleMarker>>>();
            services.TryAddScoped<IEntityCodeGenerator<TBlogger, BloggerManager<TBlogger, TModuleMarker>>, UrlFriendlyCodeGenerator<TBlogger, BloggerManager<TBlogger, TModuleMarker>>>();

            #endregion

            services.TryAddScoped<ILookupNormalizer, LowerInvariantLookupNormalizer>();
            services.TryAddScoped<BlogErrorDescriber>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new BlogBuilder(
                typeof(TArticle), typeof(TCategory), typeof(TBlogger),
                services);
        }
    }
}
