using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface IArticleStore<TArticle> :
        IEntityStore<TArticle>,
        ITimeTrackedEntityStore<TArticle>,
        ICodeBasedEntityStore<TArticle>
        where TArticle : class
    { }
}
