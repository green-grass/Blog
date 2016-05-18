using System.Linq;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface IQueryableArticleStore<TArticle, TModuleMarker>
        : IQueryableEntityStore<TArticle>,
          IArticleStore<TArticle, TModuleMarker>
        where TArticle : class
        where TModuleMarker : IBlogModuleMarker
    {
        IQueryable<TArticle> Articles { get; }
    }
}
