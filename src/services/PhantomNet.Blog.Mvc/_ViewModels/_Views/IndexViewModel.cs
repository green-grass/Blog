using System.Collections.Generic;

namespace PhantomNet.Blog.Mvc
{
    public interface IIndexViewModel
    {
        string Title { get; set; }

        IEnumerable<IArticleModel> Articles { get; set; }
    }

    public class IndexViewModel : IIndexViewModel
    {
        public virtual string Title { get; set; }

        public virtual IEnumerable<IArticleModel> Articles { get; set; }
    }
}
