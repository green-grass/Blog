using System.Linq;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface IQueryableCategoryStore<TCategory, TModuleMarker>
        : IQueryableEntityStore<TCategory>,
          ICategoryStore<TCategory, TModuleMarker>
        where TCategory : class
        where TModuleMarker : IBlogModuleMarker
    {
        IQueryable<TCategory> Categories { get; }
    }
}
