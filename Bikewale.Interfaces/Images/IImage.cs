using Bikewale.Entities.Images;

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
    }
}
