namespace PhantomNet.Blog.Mvc
{
    public class MvcArticleValidator<TArticle> :
        ArticleValidator<TArticle, MvcArticleManager<TArticle>>,
        IMvcArticleValidator<TArticle>
        where TArticle : class
    {
        public MvcArticleValidator(BlogErrorDescriber errors = null) : base(errors) { }
    }
}
