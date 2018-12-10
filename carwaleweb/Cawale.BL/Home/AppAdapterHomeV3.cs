using AutoMapper;
using Carwale.DTOs;
using Carwale.DTOs.Classified.PopularUC;
using Carwale.DTOs.NewCars;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified.PopularUsedCarsDetails;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.Home;
using Carwale.Notifications;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using Carwale.Interfaces.NewCars;
using Carwale.Entity.AdapterModels;
using Carwale.Notifications.Logs;
using Carwale.DTOs.CMS;
using Carwale.DTOs.CMS.Articles;
using Carwale.Entity.CarData;

namespace Carwale.BL.Home
{

    public class AppAdapterHomeV3 : IServiceAdapterV2
    {
        private readonly IUnityContainer _container;
        private readonly ICarModelCacheRepository _carModelRepo;
        private readonly ICarModels _carModels;
        private readonly ICMSContent _cmsContentRepo;
        private readonly IVideosBL _videosBL;
        private readonly ushort CarModelsPageSize = Convert.ToUInt16(ConfigurationManager.AppSettings["HomeApiModelCount"] ?? "3");
        private readonly ushort ArticlesPageSize = Convert.ToUInt16(ConfigurationManager.AppSettings["HomeApiModelCount"] ?? "2");
        private readonly IPopularUCDetails _popularUcd;

        public AppAdapterHomeV3(IUnityContainer container, ICarModelCacheRepository carModelRepo, ICarModels carModels,
                                ICMSContent cmsContentRepo, IVideosBL videosBL, IPopularUCDetails popularUcd)
        {
            _container = container;
            _carModelRepo = carModelRepo;
            _carModels = carModels;
            _cmsContentRepo = cmsContentRepo;
            _videosBL = videosBL;
            _popularUcd = popularUcd;
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetHomePageData<U>(input), typeof(T));
        }

        private CarHomeV3 GetHomePageData<U>(U input)
        {
            CarHomeV3 _data = null;
            try
            {
                CarDataAdapterInputs inputParam = (CarDataAdapterInputs)Convert.ChangeType(input, typeof(U));
                var _topUcdModel = new List<PopularUCModelAppV2>();
                Pagination _page = new Pagination { PageNo = 1, PageSize = CarModelsPageSize, IsFromApp = true };
                var articleURI = new ArticleRecentURI { ApplicationId = (ushort)CMSAppId.Carwale, ContentTypes = Convert.ToInt16(CMSContentType.News).ToString(), TotalRecords = ArticlesPageSize };

                var topSelling = _carModels.GetTopSellingCarModels(_page, inputParam.CustLocation.CityId, true);
                var _topSellingList = Mapper.Map<List<Entity.CarData.TopSellingCarModel>, List<TopSellingCarModelV2>>(topSelling);

                var newLaunch = _carModels.GetLaunchedCarModelsV1(_page, inputParam.CustLocation.CityId, true);
                var _launchList = Mapper.Map<List<Entity.CarData.LaunchedCarModel>, List<LaunchedCarModelV2>>(newLaunch);

                var upcoming = _carModels.GetUpcomingCarModels(_page);
                var _upcomingList = Mapper.Map<List<UpcomingCarModel>, List<UpcomingModelV2>>(upcoming);

                var newsArticles = _cmsContentRepo.GetMostRecentArticles(articleURI);
                var _newsList = Mapper.Map<List<Entity.CMS.Articles.ArticleSummary>, List<ArticleSummaryDTOV2>>(newsArticles);

                articleURI.ContentTypes = Convert.ToInt16(CMSContentType.RoadTest).ToString();
                var reviews = _cmsContentRepo.GetMostRecentArticles(articleURI);
                var _reviewsList = Mapper.Map<List<Entity.CMS.Articles.ArticleSummary>, List<ArticleSummaryDTOV2>>(reviews);

                var objVideos = _videosBL.GetNewModelsVideosBySubCategory(EnumVideoCategory.FeaturedAndLatest, CMSAppId.Carwale, 1, 2);
                var _videoList = Mapper.Map<List<Video>, List<YouTubeVideoV2>>(objVideos);

                var ucdModel = _popularUcd.GetPopularUsedCarDetails<PopularUCModelApp>(inputParam.CustLocation != null ? inputParam.CustLocation.CityId == -1 ? string.Empty : inputParam.CustLocation.CityId.ToString() : string.Empty);
                _topUcdModel = Mapper.Map<List<PopularUCModelApp>, List<PopularUCModelAppV2>>(ucdModel);

                _data = new CarHomeV3
                {
                    RecentLaunches = _launchList,
                    TopSellingModels = _topSellingList,
                    UpcomingModels = _upcomingList,
                    News = _newsList,
                    ExpertReviews = _reviewsList,
                    Videos = _videoList,
                    InsuranceClientId = Convert.ToInt32(ConfigurationManager.AppSettings["InsuranceClientId"]),
                    ShowInsurance = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowInsurance"]),
                    OrpText = inputParam.CustLocation.CityId <= 0 ? ConfigurationManager.AppSettings["ShowPriceInCityText"] : string.Empty,
                    PopularUsedCar = _topUcdModel
                };

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return _data;
        }
    }
}
