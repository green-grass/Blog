using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface IBloggerAccessor<TBlogger>
        : IEntityAccessor<TBlogger>,
          ICodeBasedEntityAccessor<TBlogger>,
          INameBasedEntityAccessor<TBlogger>
        where TBlogger : class
    { }
}
