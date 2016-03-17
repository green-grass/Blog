using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using PhantomNet.Entities.EntityFramework;

namespace PhantomNet.Blog.EntityFramework
{
    public class ArticleStore<TArticle> : ArticleStore<TArticle, DbContext>
        where TArticle : Article<string>
    {
        public ArticleStore(DbContext context, BlogErrorDescriber errors = null) : base(context, errors) { }
    }

    public class ArticleStore<TArticle, TContext> : ArticleStore<TArticle, TContext, string>
        where TArticle : Article<string>
        where TContext : DbContext
    {
        public ArticleStore(TContext context, BlogErrorDescriber errors = null) : base(context, errors) { }
    }

    public class ArticleStore<TArticle, TContext, TKey> :
        QueryableEntityStoreBase<TArticle, TContext, TKey>,
        IQueryableArticleStore<TArticle>,
        IEntityStoreMarker<TArticle, TContext, TKey>,
        ITimeTrackedEntityStoreMarker<TArticle, TContext, TKey>,
        ICodeBasedEntityStoreMarker<TArticle, TContext, TKey>
        where TArticle : Article<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        #region Constructors

        public ArticleStore(TContext context, BlogErrorDescriber errors = null)
            : base(context)
        {
            ErrorDescriber = errors ?? new BlogErrorDescriber();
        }

        #endregion

        #region Properties

        private BlogErrorDescriber ErrorDescriber { get; set; }

        public IQueryable<TArticle> Articles => Entities;

        #endregion

        #region Public Operations

        #region Entity

        public virtual Task<EntityResult> CreateAsync(TArticle article, CancellationToken cancellationToken)
        {
            return this.CreateEntityAsync(article, cancellationToken);
        }

        public virtual Task<EntityResult> UpdateAsync(TArticle article, CancellationToken cancellationToken)
        {
            return this.UpdateEntityAsync(article, cancellationToken);
        }

        public virtual Task<EntityResult> DeleteAsync(TArticle article, CancellationToken cancellationToken)
        {
            return this.DeleteEntityAsync(article, cancellationToken);
        }

        public virtual Task<T> FindByIdAsync<T>(string id, CancellationToken cancellationToken)
            where T : class
        {
            return this.FindEntityByIdAsync<TArticle, TContext, TKey, T>(id, cancellationToken);
        }

        public virtual Task<string> GetIdAsync(TArticle article, CancellationToken cancellationToken)
        {
            return this.GetEntityIdAsync(article, cancellationToken);
        }

        public virtual IQueryable<TArticle> Filter(IQueryable<TArticle> query, string filter)
        {
            // TODO:: Revise all properties
            return this.FilterEntities(query, filter,
                directFilter: () => query.Where(x => x.Title.Contains(filter) ||
                                                     x.UrlFriendlyTitle.Contains(filter) ||
                                                     x.ShortContent.Contains(filter) ||
                                                     x.Content.Contains(filter)));
        }

        public virtual IQueryable<TArticle> PreSort(IQueryable<TArticle> query)
        {
            return this.PreSortEntities(query,
                directPreSort: () => query);
        }

        public virtual IQueryable<TArticle> DefaultSort(IQueryable<TArticle> query)
        {
            return this.DefaultSortEntities(query,
                directDefaultSort: () => query.OrderByDescending(x => x.PublishDate),
                directOrderedDefaultSort: orderedQuery => orderedQuery.ThenByDescending(x => x.PublishDate));
        }

        public virtual Task<int> CountAsync(IQueryable<TArticle> articles, CancellationToken cancellationToken)
        {
            return this.CountEntitiesAsync(articles, cancellationToken);
        }

        #endregion

        #region TimeTrackedEntity

        public virtual Task<TArticle> FindLatestAsync(CancellationToken cancellationToken)
        {
            return this.FindLatestEntityAsync(cancellationToken);
        }

        public virtual Task SetDataCreateDateAsync(TArticle article, DateTime dataCreateDate, CancellationToken cancellationToken)
        {
            return this.SetEntityDataCreateDateAsync(article, dataCreateDate, cancellationToken);
        }

        public virtual Task SetDataLastModifyDateAsync(TArticle article, DateTime dataLastModifyDate, CancellationToken cancellationToken)
        {
            return this.SetEntityDataLastModifyDateAsync(article, dataLastModifyDate, cancellationToken);
        }

        #endregion

        #region CodeBasedEntity

        public virtual Task<TArticle> FindByCodeAsync(string normalizedCode, CancellationToken cancellationToken)
        {
            return this.FindEntityByCodeAsync(normalizedCode, cancellationToken, x => x.UrlFriendlyTitle, null);
        }

        public virtual Task<string> GetCodeAsync(TArticle article, CancellationToken cancellationToken)
        {
            return this.GetEntityCodeAsync(article, cancellationToken, x => x.UrlFriendlyTitle, null);
        }

        public virtual Task SetCodeAsync(TArticle article, string code, CancellationToken cancellationToken)
        {
            return this.SetEntityCodeAsync(article, code, cancellationToken, x => x.UrlFriendlyTitle, null);
        }

        #endregion

        #endregion
    }
}
