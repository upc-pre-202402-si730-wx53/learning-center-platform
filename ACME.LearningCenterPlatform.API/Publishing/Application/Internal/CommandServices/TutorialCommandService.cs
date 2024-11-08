﻿using ACME.LearningCenterPlatform.API.Publishing.Application.Internal.OutboundServices.ACL;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Aggregates;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Commands;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.ValueObjects;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Repositories;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Services;
using ACME.LearningCenterPlatform.API.Shared.Domain.Repositories;

namespace ACME.LearningCenterPlatform.API.Publishing.Application.Internal.CommandServices
{
    public class TutorialCommandService(ITutorialRepository tutorialRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IExternalProfileService externalProfileService) : ITutorialCommandService
    {

        async Task<Tutorial?> ITutorialCommandService.Handle(CreateTutorialCommand command)
        {
            var category = await categoryRepository.FindByIdAsync(command.CategoryId);
            if (category is null) throw new Exception("Category not found");
            if (await tutorialRepository.ExistByTitleAsync(command.Title)) throw new Exception("Tutorial already exists");
            var tutorial = new Tutorial(command);
            await tutorialRepository.AddAsync(tutorial);
            await unitOfWork.CompleteAsync();
            tutorial.Category = category;
            return tutorial;

        }

        async Task<Tutorial?> ITutorialCommandService.Handle(AddVideoAssetToTutorialCommand command)
        {
            var tutorial = await tutorialRepository.FindByIdAsync(command.TutorialId);
            if (tutorial is null) throw new Exception("Tutorial not found");
            tutorial.AddVideo(command.VideoUrl);
            await unitOfWork.CompleteAsync();
            return tutorial;
        }

        async Task<ProfileId?> ITutorialCommandService.GetProfileId(string email)
        {
            var profileId = await externalProfileService.FetchProfileIdByEmail(email);
            Console.WriteLine($"Profile ID 2: {profileId}");
            return profileId;
        }

    }
}
