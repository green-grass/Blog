using System;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PhantomNet.Blog;
using PhantomNet.Blog.EntityFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlogEntityFrameworkBuilderExtensions
    {
        public static BlogBuilder AddEntityFrameworkStores<TContext>(this BlogBuilder builder)
           where TContext : DbContext
        {
            builder.Services.TryAdd(GetDefaultServices(builder.ArticleType, typeof(TContext)));
            return builder;
        }

        public static BlogBuilder AddEntityFrameworkStores<TContext, TKey>(this BlogBuilder builder)
            where TContext : DbContext
            where TKey : IEquatable<TKey>
        {
            builder.Services.TryAdd(GetDefaultServices(builder.ArticleType, typeof(TContext), typeof(TKey)));
            return builder;
        }

        private static IServiceCollection GetDefaultServices(Type articleType, Type contextType, Type keyType = null)
        {
            var articleStoreType = keyType == null ?
                typeof(ArticleStore<,>).MakeGenericType(articleType, contextType) :
                typeof(ArticleStore<,,>).MakeGenericType(articleType, contextType, keyType);

            var services = new ServiceCollection();

            services.AddScoped(typeof(IArticleStore<>).MakeGenericType(articleType), articleStoreType);

            return services;
        }
    }
}
