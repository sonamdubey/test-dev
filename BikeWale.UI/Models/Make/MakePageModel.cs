
using Bikewale.Entities;
using Bikewale.Interfaces.Dealer;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Model for make page
    /// </summary>
    public class MakePageModel
    {
        public string RedirectUrl;
        private uint _topCount, _makeId;
        public StatusCodes Status;
        private IDealerCacheRepository _dealerServiceCenters;

        public MakePageModel(uint makeId, uint topCount, IDealerCacheRepository _dealerServiceCenters)
        {
            this._makeId = makeId;
            this._dealerServiceCenters = _dealerServiceCenters;
            _topCount = topCount > 0 ? topCount : 9;
        }

        /// <summary>
        /// Gets the data for homepage
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// </returns>
        public MakePageVM GetData()
        {
            MakePageVM objData = new MakePageVM();
            var baseDetails = _dealerServiceCenters.GetPopularCityDealer(this._makeId, this._topCount);
            if (baseDetails != null)
            {
                objData.DealerServiceCenters = new DealerServiceCenterWidgetVM();
                objData.DealerServiceCenters.DealerDetails = baseDetails.DealerDetails;
                objData.DealerServiceCenters.TotalDealerCount = baseDetails.TotalDealerCount;
                objData.DealerServiceCenters.TotalServiceCenterCount = baseDetails.TotalServiceCenterCount;
            }
            return objData;
        }
    }
}