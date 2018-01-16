using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
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
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.ServiceCenters
{
    /// <summary>
    /// Created by Sajal Gupta on 29-03-2017
    /// Description : This Model will fetch data for service centers in city page 
    /// Modified By:Snehal Dange on 29th Sep 2017
    /// Descrption : Added BindServiceCenterPopularCityWidget
    /// Modified by : Snehal Dange on 19th Jan 2017
    /// Description : Added BindResearchMoreMakeWidget
    /// </summary>
    public class ServiceCenterCityPage
    {
        private string _cityMaskingName;
        private string _makeMaskingName;
        private uint _makeId, _cityId;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCache;
        private readonly IServiceCenter _objSC;
        private readonly IServiceCenterCacheRepository _objSCCache;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;

        public MakeMaskingResponse objResponse;
        public StatusCodes status;

        public uint NearByCitiesWidgetTopCount { get; set; }
        public uint UsedBikeWidgetTopCount { get; set; }
        public uint BikeShowroomWidgetTopCount { get; set; }
        public bool IsMobile { get; internal set; }

        public ServiceCenterCityPage(IDealerCacheRepository objDealerCache, IUsedBikeDetailsCacheRepository objUsedCache, IBikeModels<BikeModelEntity, int> bikeModels, IServiceCenterCacheRepository objSCCache, IServiceCenter objSC, IBikeMakesCacheRepository bikeMakesCache, string cityMaskingName, string makeMaskingName)
        {
            _objSC = objSC;
            _objSCCache = objSCCache;
            _bikeModels = bikeModels;
            _objDealerCache = objDealerCache;
            _objUsedCache = objUsedCache;
            _bikeMakesCache = bikeMakesCache;
            _cityMaskingName = cityMaskingName;
            _makeMaskingName = makeMaskingName;

            ProcessQuery(makeMaskingName, cityMaskingName);
        }

        public ServiceCenterCityPageVM GetData()
        {
            ServiceCenterCityPageVM objVM = null;
            try
            {
                objVM = new ServiceCenterCityPageVM();

                if (_cityId > 0)
                    objVM.City = new CityHelper().GetCityById(_cityId);

                if (_makeId > 0)
                    objVM.Make = _bikeMakesCache.GetMakeDetails(_makeId);

                objVM.ServiceCentersListObject = _objSC.GetServiceCentersByCity(_cityId, (int)_makeId);
                objVM.NearByCityWidgetData = new ServiceCentersInNearByCities(_objSCCache, NearByCitiesWidgetTopCount, _cityId, objVM.Make).GetData();

                MostPopularBikesWidget objPopularBikesWidget = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, false, true, PQSourceEnum.Mobile_ServiceCenter_Listing_CityPage, 0, _makeId);
                objPopularBikesWidget.TopCount = 9;
                objPopularBikesWidget.CityId = _cityId;
                objVM.PopularWidgetData = objPopularBikesWidget.GetData();



                objVM.UsedBikesByMakeList = BindUsedBikeByModel(objVM.City);

                BindDealersWidget(objVM);
                BindServiceCenterPopularCityWidget(objVM);
                BindResearchMoreMakeWidget(objVM);
                objVM.BrandCityPopupWidget = new BrandCityPopupModel(EnumBikeType.ServiceCenter, (uint)_makeId, (uint)_cityId).GetData();

                objVM.BikeCityPopup = new PopUp.BikeCityPopup()
                {
                    ApiUrl = "/api/servicecenter/cities/make/" + objVM.Make.MakeId + "/",
                    PopupShowButtonMessage = "Show service centers",
                    PopupSubHeading = "See service centers in your city!",
                    FetchDataPopupMessage = "Fetching service centers for ",
                    RedirectUrl = string.Format("/service-centers/{0}/", objVM.Make.MaskingName),
                    IsCityWrapperPresent = 0

                };

                BindPageMetas(objVM);
                if (_cityId > 0)
                {
                    GetServiceCenterBrandsInCity(objVM);
                }
                objVM.Page = Entities.Pages.GAPages.ServiceCenter_City_Page;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterCityPage.GetData()");
            }
            return objVM;
        }

        private void BindPageMetas(ServiceCenterCityPageVM objPageVM)
        {

            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null && objPageVM.Make != null && objPageVM.City != null && objPageVM.ServiceCentersListObject != null)
                {
                    objPageVM.PageMetaTags.Title = string.Format("{0} service centers in {1} | {0} bike servicing in {1} - BikeWale", objPageVM.Make.MakeName, objPageVM.City.CityName);
                    objPageVM.PageMetaTags.Keywords = string.Format("{0} servicing {1}, {0} service center in {1}, {0} Service centers, {0} service schedules, {0} bike repair, repairing, servicing", objPageVM.Make.MakeName, objPageVM.City.CityName);
                    objPageVM.PageMetaTags.Description = string.Format("There are {0} {1} service centers in {2}. Get in touch with your nearest {1} service center for service repairing, schedule details, pricing, pick and drop facility. Check the Service schedule for {1} bikes now.", objPageVM.ServiceCentersListObject.Count, objPageVM.Make.MakeName, objPageVM.City.CityName);
                    objPageVM.Page_H1 = string.Format("{0} Service Center{1} in {2}", objPageVM.Make.MakeName, (objPageVM.ServiceCentersListObject.Count > 1 ? "s" : ""), objPageVM.City.CityName);
                    SetBreadcrumList(objPageVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterIndiaPage.BindPageMetas()");
            }
        }

        private void ProcessQuery(string makeMaskingName, string cityMaskingName)
        {
            objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
            _cityId = CitiMapping.GetCityId(cityMaskingName);

            if (objResponse != null)
            {
                if (objResponse.StatusCode == 200 && _cityId > 0)
                {
                    _makeId = objResponse.MakeId;
                    status = StatusCodes.ContentFound;
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

        private UsedBikeModelsWidgetVM BindUsedBikeByModel(CityEntityBase city)
        {
            UsedBikeModelsWidgetVM UsedBikeModel = new UsedBikeModelsWidgetVM();
            try
            {
                UsedBikeModel.CityDetails = city;
                if (_makeId > 0)
                {
                    UsedBikeModel.UsedBikeModelList = _objUsedCache.GetUsedBikeByModelCountInCity(_makeId, _cityId, UsedBikeWidgetTopCount);
                }
                else
                {
                    UsedBikeModel.UsedBikeModelList = _objUsedCache.GetUsedBikeCountInCity(_cityId, UsedBikeWidgetTopCount);
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCenterIndiaPage.BindUsedBikeByModel()");
            }
            return UsedBikeModel;
        }

        private void BindDealersWidget(ServiceCenterCityPageVM objVM)
        {
            try
            {
                DealerCardWidget objDealer = new DealerCardWidget(_objDealerCache, _cityId, _makeId);
                objDealer.TopCount = BikeShowroomWidgetTopCount;
                objVM.DealersWidgetData = objDealer.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterDetailsPage.BindDealersWidget()");
            }
        }

        /// <summary>
        /// Created By : Snehal Dange on 29Sep 2017
        /// Description : Method for service center in popular cities widget 
        /// </summary>
        /// <param name="objVM"></param>
        private void BindServiceCenterPopularCityWidget(ServiceCenterCityPageVM objVM)
        {
            DealersServiceCentersIndiaWidgetVM objData = new DealersServiceCentersIndiaWidgetVM();
            try
            {
                uint topCount = 8;
                objData.DealerServiceCenters = _objDealerCache.GetPopularCityDealer(_makeId, topCount);
                objData.MakeMaskingName = _makeMaskingName;
                objData.MakeName = objVM.Make.MakeName;
                objData.CityCardTitle = "service centers in";
                objData.CityCardLink = "service-centers";
                objData.IsServiceCenterPage = true;
                objVM.DealersServiceCenterPopularCities = objData;
                if (objData.DealerServiceCenters.DealerDetails.Any())
                {
                    objVM.DealersServiceCenterPopularCities.DealerServiceCenters.DealerDetails = objVM.DealersServiceCenterPopularCities.
                                                                                    DealerServiceCenters.DealerDetails.Where(m => !m.CityId.Equals(_cityId)).ToList();
                }
                DealersEntity obj = _objDealerCache.GetDealerByMakeCity(_cityId, _makeId);
                objVM.IsShowroomPresentInCity = obj.TotalCount > 0;


            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCenterDetailsPage.BindServiceCenterPopularCityWidget");
            }

        }

        /// <summary>
        /// Created By :Snehal Dange on 2nd Nov 2017
        /// Description: Breadcrum for service center city page
        /// Modified by : Snehal Dange on 27th Dec 2017
        /// Desc        : Added 'New Bikes' in breadcrumb
        /// </summary>
        /// <param name="objPage"></param>
        private void SetBreadcrumList(ServiceCenterCityPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null)
                {
                    IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                    string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                    ushort position = 1;
                    if (IsMobile)
                    {
                        url += "m/";
                    }

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", url), "New Bikes"));
                    if (objPageVM.Make != null)
                    {
                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}{1}-bikes/", url, objPageVM.Make.MaskingName), string.Format("{0} Bikes", objPageVM.Make.MakeName)));
                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}service-centers/{1}/", url, objPageVM.Make.MaskingName), objPageVM.Make.MakeName + " Service Centers in India"));
                    }
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objPageVM.Page_H1));
                    objPageVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterCityPage.SetBreadcrumList");
            }

        }


        /// <summary>
        /// Created By: Snehal Dange on 13th Dec 2017
        /// Decsription : To get list of similar brands of service center in that city 
        /// </summary>
        /// <param name="objDealerDetails"></param>
        private void GetServiceCenterBrandsInCity(ServiceCenterCityPageVM objData)
        {

            try
            {
                uint topCount = 9;
                if (objData != null)
                {
                    objData.SimilarBrandsByCity = new OtherMakesVM();
                    if (objData.SimilarBrandsByCity != null && _bikeMakesCache != null)
                    {
                        var similarBrandsList = _bikeMakesCache.GetServiceCenterBrandsInCity(_cityId);
                        if (_makeId > 0 && similarBrandsList != null && similarBrandsList.Any())
                        {
                            objData.SimilarBrandsByCity.Makes = Utility.BikeFilter.FilterMakesByCategory(_makeId, similarBrandsList);
                        }
                        if (objData.SimilarBrandsByCity.Makes != null && objData.SimilarBrandsByCity.Makes.Any())
                        {
                            objData.SimilarBrandsByCity.Makes = objData.SimilarBrandsByCity.Makes.Take((int)topCount);

                        }
                        objData.SimilarBrandsByCity.CardText = "Service Center";
                        objData.SimilarBrandsByCity.PageLinkFormat = string.Format("/service-centers/{0}/{1}/", "{0}", objData.City.CityMaskingName);
                        objData.SimilarBrandsByCity.PageTitleFormat = string.Format("{0} Service Centers in {1}", "{0}", objData.City.CityName);
                    }
                }

            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ServiceCenterDetailsPage.GetServiceCenterBrandsInCity_Make_{0}_City_{1}", _makeId, _cityId));
            }

        }


        /// <summary>
        /// Created by :  Snehal Dange on 19th Jan 2018
        /// Description: Method to bind research more about make widget data
        /// </summary>
        /// <param name="objData"></param>
        private void BindResearchMoreMakeWidget(ServiceCenterCityPageVM objData)
        {

            try
            {
                if (_makeId > 0 && objData != null)
                {
                    objData.ResearchMoreMakeWidget = new ResearchMoreAboutMakeVM();

                    if (objData.ResearchMoreMakeWidget != null)
                    {
                        if (objData.City != null && objData.City.CityId > 0)
                        {
                            objData.ResearchMoreMakeWidget.WidgetObj = _bikeMakesCache.ResearchMoreAboutMakeByCity(_makeId, objData.City.CityId);
                            if (objData.ResearchMoreMakeWidget.WidgetObj != null)
                            {
                                objData.ResearchMoreMakeWidget.WidgetObj.City = objData.City;
                            }

                        }
                        else
                        {
                            objData.ResearchMoreMakeWidget.WidgetObj = _bikeMakesCache.ResearchMoreAboutMake(_makeId);
                        }
                        if (objData.ResearchMoreMakeWidget.WidgetObj != null)
                        {
                            objData.ResearchMoreMakeWidget.WidgetObj.ShowServiceCenterLink = false;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("ServiceCenterCityPage.BindResearchMoreMakeWidget() makeId:{0} , cityId:{1}", _makeId, (objData.City != null ? objData.City.CityId.ToString() : "0")));
            }
        }
    }
}