using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface ICategoryAccessor<TCategory>
        : IEntityAccessor<TCategory>,
          ICodeBasedEntityAccessor<TCategory>,
          INameBasedEntityAccessor<TCategory>
        where TCategory : class
    { }
}
