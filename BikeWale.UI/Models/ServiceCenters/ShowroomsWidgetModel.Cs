using Bikewale.Interfaces.Dealer;
using Bikewale.Models.Shared;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 30-Mar-2017
    /// Widegt to show Dealers showroom count and showroom widget
    /// </summary>
    public class ShowroomsWidgetModel
    {
        private readonly IDealerCacheRepository _dealerCache = null;
        private uint _topCount, _dealerServiceCenterCount, _cityId, _makeId;
        private string _cityName, _makeName, _makeMaskingName;
        public ShowroomsWidgetModel(uint topCount, uint dealerServiceCenterCount, uint cityId, string cityName, uint makeId, string makeName, string MakeMaskingName, IDealerCacheRepository dealerCache)
        {
            _makeId = makeId;
            _makeName = makeName;
            _makeMaskingName = MakeMaskingName;
            _cityId = cityId;
            _cityName = cityName;
            _dealerCache = dealerCache;
            if (topCount > 0)
                _topCount = topCount;
            if (dealerServiceCenterCount > 0)
                _dealerServiceCenterCount = dealerServiceCenterCount;
        }

        public ShowroomsVM GetData()
        {
            ShowroomsVM objShowrooms = new ShowroomsVM();
            if (_cityId > 0)
            {
                objShowrooms.dealers = _dealerCache.GetDealerByMakeCity(_cityId, _makeId, 0);
                if (objShowrooms.dealers != null && objShowrooms.dealers.Dealers != null && objShowrooms.dealers.Dealers.Count() > 0)
                {
                    objShowrooms.dealers.Dealers = objShowrooms.dealers.Dealers.Take((int)_dealerServiceCenterCount);
                }
            }
            else
            {
                objShowrooms.dealerServiceCenter = _dealerCache.GetPopularCityDealer(_makeId, _topCount);
            }
            objShowrooms.CityId = _cityId;
            objShowrooms.CityName = _cityName;
            objShowrooms.MakeName = _makeName;
            objShowrooms.MakeMaskingName = _makeMaskingName;
            if (objShowrooms.dealerServiceCenter != null)
            {
                bool IsDealerAvailable = (objShowrooms.dealerServiceCenter.TotalDealerCount > 0 || objShowrooms.dealerServiceCenter.TotalServiceCenterCount > 0);
                bool IsServiceCenterAvailable = (objShowrooms.dealerServiceCenter.TotalServiceCenterCount > 0);
                objShowrooms.IsDataAvailable = IsDealerAvailable || IsServiceCenterAvailable;
                objShowrooms.Title = string.Format("{0}{1}", IsDealerAvailable ? "Dealers " : string.Empty, IsServiceCenterAvailable ? "& Service Centers" : string.Empty);
            }
            else
            {
                objShowrooms.Title = "Dealers & Service Centers";
            }
            return objShowrooms;
        }
    }
}