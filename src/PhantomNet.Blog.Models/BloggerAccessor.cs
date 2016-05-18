using System;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class BloggerAccessor<TArticle, TCategory, TBlogger>
        : BloggerAccessor<TArticle, TCategory, TBlogger, int>
        where TArticle : Article<TArticle, TCategory, TBlogger>
        where TCategory : Category<TArticle, TCategory, TBlogger>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger>
    { }

    public class BloggerAccessor<TArticle, TCategory, TBlogger, TKey>
        : IBloggerAccessor<TBlogger>
        where TArticle : Article<TArticle, TCategory, TBlogger, TKey>
        where TCategory : Category<TArticle, TCategory, TBlogger, TKey>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, TKey>
        where TKey : IEquatable<TKey>
    {
        public virtual string GetId(TBlogger blogger)
        {
            return this.GetEntityId<TBlogger, TKey>(blogger);
        }

        public virtual string GetCode(TBlogger blogger)
        {
            return this.GetEntityCode(blogger,
                codeSelector: x => x.UrlFriendlyPenName,
                directGetCode: null);
        }

        public virtual void SetCode(TBlogger blogger, string code)
        {
            this.SetEntityCode(blogger, code,
                codeSelector: x => x.UrlFriendlyPenName,
                directSetCode: null);
        }

        public virtual string GetName(TBlogger blogger)
        {
            return this.GetEntityName(blogger,
                nameSelector: x => x.PenName,
                directGetName: null);
        }

        public virtual void SetName(TBlogger blogger, string name)
        {
            this.SetEntityName(blogger, name,
                nameSelector: x => x.PenName,
                directSetName: null);
        }

        public virtual void SetNormalizedName(TBlogger blogger, string normalizedName)
        {
            throw new InvalidOperationException();
        }
    }
}
