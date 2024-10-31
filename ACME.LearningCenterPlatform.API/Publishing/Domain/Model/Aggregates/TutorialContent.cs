using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Commands;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Entities;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.ValueObjects;

namespace ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Aggregates
{
    public partial class Tutorial : IPublishable
    {
        public ICollection<Asset> Assets { get; }
        public EPublishingStatus Status { get; protected set; }

        public bool Readeble => HasReadableAssets;
        public bool Viewable => HasViewableAssets;

        public bool HasReadableAssets => Assets.Any(asset => asset.Readable);
        public bool HasViewableAssets => Assets.Any(asset => asset.Viewable);


        /// <summary>
        ///  Defaul constructor for the tutorial entity
        /// </summary>
        public Tutorial()
        {
            Title = string.Empty;
            Summary = string.Empty;
            Assets = new List<Asset>();
            Status = EPublishingStatus.Draft;
        }

        public Tutorial(CreateTutorialCommand command) : this(command.Title, command.Summary, command.CategoryId)
        {

        }

        /// <summary>
        /// Verify if an image already exists in the tutorial
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns>
        /// True if the image asset exists, false otherwise
        /// </returns>
        private bool ExistsImageByUrl(string imageUrl) =>
            Assets.Any(asset => asset.AssetType == EAssetType.Image && (string)asset.GetContent() == imageUrl);


        /// <summary>
        /// Verify if a video already exists in the tutorial
        /// </summary>
        /// <param name="videoUrl"></param>
        /// <returns>
        /// True if the video asset exists, false otherwise
        /// </returns>
        private bool ExistsVideoByUrl(string videoUrl) =>
            Assets.Any(asset => asset.AssetType == EAssetType.Video && (string)asset.GetContent() == videoUrl);

        /// <summary>
        /// Verify if a readable content asset already exists in the tutorial
        /// </summary>
        /// <param name="content"></param>
        /// <returns>
        /// True if the readable content asset exists, false otherwise
        /// </returns>
        private bool ExistsReadableContent(string content) => 
            Assets.Any(asset => asset.AssetType == EAssetType.ReadableContentItem && (string)asset.GetContent() == content);


        private bool HasAllAssetsWithStatus(EPublishingStatus status) =>
            Assets.All(asset => asset.Status == status);


        public void AddImage(string imageUrl)
        {
            if (ExistsImageByUrl(imageUrl)) return;
            Assets.Add(new ImageAsset(imageUrl));
        }

        public void AddVideo(string videoUrl)
        {
            if (ExistsVideoByUrl(videoUrl)) return;
            Assets.Add(new VideoAsset(videoUrl));
        }

        public void AddReadableContent(string content)
        {
            if(ExistsReadableContent(content)) return;
            Assets.Add(new ReadableContentAsset(content));
        }
        public void RemoveAsset(AcmeAssetIdentifier identifier)
        {
            var asset = Assets.FirstOrDefault(a => a.AssetIdentifier == identifier);
            if (asset is null) return;
            Assets.Remove(asset);
        }

        public void CrealAssets() => Assets.Clear();

        void IPublishable.ApproveAndLock()
        {
            if (HasAllAssetsWithStatus(EPublishingStatus.ApprovedAndLocked))
                Status = EPublishingStatus.ApprovedAndLocked;
        }

        void IPublishable.Reject()
        {
            Status = EPublishingStatus.Draft;
        }

        void IPublishable.ReturnToEdit()
        {
            Status = EPublishingStatus.ReadyToEdit;
        }

        void IPublishable.SendToApproval()
        {
            if(HasAllAssetsWithStatus(EPublishingStatus.ReadyToApproval))
                Status = EPublishingStatus.ReadyToApproval;
        }

        void IPublishable.SendToEdit()
        {
            if (HasAllAssetsWithStatus(EPublishingStatus.ReadyToEdit))
                Status = EPublishingStatus.ReadyToEdit;
        }

        public List<ContentItem> GetContent()
        {
            var content = new List<ContentItem>();
            if(Assets.Count > 0)
                content.AddRange(Assets.Select(asset => 
                new ContentItem(asset.AssetType.ToString(), asset.GetContent() as string ?? String.Empty)));

            return content;
        }


    }
}
