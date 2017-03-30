
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by :- Subodh Jain 30 March 2017
    ///Summary :- Dealer card widget
    /// </summary>
    public class DealerCardWidget
    {
        private readonly IDealerCacheRepository _objDealerCache = null;

        protected uint cityId, makeId;
        public uint DealerId { get; set; }
        public uint TopCount { get; set; }
        public uint ModelId { get; set; }
        public DealerCardWidget(IDealerCacheRepository objDealerCache, uint cityId, uint makeId)
        {
            _objDealerCache = objDealerCache;
            this.cityId = cityId;
            this.makeId = makeId;
        }
        /// <summary>
        /// Created By :- Subodh Jain 30 March 2017
        /// Summary :- To fetch data for Other dealer widget
        /// </summary>
        /// <returns></returns>
        public DealerCardVM GetData()
        {
            DealerCardVM objDealerList = new DealerCardVM();
            try
            {
                DealersEntity objDealer = null;
                objDealer = _objDealerCache.GetDealerByMakeCity(cityId, makeId, ModelId);

                if (objDealer != null)
                {
                    objDealer.Dealers = objDealer.Dealers.Where(m => m.DealerId != DealerId);
                    objDealer.Dealers = objDealer.Dealers.Take((int)TopCount);
                    objDealerList.Dealers = objDealer.Dealers;
                    objDealerList.TotalCount = objDealer.TotalCount;
                    objDealerList.MakeName = objDealer.MakeName;
                    objDealerList.CityName = objDealer.CityName;
                    objDealerList.CityMaskingName = objDealer.CityMaskingName;
                    objDealerList.MakeMaskingName = objDealer.MakeMaskingName;
                }

            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomDealerDetail.BindOtherDealerWidget()");
            }
            return objDealerList;
        }
    }
}