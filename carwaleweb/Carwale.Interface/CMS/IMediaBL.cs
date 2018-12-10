using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Media;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using System.Collections.Generic;

namespace Carwale.Interfaces.CMS
{
    public interface IMediaBL
    {
        Media GetMediaListing(ArticleByCatURI queryString);
        List<ModelImage> GetModelImages(List<ModelImage> modelPhotos);

        List<ModelImage> GetModelCarouselImages(List<ModelImage> modelPhotos, string versionHostUrl = null, string versionOriginalImage = null);
        List<ModelImage> GetModelImagesSlug(int modelId);
        bool IsModelColorPhotosPresent(List<ModelColors> modelColors);
        List<CarModelDetails> GetUserHistoryModelDetails(int modelId, int count);
    }
}
