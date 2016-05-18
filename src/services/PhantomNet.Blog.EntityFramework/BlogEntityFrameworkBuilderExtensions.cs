using System;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PhantomNet.Blog;
using PhantomNet.Blog.EntityFramework;
using PhantomNet.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlogEntityFrameworkBuilderExtensions
    {
        public static BlogBuilder AddEntityFrameworkStores<TContext>(this BlogBuilder builder)
            where TContext : DbContext
        {
            return builder.AddEntityFrameworkStores<TContext, IBlogModuleMarker>();
        }

        public static BlogBuilder AddEntityFrameworkStores<TContext, TModuleMarker>(this BlogBuilder builder)
            where TContext : DbContext
            where TModuleMarker : IBlogModuleMarker
        {
            builder.Services.TryAdd(GetDefaultServices<TModuleMarker>(
                builder.ArticleType, builder.CategoryType, builder.BloggerType,
                typeof(TContext)));
            return builder;
        }

        public static BlogBuilder AddEntityFrameworkStores<TContext, TKey, TModuleMarker>(this BlogBuilder builder)
            where TContext : DbContext
            where TKey : IEquatable<TKey>
            where TModuleMarker : IBlogModuleMarker
        {
            builder.Services.TryAdd(GetDefaultServices<TModuleMarker>(
                builder.ArticleType, builder.CategoryType, builder.BloggerType,
                typeof(TContext), typeof(TKey)));
            return builder;
        }

        private static IServiceCollection GetDefaultServices<TModuleMarker>(
            Type articleType, Type categoryType, Type bloggerType,
            Type contextType, Type keyType = null)
            where TModuleMarker : IBlogModuleMarker
        {
            var services = new ServiceCollection();

            #region Accessors

            var articleAccessorType = keyType == null ?
                typeof(ArticleAccessor<,,>).MakeGenericType(articleType, categoryType, bloggerType) :
                typeof(ArticleAccessor<,,,>).MakeGenericType(articleType, categoryType, bloggerType, keyType);

            var categoryAccessorType = keyType == null ?
                typeof(CategoryAccessor<,,>).MakeGenericType(articleType, categoryType, bloggerType) :
                typeof(CategoryAccessor<,,,>).MakeGenericType(articleType, categoryType, bloggerType, keyType);

            var bloggerAccessorType = keyType == null ?
                typeof(BloggerAccessor<,,>).MakeGenericType(articleType, categoryType, bloggerType) :
                typeof(BloggerAccessor<,,,>).MakeGenericType(articleType, categoryType, bloggerType, keyType);

            services.AddScoped(typeof(IArticleAccessor<>).MakeGenericType(articleType), articleAccessorType);
            services.AddScoped(typeof(INameBasedEntityAccessor<>).MakeGenericType(articleType), articleAccessorType);
            services.AddScoped(typeof(ICategoryAccessor<>).MakeGenericType(categoryType), categoryAccessorType);
            services.AddScoped(typeof(INameBasedEntityAccessor<>).MakeGenericType(categoryType), categoryAccessorType);
            services.AddScoped(typeof(IBloggerAccessor<>).MakeGenericType(bloggerType), bloggerAccessorType);
            services.AddScoped(typeof(INameBasedEntityAccessor<>).MakeGenericType(bloggerType), bloggerAccessorType);

            #endregion

            #region Stores

            var articleStoreType = keyType == null ?
                typeof(ArticleStore<,,,,>).MakeGenericType(articleType, categoryType, bloggerType, contextType, typeof(TModuleMarker)) :
                typeof(ArticleStore<,,,,,>).MakeGenericType(articleType, categoryType, bloggerType, contextType, keyType, typeof(TModuleMarker));

            var categoryStoreType = keyType == null ?
                typeof(CategoryStore<,,,,>).MakeGenericType(articleType, categoryType, bloggerType, contextType, typeof(TModuleMarker)) :
                typeof(CategoryStore<,,,,,>).MakeGenericType(articleType, categoryType, bloggerType, contextType, keyType, typeof(TModuleMarker));

            var bloggerStoreType = keyType == null ?
                typeof(BloggerStore<,,,,>).MakeGenericType(articleType, categoryType, bloggerType, contextType, typeof(TModuleMarker)) :
                typeof(BloggerStore<,,,,,>).MakeGenericType(articleType, categoryType, bloggerType, contextType, keyType, typeof(TModuleMarker));

            services.AddScoped(typeof(IArticleStore<,>).MakeGenericType(articleType, typeof(TModuleMarker)), articleStoreType);
            services.AddScoped(typeof(ICategoryStore<,>).MakeGenericType(categoryType, typeof(TModuleMarker)), categoryStoreType);
            services.AddScoped(typeof(IBloggerStore<,>).MakeGenericType(bloggerType, typeof(TModuleMarker)), bloggerStoreType);

            #endregion

            return services;
        }
    }
}
