using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Entities;

namespace ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Aggregates
{
    /// <summary>
    ///  Tutorial aggregate root entity
    /// </summary>
    /// <remarks>
    /// This class is used to represent a tutorial in the applications.
    /// </remarks>
    public partial class Tutorial
    {
        public int Id { get; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public Category Category { get; internal set; }
        public int CategoryId { get; private set; }


        /// <summary>
        /// Default constructor for the tutorial entity
        /// </summary>
        /// <param name="title"></param>
        /// <param name="summary"></param>
        /// <param name="categoryId"></param>
        public Tutorial(string title, string summary, int categoryId) : this()
        {
            Title = title;
            Summary = summary;
            CategoryId = categoryId;
        }
    }
}
