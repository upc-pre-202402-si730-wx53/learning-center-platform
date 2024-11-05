namespace ACME.LearningCenterPlatform.API.Profiles.Interfaces.ACL
{
    public interface IProfilesContextFacade
    {
        Task<int> CreateProfile(string FirstName,
           string LastName,
           string Email,
           string Street,
           string Number,
           string City,
           string PostalCode,
           string Country);


        Task<int> FetchProfileIdByEmail(string email);

    }
}
