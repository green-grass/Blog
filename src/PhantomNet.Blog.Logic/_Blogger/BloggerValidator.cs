using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class BloggerValidator<TBlogger, TBloggerManager>
        : IEntityValidator<TBlogger, TBloggerManager>
        where TBlogger : class
        where TBloggerManager : class,
                                IEntityManager<TBlogger>,
                                ICodeBasedEntityManager<TBlogger>
    {
        public BloggerValidator(IBloggerAccessor<TBlogger> bloggerAccessor, BlogErrorDescriber errors = null)
        {
            BloggerAccessor = bloggerAccessor;
            ErrorDescriber = errors ?? new BlogErrorDescriber();
        }

        private BlogErrorDescriber ErrorDescriber { get; set; }

        protected IBloggerAccessor<TBlogger> BloggerAccessor { get; }

        public virtual async Task<EntityResult> ValidateAsync(TBloggerManager manager, TBlogger blogger)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (blogger == null)
            {
                throw new ArgumentNullException(nameof(blogger));
            }

            var errors = new List<EntityError>();

            await ValidateUrlFriendlyPenName(manager, blogger, errors);

            if (errors.Count > 0)
            {
                return EntityResult.Failed(errors.ToArray());
            }

            return EntityResult.Success;
        }

        protected virtual async Task ValidateUrlFriendlyPenName(TBloggerManager manager, TBlogger blogger, List<EntityError> errors)
        {
            var urlFriendlyPenName = BloggerAccessor.GetCode(blogger);
            if (string.IsNullOrWhiteSpace(urlFriendlyPenName))
            {
                errors.Add(ErrorDescriber.InvalidBloggerUrlFriendlyPenName(urlFriendlyPenName));
            }
            else
            {
                var owner = await manager.FindByCodeAsync(urlFriendlyPenName);
                if (owner != null &&
                    !string.Equals(BloggerAccessor.GetId(owner), BloggerAccessor.GetId(blogger)))
                {
                    errors.Add(ErrorDescriber.DuplicateBloggerUrlFriendlyPenName(urlFriendlyPenName));
                }
            }
        }
    }
}
