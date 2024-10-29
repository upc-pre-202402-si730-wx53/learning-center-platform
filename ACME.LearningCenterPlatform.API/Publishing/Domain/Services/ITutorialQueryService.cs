using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Aggregates;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Queries;

namespace ACME.LearningCenterPlatform.API.Publishing.Domain.Services
{
    public interface ITutorialQueryService
    {
        public Task<Tutorial?> Handle(GetTutorialByIdQuery query);
        public Task<IEnumerable<Tutorial>> Handle(GetAllTutorialsQuery query);
        public Task<IEnumerable<Tutorial>> Handle(GetAllTutorialsByCategoryIdQuery query);

    }
}
