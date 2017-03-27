
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Used;
using Bikewale.Memcache;
using System;
using System.Linq;
namespace Bikewale.Models.DealerShowroom
{
    public class DealerShowroomCityPage
    {
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;

        public MakeMaskingResponse objResponse;
        public uint cityId, makeId, topCount;
        public StatusCodes status;
        public BikeMakeEntityBase objMake;
        public CityEntityBase CityDetails;
        public DealerShowroomCityPage(IDealerCacheRepository objDealerCache, IUsedBikeDetailsCacheRepository objUsedCache, IBikeMakesCacheRepository<int> bikeMakesCache, string makeMaskingName, string cityMaskingName)
        {
            _objDealerCache = objDealerCache;
            _bikeMakesCache = bikeMakesCache;
            _objUsedCache = objUsedCache;
            ProcessQuery(makeMaskingName, cityMaskingName);
        }
        public DealerShowroomCityPageVM GetData()
        {
            DealerShowroomCityPageVM objDealerVM = new DealerShowroomCityPageVM();

            objMake = new MakeHelper().GetMakeNameByMakeId(makeId);
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
            return objDealerVM;
        }
        private DealersEntity BindDataDealers()
        {
            DealersEntity objDealerList = null;
            objDealerList = _objDealerCache.GetDealerByMakeCity(cityId, makeId);
            return objDealerList;
        }
        private NearByCityDealer BindOtherDealerInCitiesWidget()
        {
            NearByCityDealer objDealer = new NearByCityDealer();
            objDealer.objDealerInNearCityList = _objDealerCache.FetchNearByCityDealersCount(makeId, cityId);
            if (objDealer != null && objDealer.objDealerInNearCityList != null && objDealer.objDealerInNearCityList.Count() > 0)
            {
                objDealer.objDealerInNearCityList = objDealer.objDealerInNearCityList.Take((int)topCount);
            }
            objDealer.Make = objMake;

            return objDealer;
        }
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
        private UsedBikeModels BindUsedBikeByModel()
        {
            UsedBikeModels UsedBikeModel = new UsedBikeModels();
            try
            {
                if (makeId > 0)
                {
                    UsedBikeModel.UsedBikeModelList = _objUsedCache.GetPopularUsedModelsByMake(makeId, topCount);
                }
                else
                {
                    UsedBikeModel.UsedBikeModelList = _objUsedCache.GetUsedBike(topCount);
                }
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindUsedBikeByModel()");
            }
            if (cityId > 0)
                UsedBikeModel.CityDetails = CityDetails;
            return UsedBikeModel;

        }
    }
}