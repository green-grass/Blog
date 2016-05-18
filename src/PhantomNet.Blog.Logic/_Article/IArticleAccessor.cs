using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface IArticleAccessor<TArticle>
        : IEntityAccessor<TArticle>,
          ITimeTrackedEntityAccessor<TArticle>,
          ICodeBasedEntityAccessor<TArticle>,
          INameBasedEntityAccessor<TArticle>
        where TArticle : class
    { }
}
