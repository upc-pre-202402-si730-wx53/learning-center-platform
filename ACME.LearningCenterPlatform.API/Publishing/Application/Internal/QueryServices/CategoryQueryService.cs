using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Entities;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Queries;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Repositories;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Services;

namespace ACME.LearningCenterPlatform.API.Publishing.Application.Internal.QueryServices
{
    public class CategoryQueryService(ICategoryRepository categoryRepository) : ICategoryQueryService
    {
        async Task<Category?> ICategoryQueryService.Handle(GetCategoryByIdQuery query)
        {
            return await categoryRepository.FindByIdAsync(query.CategoryId);
        }

        async Task<IEnumerable<Category>> ICategoryQueryService.Handle(GetAllCategoriesQuery query)
        {
            return await categoryRepository.ListAsync();
        }
    }
}
