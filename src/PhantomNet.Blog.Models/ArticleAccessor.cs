using System;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class ArticleAccessor<TArticle, TCategory, TBlogger>
        : ArticleAccessor<TArticle, TCategory, TBlogger, int>
        where TArticle : Article<TArticle, TCategory, TBlogger>
        where TCategory : Category<TArticle, TCategory, TBlogger>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger>
    { }

    public class ArticleAccessor<TArticle, TCategory, TBlogger, TKey>
        : IArticleAccessor<TArticle>
        where TArticle : Article<TArticle, TCategory, TBlogger, TKey>
        where TCategory : Category<TArticle, TCategory, TBlogger, TKey>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, TKey>
        where TKey : IEquatable<TKey>
    {
        public virtual string GetId(TArticle article)
        {
            return this.GetEntityId<TArticle, TKey>(article);
        }

        public virtual void SetDataCreateDate(TArticle article, DateTime dataCreateDate)
        {
            this.SetEntityDataCreateDate(article, dataCreateDate);
        }

        public virtual void SetDataLastModifyDate(TArticle article, DateTime dataLastModifyDate)
        {
            this.SetEntityDataLastModifyDate(article, dataLastModifyDate);
        }

        public virtual string GetCode(TArticle article)
        {
            return this.GetEntityCode(article,
                codeSelector: x => x.UrlFriendlyTitle,
                directGetCode: null);
        }

        public virtual void SetCode(TArticle article, string code)
        {
            this.SetEntityCode(article, code,
                codeSelector: x => x.UrlFriendlyTitle,
                directSetCode: null);
        }

        public virtual string GetName(TArticle article)
        {
            return this.GetEntityName(article,
                nameSelector: x => x.Title,
                directGetName: null);
        }

        public virtual void SetName(TArticle article, string name)
        {
            this.SetEntityName(article, name,
                nameSelector: x => x.Title,
                directSetName: null);
        }

        public virtual void SetNormalizedName(TArticle article, string normalizedName)
        {
            throw new InvalidOperationException();
        }
    }
}
