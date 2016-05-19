using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PhantomNet.Entities;

namespace PhantomNet.Blog.Mvc
{
    public class MvcBloggerManager<TBlogger, TModuleMarker> : MvcBloggerManager<TBlogger, MvcBloggerManager<TBlogger, TModuleMarker>, TModuleMarker>
        where TBlogger : class
        where TModuleMarker : IBlogModuleMarker
    {
        public MvcBloggerManager(
            IBloggerStore<TBlogger, TModuleMarker> store, IBloggerAccessor<TBlogger> bloggerAccessor,
            IEnumerable<IEntityValidator<TBlogger, MvcBloggerManager<TBlogger, TModuleMarker>>> bloggerValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TBlogger, MvcBloggerManager<TBlogger, TModuleMarker>> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<MvcBloggerManager<TBlogger, TModuleMarker>> logger,
            IHttpContextAccessor contextAccessor)
            : base(store, bloggerAccessor, bloggerValidators, keyNormalizer, codeGenerator, errors, services, logger, contextAccessor)
        { }
    }

    public class MvcBloggerManager<TBlogger, TBloggerManager, TModuleMarker>
        : BloggerManager<TBlogger, TBloggerManager, TModuleMarker>
        where TBlogger : class
        where TBloggerManager : MvcBloggerManager<TBlogger, TBloggerManager, TModuleMarker>
        where TModuleMarker : IBlogModuleMarker
    {
        private readonly HttpContext _context;

        public MvcBloggerManager(
            IBloggerStore<TBlogger, TModuleMarker> store, IBloggerAccessor<TBlogger> bloggerAccessor,
            IEnumerable<IEntityValidator<TBlogger, TBloggerManager>> bloggerValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TBlogger, TBloggerManager> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<MvcBloggerManager<TBlogger, TBloggerManager, TModuleMarker>> logger,
            IHttpContextAccessor contextAccessor)
            : base(store, bloggerAccessor, bloggerValidators, keyNormalizer, codeGenerator, errors, services, logger)
        {
            _context = contextAccessor?.HttpContext;
        }

        protected override CancellationToken CancellationToken => _context?.RequestAborted ?? base.CancellationToken;
    }
}
