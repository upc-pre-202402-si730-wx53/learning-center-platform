namespace ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Commands
{
    public record CreateTutorialCommand(string Title, string Description, int CategoryId);
}
