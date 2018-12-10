using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.Models;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.CMS
{
    public class UserReviewsListAdapter : IServiceAdapterV2
    {
        private readonly ICarModelCacheRepository _modelsCacheRepo;
        private readonly ICarVersionCacheRepository _versionCacheRepo;
        private readonly ICarModels _carModelBL;
        private readonly IUserReviewsCache _cmsCacheRepo;
        private readonly ICarPriceQuoteAdapter _carPrice;
        private readonly ICarMakesCacheRepository _makeCacheRepo;
        private readonly ICMSContent _contentBl;
        private readonly IVideosBL _videosBL;
		private readonly ICarVersions _versionBl;
        public UserReviewsListAdapter(ICarModelCacheRepository modelsCacheRepo,
            ICarModels carModelBL,
            IUserReviewsCache cmsCacheRepo,
            ICarVersionCacheRepository versionCacheRepo,
            ICarPriceQuoteAdapter carPrice,
            ICarMakesCacheRepository makeCacheRepo,
            ICMSContent contentBl,
           IVideosBL videosBL,
		   ICarVersions versionBl)
        {
            _modelsCacheRepo = modelsCacheRepo;
            _carModelBL = carModelBL;
            _cmsCacheRepo = cmsCacheRepo;
            _carPrice = carPrice;
            _makeCacheRepo = makeCacheRepo;
            _versionCacheRepo = versionCacheRepo;
            _contentBl = contentBl;
			_versionBl = versionBl;
            _videosBL = videosBL;
        }
     
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetUserReviewsListDTO((UserReviewUriEntity)Convert.ChangeType(input, typeof(U))), typeof(T));
        }

        private UserReviewsListDTO GetUserReviewsListDTO(UserReviewUriEntity input)
        {
            var userReviewsDTO = new UserReviewsListDTO();
            try
            {
                int startIndex = ((Convert.ToInt32(input.PageNo) - 1) * Convert.ToInt32(input.PageSize)) + 1;
                int endIndex = startIndex + Convert.ToInt32(input.PageSize) - 1;
                CarModelDetails modelDetails = _modelsCacheRepo.GetModelDetailsById(input.ModelId);
                bool isExpertReviewAvail = _contentBl.GetCMSRoadTestCount(modelDetails.MakeId, modelDetails.ModelId, -1, 1, (int)Entity.CMS.CMSAppId.Carwale)>0;
                var videos = _videosBL.GetVideosByModelId(input.ModelId, Entity.CMS.CMSAppId.Carwale, 1, -1);
                modelDetails.VideoCount = videos != null ? videos.Count : 0;
                SubNavigationDTO quickMenu = _carModelBL.GetModelQuickMenu(modelDetails,null,false, isExpertReviewAvail, "UserReviewDetailsPage","");
                quickMenu.Page = Entity.Enum.Pages.UserReviewListing;
                quickMenu.ShowColorsLink = CMSCommon.IsModelColorPhotosPresent(_modelsCacheRepo.GetModelColorsByModel(modelDetails.ModelId));
                var userReviewList = _cmsCacheRepo.GetUserReviewsList(modelDetails.MakeId,input.ModelId,input.VersionId,startIndex,endIndex,input.SortCriteria);
                
                var modelPrice = modelDetails.New ? _carPrice.GetModelsCarPriceOverview(new List<int>() { modelDetails.ModelId }, input.CityId) : new Dictionary<int,PriceOverview>();
                if (modelPrice != null)
                {
                    PriceOverview price;
                    modelPrice.TryGetValue(modelDetails.ModelId, out price);
                    if (price != null) userReviewsDTO.CarPriceOverview = price;
                    else userReviewsDTO.CarPriceOverview = new PriceOverview();
                }
                userReviewsDTO.UserReviewsList = userReviewList;
                userReviewsDTO.QuickMenuDetails = quickMenu;
                userReviewsDTO.ModelDetails = modelDetails;
                userReviewsDTO.MakeList = _makeCacheRepo.GetCarMakesFromLocalCache();
                userReviewsDTO.PageNumber = (int)Math.Ceiling(modelDetails.ReviewCount / 10.0 );
                var versionDetailsList = _versionBl.GetCarVersions(input.ModelId, Entity.Status.All).FindAll(x => !x.Futuristic);
                userReviewsDTO.CarVersions = versionDetailsList != null ? versionDetailsList.Where(c => c.ReviewCount > 0).ToList()  : null;
            }
            catch (Exception ex)
            {
                ExceptionHandler err = new ExceptionHandler(ex, "UserReviewsListAdapter.GetUserReviewsListDTO()");
                err.LogException();
            }
            return userReviewsDTO;
        }
    }
}
