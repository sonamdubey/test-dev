using System.Collections.Generic;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.CMS.Media;

namespace Carwale.Interfaces.CMS.Photos
{
    public interface IPhotos
    {
        /// <summary>
        /// function for getting given no. of photos list by given modelid
        /// written by Natesh kumar on 19/9/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        List<ModelImage> GetModelPhotosByCount(ModelPhotosBycountURI queryString, List<ModelImage> modelImages = null);

        // function to get photolist of  models(New and used car Images Only)
        PhotosListing GetPopularModelPhotos(ArticleByCatURI queryString);
        // function to get photolist of  models(New car Images Only)
        PhotosListing GetPopularNewModelPhotos(ArticleByCatURI queryString);

        List<ModelImage> GetArticlePhotos(ArticlePhotoUri queryString);
        /// <summary>
        /// function to get photolist of other model  with given modelid
        /// written by Natesh kumar on 19/9/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        CMSImage GetOtherModelPhotosList(RelatedPhotoURI queryString);

        /// <summary>
        /// function to get similar model photolist with given modelid
        /// written by Natesh kumar on 19/9/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        CMSImage GetSimilarModelPhotosList(RelatedPhotoURI queryString);

        /// <summary>
        /// used in appwebapi newcarphotos to get photos based on modelId and categoryId
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        List<ModelImage> GetNewCarPhotos(ModelPhotoURI queryString);

        /// <summary>
        /// Function to get list of model images by makeid
        /// written by abhishek lovanshi
        /// </summary>
        /// <param name="makeId">makeId of make in int format </param>
        /// <returns></returns>
        List<CMSImage> GetModelsPhotos(int makeId);

        /// <summary>
        /// Function to set list as required for ImageCarousel
        /// written by abhishek lovanshi
        /// </summary>
        /// <param name="modelsPhotos">list of ModelImageCarousal</param>
        /// <returns></returns>
        void GetMakeImageGallary(List<Carwale.Entity.ViewModels.ModelImageCarousal> modelsPhotos);
    }
}
