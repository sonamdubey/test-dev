using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Models.BikeCare;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.ServiceCenters
{
    /// <summary>
    /// Created by Sajal Gupta on 27-03-2017
    /// This Model will fetch data for service centers in india page
    /// </summary>
    public class ServiceCenterIndiaPage
    {
        private readonly string _makeMaskingName;
        private readonly IServiceCenterCacheRepository _objCache = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCache = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly ICMSCacheContent _articles = null;

        public StatusCodes status;
        public string redirectUrl;
        public bool isMobile { get; set; }

        private uint _makeId;
        private uint _cookieCityId;

        public MakeMaskingResponse objResponse;

        private uint _usedBikesTopCount = 9;
        private uint _bikeCareRecordsCount = 3;

        public ServiceCenterIndiaPage(ICMSCacheContent articles, IUsedBikeDetailsCacheRepository objUsedCache, IBikeMakesCacheRepository bikeMakesCache, IServiceCenterCacheRepository objCache, string makemaskingName)
        {
            _objUsedCache = objUsedCache;
            _bikeMakesCache = bikeMakesCache;
            _makeMaskingName = makemaskingName;
            _objCache = objCache;
            _articles = articles;

            ProcessQuery(_makeMaskingName);
        }

        public ServiceCenterIndiaPageVM GetData()
        {
            ServiceCenterIndiaPageVM objVM = null;
            try
            {
                objVM = new ServiceCenterIndiaPageVM();
                objVM.ServiceCentersCityList = _objCache.GetServiceCenterList(Convert.ToUInt32(_makeId));
                objVM.Make = _bikeMakesCache.GetMakeDetails(_makeId);
                objVM.ServiceCenterBrandsList = new ServiceCentersByBrand(_objCache, _makeId).GetData();
                objVM.UsedBikesByMakeList = BindUsedBikeByModel(_usedBikesTopCount);
                objVM.BikeCareWidgetVM = new RecentBikeCare(_articles).GetData(_bikeCareRecordsCount, 0, 0);

                BindPageMetas(objVM);
                objVM.Page = Entities.Pages.GAPages.ServiceCenter_Country_Page;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterIndiaPage.GetData()");
            }
            return objVM;
        }

        private void BindPageMetas(ServiceCenterIndiaPageVM objPageVM)
        {

            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null && objPageVM.Make != null && objPageVM.ServiceCentersCityList != null)
                {
                    objPageVM.PageMetaTags.Title = string.Format("Authorised {0}  Service Centers in India | {0} bike servicing  in India -  BikeWale", objPageVM.Make.MakeName);
                    objPageVM.PageMetaTags.Keywords = string.Format("{0} Servicing centers, {0} service centers, {0} service center contact details, Service Schedule for {0} bikes, bike repair, {0} bike repairing", objPageVM.Make.MakeName);
                    objPageVM.PageMetaTags.Description = string.Format("There are {1} authorised {0}  service centers in {2} cities in India. Get in touch with your nearest {0} bikes service center to get your bike serviced. Check your service schedules now.", objPageVM.Make.MakeName, objPageVM.ServiceCentersCityList.ServiceCenterCount, objPageVM.ServiceCentersCityList.CityCount);
                    objPageVM.Page_H1 = string.Format("{0} Service Centers in India", objPageVM.Make.MakeName);
                    SetBreadcrumList(objPageVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterIndiaPage.BindPageMetas()");
            }
        }

        private void ProcessQuery(string makeMaskingName)
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                _cookieCityId = currentCityArea.CityId;

                objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200)
                    {
                        _makeId = objResponse.MakeId;
                        var _cities = _objCache.GetServiceCenterCities(_makeId);

                        CityEntityBase city = null;

                        if (_cities != null && _cities.Any())
                        {
                            var _city = _cities.FirstOrDefault(x => x.CityId == _cookieCityId);

                            if (_city != null)
                            {
                                city = new CityHelper().GetCityById(_cookieCityId);

                                if (city != null)
                                {
                                    redirectUrl = String.Format("/service-centers/{0}/{1}/", makeMaskingName, city.CityMaskingName);
                                    status = StatusCodes.RedirectTemporary;
                                }
                            }
                            else
                            {
                                status = StatusCodes.ContentFound;
                            }
                        }

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
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCenterIndiaPage.ProcessQuery()");
            }
        }

        private UsedBikeModelsWidgetVM BindUsedBikeByModel(uint topCount)
        {
            UsedBikeModelsWidgetVM UsedBikeModel = new UsedBikeModelsWidgetVM();
            try
            {
                if (_makeId > 0)
                {
                    UsedBikeModel.UsedBikeModelList = _objUsedCache.GetPopularUsedModelsByMake(_makeId, topCount);
                }
                else
                {
                    UsedBikeModel.UsedBikeModelList = _objUsedCache.GetUsedBike(topCount);
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCenterIndiaPage.BindUsedBikeByModel()");
            }
            return UsedBikeModel;
        }

        /// <summary>
        /// Created By :Snehal Dange on 2th Nov 2017
        /// Description: Breadcrum list for service center page.
        /// Modified by : Snehal Dange on 27th Dec 2017
        /// Desc        : Added 'New Bikes' in breadcrumb
        /// </summary>
        /// <param name="objPage"></param>
        private void SetBreadcrumList(ServiceCenterIndiaPageVM objPageVM)
        {

            try
            {
                if (objPageVM != null)
                {
                    IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                    string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                    ushort position = 1;
                    if (isMobile)
                    {
                        url += "m/";
                    }

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", url), "New Bikes"));
                    if (objPageVM.Make != null)
                    {
                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}{1}-bikes/", url, objPageVM.Make.MaskingName), string.Format("{0} Bikes", objPageVM.Make.MakeName)));
                    }

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objPageVM.Page_H1));


                    objPageVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
                }

            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCenterIndiaPage.SetBreadcrumList()");

            }

        }
    }
}