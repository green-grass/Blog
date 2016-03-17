using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class BlogErrorDescriber : EntityErrorDescriber
    {
        public virtual EntityError InvalidArticleUrlFriendlyTitle(string articleUrlFriendlyTitle)
        {
            return new EntityError {
                Code = nameof(InvalidArticleUrlFriendlyTitle),
                Description = string.Format(Resources.InvalidArticleUrlFriendlyTitle, articleUrlFriendlyTitle)
            };
        }

        public virtual EntityError DuplicateArticleUrlFriendlyTitle(string articleUrlFriendlyTitle)
        {
            return new EntityError {
                Code = nameof(DuplicateArticleUrlFriendlyTitle),
                Description = string.Format(Resources.DuplicateArticleUrlFriendlyTitle, articleUrlFriendlyTitle)
            };
        }
    }
}
