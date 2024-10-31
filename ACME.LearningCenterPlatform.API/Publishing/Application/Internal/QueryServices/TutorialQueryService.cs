using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Aggregates;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Queries;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Repositories;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Services;

namespace ACME.LearningCenterPlatform.API.Publishing.Application.Internal.QueryServices
{
    public class TutorialQueryService(ITutorialRepository tutorialRepository) : ITutorialQueryService
    {
        async Task<Tutorial?> ITutorialQueryService.Handle(GetTutorialByIdQuery query)
        {
            return await tutorialRepository.FindByIdAsync(query.TutorialId);
        }

        async Task<IEnumerable<Tutorial>> ITutorialQueryService.Handle(GetAllTutorialsQuery query)
        {
            return await tutorialRepository.ListAsync();
        }

        async Task<IEnumerable<Tutorial>> ITutorialQueryService.Handle(GetAllTutorialsByCategoryIdQuery query)
        {
            return await tutorialRepository.FindByCategoryIdAsync(query.CategoryId);
        }
    }
}
