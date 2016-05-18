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

        public virtual EntityError InvalidCategoryUrlFriendlyName(string categoryUrlFriendlyName)
        {
            return new EntityError {
                Code = nameof(InvalidCategoryUrlFriendlyName),
                Description = string.Format(Resources.InvalidCategoryUrlFriendlyName, categoryUrlFriendlyName)
            };
        }

        public virtual EntityError DuplicateCategoryUrlFriendlyName(string categoryUrlFriendlyName)
        {
            return new EntityError {
                Code = nameof(DuplicateCategoryUrlFriendlyName),
                Description = string.Format(Resources.DuplicateCategoryUrlFriendlyName, categoryUrlFriendlyName)
            };
        }

        public virtual EntityError InvalidBloggerUrlFriendlyPenName(string bloggerUrlFriendlyPenName)
        {
            return new EntityError {
                Code = nameof(InvalidBloggerUrlFriendlyPenName),
                Description = string.Format(Resources.InvalidBloggerUrlFriendlyPenName, bloggerUrlFriendlyPenName)
            };
        }

        public virtual EntityError DuplicateBloggerUrlFriendlyPenName(string bloggerUrlFriendlyPenName)
        {
            return new EntityError {
                Code = nameof(DuplicateBloggerUrlFriendlyPenName),
                Description = string.Format(Resources.DuplicateBloggerUrlFriendlyPenName, bloggerUrlFriendlyPenName)
            };
        }
    }
}
