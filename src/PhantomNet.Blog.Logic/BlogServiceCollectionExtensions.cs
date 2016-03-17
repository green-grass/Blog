using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PhantomNet.Blog;
using PhantomNet.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlogServiceCollectionExtensions
    {
        public static BlogBuilder AddBlog<TArticle>(
            this IServiceCollection services)
            where TArticle : class
        {
            return services.AddBlog<TArticle>(setupAction: null);
        }

        public static BlogBuilder AddBlog<TArticle>(
            this IServiceCollection services,
            Action<BlogOptions> setupAction)
            where TArticle : class
        {
            // Services used by identity
            services.AddOptions();

            // Blog services
            services.TryAddScoped<ArticleManager<TArticle>, ArticleManager<TArticle>>();
            services.TryAddScoped<IArticleValidator<TArticle>, ArticleValidator<TArticle>>();
            services.TryAddScoped<ILookupNormalizer, LowerInvariantLookupNormalizer>();
            services.TryAddScoped<BlogErrorDescriber>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new BlogBuilder(typeof(TArticle), services);
        }
    }
}
