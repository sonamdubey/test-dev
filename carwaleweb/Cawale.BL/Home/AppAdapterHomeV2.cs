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

namespace Carwale.BL.Home
{
    //Created By : Sachin Bharti on 4 Aug 2016
    //logic to get app home page data
    public class AppAdapterHomeV2 : IServiceAdapter
    {
        private readonly IUnityContainer _container;
        private readonly ICarModelCacheRepository _carModelRepo;
        private readonly ICarModels _carModels;
        private readonly ICMSContent _cmsContentRepo;
        private readonly IVideosBL _videosBL;
        private readonly IPopularUCDetails _popularUCD;

        public AppAdapterHomeV2(IUnityContainer container, ICarModelCacheRepository carModelRepo, ICarModels carModels,
                                ICMSContent cmsContentRepo, IVideosBL videosBL, IPopularUCDetails popularUCD)
        {
            _container = container;
            _carModelRepo = carModelRepo;
            _carModels = carModels;
            _cmsContentRepo = cmsContentRepo;
            _videosBL = videosBL;
            _popularUCD = popularUCD;
        }

        public T Get<T>(string cityId)
        {
            return (T)Convert.ChangeType(GetHomePageData(cityId), typeof(T));
        }

        private CarHomeV2 GetHomePageData(string cityId)
        {
            CarHomeV2 _data = null;
            try
            {
                ushort CarModelsPageSize = System.Configuration.ConfigurationManager.AppSettings["HomeApiModelCount"] != null ? Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["HomeApiModelCount"]) : (ushort)3/*Default*/;
                ushort ArticlesPageSize = System.Configuration.ConfigurationManager.AppSettings["HomeApiArticleCount"] != null ? Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["HomeApiModelCount"]) : (ushort)2/*Default*/;
                var _upcomingList = new List<UpcomingModelV2>();
                var _topSellingList = new List<TopSellingCarModelV2>();
                var _launchList = new List<LaunchedCarModelV2>();
                var _reviewsList = new List<Carwale.DTOs.CMS.Articles.ArticleSummaryDTOV2>();
                var _videoList = new List<YouTubeVideoV2>();
                var _newsList = new List<Carwale.DTOs.CMS.Articles.ArticleSummaryDTOV2>();
                var _topUCDModel = new List<PopularUCModelAppV2>();

                Pagination _page = new Pagination() { PageNo = 1, PageSize = CarModelsPageSize, IsFromApp = true };
                var articleURI = new ArticleRecentURI() { ApplicationId = (ushort)CMSAppId.Carwale, ContentTypes = Convert.ToInt16(CMSContentType.News).ToString(), TotalRecords = ArticlesPageSize };

                var topSelling = _carModels.GetTopSellingCarModels(_page, int.Parse(cityId));
                _topSellingList = Mapper.Map<List<Entity.CarData.TopSellingCarModel>, List<TopSellingCarModelV2>>(topSelling);

                var newLaunch = _carModels.GetLaunchedCarModels(_page, int.Parse(cityId));
                _launchList = Mapper.Map<List<Entity.CarData.LaunchedCarModel>, List<LaunchedCarModelV2>>(newLaunch);           

                var upcoming = _carModels.GetUpcomingCarModels(_page);
                _upcomingList = Mapper.Map<List<Entity.CarData.UpcomingCarModel>, List<UpcomingModelV2>>(upcoming);

                var newsArticles = _cmsContentRepo.GetMostRecentArticles(articleURI);
                _newsList = Mapper.Map<List<Carwale.Entity.CMS.Articles.ArticleSummary>, List<Carwale.DTOs.CMS.Articles.ArticleSummaryDTOV2>>(newsArticles);

                articleURI.ContentTypes = Convert.ToInt16(CMSContentType.RoadTest).ToString();
                var reviews = _cmsContentRepo.GetMostRecentArticles(articleURI);
                _reviewsList = Mapper.Map<List<Carwale.Entity.CMS.Articles.ArticleSummary>, List<Carwale.DTOs.CMS.Articles.ArticleSummaryDTOV2>>(reviews);

                var objVideos = _videosBL.GetNewModelsVideosBySubCategory(EnumVideoCategory.FeaturedAndLatest, CMSAppId.Carwale, 1, 2);
                _videoList = Mapper.Map<List<Video>, List<YouTubeVideoV2>>(objVideos);

                var UCDModel = _popularUCD.GetPopularUsedCarDetails<PopularUCModelApp>(cityId == "-1" ? "" : cityId);
                _topUCDModel = Mapper.Map<List<PopularUCModelApp>, List<PopularUCModelAppV2>>(UCDModel);

                _data = new CarHomeV2()
                {
                    RecentLaunches = _launchList,
                    TopSellingModels = _topSellingList,
                    UpcomingModels = _upcomingList,
                    PopularUsedCar = _topUCDModel,
                    News = _newsList,
                    ExpertReviews = _reviewsList,
                    Videos = _videoList,
                    InsuranceClientId = Convert.ToInt32(ConfigurationManager.AppSettings["InsuranceClientId"]),
                    ShowInsurance = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowInsurance"])
                };
            }
            catch (Exception ex)
            {
                ExceptionHandler err = new ExceptionHandler(ex, "AppAdapterHome");
                err.LogException();
            }
            return _data;
        }

        public T Get<T>()
        {
            throw new NotImplementedException();
        }
    }
}
