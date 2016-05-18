using Microsoft.Extensions.OptionsModel;
using PhantomNet.Blog.Mvc;
#if DNX451
using PhantomNet.Mvc;
#endif

namespace PhantomNet.Blog.JDev.Controllers
{
    public class BlogController : BlogController<
        ArticleManager<Article, IBlogModuleMarker>,
        CategoryManager<Category, IBlogModuleMarker>,
        BloggerManager<Blogger, IBlogModuleMarker>,
        Article, Category, Blogger,
        IBlogModuleMarker,
        ArticleModel, CategoryModel, BloggerModel,
        IndexViewModel, ArticleViewModel, CategoryViewModel, SearchViewModel>
    {
#if DNX451
        public BlogController(
            ArticleManager<Article, IBlogModuleMarker> articleManager,
            CategoryManager<Category, IBlogModuleMarker> categoryManager,
            BloggerManager<Blogger, IBlogModuleMarker> bloggerManager,
            IOptions<MvcBlogOptions> optionsAccessor,
            IMapperContainer<IBlogControllerMappingMarker> mapperContainer)
            : base(articleManager, categoryManager, bloggerManager, optionsAccessor, mapperContainer)
        { }
#else
        public BlogController(
            ArticleManager<Article, IBlogModuleMarker> articleManager,
            CategoryManager<Category, IBlogModuleMarker> categoryManager,
            BloggerManager<Blogger, IBlogModuleMarker> bloggerManager,
            IOptions<MvcBlogOptions> optionsAccessor)
            : base(articleManager, categoryManager, bloggerManager, optionsAccessor)
        { }
#endif
    }
}
