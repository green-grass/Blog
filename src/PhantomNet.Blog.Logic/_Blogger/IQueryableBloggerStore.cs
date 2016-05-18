using System.Linq;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface IQueryableBloggerStore<TBlogger, TModuleMarker>
        : IQueryableEntityStore<TBlogger>,
          IBloggerStore<TBlogger, TModuleMarker>
        where TBlogger : class
        where TModuleMarker : IBlogModuleMarker
    {
        IQueryable<TBlogger> Bloggers { get; }
    }
}
