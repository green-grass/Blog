using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhantomNet.Entities;

namespace PhantomNet.Blog
{
    public class CategoryValidator<TCategory, TCategoryManager>
        : IEntityValidator<TCategory, TCategoryManager>
        where TCategory : class
        where TCategoryManager : class,
                                 IEntityManager<TCategory>,
                                 ICodeBasedEntityManager<TCategory>
    {
        public CategoryValidator(ICategoryAccessor<TCategory> categoryAccessor, BlogErrorDescriber errors = null)
        {
            CategoryAccessor = categoryAccessor;
            ErrorDescriber = errors ?? new BlogErrorDescriber();
        }

        private BlogErrorDescriber ErrorDescriber { get; set; }

        protected ICategoryAccessor<TCategory> CategoryAccessor { get; }

        public virtual async Task<EntityResult> ValidateAsync(TCategoryManager manager, TCategory category)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var errors = new List<EntityError>();

            await ValidateUrlFriendlyName(manager, category, errors);

            if (errors.Count > 0)
            {
                return EntityResult.Failed(errors.ToArray());
            }

            return EntityResult.Success;
        }

        protected virtual async Task ValidateUrlFriendlyName(TCategoryManager manager, TCategory category, List<EntityError> errors)
        {
            var urlFriendlyTitle = CategoryAccessor.GetCode(category);
            if (string.IsNullOrWhiteSpace(urlFriendlyTitle))
            {
                errors.Add(ErrorDescriber.InvalidCategoryUrlFriendlyName(urlFriendlyTitle));
            }
            else
            {
                var owner = await manager.FindByCodeAsync(urlFriendlyTitle);
                if (owner != null &&
                    !string.Equals(CategoryAccessor.GetId(owner), CategoryAccessor.GetId(category)))
                {
                    errors.Add(ErrorDescriber.DuplicateCategoryUrlFriendlyName(urlFriendlyTitle));
                }
            }
        }
    }
}
