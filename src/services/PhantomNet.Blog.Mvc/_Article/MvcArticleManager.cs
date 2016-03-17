using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using PhantomNet.Entities;

namespace PhantomNet.Blog.Mvc
{
    public class MvcArticleManager<TArticle> : MvcArticleManager<TArticle, MvcArticleManager<TArticle>>
        where TArticle : class
    {
        public MvcArticleManager(
            IArticleStore<TArticle> store,
            IEnumerable<IMvcArticleValidator<TArticle>> articleValidators,
            ILookupNormalizer keyNormalizer,
            BlogErrorDescriber errors,
            ILogger<MvcArticleManager<TArticle>> logger,
            IHttpContextAccessor contextAccessor) :
            base(store, articleValidators, keyNormalizer, errors, logger, contextAccessor)
        { }
    }

    public class MvcArticleManager<TArticle, TArticleManager> :
        ArticleManager<TArticle, TArticleManager>
        where TArticle : class
        where TArticleManager : MvcArticleManager<TArticle, TArticleManager>
    {
        private readonly HttpContext _context;

        public MvcArticleManager(
            IArticleStore<TArticle> store,
            IEnumerable<IArticleValidator<TArticle, TArticleManager>> articleValidators,
            ILookupNormalizer keyNormalizer,
            BlogErrorDescriber errors,
            ILogger<MvcArticleManager<TArticle, TArticleManager>> logger,
            IHttpContextAccessor contextAccessor) :
            base(store, articleValidators, keyNormalizer, errors, logger)
        {
            _context = contextAccessor?.HttpContext;
        }

        protected override CancellationToken CancellationToken => _context?.RequestAborted ?? base.CancellationToken;
    }
}
