using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace PhantomNet.Blog
{
    public class BlogBuilder
    {
        public BlogBuilder(Type articleType, IServiceCollection services)
        {
            ArticleType = articleType;
            Services = services;
        }

        #region Properties

        public Type ArticleType { get; }

        public IServiceCollection Services { get; }

        #endregion

        #region Article

        public virtual BlogBuilder AddArticleManager<T>() where T : class
        {
            var articleManagerType = typeof(ArticleManager<>).MakeGenericType(ArticleType);
            var customType = typeof(T);
            if (articleManagerType == customType ||
                !articleManagerType.GetTypeInfo().IsAssignableFrom(customType.GetTypeInfo()))
            {
                throw new InvalidOperationException(string.Format(Resources.InvalidManagerType, customType.Name, nameof(ArticleManager<object>), ArticleType.Name));
            }

            Services.AddScoped(customType, services => services.GetRequiredService(articleManagerType));
            return AddScoped(articleManagerType, customType);
        }

        public virtual BlogBuilder AddArticleStore<T>() where T : class
        {
            return AddScoped(typeof(IArticleStore<>).MakeGenericType(ArticleType), typeof(T));
        }

        public virtual BlogBuilder AddArticleValidator<T>() where T : class
        {
            return AddScoped(typeof(IArticleValidator<>).MakeGenericType(ArticleType), typeof(T));
        }

        #endregion

        public virtual BlogBuilder AddErrorDescriber<T>() where T : BlogErrorDescriber
        {
            Services.AddScoped<BlogErrorDescriber, T>();
            return this;
        }

        private BlogBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }
    }
}
