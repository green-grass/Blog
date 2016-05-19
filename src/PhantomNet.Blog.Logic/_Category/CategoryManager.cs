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
    public class CategoryManager<TCategory, TModuleMarker> : CategoryManager<TCategory, CategoryManager<TCategory, TModuleMarker>, TModuleMarker>
        where TCategory : class
        where TModuleMarker : IBlogModuleMarker
    {
        public CategoryManager(
            ICategoryStore<TCategory, TModuleMarker> store, ICategoryAccessor<TCategory> categoryAccessor,
            IEnumerable<IEntityValidator<TCategory, CategoryManager<TCategory, TModuleMarker>>> categoryValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TCategory, CategoryManager<TCategory, TModuleMarker>> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<CategoryManager<TCategory, TModuleMarker>> logger)
            : base(store, categoryAccessor, categoryValidators, keyNormalizer, codeGenerator, errors, services, logger)
        { }
    }

    public class CategoryManager<TCategory, TCategoryManager, TModuleMarker>
        : EntityManagerBase<TCategory, TCategoryManager>,
          IEntityManager<TCategory>,
          // TODO:: IActivatableEntityManager<TCategory>,
          ICodeBasedEntityManager<TCategory>
        where TCategory : class
        where TCategoryManager : CategoryManager<TCategory, TCategoryManager, TModuleMarker>
        where TModuleMarker : IBlogModuleMarker
    {
        public CategoryManager(
            ICategoryStore<TCategory, TModuleMarker> store, ICategoryAccessor<TCategory> categoryAccessor,
            IEnumerable<IEntityValidator<TCategory, TCategoryManager>> categoryValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TCategory, TCategoryManager> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<CategoryManager<TCategory, TCategoryManager, TModuleMarker>> logger)
            : base(store, categoryAccessor,
                   categoryValidators,
                   keyNormalizer ?? new LowerInvariantLookupNormalizer(),
                   codeGenerator ?? new UrlFriendlyCodeGenerator<TCategory, TCategoryManager>(categoryAccessor, services.GetRequiredService<IOptions<UrlFriendlyCodeGeneratorOptions>>()),
                   errors ?? new BlogErrorDescriber(),
                   logger)
        { }

        protected virtual ICategoryStore<TCategory, TModuleMarker> CategoryStore => Store as ICategoryStore<TCategory, TModuleMarker>;

        public virtual IQueryable<TCategory> Categories => Entities;

        public virtual Task<EntityResult> CreateAsync(TCategory category)
        {
            return CreateEntityAsync(category);
        }

        public virtual Task<EntityResult> UpdateAsync(TCategory category)
        {
            return UpdateEntityAsync(category);
        }

        public virtual Task<EntityResult> DeleteAsync(TCategory category)
        {
            return DeleteEntityAsync(category);
        }

        public virtual Task<EntityQueryResult<TCategory>> SearchAsync(string search, int? pageNumber, int? pageSize, string sort, bool reverse)
        {
            return SearchEntitiesAsync(search, pageNumber, pageSize, sort, reverse);
        }

        public virtual Task<TCategory> FindByIdAsync(string id)
        {
            return FindEntityByIdAsync(id);
        }

        public virtual Task<TCategory> FindByCodeAsync(string code)
        {
            return FindEntityByCodeAsync(code);
        }

        public virtual Task<TCategory> FindByUrlFriendlyNameAsync(string urlFriendlyName)
        {
            return FindByCodeAsync(urlFriendlyName);
        }
    }
}
