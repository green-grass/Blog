using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhantomNet.Entities.EntityFramework;

namespace PhantomNet.Blog.EntityFramework
{
    public class BloggerStore<TArticle, TCategory, TBlogger, TContext, TModuleMarker>
        : BloggerStore<TArticle, TCategory, TBlogger, TContext, int, TModuleMarker>
        where TArticle : Article<TArticle, TCategory, TBlogger, int>
        where TCategory : Category<TArticle, TCategory, TBlogger, int>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, int>
        where TContext : DbContext
        where TModuleMarker : IBlogModuleMarker
    {
        public BloggerStore(TContext context, BlogErrorDescriber errors = null) : base(context, errors) { }
    }

    public class BloggerStore<TArticle, TCategory, TBlogger, TContext, TKey, TModuleMarker>
        : QueryableEntityStoreBase<TBlogger, TContext, TKey>,
          IQueryableBloggerStore<TBlogger, TModuleMarker>,
          IQueryableEntityStoreMarker<TBlogger, TContext, TKey>,
          ITimeTrackedEntityStoreMarker<TBlogger, TContext, TKey>,
        IQueryableCodeBasedEntityStoreMarker<TBlogger, TContext, TKey>
        where TArticle : Article<TArticle, TCategory, TBlogger, TKey>
        where TCategory : Category<TArticle, TCategory, TBlogger, TKey>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TModuleMarker : IBlogModuleMarker
    {
        #region Constructors

        public BloggerStore(TContext context, BlogErrorDescriber errors = null) : base(context)
        {
            ErrorDescriber = errors ?? new BlogErrorDescriber();
        }

        #endregion

        #region Properties

        private BlogErrorDescriber ErrorDescriber { get; set; }

        public IQueryable<TBlogger> Bloggers => Entities;

        #endregion

        #region Public Operations

        #region Entity

        public virtual Task<EntityResult> CreateAsync(TBlogger blogger, CancellationToken cancellationToken)
        {
            return this.CreateEntityAsync(blogger, cancellationToken);
        }

        public virtual Task<EntityResult> UpdateAsync(TBlogger blogger, CancellationToken cancellationToken)
        {
            return this.UpdateEntityAsync(blogger, cancellationToken);
        }

        public virtual Task<EntityResult> DeleteAsync(TBlogger blogger, CancellationToken cancellationToken)
        {
            return this.DeleteEntityAsync(blogger, cancellationToken);
        }

        public virtual Task<T> FindByIdAsync<T>(string id, CancellationToken cancellationToken)
            where T : class
        {
            return this.FindEntityByIdAsync<TBlogger, TContext, TKey, T>(id, cancellationToken);
        }

        public virtual IQueryable<TBlogger> Filter(IQueryable<TBlogger> query, string filter)
        {
            return this.FilterEntities(query, filter,
                filter: q => q.Where(x => x.PenName.Contains(filter) ||
                                          x.UrlFriendlyPenName.Contains(filter) ||
                                          x.FullName.Contains(filter) ||
                                          x.Introduction.Contains(filter) ||
                                          x.Biography.Contains(filter) ||
                                          x.Filters.Contains(filter)));
        }

        public virtual IQueryable<TBlogger> PreSort(IQueryable<TBlogger> query)
        {
            return this.PreSortEntities(query, sort: q => q);
        }

        public virtual IQueryable<TBlogger> DefaultSort(IQueryable<TBlogger> query)
        {
            return this.DefaultSortEntities(query,
                sort: q => q.OrderBy(x => x.PenName),
                orderedSort: q => q.ThenBy(x => x.PenName));
        }

        public virtual Task<int> CountAsync(IQueryable<TBlogger> bloggers, CancellationToken cancellationToken)
        {
            return this.CountEntitiesAsync(bloggers, cancellationToken);
        }

        #endregion

        #region CodeBasedEntity

        public virtual Task<TBlogger> FindByCodeAsync(string normalizedCode, CancellationToken cancellationToken)
        {
            return this.FindEntityByCodeAsync(normalizedCode, x => x.UrlFriendlyPenName, cancellationToken);
        }


        #endregion

        #region Blogger

        public virtual Task<TBlogger> FindByUrlFriendlyPenNameAsync(string urlFriendlyTitle, CancellationToken cancellationToken)
        {
            return FindByCodeAsync(urlFriendlyTitle, cancellationToken);
        }

        #endregion

        #endregion
    }
}
