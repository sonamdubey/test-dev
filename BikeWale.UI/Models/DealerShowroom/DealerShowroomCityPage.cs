﻿
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Memcache;
using Bikewale.Models.Make;
using Bikewale.Models.ServiceCenters;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Models.DealerShowroom
{
    /// <summary>
    /// Created By :- Subodh Jain 27 March 2017
    /// Summary :- Sealer Showroom in city page model
    /// </summary>
    public class DealerShowroomCityPage
    {
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly IServiceCenter _objSC = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private DealerShowroomCityPageVM objDealerVM;
        public MakeMaskingResponse objResponse;
        public uint cityId, makeId, TopCount;
        public StatusCodes status;
        public BikeMakeEntityBase objMake;
        public CityEntityBase CityDetails;

        public bool IsMobile { get; internal set; }

        //Constructor
        public DealerShowroomCityPage(IBikeModels<BikeModelEntity, int> bikeModels, IServiceCenter objSC, IDealerCacheRepository objDealerCache, IUsedBikeDetailsCacheRepository objUsedCache, IBikeMakesCacheRepository<int> bikeMakesCache, string makeMaskingName, string cityMaskingName, uint topCount)
        {
            _objDealerCache = objDealerCache;
            _bikeMakesCache = bikeMakesCache;
            _objUsedCache = objUsedCache;
            _objSC = objSC;
            _bikeModels = bikeModels;
            TopCount = topCount;
            ProcessQuery(makeMaskingName, cityMaskingName);
        }
        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To Fetch Data realted to Dealer in city Page
        /// Modified by : Aditi Srivastava on 19 MAy 2017
        /// Summary     : Added variable for GA trigger 
        /// </summary>
        /// <returns></returns>
        public DealerShowroomCityPageVM GetData()
        {
            objDealerVM = new DealerShowroomCityPageVM();

            try
            {


                objMake = _bikeMakesCache.GetMakeDetails(makeId);
                if (objMake != null)
                    objDealerVM.Make = objMake;
                if (cityId > 0)
                {
                    CityDetails = new CityHelper().GetCityById(cityId);
                    objDealerVM.CityDetails = CityDetails;
                }
                objDealerVM.DealersList = BindDataDealers();
                if (objDealerVM.DealersList != null && objDealerVM.DealersList.Dealers != null)
                {
                    objDealerVM.TotalDealers = (uint)objDealerVM.DealersList.Dealers.Count();
                }
                objDealerVM.DealerCountCity = BindOtherDealerInCitiesWidget();
                objDealerVM.UsedBikeModel = BindUsedBikeByModel();
                objDealerVM.BrandCityPopupWidget = new BrandCityPopupModel(EnumBikeType.Dealer, (uint)objMake.MakeId, cityId).GetData();
                objDealerVM.ServiceCenterDetails = BindServiceCenterWidget();
                objDealerVM.PopularBikes = BindMostPopularBikes();
                BindPageMetas(objDealerVM);
                BindLeadCapture(objDealerVM);
                BindShowroomPopularCityWidget(objDealerVM);

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomCityPage.GetData()");
            }
            return objDealerVM;
        }
        /// <summary>
        /// Created by :- Subodh Jain 30 March 2017
        /// Summary :- Added lead popup
        /// </summary>
        /// <param name="objDealerDetails"></param>
        private void BindLeadCapture(DealerShowroomCityPageVM objDealerDetails)
        {
            objDealerDetails.LeadCapture = new LeadCaptureEntity()
            {

                CityId = cityId,
            };
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for most popular bikes
        /// </summary>
        /// <returns></returns>
        private MostPopularBikeWidgetVM BindMostPopularBikes()
        {
            MostPopularBikeWidgetVM objPopularBikes = new MostPopularBikeWidgetVM();
            try
            {
                MostPopularBikesWidget popularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, false, true, PQSourceEnum.Desktop_DealerLocator_Detail_AvailableModels, 0, (uint)objMake.MakeId);
                popularBikes.TopCount = 9;
                popularBikes.CityId = cityId;
                objPopularBikes = popularBikes.GetData();
                objPopularBikes.PageCatId = 5;
                objPopularBikes.PQSourceId = PQSourceEnum.Desktop_HP_MostPopular;
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomDealerDetail.BindMostPopularBikes()");
            }
            return objPopularBikes;
        }
        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for service center
        /// </summary>
        /// <returns></returns>
        private ServiceCenterDetailsWidgetVM BindServiceCenterWidget()
        {
            ServiceCenterDetailsWidgetVM ServiceCenterVM = null;
            try
            {

                ServiceCentersCard objServcieCenter = new ServiceCentersCard(_objSC, TopCount, (uint)objMake.MakeId, CityDetails.CityId);
                ServiceCenterVM = objServcieCenter.GetData();
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomDealerDetail.BindServiceCenterWidget()");
            }

            return ServiceCenterVM;

        }
        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about dealers of other brands
        /// </summary>
        /// <returns></returns>
        private void BindPageMetas(DealerShowroomCityPageVM objPage)
        {

            try
            {
                objPage.PageMetaTags.Title = String.Format("{0} showroom in {1} | {2} {0} bike dealers - BikeWale", objMake.MakeName, CityDetails.CityName, objPage.TotalDealers);
                objPage.PageMetaTags.Keywords = String.Format("{0} showroom {1}, {0} dealers {1}, {1} bike showroom, {1} bike dealers,{1} dealers, {1} bike showroom, bike dealers, bike showroom, dealerships", objMake.MakeName, CityDetails.CityName);
                objPage.PageMetaTags.Description = String.Format("Find address, contact details and direction for {2} {0} showrooms in {1}. Contact {0} showroom near you for prices, EMI options, and availability of {0} bike", objMake.MakeName, CityDetails.CityName, objPage.TotalDealers);
                objPage.Page_H1 = string.Format("{0} Showrooms in {1}", objDealerVM.Make.MakeName, objDealerVM.CityDetails.CityName);

                SetBreadcrumList(objPage);
                SetPageJSONLDSchema(objPage);


            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomCityPage.BindPageMetas()");
            }
        }


        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(DealerShowroomCityPageVM objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            if (webpage != null)
            {
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(DealerShowroomCityPageVM objPage)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                url += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));

            url += "dealer-showroom-locator/";
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Showroom Locator"));

            if (objDealerVM != null && objDealerVM.Make != null)
            {
                url = string.Format("{0}-dealer-showrooms-in-india/", objDealerVM.Make.MaskingName);
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, objDealerVM.Make.MakeName + " Showrooms"));
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, null, objPage.Page_H1));

            objPage.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To Fetch Data realted to Dealer in city Page
        /// </summary>
        /// <returns></returns>
        private DealersEntity BindDataDealers()
        {
            DealersEntity objDealerList = null;
            try
            {
                objDealerList = _objDealerCache.GetDealerByMakeCity(cityId, makeId);
                if (objDealerList != null && objDealerList.Dealers != null && objDealerList.Dealers.Any())
                    foreach (var dealer in objDealerList.Dealers)
                    {
                        dealer.GetOffersGALabel = string.Format("{0}_{1}_{2}", objMake.MakeName, dealer.City, dealer.objArea.AreaName);
                    }
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomCityPage.BindDataDealers()");
            }
            return objDealerList;
        }
        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To Fetch Data For other dealers in near by cities
        /// </summary>
        /// <returns></returns>
        private NearByCityDealer BindOtherDealerInCitiesWidget()
        {
            NearByCityDealer objDealer = new NearByCityDealer();
            uint topCount = 9;
            try
            {
                objDealer.objDealerInNearCityList = _objDealerCache.FetchNearByCityDealersCount(makeId, cityId);
                if (objDealer != null && objDealer.objDealerInNearCityList != null && objDealer.objDealerInNearCityList.Any())
                {
                    objDealer.objDealerInNearCityList = objDealer.objDealerInNearCityList.Take((int)topCount);
                }
                objDealer.Make = objMake;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomCityPage.BindOtherDealerInCitiesWidget()");

            }

            return objDealer;
        }
        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To process in put query string
        /// </summary>
        /// <returns></returns>
        private void ProcessQuery(string makeMaskingName, string cityMaskingName)
        {
            objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
            if (objResponse != null)
            {
                if (objResponse.StatusCode == 200)
                {
                    makeId = objResponse.MakeId;
                    cityId = CitiMapping.GetCityId(cityMaskingName);
                    status = StatusCodes.ContentFound;
                    if (cityId <= 0)
                        status = StatusCodes.ContentNotFound;
                }
                else if (objResponse.StatusCode == 301)
                {
                    status = StatusCodes.RedirectPermanent;
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            else
            {
                status = StatusCodes.ContentNotFound;
            }
        }
        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To Fetch Data realted to Used Bike in city
        /// </summary>
        /// <returns></returns>
        private UsedBikeModelsWidgetVM BindUsedBikeByModel()
        {
            UsedBikeModelsWidgetVM UsedBikeModel = new UsedBikeModelsWidgetVM();
            try
            {

                UsedBikeModelsWidgetModel objUsedBike = new UsedBikeModelsWidgetModel(TopCount, _objUsedCache);
                if (makeId > 0)
                    objUsedBike.makeId = makeId;
                if (cityId > 0)
                    objUsedBike.cityId = cityId;
                UsedBikeModel = objUsedBike.GetData();
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomCityPage.BindUsedBikeByModel()");
            }

            return UsedBikeModel;

        }

        /// <summary>
        /// Created By : Snehal Dange on 3rd Oct 2017
        /// Description : Method for showrooms in popular cities widget 
        /// </summary>
        /// <param name="objVM"></param>
        private void BindShowroomPopularCityWidget(DealerShowroomCityPageVM objDealerDetails)
        {
            DealersServiceCentersIndiaWidgetVM objData = new DealersServiceCentersIndiaWidgetVM();
            try
            {
                uint topCount = 8;
                objData.DealerServiceCenters = _objDealerCache.GetPopularCityDealer(_makeId, topCount);
                objData.MakeMaskingName = _makeMaskingName;
                objData.MakeName = objVM.Make.MakeName;
                objVM.DealersServiceCenterPopularCities = objData;
                if (objData.DealerServiceCenters.DealerDetails.Any())
                {
                    objVM.DealersServiceCenterPopularCities.DealerServiceCenters.DealerDetails = objVM.DealersServiceCenterPopularCities.DealerServiceCenters.DealerDetails.Where(m => !m.CityId.Equals(_cityId)).ToList();
                }

            }
            catch (System.Exception ex)
            {

                ErrorClass er = new ErrorClass(ex, "ServiceCenterDetailsPage.BindServiceCenterPopularCityWidget");
            }

        }
    }
}