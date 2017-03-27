
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Dealer;
using Bikewale.Utility;
namespace Bikewale.Models
{
    public class DealerShowroomDealerDetail
    {
        private readonly IDealerCacheRepository _objDealerCache = null;

        public uint cityId, makeId;
        public DealerShowroomDealerDetail(IDealerCacheRepository objDealerCache)
        {
            _objDealerCache = objDealerCache;
        }
        public DealerShowroomDealerDetailsVM GetData()
        {
            DealerShowroomDealerDetailsVM objDealerDetails = new DealerShowroomDealerDetailsVM();
            cityId = GlobalCityArea.GetGlobalCityArea().CityId;
            objDealerDetails.DealersList = BindOtherDealerWidget();
            return objDealerDetails;
        }
        public DealersEntity BindOtherDealerWidget()
        {
            DealersEntity objDealerList = null;
            objDealerList = _objDealerCache.GetDealerByMakeCity(cityId, makeId);
            return objDealerList;
        }
    }
}