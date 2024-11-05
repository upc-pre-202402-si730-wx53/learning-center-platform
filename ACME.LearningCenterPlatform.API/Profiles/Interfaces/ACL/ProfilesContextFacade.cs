using ACME.LearningCenterPlatform.API.Profiles.Domain.Model.Commands;
using ACME.LearningCenterPlatform.API.Profiles.Domain.Model.Queries;
using ACME.LearningCenterPlatform.API.Profiles.Domain.Services;
using ACME.LearningCenterPlatform.API.Profiles.Domain.Model.ValueObjects;

namespace ACME.LearningCenterPlatform.API.Profiles.Interfaces.ACL
{
    public class ProfilesContextFacade(IProfileCommandService profileCommandService, IProfileQueryService profileQueryService) : IProfilesContextFacade
    {
        public async Task<int> CreateProfile(string FirstName, 
            string LastName, 
            string Email, 
            string Street, 
            string Number,
            string City, 
            string PostalCode, 
            string Country)
        {
            var createProfileCommand = new CreateProfileCommand(FirstName, LastName, Email, Street, Number, City, PostalCode, Country);
            var profile = await profileCommandService.Handle(createProfileCommand);
            return profile?.Id ?? 0;
        }

        public async Task<int> FetchProfileIdByEmail(string email)
        {
            var getProfileByEmailQuery = new GetProfileByEmailQuery(new EmailAddress(email));
            var profile = await profileQueryService.Handle(getProfileByEmailQuery);
            Console.WriteLine($"Profile ID: {profile?.Id}");
            return profile?.Id ?? 0;
        }

    }
}
