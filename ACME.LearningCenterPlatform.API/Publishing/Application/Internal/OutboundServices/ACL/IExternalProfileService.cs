using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.ValueObjects;

namespace ACME.LearningCenterPlatform.API.Publishing.Application.Internal.OutboundServices.ACL
{
    public interface IExternalProfileService
    {
        Task<ProfileId?> FetchProfileIdByEmail(string email);
        Task<ProfileId?> CreateProfile(string firstName, string lastName, string email, string street, string number, string city, string postalCode, string country);
    }
}
