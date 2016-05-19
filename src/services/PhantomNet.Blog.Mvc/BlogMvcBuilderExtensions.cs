using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PhantomNet.Blog;
using PhantomNet.Blog.Mvc;
using PhantomNet.Entities;
// TODO:: AutoMapper
#if NET451
using AutoMapper;
using PhantomNet.Mvc;
#endif

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlogMvcBuilderExtensions
    {
        public static BlogBuilder AddMvc<TArticleModel, TCategoryModel, TBloggerModel>(this BlogBuilder builder, Action<MvcBlogOptions> setupAction = null)
            where TArticleModel : class
            where TCategoryModel : class
            where TBloggerModel : class
        {
            return builder.AddMvc<TArticleModel, TCategoryModel, TBloggerModel, IBlogControllerMappingMarker, IBlogModuleMarker>(setupAction);
        }

        public static BlogBuilder AddMvc<TArticleModel, TCategoryModel, TBloggerModel, TBlogMappingMarker, TModuleMarker>(this BlogBuilder builder, Action<MvcBlogOptions> setupAction = null)
            where TArticleModel : class
            where TCategoryModel : class
            where TBloggerModel : class
            where TBlogMappingMarker : IBlogControllerMappingMarker
            where TModuleMarker : IBlogModuleMarker
        {
            // Hosting doesn't add IHttpContextAccessor by default
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var articleManagerType = typeof(MvcArticleManager<,>).MakeGenericType(builder.ArticleType, typeof(TModuleMarker));
            builder.Services.TryAddScoped(articleManagerType);
            builder.Services.TryAddScoped(
                typeof(IEntityValidator<,>).MakeGenericType(builder.ArticleType, articleManagerType),
                typeof(ArticleValidator<,>).MakeGenericType(builder.ArticleType, articleManagerType));
            builder.Services.TryAddScoped(
                typeof(IEntityCodeGenerator<,>).MakeGenericType(builder.ArticleType, articleManagerType),
                typeof(UrlFriendlyCodeGenerator<,>).MakeGenericType(builder.ArticleType, articleManagerType));

            var categoryManagerType = typeof(MvcCategoryManager<,>).MakeGenericType(builder.CategoryType, typeof(TModuleMarker));
            builder.Services.TryAddScoped(categoryManagerType);
            builder.Services.TryAddScoped(
                typeof(IEntityValidator<,>).MakeGenericType(builder.CategoryType, categoryManagerType),
                typeof(CategoryValidator<,>).MakeGenericType(builder.CategoryType, categoryManagerType));
            builder.Services.TryAddScoped(
                typeof(IEntityCodeGenerator<,>).MakeGenericType(builder.CategoryType, categoryManagerType),
                typeof(UrlFriendlyCodeGenerator<,>).MakeGenericType(builder.CategoryType, categoryManagerType));

            var bloggerManagerType = typeof(MvcBloggerManager<,>).MakeGenericType(builder.BloggerType, typeof(TModuleMarker));
            builder.Services.TryAddScoped(bloggerManagerType);
            builder.Services.TryAddScoped(
                typeof(IEntityValidator<,>).MakeGenericType(builder.BloggerType, bloggerManagerType),
                typeof(BloggerValidator<,>).MakeGenericType(builder.BloggerType, bloggerManagerType));
            builder.Services.TryAddScoped(
                typeof(IEntityCodeGenerator<,>).MakeGenericType(builder.BloggerType, bloggerManagerType),
                typeof(UrlFriendlyCodeGenerator<,>).MakeGenericType(builder.BloggerType, bloggerManagerType));

            if (setupAction != null)
            {
                builder.Services.Configure(setupAction);
            }

#if NET451
            var mapperConfiguration = new MapperConfiguration(configure => {
                configure.CreateMap(builder.ArticleType, typeof(TArticleModel));
                configure.CreateMap(builder.CategoryType, typeof(TCategoryModel));
                configure.CreateMap(builder.BloggerType, typeof(TBloggerModel));
            });

            builder.Services.AddSingleton<IMapperContainer<TBlogMappingMarker>>(
                implementationFactory => new MapperContainer<TBlogMappingMarker>(mapperConfiguration.CreateMapper()));
#endif

            return builder;
        }
    }
}
