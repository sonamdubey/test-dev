using System.Collections.Generic;
namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 28 Oct 2016
    /// Description :   SellBike Image Upload Result Entity
    /// It contains one request's status
    /// </summary>
    public class SellBikeImageUploadResultEntity
    {
        public ICollection<SellBikeImageUploadResultBase> ImageResult { get; set; }
        public ImageUploadResultStatus Status { get; set; }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 28 Oct 2016
    /// Description :   SellBikeImageUploadResult Base
    /// It shows individual image status
    /// </summary>
    public class SellBikeImageUploadResultBase
    {
        public ImageUploadStatus Status { get; set; }
        public string ImgUrl { get; set; }
        public string PhotoId { get; set; }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 28 Oct 2016
    /// Description :   SellBike Image Upload Result Status
    /// </summary>
    public enum ImageUploadResultStatus
    {
        Success = 1,
        UnauthorizedAccess = 2,
        FileUploadLimitExceeded = 3,
        InternalError = 4
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 28 Oct 2016
    /// Description :   SellBikeImageUploadResultBase status
    /// </summary>
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
