using System.Collections.Generic;
namespace Bikewale.Entities.Used
{
    public class SellBikeImageUploadResultEntity
    {
        public ICollection<SellBikeImageUploadResultBase> ImageResult { get; set; }
        public ImageUploadResultStatus Status { get; set; }
    }

    public class SellBikeImageUploadResultBase
    {
        public ImageUploadStatus Status { get; set; }
        public string PhotoId { get; set; }
    }

    public enum ImageUploadResultStatus
    {
        Success = 1,
        UnauthorizedAccess = 2,
        FileUploadLimitExceeded = 3
    }

    public enum ImageUploadStatus
    {
        Success = 1,
        NoFile = 2,
        InvalidImageFileExtension = 3,
        ErrorPhotoIdGeneration = 4,
        UrlNotWellFormed = 5,
        MaxImageSizeExceeded = 6
    }
}
