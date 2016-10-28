
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Bikewale.DTO.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 28 Oct 2016
    /// Description :   SellBike Image Upload Result DTO
    /// It contains one request's status
    /// </summary>
    public class SellBikeImageUploadResultDTO
    {
        [JsonProperty("imageResult")]
        public IEnumerable<SellBikeImageUploadResultDTOBase> ImageResult { get; set; }
        [JsonProperty("status")]
        public ImageUploadResultStatusDTO Status { get; set; }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 28 Oct 2016
    /// Description :   SellBikeImageUploadResult Base
    /// It shows individual image status
    /// </summary>
    public class SellBikeImageUploadResultDTOBase
    {
        [JsonProperty("status")]
        public ImageUploadStatusDTO Status { get; set; }
        [JsonProperty("imgUrl")]
        public string ImgUrl { get; set; }
        [JsonProperty("photoId")]
        public string PhotoId { get; set; }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 28 Oct 2016
    /// Description :   SellBike Image Upload Result DTO Status
    /// </summary>
    public enum ImageUploadResultStatusDTO
    {
        Success = 1,
        UnauthorizedAccess = 2,
        FileUploadLimitExceeded = 3
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 28 Oct 2016
    /// Description :   SellBikeImageUploadResult DTO Base status
    /// </summary>
    public enum ImageUploadStatusDTO
    {
        Success = 1,
        NoFile = 2,
        InvalidImageFileExtension = 3,
        ErrorPhotoIdGeneration = 4,
        UrlNotWellFormed = 5,
        MaxImageSizeExceeded = 6
    }
}
