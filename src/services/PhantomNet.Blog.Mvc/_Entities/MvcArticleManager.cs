using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PhantomNet.Entities;

namespace PhantomNet.Blog.Mvc
{
    public class MvcArticleManager<TArticle, TModuleMarker> : MvcArticleManager<TArticle, MvcArticleManager<TArticle, TModuleMarker>, TModuleMarker>
        where TArticle : class
        where TModuleMarker : IBlogModuleMarker
    {
        public MvcArticleManager(
            IArticleStore<TArticle, TModuleMarker> store, IArticleAccessor<TArticle> articleAccessor,
            IEnumerable<IEntityValidator<TArticle, MvcArticleManager<TArticle, TModuleMarker>>> articleValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TArticle, MvcArticleManager<TArticle, TModuleMarker>> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<MvcArticleManager<TArticle, TModuleMarker>> logger,
            IHttpContextAccessor contextAccessor)
            : base(store, articleAccessor, articleValidators, keyNormalizer, codeGenerator, errors, services, logger, contextAccessor)
        { }
    }

    public class MvcArticleManager<TArticle, TArticleManager, TModuleMarker>
        : ArticleManager<TArticle, TArticleManager, TModuleMarker>
        where TArticle : class
        where TArticleManager : MvcArticleManager<TArticle, TArticleManager, TModuleMarker>
        where TModuleMarker : IBlogModuleMarker
    {
        private readonly HttpContext _context;

        public MvcArticleManager(
            IArticleStore<TArticle, TModuleMarker> store, IArticleAccessor<TArticle> articleAccessor,
            IEnumerable<IEntityValidator<TArticle, TArticleManager>> articleValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TArticle, TArticleManager> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<MvcArticleManager<TArticle, TArticleManager, TModuleMarker>> logger,
            IHttpContextAccessor contextAccessor)
            : base(store, articleAccessor, articleValidators, keyNormalizer, codeGenerator, errors, services, logger)
        {
            _context = contextAccessor?.HttpContext;
        }

        protected override CancellationToken CancellationToken => _context?.RequestAborted ?? base.CancellationToken;
    }
}
