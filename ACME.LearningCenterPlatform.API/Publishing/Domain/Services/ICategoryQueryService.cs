using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Entities;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Queries;

namespace ACME.LearningCenterPlatform.API.Publishing.Domain.Services
{
    public interface ICategoryQueryService
    {
        public Task<Category?> Handle(GetCategoryByIdQuery query);
        public IEnumerable<Category> Handle(GetAllCategoriesQuery query);
    }
}
