using System;
using PhantomNet.Entities.EntityMarkers;

namespace PhantomNet.Blog
{
    public class Article : Article<string>
    {
        public Article()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class Article<TKey> :
        IIdWiseEntity<TKey>,
        IConcurrencyStampWiseEntity,
        ITimeWiseEntity
        where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; set; }

        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public virtual DateTime DataCreateDate { get; set; }

        public virtual DateTime DataLastModifyDate { get; set; }

        public virtual DateTime PublishDate { get; set; }

        public virtual string Title { get; set; }

        public virtual string UrlFriendlyTitle { get; set; }

        public virtual string ShortContent { get; set; }

        public virtual string Content { get; set; }
    }
}
