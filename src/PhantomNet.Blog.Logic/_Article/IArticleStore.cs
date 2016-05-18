using System.Threading;
using System.Threading.Tasks;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface IArticleStore<TArticle, TModuleMarker>
        : IEntityStore<TArticle>,
          ITimeTrackedEntityStore<TArticle>,
          IActivatableEntityStore<TArticle>,
          ICodeBasedEntityStore<TArticle>
        where TArticle : class
        where TModuleMarker : IBlogModuleMarker
    {
        Task<TArticle> FindByUrlFriendlyTitleAsync(string urlFriendlyTitle, CancellationToken cancellationToken);
    }
}
