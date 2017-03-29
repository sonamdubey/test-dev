using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Models.BikeCare;
using System;

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
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly ICMSCacheContent _articles = null;

        public StatusCodes status;

        private uint _makeId;
        public MakeMaskingResponse objResponse;

        private uint _usedBikesTopCount = 9;
        private uint _bikeCareRecordsCount = 3;

        public ServiceCenterIndiaPage(ICMSCacheContent articles, IUsedBikeDetailsCacheRepository objUsedCache, IBikeMakesCacheRepository<int> bikeMakesCache, IServiceCenterCacheRepository objCache, string makemaskingName)
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
                objVM.Make = new MakeHelper().GetMakeNameByMakeId(_makeId);
                objVM.ServiceCenterBrandsList = new ServiceCentersByBrand(_objCache, _makeId).GetData();
                objVM.UsedBikesByMakeList = BindUsedBikeByModel(_usedBikesTopCount);
                objVM.BikeCareWidgetVM = new RecentBikeCare(_articles).GetData(_bikeCareRecordsCount, 0, 0);
                BindPageMetas(objVM);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterIndiaPage.GetData()");
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
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterIndiaPage.BindPageMetas()");
            }
        }

        private void ProcessQuery(string makeMaskingName)
        {
            try
            {
                objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200)
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
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterIndiaPage.ProcessQuery()");
            }
        }

        private UsedBikeModelsVM BindUsedBikeByModel(uint topCount)
        {
            UsedBikeModelsVM UsedBikeModel = new UsedBikeModelsVM();
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

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterIndiaPage.BindUsedBikeByModel()");
            }
            return UsedBikeModel;
        }
    }
}