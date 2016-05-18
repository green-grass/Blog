using System;
using PhantomNet.Entities.EntityMarkers;

namespace PhantomNet.Blog
{
    public class Blogger : Blogger<Article, Category, Blogger> { }

    public class Blogger<TArticle, TCategory, TBlogger>
        : Blogger<TArticle, TCategory, TBlogger, int>
        where TArticle : Article<TArticle, TCategory, TBlogger>
        where TCategory : Category<TArticle, TCategory, TBlogger>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger>
    { }

    public partial class Blogger<TArticle, TCategory, TBlogger, TKey>
        : IIdWiseEntity<TKey>,
          IConcurrencyStampWiseEntity,
          IIsActiveWiseEntity
        where TArticle : Article<TArticle, TCategory, TBlogger, TKey>
        where TCategory : Category<TArticle, TCategory, TBlogger, TKey>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, TKey>
        where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; set; }

        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public virtual bool IsActive { get; set; }

        public virtual string PenName { get; set; }

        public virtual string UrlFriendlyPenName { get; set; }

        public virtual string FullName { get; set; }

        public virtual string Introduction { get; set; }

        public virtual string Biography { get; set; }

        public virtual string Filters { get; set; }
    }
}
