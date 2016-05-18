using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using PhantomNet.Entities;

namespace PhantomNet.Blog.Mvc
{
    public class MvcCategoryManager<TCategory, TModuleMarker> : MvcCategoryManager<TCategory, MvcCategoryManager<TCategory, TModuleMarker>, TModuleMarker>
        where TCategory : class
        where TModuleMarker : IBlogModuleMarker
    {
        public MvcCategoryManager(
            ICategoryStore<TCategory, TModuleMarker> store, ICategoryAccessor<TCategory> categoryAccessor,
            IEnumerable<IEntityValidator<TCategory, MvcCategoryManager<TCategory, TModuleMarker>>> categoryValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TCategory, MvcCategoryManager<TCategory, TModuleMarker>> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<MvcCategoryManager<TCategory, TModuleMarker>> logger,
            IHttpContextAccessor contextAccessor)
            : base(store, categoryAccessor, categoryValidators, keyNormalizer, codeGenerator, errors, services, logger, contextAccessor)
        { }
    }

    public class MvcCategoryManager<TCategory, TCategoryManager, TModuleMarker>
        : CategoryManager<TCategory, TCategoryManager, TModuleMarker>
        where TCategory : class
        where TCategoryManager : MvcCategoryManager<TCategory, TCategoryManager, TModuleMarker>
        where TModuleMarker : IBlogModuleMarker
    {
        private readonly HttpContext _context;

        public MvcCategoryManager(
            ICategoryStore<TCategory, TModuleMarker> store, ICategoryAccessor<TCategory> categoryAccessor,
            IEnumerable<IEntityValidator<TCategory, TCategoryManager>> categoryValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TCategory, TCategoryManager> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<MvcCategoryManager<TCategory, TCategoryManager, TModuleMarker>> logger,
            IHttpContextAccessor contextAccessor)
            : base(store, categoryAccessor, categoryValidators, keyNormalizer, codeGenerator, errors, services, logger)
        {
            _context = contextAccessor?.HttpContext;
        }

        protected override CancellationToken CancellationToken => _context?.RequestAborted ?? base.CancellationToken;
    }
}
