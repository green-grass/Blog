using System;
using System.Collections.Generic;
using PhantomNet.Entities.EntityMarkers;

namespace PhantomNet.Blog
{
    public class Category : Category<Article, Category, Blogger> { }

    public class Category<TArticle, TCategory, TBlogger>
        : Category<TArticle, TCategory, TBlogger, int>
        where TArticle : Article<TArticle, TCategory, TBlogger>
        where TCategory : Category<TArticle, TCategory, TBlogger>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger>
    {
        public virtual int? ParentId { get; set; }
    }

    public partial class Category<TArticle, TCategory, TBlogger, TKey>
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

        public virtual string Language { get; set; }

        public virtual int Position { get; set; }

        public virtual string Name { get; set; }

        public virtual string UrlFriendlyName { get; set; }

        public virtual string DescriptionMeta { get; set; }

        public virtual string KeywordsMeta { get; set; }

        public virtual string Filters { get; set; }
    }

    partial class Category<TArticle, TCategory, TBlogger, TKey>
    {
        public virtual TCategory Parent { get; set; }

        public virtual ICollection<TCategory> Children { get; set; }

        public virtual ICollection<TArticle> Articles { get; set; }
    }
}
