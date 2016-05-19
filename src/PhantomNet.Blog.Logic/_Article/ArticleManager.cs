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
    public class ArticleManager<TArticle, TModuleMarker> : ArticleManager<TArticle, ArticleManager<TArticle, TModuleMarker>, TModuleMarker>
        where TArticle : class
        where TModuleMarker : IBlogModuleMarker
    {
        public ArticleManager(
            IArticleStore<TArticle, TModuleMarker> store, IArticleAccessor<TArticle> articleAccessor,
            IEnumerable<IEntityValidator<TArticle, ArticleManager<TArticle, TModuleMarker>>> articleValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TArticle, ArticleManager<TArticle, TModuleMarker>> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<ArticleManager<TArticle, TModuleMarker>> logger)
            : base(store, articleAccessor, articleValidators, keyNormalizer, codeGenerator, errors, services, logger)
        { }
    }

    public class ArticleManager<TArticle, TArticleManager, TModuleMarker>
        : EntityManagerBase<TArticle, TArticleManager>,
          IEntityManager<TArticle>,
          ITimeTrackedEntityManager<TArticle>,
          IActivatableEntityManager<TArticle>,
          ICodeBasedEntityManager<TArticle>
        where TArticle : class
        where TArticleManager : ArticleManager<TArticle, TArticleManager, TModuleMarker>
        where TModuleMarker : IBlogModuleMarker
    {
        public ArticleManager(
            IArticleStore<TArticle, TModuleMarker> store, IArticleAccessor<TArticle> articleAccessor,
            IEnumerable<IEntityValidator<TArticle, TArticleManager>> articleValidators,
            ILookupNormalizer keyNormalizer,
            IEntityCodeGenerator<TArticle, TArticleManager> codeGenerator,
            BlogErrorDescriber errors,
            IServiceProvider services,
            ILogger<ArticleManager<TArticle, TArticleManager, TModuleMarker>> logger)
            : base(store, articleAccessor,
                   articleValidators,
                   keyNormalizer ?? new LowerInvariantLookupNormalizer(),
                   codeGenerator ?? new UrlFriendlyCodeGenerator<TArticle, TArticleManager>(articleAccessor, services.GetRequiredService<IOptions<UrlFriendlyCodeGeneratorOptions>>()),
                   errors ?? new BlogErrorDescriber(),
                   logger)
        { }

        protected virtual IArticleStore<TArticle, TModuleMarker> ArticleStore => Store as IArticleStore<TArticle, TModuleMarker>;

        public virtual IQueryable<TArticle> Articles => Entities;

        public virtual Task<EntityResult> CreateAsync(TArticle article)
        {
            return CreateEntityAsync(article);
        }

        public virtual Task<EntityResult> UpdateAsync(TArticle article)
        {
            return UpdateEntityAsync(article);
        }

        public virtual Task<EntityResult> DeleteAsync(TArticle article)
        {
            return DeleteEntityAsync(article);
        }

        public virtual Task<EntityQueryResult<TArticle>> SearchAsync(string search, int? pageNumber, int? pageSize, string sort, bool reverse)
        {
            return SearchEntitiesAsync(search, pageNumber, pageSize, sort, reverse);
        }

        public Task<EntityQueryResult<TArticle>> SearchAsync(bool? isActive, string search, int? pageNumber, int? pageSize, string sort, bool reverse)
        {
            return SearchEntitiesAsync(isActive, search, pageNumber, pageSize, sort, reverse);
        }

        public virtual Task<TArticle> FindByIdAsync(string id)
        {
            return FindEntityByIdAsync(id);
        }

        public virtual Task<TArticle> FindLatestAsync()
        {
            return FindLatestEntityAsync();
        }

        public virtual Task<TArticle> FindByCodeAsync(string code)
        {
            return FindEntityByCodeAsync(code);
        }

        public virtual Task<TArticle> FindByUrlFriendlyTitleAsync(string urlFriendlyTitle)
        {
            return FindByCodeAsync(urlFriendlyTitle);
        }
    }
}
