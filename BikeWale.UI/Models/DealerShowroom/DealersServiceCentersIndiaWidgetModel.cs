using Bikewale.Interfaces.Dealer;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Mar-2017
    /// Model for dealers and service centers
    /// </summary>
    public class DealersServiceCentersIndiaWidgetModel
    {
        private readonly IDealerCacheRepository _dealerCache = null;
        private uint _topCount, _makeId;
        private string _makeName, _makeMaskingName;
        public DealersServiceCentersIndiaWidgetModel(uint makeId, string makeName, string MakeMaskingName, IDealerCacheRepository dealerCache)
        {
            _makeId = makeId;
            _makeName = makeName;
            _makeMaskingName = MakeMaskingName;
            _dealerCache = dealerCache;
            _topCount = 10;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 31-Mar-2017 
        /// </returns>
        public DealersServiceCentersIndiaWidgetVM GetData()
        {
            DealersServiceCentersIndiaWidgetVM objData = new DealersServiceCentersIndiaWidgetVM();
            objData.DealerServiceCenters = _dealerCache.GetPopularCityDealer(_makeId, _topCount);
            return objData;
        }
    }
}