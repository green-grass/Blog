using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class ArticleValidator<TArticle, TArticleManager>
        : IEntityValidator<TArticle, TArticleManager>
        where TArticle : class
        where TArticleManager : class,
                                IEntityManager<TArticle>,
                                ICodeBasedEntityManager<TArticle>
    {
        public ArticleValidator(IArticleAccessor<TArticle> articleAccessor, BlogErrorDescriber errors = null)
        {
            ArticleAccessor = articleAccessor;
            ErrorDescriber = errors ?? new BlogErrorDescriber();
        }

        private BlogErrorDescriber ErrorDescriber { get; set; }

        protected IArticleAccessor<TArticle> ArticleAccessor { get; }

        public virtual async Task<EntityResult> ValidateAsync(TArticleManager manager, TArticle article)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            var errors = new List<EntityError>();

            await ValidateUrlFriendlyTitle(manager, article, errors);

            if (errors.Count > 0)
            {
                return EntityResult.Failed(errors.ToArray());
            }

            return EntityResult.Success;
        }

        protected virtual async Task ValidateUrlFriendlyTitle(TArticleManager manager, TArticle article, List<EntityError> errors)
        {
            var urlFriendlyTitle = ArticleAccessor.GetCode(article);
            if (string.IsNullOrWhiteSpace(urlFriendlyTitle))
            {
                errors.Add(ErrorDescriber.InvalidArticleUrlFriendlyTitle(urlFriendlyTitle));
            }
            else
            {
                var owner = await manager.FindByCodeAsync(urlFriendlyTitle);
                if (owner != null &&
                    !string.Equals(ArticleAccessor.GetId(owner), ArticleAccessor.GetId(article)))
                {
                    errors.Add(ErrorDescriber.DuplicateArticleUrlFriendlyTitle(urlFriendlyTitle));
                }
            }
        }
    }
}
