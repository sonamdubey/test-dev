using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Images;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Images
{
    /// <summary>
    /// Created by  :   Sumit Kate on 15 Nov 2016
    /// Description :   Image Upload BAL interface
    /// </summary>
    public interface IImage
    {
        ImageToken GenerateImageUploadToken(Image objImage);
        ImageToken ProcessImageUpload(ImageToken token);
        IEnumerable<ModelImages> GetBikeModelsPhotos(string modelIds, string categoryIds, int requiredImageCount);
    }
}
