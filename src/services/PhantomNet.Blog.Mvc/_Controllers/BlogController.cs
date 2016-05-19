using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
#if NET451
using AutoMapper;
using PhantomNet.Mvc;
#endif

namespace PhantomNet.Blog.Mvc
{
    public class BlogController<
        // Managers
        TArticleManager, TCategoryManager, TBloggerManager,
        // Entities
        TArticle, TCategory, TBlogger,
        // Module
        TModuleMarker,
        // Models
        TArticleModel, TCategoryModel, TBloggerModel,
        // ViewModels
        TIndexViewModel, TArticleViewModel, TCategoryViewModel, TSearchViewModel>
        : Controller
        // Managers
        where TArticleManager : ArticleManager<TArticle, TArticleManager, TModuleMarker>
        where TCategoryManager : CategoryManager<TCategory, TCategoryManager, TModuleMarker>
        where TBloggerManager : BloggerManager<TBlogger, TBloggerManager, TModuleMarker>
        // Entities
        where TArticle : class
        where TCategory : class
        where TBlogger : class
        // Module
        where TModuleMarker : IBlogModuleMarker
        // Models
        where TArticleModel : class, IArticleModel, new()
        where TCategoryModel : class, ICategoryModel, new()
        where TBloggerModel : class, IBloggerModel, new()
        // ViewModels
        where TIndexViewModel : class, IIndexViewModel, new()
        where TArticleViewModel : class, IArticleViewModel, new()
        where TCategoryViewModel : class, ICategoryViewModel, new()
        where TSearchViewModel : class, ISearchViewModel, new()
    {
#if NET451
        public BlogController(
            TArticleManager articleManager,
            TCategoryManager categoryManager,
            TBloggerManager bloggerManager,
            IOptions<MvcBlogOptions> optionsAccessor,
            IMapperContainer<IBlogControllerMappingMarker> mapperContainer)
        {
            ArticleManager = articleManager;
            CategoryManager = categoryManager;
            BloggerManager = bloggerManager;

            PageSize = optionsAccessor.Value.PageSize;
            IndexTitle = optionsAccessor.Value.IndexTitle;

            Mapper = mapperContainer.Mapper;
        }
#else
        public BlogController(
            TArticleManager articleManager,
            TCategoryManager categoryManager,
            TBloggerManager bloggerManager,
            IOptions<MvcBlogOptions> optionsAccessor)
        {
            ArticleManager = articleManager;
            CategoryManager = categoryManager;
            BloggerManager = bloggerManager;

            PageSize = optionsAccessor.Value.PageSize;
            IndexTitle = optionsAccessor.Value.IndexTitle;
        }
#endif

        protected virtual TArticleManager ArticleManager { get; set; }

        protected virtual TCategoryManager CategoryManager { get; set; }

        protected virtual TBloggerManager BloggerManager { get; set; }

        protected virtual int PageSize { get; set; }

        protected virtual string IndexTitle { get; set; }

#if NET451
        protected virtual IMapper Mapper { get; set; }
#endif

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var searchResult = await ArticleManager.SearchAsync(true, null, pageNumber, PageSize, null, false);
#if NET451
            var articles = Mapper.Map<List<TArticleModel>>(searchResult.Results);
#else
            var articles = new List<TArticleModel>();
#endif

            var viewModel = new TIndexViewModel {
                Title = IndexTitle,
                Articles = articles
            };

            return View(viewModel);
        }
    }
}
