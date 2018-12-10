using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.ImageUpload;
using System;

namespace Carwale.Interfaces.ImageUpload
{
    public interface IImageBL
    {
        string GenerateHash(uint uniqueId);
        ImageTokenDTO GenerateImageUploadToken(int inquiryId, string extension, int imageType);
        Token GetToken();
        byte[] HmacSHA256(String data, byte[] key);
        void PushToIPCQueue(int imageId, string originalImgPath);
    }
}