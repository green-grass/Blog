using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhantomNet.Entities.EntityFramework;

namespace PhantomNet.Blog.EntityFramework
{
    public class CategoryStore<TArticle, TCategory, TBlogger, TContext, TModuleMarker>
        : CategoryStore<TArticle, TCategory, TBlogger, TContext, int, TModuleMarker>
        where TArticle : Article<TArticle, TCategory, TBlogger, int>
        where TCategory : Category<TArticle, TCategory, TBlogger, int>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, int>
        where TContext : DbContext
        where TModuleMarker : IBlogModuleMarker
    {
        public CategoryStore(TContext context, BlogErrorDescriber errors = null) : base(context, errors) { }
    }

    public class CategoryStore<TArticle, TCategory, TBlogger, TContext, TKey, TModuleMarker>
        : QueryableEntityStoreBase<TCategory, TContext, TKey>,
          IQueryableCategoryStore<TCategory, TModuleMarker>,
          IQueryableEntityStoreMarker<TCategory, TContext, TKey>,
          ITimeTrackedEntityStoreMarker<TCategory, TContext, TKey>,
          IQueryableCodeBasedEntityStoreMarker<TCategory, TContext, TKey>
        where TArticle : Article<TArticle, TCategory, TBlogger, TKey>
        where TCategory : Category<TArticle, TCategory, TBlogger, TKey>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TModuleMarker : IBlogModuleMarker
    {
        #region Constructors

        public CategoryStore(TContext context, BlogErrorDescriber errors = null) : base(context)
        {
            ErrorDescriber = errors ?? new BlogErrorDescriber();
        }

        #endregion

        #region Properties

        private BlogErrorDescriber ErrorDescriber { get; set; }

        public IQueryable<TCategory> Categories => Entities;

        #endregion

        #region Public Operations

        #region Entity

        public virtual Task<EntityResult> CreateAsync(TCategory category, CancellationToken cancellationToken)
        {
            return this.CreateEntityAsync(category, cancellationToken);
        }

        public virtual Task<EntityResult> UpdateAsync(TCategory category, CancellationToken cancellationToken)
        {
            return this.UpdateEntityAsync(category, cancellationToken);
        }

        public virtual Task<EntityResult> DeleteAsync(TCategory category, CancellationToken cancellationToken)
        {
            return this.DeleteEntityAsync(category, cancellationToken);
        }

        public virtual Task<T> FindByIdAsync<T>(string id, CancellationToken cancellationToken)
            where T : class
        {
            return this.FindEntityByIdAsync<TCategory, TContext, TKey, T>(id, cancellationToken);
        }

        public virtual IQueryable<TCategory> Filter(IQueryable<TCategory> query, string filter)
        {
            return this.FilterEntities(query, filter,
                filter: q => q.Where(x => x.Name.Contains(filter) ||
                                          x.UrlFriendlyName.Contains(filter) ||
                                          x.DescriptionMeta.Contains(filter) ||
                                          x.KeywordsMeta.Contains(filter) ||
                                          x.Filters.Contains(filter)));
        }

        public virtual IQueryable<TCategory> PreSort(IQueryable<TCategory> query)
        {
            return this.PreSortEntities(query, sort: q => q);
        }

        public virtual IQueryable<TCategory> DefaultSort(IQueryable<TCategory> query)
        {
            return this.DefaultSortEntities(query,
                sort: q => q.OrderBy(x => x.Position).ThenBy(x => x.Name),
                orderedSort: q => q.ThenBy(x => x.Position).ThenBy(x => x.Name));
        }

        public virtual Task<int> CountAsync(IQueryable<TCategory> categories, CancellationToken cancellationToken)
        {
            return this.CountEntitiesAsync(categories, cancellationToken);
        }

        #endregion

        #region CodeBasedEntity

        public virtual Task<TCategory> FindByCodeAsync(string normalizedCode, CancellationToken cancellationToken)
        {
            return this.FindEntityByCodeAsync(normalizedCode, x => x.UrlFriendlyName, cancellationToken);
        }


        #endregion

        #region Category

        public virtual Task<TCategory> FindByUrlFriendlyNameAsync(string urlFriendlyTitle, CancellationToken cancellationToken)
        {
            return FindByCodeAsync(urlFriendlyTitle, cancellationToken);
        }

        #endregion

        #endregion
    }
}
