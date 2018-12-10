using Carwale.BL.GrpcFiles;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Media;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Notifications;
using Grpc.CMS;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Carwale.Utility;
using Carwale.Notifications.Logs;
using Carwale.Entity.CarData;

namespace Carwale.BL.CMS.Photos
{
    public class CMSPhotosBL : IPhotos
    {
        private readonly ICarModelCacheRepository _modelsCacheRepo;
        private readonly ICarRecommendationLogic _carRecommendationLogic;
        private readonly static string contentTypeList = string.Format("{0},{1}", (int)CMSContentType.Images, (int)CMSContentType.RoadTest);
        private const int requiredImageCount = 6;
        private const int requiredModelCount = 6;
        public CMSPhotosBL(IUnityContainer container, ICarModelCacheRepository modelsCacheRepo, ICarRecommendationLogic carRecommendationLogic)
        {
            _modelsCacheRepo = modelsCacheRepo;
            _carRecommendationLogic = carRecommendationLogic;
        }

        /// <summary>
        /// for getting given no. of records of image 
        /// written by Natesh Kumar on 26/9/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public List<ModelImage> GetModelPhotosByCount(ModelPhotosBycountURI queryString, List<ModelImage> modelImages = null)
        {
            try
            {
                if (modelImages == null)
                {
                    var newqueryString = new ModelPhotoURI();
                    newqueryString.ApplicationId = queryString.ApplicationId;
                    newqueryString.CategoryIdList = queryString.CategoryIdList;
                    newqueryString.ModelId = queryString.ModelId;

                    modelImages = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetModelPhotosList(newqueryString));
                }
                var hostURL = ConfigurationManager.AppSettings["CDNHostURL"];
                if (modelImages != null && queryString != null)
                {
                    modelImages.ForEach(x => x.HostUrl = hostURL);
                    if (queryString.TotalRecords > 0 && modelImages.Count > queryString.TotalRecords)  //checking if no. of list element is more than totalrecords.
                    {
                        modelImages = modelImages.Take((int)queryString.TotalRecords).ToList<ModelImage>(); // to take first "no. of record items" from the list.
                    }
                }
                return modelImages;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSPhotosBL GetModelPhotosByCount Exception");
                objErr.LogException();
                return null;
            }
        }

        public PhotosListing GetPopularModelPhotos(ArticleByCatURI queryString)
        {
            PhotosListing result = null;
            result = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetPopularModelPhotos(queryString));
            return GetPageWisePhotos(result, queryString);
        }
        
        public PhotosListing GetPopularNewModelPhotos(ArticleByCatURI queryString)
        {
            PhotosListing result = null;
            result = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetPopularNewModelPhotos(queryString));
            return GetPageWisePhotos(result, queryString);
        }

        private PhotosListing GetPageWisePhotos(PhotosListing result, ArticleByCatURI queryString)
        {
            if (result == null || result.ImageRecordCount == 0 || !result.PhotosList.IsNotNullOrEmpty())
            {
                return null;
            }

            foreach (var photo in result.PhotosList)
            {
                if (photo.GalleryImagePath != null)
                    photo.GalleryImagePath = photo.GalleryImagePath.Split('/').Last().Replace(".jpg", ".html").ToLower();
            }

            if (queryString.PageNo > 0 && queryString.PageSize > 0)
            {
                queryString.StartIndex = queryString.PageSize * queryString.PageNo - queryString.PageSize + 1;
                queryString.EndIndex = queryString.PageSize * queryString.PageNo;
            }
            if (queryString.StartIndex > 0 && queryString.EndIndex > 0)
            {
                if ((int)(queryString.EndIndex - queryString.StartIndex + 1) < (int)(result.ImageRecordCount - queryString.StartIndex + 1))
                    result.PhotosList = result.PhotosList.GetRange((int)queryString.StartIndex - 1, (int)(queryString.EndIndex - queryString.StartIndex) + 1);
                else
                    result.PhotosList = result.PhotosList.GetRange((int)queryString.StartIndex - 1, (int)(result.ImageRecordCount - queryString.StartIndex) + 1);
            }
            return result;
        }

        /// <summary>
        /// function for getting given no. of photos list by given modelid
        /// written by Natesh kumar on 19/9/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public List<ModelImage> GetArticlePhotos(ArticlePhotoUri queryString)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetArticlePhotos(queryString));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSPhotosBL GetArticlePhotos Exception");
                objErr.LogException();
                return null;
            }
        }

        public CMSImage GetOtherModelPhotosList(RelatedPhotoURI queryString)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetOtherModelPhotosList(queryString));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSPhotosBL GetOtherModelPhotosList Exception");
                objErr.LogException();
                return null;
            }
        }
        private List<int> GetSimilarCarIds(int modelId, int count = 5)
        {
            SimilarCarRequest request = new SimilarCarRequest
            {
                CarId = modelId,
                IsVersion = false,
                Count = count,
                UserIdentifier = string.Empty,
                IsBoost = false
            };
            return _carRecommendationLogic.GetSimilarCars(request);
        }
        public CMSImage GetSimilarModelPhotosList(RelatedPhotoURI queryString)
        {
            try
            {
                var similarModelIds = GetSimilarCarIds(queryString.ModelId);
                queryString.SimilarModelsList = similarModelIds.IsNotNullOrEmpty() ? string.Join(",", similarModelIds) : "";
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetSimilarModelPhotosList(queryString));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSPhotosBL GetSimilarModelPhotosList Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<ModelImage> GetNewCarPhotos(ModelPhotoURI queryString)
        {
            try
            {
                List<ModelImage> result = null;
                var newqueryString = new ModelPhotoURI();
                newqueryString.ApplicationId = queryString.ApplicationId;
                newqueryString.CategoryIdList = queryString.CategoryIdList;
                newqueryString.ModelId = queryString.ModelId;

                result = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetModelPhotosList(newqueryString));

                return result;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSPhotosBL GetNewCarPhotos Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<CMSImage> GetModelsPhotos(int makeId)
        {
            if(makeId > 0)
            {
                string modelIds = _modelsCacheRepo.GetModelsByMake(makeId).FindAll(x => x.New).Select(item => item.ModelId).Take(requiredModelCount).ToList().ToDelimatedString(',');
                var modelPhotos = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetModelsImages(modelIds, requiredImageCount, contentTypeList, (int)Application.CarWale));
                return modelPhotos;
            }
            return new List<CMSImage>();
        }

        public void GetMakeImageGallary(List<Carwale.Entity.ViewModels.ModelImageCarousal> modelsPhotos)
        {
            if(modelsPhotos != null)
            {
                foreach (var modelsPhoto in modelsPhotos)
                {
                    if(modelsPhoto.Images.IsNotNullOrEmpty())
                    {
                        int imageCount;

                        if (modelsPhoto.Images.Count >= 6)
                        {
                            imageCount = 6;
                        }
                        else if (modelsPhoto.Images.Count >= 4)
                        {
                            imageCount = 4;
                        }
                        else
                        {
                            imageCount = 1;
                        }
                        modelsPhoto.Images = modelsPhoto.Images.Take(imageCount).ToList();
                    }
                }
            }
        }
    }
}
