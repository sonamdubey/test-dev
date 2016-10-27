
namespace Bikewale.Entities.Used
{
    public class SellBikeImageUploadResultEntity
    {
        public ImageUploadStatus Status { get; set; }
        public string PhotoId { get; set; }
    }

    public enum ImageUploadStatus
    {
        Success = 1,
        NoFile = 2,
        InvalidImageFileExtension = 3,
        ErrorPhotoIdGeneration = 4,
        UrlNotWellFormed = 5,
        UnauthorizedAccess = 6
    }
}
