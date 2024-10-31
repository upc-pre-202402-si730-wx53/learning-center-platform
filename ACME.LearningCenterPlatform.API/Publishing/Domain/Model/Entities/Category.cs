using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Commands;

namespace ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category()
        {
            Name = string.Empty;
        }

        public Category(string name)
        {
            Name = name;
        }

        public Category(CreateCaegoryCommand command)
        {
            Name = command.Name;
        }
    }
}
