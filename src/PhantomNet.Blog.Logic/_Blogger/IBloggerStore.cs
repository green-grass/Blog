using System.Threading;
using System.Threading.Tasks;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface IBloggerStore<TBlogger, TModuleMarker>
        : IEntityStore<TBlogger>,
          // TODO:: IActivatableEntityStore<TBlogger>,
          ICodeBasedEntityStore<TBlogger>
        where TBlogger : class
        where TModuleMarker : IBlogModuleMarker
    {
        Task<TBlogger> FindByUrlFriendlyPenNameAsync(string urlFriendlyPenName, CancellationToken cancellationToken);
    }
}
