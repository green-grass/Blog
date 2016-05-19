using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class BloggerManager<TBlogger, TModuleMarker> : BloggerManager<TBlogger, BloggerManager<TBlogger, TModuleMarker>, TModuleMarker>
        where TBlogger : class
        where TModuleMarker : IBlogModuleMarker
    {
        public BloggerManager(
            IBloggerStore<TBlogger, TModuleMarker> store, IBloggerAccessor<TBlogger> bloggerAccessor,
            IEnumerable<IEntityValidator<TBlogger, BloggerManager<TBlogger, TModuleMarker>>> bloggerValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TBlogger, BloggerManager<TBlogger, TModuleMarker>> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<BloggerManager<TBlogger, TModuleMarker>> logger)
            : base(store, bloggerAccessor, bloggerValidators, keyNormalizer, codeGenerator, errors, services, logger)
        { }
    }

    public class BloggerManager<TBlogger, TBloggerManager, TModuleMarker>
        : EntityManagerBase<TBlogger, TBloggerManager>,
          IEntityManager<TBlogger>,
          // TODO:: IActivatableEntityManager<TBlogger>,
          ICodeBasedEntityManager<TBlogger>
        where TBlogger : class
        where TBloggerManager : BloggerManager<TBlogger, TBloggerManager, TModuleMarker>
        where TModuleMarker : IBlogModuleMarker
    {
        public BloggerManager(
            IBloggerStore<TBlogger, TModuleMarker> store, IBloggerAccessor<TBlogger> bloggerAccessor,
            IEnumerable<IEntityValidator<TBlogger, TBloggerManager>> bloggerValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TBlogger, TBloggerManager> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<BloggerManager<TBlogger, TBloggerManager, TModuleMarker>> logger)
            : base(store, bloggerAccessor,
                   bloggerValidators,
                   keyNormalizer ?? new LowerInvariantLookupNormalizer(),
                   codeGenerator ?? new UrlFriendlyCodeGenerator<TBlogger, TBloggerManager>(bloggerAccessor, services.GetRequiredService<IOptions<UrlFriendlyCodeGeneratorOptions>>()),
                   errors ?? new BlogErrorDescriber(),
                   logger)
        { }

        protected virtual IBloggerStore<TBlogger, TModuleMarker> BloggerStore => Store as IBloggerStore<TBlogger, TModuleMarker>;

        public virtual IQueryable<TBlogger> Bloggers => Entities;

        public virtual Task<EntityResult> CreateAsync(TBlogger blogger)
        {
            return CreateEntityAsync(blogger);
        }

        public virtual Task<EntityResult> UpdateAsync(TBlogger blogger)
        {
            return UpdateEntityAsync(blogger);
        }

        public virtual Task<EntityResult> DeleteAsync(TBlogger blogger)
        {
            return DeleteEntityAsync(blogger);
        }

        public virtual Task<EntityQueryResult<TBlogger>> SearchAsync(string search, int? pageNumber, int? pageSize, string sort, bool reverse)
        {
            return SearchEntitiesAsync(search, pageNumber, pageSize, sort, reverse);
        }

        public virtual Task<TBlogger> FindByIdAsync(string id)
        {
            return FindEntityByIdAsync(id);
        }

        public virtual Task<TBlogger> FindByCodeAsync(string code)
        {
            return FindEntityByCodeAsync(code);
        }

        public virtual Task<TBlogger> FindByUrlFriendlyPenNameAsync(string urlFriendlyPenName)
        {
            return FindByCodeAsync(urlFriendlyPenName);
        }
    }
}
