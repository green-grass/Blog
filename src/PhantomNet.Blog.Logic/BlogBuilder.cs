using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class BlogBuilder
    {
        public BlogBuilder(
            Type articleType, Type categoryType, Type bloggerType,
            IServiceCollection services)
        {
            ArticleType = articleType;
            CategoryType = categoryType;
            BloggerType = bloggerType;

            Services = services;
        }

        #region Properties

        public Type ArticleType { get; }
        public Type CategoryType { get; }
        public Type BloggerType { get; }

        public IServiceCollection Services { get; }

        #endregion

        #region ErrorDescriber

        public virtual BlogBuilder AddErrorDescriber<T>() where T : BlogErrorDescriber
        {
            Services.AddScoped<BlogErrorDescriber, T>();
            return this;
        }

        #endregion

        #region Article

        public virtual BlogBuilder AddArticleManager<T, TModuleMarker>()
            where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            return AddManager<T, TModuleMarker>(typeof(ArticleManager<,>), ArticleType);
        }

        public virtual BlogBuilder AddArticleStore<T, TModuleMarker>()
            where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            return AddScoped(typeof(IArticleStore<,>).MakeGenericType(ArticleType, typeof(TModuleMarker)), typeof(T));
        }

        public virtual BlogBuilder AddArticleAccessor<T>() where T : class
        {
            return AddScoped(typeof(IArticleAccessor<>).MakeGenericType(ArticleType), typeof(T));
        }

        public virtual BlogBuilder AddArticleValidator<T, TModuleMarker>()
            where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            var managerType = typeof(ArticleManager<,>).MakeGenericType(ArticleType, typeof(TModuleMarker));
            return AddScoped(typeof(IEntityValidator<,>).MakeGenericType(ArticleType, managerType), typeof(T));
        }

        public virtual BlogBuilder AddArticleValidatorForManager<T, TArticleManager>()
            where T : class
            where TArticleManager : class
        {
            return AddScoped(typeof(IEntityValidator<,>).MakeGenericType(ArticleType, typeof(TArticleManager)), typeof(T));
        }

        public virtual BlogBuilder AddArticleCodeGenerator<T, TModuleMarker>()
            where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            var managerType = typeof(ArticleManager<,>).MakeGenericType(ArticleType, typeof(TModuleMarker));
            return AddScoped(typeof(IEntityCodeGenerator<,>).MakeGenericType(ArticleType, managerType), typeof(T));
        }

        public virtual BlogBuilder AddArticleCodeGeneratorForManager<T, TArticleManager>()
            where T : class
            where TArticleManager : class
        {
            return AddScoped(typeof(IEntityCodeGenerator<,>).MakeGenericType(ArticleType, typeof(TArticleManager)), typeof(T));
        }

        #endregion

        #region Category

        public virtual BlogBuilder AddCategoryManager<T, TModuleMarker>()
            where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            return AddManager<T, TModuleMarker>(typeof(CategoryManager<,>), CategoryType);
        }

        public virtual BlogBuilder AddCategoryStore<T, TModuleMarker>() where T : class
        {
            return AddScoped(typeof(ICategoryStore<,>).MakeGenericType(CategoryType, typeof(TModuleMarker)), typeof(T));
        }

        public virtual BlogBuilder AddCategoryAccessor<T>() where T : class
        {
            return AddScoped(typeof(ICategoryAccessor<>).MakeGenericType(CategoryType), typeof(T));
        }

        public virtual BlogBuilder AddCategoryValidator<T, TModuleMarker>()
            where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            var managerType = typeof(CategoryManager<,>).MakeGenericType(CategoryType, typeof(TModuleMarker));
            return AddScoped(typeof(IEntityValidator<,>).MakeGenericType(CategoryType, managerType), typeof(T));
        }

        public virtual BlogBuilder AddCategoryValidatorForManager<T, TCategoryManager>()
            where T : class
            where TCategoryManager : class
        {
            return AddScoped(typeof(IEntityValidator<,>).MakeGenericType(CategoryType, typeof(TCategoryManager)), typeof(T));
        }

        public virtual BlogBuilder AddCategoryCodeGenerator<T, TModuleMarker>()
            where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            var managerType = typeof(CategoryManager<,>).MakeGenericType(CategoryType, typeof(TModuleMarker));
            return AddScoped(typeof(IEntityCodeGenerator<,>).MakeGenericType(CategoryType, managerType), typeof(T));
        }

        public virtual BlogBuilder AddCategoryCodeGeneratorForManager<T, TCategoryManager>()
            where T : class
            where TCategoryManager : class
        {
            return AddScoped(typeof(IEntityCodeGenerator<,>).MakeGenericType(CategoryType, typeof(TCategoryManager)), typeof(T));
        }

        #endregion

        #region Blogger

        public virtual BlogBuilder AddBloggerManager<T, TModuleMarker>()
            where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            return AddManager<T, TModuleMarker>(typeof(BloggerManager<,>), BloggerType);
        }

        public virtual BlogBuilder AddBloggerStore<T, TModuleMarker>() where T : class
        {
            return AddScoped(typeof(IBloggerStore<,>).MakeGenericType(BloggerType, typeof(TModuleMarker)), typeof(T));
        }

        public virtual BlogBuilder AddBloggerAccessor<T>() where T : class
        {
            return AddScoped(typeof(IBloggerAccessor<>).MakeGenericType(BloggerType), typeof(T));
        }

        public virtual BlogBuilder AddBloggerValidator<T, TModuleMarker>()
            where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            var managerType = typeof(BloggerManager<,>).MakeGenericType(BloggerType, typeof(TModuleMarker));
            return AddScoped(typeof(IEntityValidator<,>).MakeGenericType(BloggerType, managerType), typeof(T));
        }

        public virtual BlogBuilder AddBloggerValidatorForManager<T, TBloggerManager>()
            where T : class
            where TBloggerManager : class
        {
            return AddScoped(typeof(IEntityValidator<,>).MakeGenericType(BloggerType, typeof(TBloggerManager)), typeof(T));
        }

        public virtual BlogBuilder AddBloggerCodeGenerator<T, TModuleMarker>()
            where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            var managerType = typeof(BloggerManager<,>).MakeGenericType(BloggerType, typeof(TModuleMarker));
            return AddScoped(typeof(IEntityCodeGenerator<,>).MakeGenericType(BloggerType, managerType), typeof(T));
        }

        public virtual BlogBuilder AddBloggerCodeGeneratorForManager<T, TBloggerManager>()
            where T : class
            where TBloggerManager : class
        {
            return AddScoped(typeof(IEntityCodeGenerator<,>).MakeGenericType(BloggerType, typeof(TBloggerManager)), typeof(T));
        }

        #endregion

        #region Helpers

        private BlogBuilder AddManager<T, TModuleMarker>(Type managerType, Type entityType) where T : class
            where TModuleMarker : IBlogModuleMarker
        {
            managerType = managerType.MakeGenericType(entityType, typeof(TModuleMarker));
            var customType = typeof(T);
            if (managerType == customType ||
                !managerType.GetTypeInfo().IsAssignableFrom(customType.GetTypeInfo()))
            {
                throw new InvalidOperationException(string.Format(Resources.InvalidManagerType, customType.Name, managerType.Name, entityType.Name, typeof(TModuleMarker).Name));
            }

            Services.AddScoped(customType, services => services.GetRequiredService(managerType));
            return AddScoped(managerType, customType);
        }

        private BlogBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }

        #endregion
    }
}
