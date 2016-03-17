using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public interface IArticleValidator<TArticle> :
        IArticleValidator<TArticle, ArticleManager<TArticle>>
        where TArticle : class
    { }

    public interface IArticleValidator<in TArticle, in TArticleManager> :
        IEntityValidator<TArticle, TArticleManager>
        where TArticle : class
        where TArticleManager : class
    { }
}
