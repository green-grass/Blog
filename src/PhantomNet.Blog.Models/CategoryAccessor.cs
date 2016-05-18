using System;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class CategoryAccessor<TArticle, TCategory, TBlogger>
        : CategoryAccessor<TArticle, TCategory, TBlogger, int>
        where TArticle : Article<TArticle, TCategory, TBlogger>
        where TCategory : Category<TArticle, TCategory, TBlogger>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger>
    { }

    public class CategoryAccessor<TArticle, TCategory, TBlogger, TKey>
        : ICategoryAccessor<TCategory>
        where TArticle : Article<TArticle, TCategory, TBlogger, TKey>
        where TCategory : Category<TArticle, TCategory, TBlogger, TKey>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, TKey>
        where TKey : IEquatable<TKey>
    {
        public virtual string GetId(TCategory category)
        {
            return this.GetEntityId<TCategory, TKey>(category);
        }

        public virtual string GetCode(TCategory category)
        {
            return this.GetEntityCode(category,
                codeSelector: x => x.UrlFriendlyName,
                directGetCode: null);
        }

        public virtual void SetCode(TCategory category, string code)
        {
            this.SetEntityCode(category, code,
                codeSelector: x => x.UrlFriendlyName,
                directSetCode: null);
        }

        public virtual string GetName(TCategory category)
        {
            return this.GetEntityName(category,
                nameSelector: x => x.Name,
                directGetName: null);
        }

        public virtual void SetName(TCategory category, string name)
        {
            this.SetEntityName(category, name,
                nameSelector: x => x.Name,
                directSetName: null);
        }

        public virtual void SetNormalizedName(TCategory category, string normalizedName)
        {
            throw new InvalidOperationException();
        }
    }
}
