using Carwale.Interfaces.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.Entity.CMS.Media;
using Carwale.Entity.CMS;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Notifications;
using Carwale.Entity.CMS.Photos;
using System.Configuration;
using Carwale.Entity.Enum;
using Carwale.Entity.CarData;
using System.Web;
using Carwale.Interfaces.CarData;
using Carwale.Notifications.Logs;

namespace Carwale.BL.CMS
{
    public class MediaAdapter : IMediaBL
    {

        private readonly IPhotos _photos;
        private readonly IVideosBL _videos;
        private readonly ICarModelCacheRepository _carModelsCache;
        public MediaAdapter(IPhotos photos, IVideosBL videos, ICarModelCacheRepository carModelsCache)
        {
            _photos = photos;
            _videos = videos;
            _carModelsCache = carModelsCache;
        }

        public Media GetMediaListing(ArticleByCatURI queryString)
        {
            Media response = new Media();
            try
            {
                List<int> CategoryList = new List<int>();
                CategoryList = queryString.CategoryIdList.Split(',').Select(int.Parse).ToList();
                foreach (int categoryId in CategoryList)
                {
                    switch (categoryId)
                    {
                        case (int)CMSContentType.Images:
                            response.Photos = queryString.ModelId == 0 ?_photos.GetPopularNewModelPhotos(queryString) : _photos.GetPopularModelPhotos(queryString);
                            break;
                        case (int)CMSContentType.Videos:
                            response.Videos = _videos.GetPopularNewModelVideos(queryString);
                            break;
                        default:
                            response.Photos = _photos.GetPopularNewModelPhotos(queryString);
                            break;
                    }
                }

            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "MediaAdapter GetMedia() Exception");
                objErr.LogException();
            }
            return response;
        }

        public List<ModelImage> GetModelImages(List<ModelImage> modelPhotos)
        {
            try
            {
                if (modelPhotos != null && modelPhotos.Count > 4)
                {
                    List<ModelImage> exteriorPhotos = modelPhotos.Where((item) => item.MainImgCategoryId == 2).ToList();
                    List<ModelImage> interiorPhotos = modelPhotos.Where((item) => item.MainImgCategoryId == 1).ToList();

                    int exteriorPhotoCount = 0;
                    int interiorPhotoCount = 0;

                    if (exteriorPhotos.Count > 2 && interiorPhotos.Count > 1)
                    {
                        exteriorPhotoCount = 3;
                        interiorPhotoCount = 2;
                    }
                    else if (exteriorPhotos.Count < 3)
                    {
                        exteriorPhotoCount = exteriorPhotos.Count;
                        interiorPhotoCount = (5 - exteriorPhotoCount);
                    }
                    else
                    {
                        interiorPhotoCount = interiorPhotos.Count;
                        exteriorPhotoCount = (5 - interiorPhotoCount);
                    }
                    modelPhotos = exteriorPhotos.Take(exteriorPhotoCount).ToList();
                    modelPhotos.AddRange(interiorPhotos.Take(interiorPhotoCount).ToList());
                }
                return modelPhotos;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "MediaAdapter.GetModelImages() Exception");
                objErr.LogException();
                return null;
            }
        }
        public List<ModelImage> GetModelCarouselImages(List<ModelImage> modelPhotos, string versionHostUrl = null, string versionOriginalImage = null)
        {
            if (modelPhotos == null)
                return null;
            int imageCount;
			try
			{
				Int32.TryParse(ConfigurationManager.AppSettings["ModelImageCarouselCount"], out imageCount);
				var modelPhotosCarousel = modelPhotos.Take(imageCount + 1).ToList();
				if (!string.IsNullOrWhiteSpace(versionOriginalImage) && modelPhotosCarousel.Count > 0)
				{
					var versionPhotosCarousel = new List<ModelImage>();
					versionPhotosCarousel.Add(new ModelImage { HostUrl = versionHostUrl, OriginalImgPath = versionOriginalImage });
					modelPhotosCarousel.RemoveAt(0);
					versionPhotosCarousel.AddRange(modelPhotosCarousel);
					return versionPhotosCarousel;
				}
				return modelPhotosCarousel;
			}
			catch (Exception err)
			{
				Logger.LogException(err, "MediaAdapter.GetModelCarouselImages()");
				return null;
			}

		}


        public List<ModelImage> GetModelImagesSlug(int modelId)
        {
            var photosURI = new ModelPhotosBycountURI()
            {
                ApplicationId = (ushort)CMSAppId.Carwale,
                CategoryIdList = String.Format("{0},{1}", CMSContentType.RoadTest.ToString("D"), CMSContentType.Images.ToString("D")),
                ModelId = modelId,
                PlatformId = Platform.CarwaleDesktop.ToString("D")
            };
            List<ModelImage> modelPhotos = _photos.GetModelPhotosByCount(photosURI);

            return GetModelImages(modelPhotos);
        }

        public bool IsModelColorPhotosPresent(List<ModelColors> modelColors)
        {
            if (modelColors != null && modelColors.Count > 0)
            {
                int count = 0;
                modelColors.ForEach(x => count = (!string.IsNullOrEmpty(x.OriginalImgPath) ? count + 1 : count));
                if ((double)count / modelColors.Count >= 0.7)
                    return true;
            }
            return false;
        }

        public List<CarModelDetails> GetUserHistoryModelDetails(int modelId, int count)
        {
            HttpRequest request = HttpContext.Current.Request;

            HttpCookie userModelHistoryCookie = request.Cookies["_userModelHistory"];
            if (userModelHistoryCookie != null && !string.IsNullOrEmpty(userModelHistoryCookie.Value))
            {
                IEnumerable<int> userHistoryModels = userModelHistoryCookie.Value.Split('~').Select(Int32.Parse);
                if (userHistoryModels.Contains(modelId))
                    userHistoryModels = userHistoryModels.Where(carModelId => carModelId != modelId);

                return _carModelsCache.MultiGetModelDetails(userHistoryModels.Reverse().Take(count)).Where(model => model.New && model.PhotoCount > 0).ToList();
            }

            return null;
        }
    }
}
