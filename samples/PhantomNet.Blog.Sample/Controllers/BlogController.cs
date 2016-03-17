using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PhantomNet.Blog.Mvc;

namespace PhantomNet.Blog.Sample.Controllers
{
    public class BlogController : Controller
    {
        private readonly MvcArticleManager<Article> _articleManager;

        public BlogController(MvcArticleManager<Article> articleManager)
        {
            _articleManager = articleManager;
        }

        public Task<IActionResult> Index()
        {
            //var models = _articleManager.SearchAsync()
            return Task.FromResult((IActionResult)View());
        }

        public Task<IActionResult> Article()
        {
            return Task.FromResult((IActionResult)View());
        }

        public Task<IActionResult> Category()
        {
            return Task.FromResult((IActionResult)View());
        }
    }
}
