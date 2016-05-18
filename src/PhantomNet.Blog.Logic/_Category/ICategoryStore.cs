using System.Threading;
using System.Threading.Tasks;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface ICategoryStore<TCategory, TModuleMarker>
        : IEntityStore<TCategory>,
          // TODO:: IActivatableEntityStore<TCategory>,
          ICodeBasedEntityStore<TCategory>
        where TCategory : class
        where TModuleMarker : IBlogModuleMarker
    {
        Task<TCategory> FindByUrlFriendlyNameAsync(string urlFriendlyName, CancellationToken cancellationToken);
    }
}
