using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Aggregates;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Repositories;
using ACME.LearningCenterPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.LearningCenterPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ACME.LearningCenterPlatform.API.Publishing.Infrastructure.Persistence.EFC.Repositories
{
    public class TutorialRepository(AppDbContext context) : BaseRepository<Tutorial>(context), ITutorialRepository
    {
        public async Task<IEnumerable<Tutorial>> FindByCategoryIdAsync(int cateogoryId)
        {
            return await context.Set<Tutorial>()
                .Include(tutorial => tutorial.Category)
                .Where(tutorial => tutorial.CategoryId == cateogoryId)
                .ToListAsync();
        }
    }
}
