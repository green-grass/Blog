using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class ArticleValidator<TArticle> :
        ArticleValidator<TArticle, ArticleManager<TArticle>>,
        IArticleValidator<TArticle>
        where TArticle : class
    {
        public ArticleValidator(BlogErrorDescriber errors = null) : base(errors) { }
    }

    public class ArticleValidator<TArticle, TArticleManager> :
        IArticleValidator<TArticle, TArticleManager>
        where TArticle : class
        where TArticleManager : class,
                                IEntityManager<TArticle>,
                                ICodeBasedEntityManager<TArticle>
    {
        public ArticleValidator(BlogErrorDescriber errors = null)
        {
            ErrorDescriber = errors ?? new BlogErrorDescriber();
        }

        private BlogErrorDescriber ErrorDescriber { get; set; }

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
            var urlFriendlyTitle = await manager.GetCodeAsync(article);
            if (string.IsNullOrWhiteSpace(urlFriendlyTitle))
            {
                errors.Add(ErrorDescriber.InvalidArticleUrlFriendlyTitle(urlFriendlyTitle));
            }
            else
            {
                var owner = await manager.FindByCodeAsync(urlFriendlyTitle);
                if (owner != null &&
                    !string.Equals(await manager.GetIdAsync(owner), await manager.GetIdAsync(article)))
                {
                    errors.Add(ErrorDescriber.DuplicateArticleUrlFriendlyTitle(urlFriendlyTitle));
                }
            }
        }
    }
}
