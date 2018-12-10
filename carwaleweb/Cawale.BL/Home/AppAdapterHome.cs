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

namespace Carwale.BL.Home
{
    //Created By : Ashwini Todkar on 5 Aug 2015 
    //logic to get app home page data
    public class AppAdapterHome : IServiceAdapter
    {
        private readonly IUnityContainer _container;

        public AppAdapterHome(IUnityContainer container)
        {
            _container = container;
        }

        public T Get<T>(string cityId)
        {
            return (T)Convert.ChangeType(GetHomePageData(cityId), typeof(T));
        }

        private CarHome GetHomePageData(string cityId)
        {
            CarHome _data = null;
            try
            {
                ushort CarModelsPageSize = System.Configuration.ConfigurationManager.AppSettings["HomeApiModelCount"] != null ? Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["HomeApiModelCount"]) : (ushort)3/*Default*/;
                ushort ArticlesPageSize = System.Configuration.ConfigurationManager.AppSettings["HomeApiArticleCount"] != null ? Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["HomeApiModelCount"]) : (ushort)2/*Default*/;

                ICarModelCacheRepository carModelsCache = _container.Resolve<ICarModelCacheRepository>();
                ICarModels carModelBL = _container.Resolve<ICarModels>();
                ICMSContent cmsCacheContainer = _container.Resolve<ICMSContent>();
                IVideosBL blObj = _container.Resolve<IVideosBL>();
                IPopularUCDetails _popularUCBL = _container.Resolve<IPopularUCDetails>();

                var _upcomingList = new List<UpcomingModel>();
                var _topSellingList = new List<TopSellingCarModel>();
                var _launchList = new List<LaunchedCarModel>();
                var _reviewsList = new List<Article>();
                var _videoList = new List<YouTubeVideo>();

                Pagination _page = new Pagination() { PageNo = 1, PageSize = CarModelsPageSize, IsFromApp = true };

                var articleURI = new ArticleRecentURI() { ApplicationId = (ushort)CMSAppId.Carwale, ContentTypes = Convert.ToInt16(CMSContentType.News).ToString(), TotalRecords = ArticlesPageSize };              

                carModelBL.GetLaunchedCarModels(_page).ForEach(item => _launchList.Add(new LaunchedCarModel() { Make = item.Make, Model = item.Model, Image = item.Image, Review = item.Review,Version = item.Version, City = new DTOs.City() { CityId = item.City.CityId, CityName = item.City.CityName }, Price = Format.PriceLacCr(item.Price.MinPrice.ToString()) + " - " + Format.PriceLacCr(item.Price.MaxPrice.ToString()), LaunchedDate = item.LaunchedDate }));              
                carModelBL.GetTopSellingCarModels(_page).ForEach(item => _topSellingList.Add(new TopSellingCarModel() { Make = item.Make, Model = item.Model, Image = item.Image, Review = item.Review, City = new DTOs.City() { CityId = item.City.CityId, CityName = item.City.CityName }, Price = Format.PriceLacCr(item.Price.MinPrice.ToString()) + " - " + Format.PriceLacCr(item.Price.MaxPrice.ToString()) }));
                carModelBL.GetUpcomingCarModels(_page).ForEach(item => _upcomingList.Add(new UpcomingModel() { Price = Format.PriceLacCr(item.Price.MinPrice.ToString("0.")) + " - " + Format.PriceLacCr(item.Price.MaxPrice.ToString("0.")), Image = item.Image, ExpectedLaunch = item.ExpectedLaunch, ExpectedLaunchId = item.ExpectedLaunchId, Make = new MakeEntity() { MakeName = item.MakeName }, Model = new ModelEntity() { MaskingName = item.MaskingName, ModelName = item.ModelName, ModelId = item.ModelId } }));

                var _newsList = new List<Article>();


                cmsCacheContainer.GetMostRecentArticles(articleURI).ForEach(item => _newsList.Add(new Article() { Author = new Author { Name = item.AuthorName, MaskingName = item.AuthorMaskingName }, Image = new Carwale.Entity.CarData.CarImageBase() { HostUrl = item.HostUrl, ImagePath = item.OriginalImgUrl }, PublishedDate = ExtensionMethods.ConvertDateToDays(item.DisplayDate), Title = item.Title, Url = item.ArticleUrl, Description = item.Description, BasicId = item.BasicId }));

                articleURI.ContentTypes = Convert.ToInt16(CMSContentType.RoadTest).ToString();

                cmsCacheContainer.GetMostRecentArticles(articleURI).ForEach(item => _reviewsList.Add(new Article() { Author = new Author { Name = item.AuthorName, MaskingName = item.AuthorMaskingName }, Image = new Carwale.Entity.CarData.CarImageBase() { HostUrl = item.HostUrl, ImagePath = item.OriginalImgUrl }, PublishedDate = item.DisplayDate.ToString("MMMM dd, yyyy"), Title = item.Title, Url = item.ArticleUrl, Description = item.Description, BasicId = item.BasicId }));

                var objVideos = blObj.GetNewModelsVideosBySubCategory(EnumVideoCategory.FeaturedAndLatest, CMSAppId.Carwale, 1, 2);

                if (objVideos != null)
                    objVideos.ForEach(item => _videoList.Add(new YouTubeVideo() { Image = new Carwale.Entity.CarData.CarImageBase() { HostUrl = item.ImgHost, ImagePath = item.ImagePath }, Likes = item.Likes, Title = item.VideoTitle, PublishedDate = item.DisplayDate.ToString("MMMM dd, yyyy"), Url = item.VideoUrl, Views = item.Views, VideoId = item.VideoId }));

                _data = new CarHome()
                {
                    RecentLaunches = _launchList,
                    TopSellingModels = _topSellingList,
                    UpcomingModels = _upcomingList,
                    PopularUsedCar = _popularUCBL.GetPopularUsedCarDetails<PopularUCModelApp>(cityId == "-1" ? ConfigurationManager.AppSettings["DefaultCityId"] : cityId),
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
