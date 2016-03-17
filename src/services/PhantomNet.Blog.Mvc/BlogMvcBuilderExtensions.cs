using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PhantomNet.Blog;
using PhantomNet.Blog.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlogMvcBuilderExtensions
    {
        public static BlogBuilder AddMvc(this BlogBuilder builder)
        {
            // Hosting doesn't add IHttpContextAccessor by default
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var articleManagerType = typeof(MvcArticleManager<>).MakeGenericType(builder.ArticleType);
            builder.Services.TryAddScoped(articleManagerType);
            builder.Services.TryAddScoped(typeof(MvcArticleValidator<>).MakeGenericType(builder.ArticleType));

            return builder;
        }
    }
}
