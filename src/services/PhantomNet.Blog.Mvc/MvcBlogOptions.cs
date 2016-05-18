using System.Collections.Generic;

namespace PhantomNet.Blog.Mvc
{
    public class MvcBlogOptions
    {
        public int PageSize { get; set; } = 30;

        public string IndexTitle { get; set; } = "Blog";
    }
}
