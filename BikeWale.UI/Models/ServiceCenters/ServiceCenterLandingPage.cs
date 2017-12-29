using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Used;
using Bikewale.Models.BikeCare;
using Bikewale.ServiceCenters;
using Bikewale.Utility;
using System;

namespace Bikewale.Models.ServiceCenters
{
    /// <summary>
    /// Created by Sajal Gupta on 27-03-2017
    /// This Model will fetch data for service centers landing page
    /// </summary>
    public class ServiceCenterLandingPage
    {
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly ICityCacheRepository _ICityCache = null;

        public ushort BrandWidgetTopCount { get; set; }
        public ushort BikeCareRecordsCount { get; set; }
        public ushort PopularBikeWidgetTopCount { get; set; }
        public ushort NewLaunchedBikesWidgtData { get; set; }
        public ushort UpcomingBikesWidgetData { get; set; }
        public ushort UsedBikeModelWidgetTopCount { get; set; }

        public ServiceCenterLandingPage(ICityCacheRepository ICityCache, IUsedBikeDetailsCacheRepository objUsedCache, IUpcoming upcoming, INewBikeLaunchesBL newLaunches, IBikeModels<BikeModelEntity, int> bikeModels, ICMSCacheContent articles, IBikeMakesCacheRepository bikeMakes)
        {
            _bikeMakes = bikeMakes;
            _articles = articles;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
            _upcoming = upcoming;
            _ICityCache = ICityCache;
            _objUsedCache = objUsedCache;
        }

        public ServiceCenterLandingPageVM GetData()
        {
            ServiceCenterLandingPageVM objVM = null;
            try
            {
                objVM = new ServiceCenterLandingPageVM();
                objVM.MakesList = _bikeMakes.GetMakesByType(EnumBikeType.ServiceCenter);
                objVM.Brands = new BrandWidgetModel(BrandWidgetTopCount, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.ServiceCenter);
                objVM.BikeCareWidgetVM = new RecentBikeCare(_articles).GetData(BikeCareRecordsCount, 0, 0);

                MostPopularBikesWidget objPopularBikesWidget = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, true, false, PQSourceEnum.Mobile_ServiceCenter_DefaultPage, 0);
                objPopularBikesWidget.TopCount = PopularBikeWidgetTopCount;
                objVM.PopularWidgetData = objPopularBikesWidget.GetData();
                objVM.NewLaunchWidgetData = new NewLaunchedWidgetModel(NewLaunchedBikesWidgtData, _newLaunches).GetData();
                objVM.UpcomingWidgetData = BindUpCompingBikesWidget();
                BindCity(objVM);
                objVM.UsedBikesModelWidgetData = new UsedBikeModelsWidgetVM();
                objVM.UsedBikesModelWidgetData.CityDetails = objVM.City;
                objVM.UsedBikesCityWidgetData = BindUsedBikeCityWidget(objVM);
                BindPageMetas(objVM);
                objVM.Page = Entities.Pages.GAPages.ServiceCenter_Landing_Page;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterLandingPage.GetData()");
            }
            return objVM;
        }

        private void BindPageMetas(ServiceCenterLandingPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null)
                {
                    objPageVM.PageMetaTags.Title = "Locate Authorised Bike Service Center | Bikes Servicing Center Nearby - BikeWale";
                    objPageVM.PageMetaTags.Keywords = "servicing, bike servicing, authorised service centers, bike service centers, servicing bikes, bike repairing, repair bikes";
                    objPageVM.PageMetaTags.Description = "Locate authorised service centers in India. Find authorised service centers of Hero, Honda, Bajaj, Royal Enfield, Harley Davidson, Yamaha, KTM, Aprilia and many more brands in more than 1000+ cities.";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterLandingPage.BindPageMetas()");
            }
        }

        private void BindCity(ServiceCenterLandingPageVM objVM)
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                var _cookieCityId = currentCityArea.CityId;

                if (_cookieCityId > 0)
                {
                    objVM.City = new CityHelper().GetCityById(_cookieCityId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterLandingPage.BindCity()");
            }
        }

        private UsedBikeCitiesWidgetVM BindUsedBikeCityWidget(ServiceCenterLandingPageVM objVM)
        {
            UsedBikeCitiesWidgetVM widgetObj = null;
            try
            {
                string cityWidgetTitle = string.Empty, cityWidgetHref = string.Empty;

                if (objVM.City != null)
                {
                    objVM.UsedBikesModelWidgetData.UsedBikeModelList = _objUsedCache.GetUsedBikeCountInCity(objVM.City.CityId, UsedBikeModelWidgetTopCount);
                }
                else
                {
                    objVM.UsedBikesModelWidgetData.UsedBikeModelList = _objUsedCache.GetUsedBike(UsedBikeModelWidgetTopCount);
                }

                cityWidgetTitle = "Second hand bikes in India";
                cityWidgetHref = "/used/bikes-in-india/";

                widgetObj = new UsedBikeCitiesWidgetModel(cityWidgetTitle, cityWidgetHref, _ICityCache).GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterLandingPage.BindUsedBikeCityWidget()");
            }
            return widgetObj;
        }

        private UpcomingBikesWidgetVM BindUpCompingBikesWidget()
        {
            UpcomingBikesWidgetVM objUpcomingBikes = null;
            try
            {
                UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);

                objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
                {
                    PageSize = UpcomingBikesWidgetData,
                    PageNo = 1
                };
                objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
                objUpcomingBikes = objUpcoming.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterLandingPage.BindUpCompingBikesWidget()");
            }
            return objUpcomingBikes;
        }

    }
}