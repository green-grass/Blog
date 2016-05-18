using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using PhantomNet.Entities.EntityFramework;

namespace PhantomNet.Blog.EntityFramework
{
    public class ArticleStore<TArticle, TCategory, TBlogger, TContext, TModuleMarker>
        : ArticleStore<TArticle, TCategory, TBlogger, TContext, int, TModuleMarker>
        where TArticle : Article<TArticle, TCategory, TBlogger, int>
        where TCategory : Category<TArticle, TCategory, TBlogger, int>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, int>
        where TContext : DbContext
        where TModuleMarker : IBlogModuleMarker
    {
        public ArticleStore(TContext context, BlogErrorDescriber errors = null) : base(context, errors) { }
    }

    public class ArticleStore<TArticle, TCategory, TBlogger, TContext, TKey, TModuleMarker>
        : QueryableEntityStoreBase<TArticle, TContext, TKey>,
          IQueryableArticleStore<TArticle, TModuleMarker>,
          IQueryableEntityStoreMarker<TArticle, TContext, TKey>,
          IQueryableTimeTrackedEntityStoreMarker<TArticle, TContext, TKey>,
          IActivatableEntityStoreMarker<TArticle, TContext, TKey>,
          IQueryableCodeBasedEntityStoreMarker<TArticle, TContext, TKey>
        where TArticle : Article<TArticle, TCategory, TBlogger, TKey>
        where TCategory : Category<TArticle, TCategory, TBlogger, TKey>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TModuleMarker : IBlogModuleMarker
    {
        #region Constructors

        public ArticleStore(TContext context, BlogErrorDescriber errors = null) : base(context)
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

        public virtual IQueryable<TArticle> Filter(IQueryable<TArticle> query, string filter)
        {
            return this.FilterEntities(query, filter,
                filter: q => q.Where(x => x.Author.Contains(filter) ||
                                          x.SourceUrl.Contains(filter) ||
                                          x.Title.Contains(filter) ||
                                          x.UrlFriendlyTitle.Contains(filter) ||
                                          x.ShortContent.Contains(filter) ||
                                          x.Content.Contains(filter) ||
                                          x.Tags.Contains(filter) ||
                                          x.Series.Contains(filter) ||
                                          x.DescriptionMeta.Contains(filter) ||
                                          x.KeywordsMeta.Contains(filter) ||
                                          x.Filters.Contains(filter)));
        }

        public virtual IQueryable<TArticle> PreSort(IQueryable<TArticle> query)
        {
            return this.PreSortEntities(query, sort: q => q);
        }

        public virtual IQueryable<TArticle> DefaultSort(IQueryable<TArticle> query)
        {
            return this.DefaultSortEntities(query,
                sort: q => q.OrderByDescending(x => x.PublishDate),
                orderedSort: q => q.ThenByDescending(x => x.PublishDate));
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

        #endregion

        #region ActivatableEntity

        public IQueryable<TArticle> FilterByIsActive(IQueryable<TArticle> query, bool? isActive)
        {
            return this.FilterEntitiesByIsActive(query, isActive);
        }

        #endregion

        #region CodeBasedEntity

        public virtual Task<TArticle> FindByCodeAsync(string normalizedCode, CancellationToken cancellationToken)
        {
            return this.FindEntityByCodeAsync(normalizedCode, x => x.UrlFriendlyTitle, cancellationToken);
        }


        #endregion

        #region Article

        public virtual Task<TArticle> FindByUrlFriendlyTitleAsync(string urlFriendlyTitle, CancellationToken cancellationToken)
        {
            return FindByCodeAsync(urlFriendlyTitle, cancellationToken);
        }

        #endregion

        #endregion
    }
}
