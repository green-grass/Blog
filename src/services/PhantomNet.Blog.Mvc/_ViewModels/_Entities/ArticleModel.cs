using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhantomNet.Blog.Mvc
{
    public interface IArticleModel
    {
        string Title { get; set; }

        string ShortContent { get; set; }
    }

    public class ArticleModel : IArticleModel
    {
        public virtual string Title { get; set; }

        public virtual string ShortContent { get; set; }
    }
}
