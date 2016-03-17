using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class ArticleManager<TArticle> : ArticleManager<TArticle, ArticleManager<TArticle>>
        where TArticle : class
    {
        public ArticleManager(
            IArticleStore<TArticle> store,
            IEnumerable<IArticleValidator<TArticle>> articleValidators,
            ILookupNormalizer keyNormalizer,
            BlogErrorDescriber errors,
            ILogger<ArticleManager<TArticle>> logger) :
            base(store, articleValidators, keyNormalizer, errors, logger)
        { }
    }

    public class ArticleManager<TArticle, TArticleManager> :
        EntityManagerBase<TArticle, TArticleManager>,
        IEntityManager<TArticle>,
        ITimeTrackedEntityManager<TArticle>,
        ICodeBasedEntityManager<TArticle>
        where TArticle : class
        where TArticleManager : ArticleManager<TArticle, TArticleManager>
    {
        public ArticleManager(
            IArticleStore<TArticle> store,
            IEnumerable<IArticleValidator<TArticle, TArticleManager>> articleValidators,
            ILookupNormalizer keyNormalizer,
            BlogErrorDescriber errors,
            ILogger<ArticleManager<TArticle, TArticleManager>> logger) :
            base(store,
                 articleValidators,
                 keyNormalizer ?? new UpperInvariantLookupNormalizer(),
                 null,
                 errors ?? new BlogErrorDescriber(),
                 logger)
        { }

        protected virtual IArticleStore<TArticle> ArticleStore => Store as IArticleStore<TArticle>;

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

        //public virtual Task<EntityQueryResult<TArticle>> FilterAsync()
        //{

        //}

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

        public virtual Task<string> GetIdAsync(TArticle article)
        {
            return GetEntityIdAsync(article);
        }

        public virtual Task<string> GetCodeAsync(TArticle article)
        {
            return GetEntityCodeAsync(article);
        }
    }
}
