using System.Linq;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface IQueryableArticleStore<TArticle> :
        IQueryableEntityStore<TArticle>,
        IArticleStore<TArticle>
        where TArticle : class
    {
        IQueryable<TArticle> Articles { get; }
    }
}
