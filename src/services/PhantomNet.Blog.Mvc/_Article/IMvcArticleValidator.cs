namespace PhantomNet.Blog.Mvc
{
    public interface IMvcArticleValidator<TArticle> :
        IArticleValidator<TArticle, MvcArticleManager<TArticle>>
        where TArticle : class
    { }
}
