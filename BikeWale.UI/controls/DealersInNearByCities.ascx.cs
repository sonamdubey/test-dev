using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Dealer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.controls
{
    /// <summary>
    /// Added By : Sajal Gupta on 19-12-2016;
    /// Desc : class to bind Dealers Count In Near By Cities 
    /// </summary>
    public class DealersInNearByCities : System.Web.UI.UserControl
    {
        public int FetchedRecordsCount;
        public uint MakeId { get; set; }
        public uint CityId { get; set; }
        public int TopCount { get; set; }
        public string MakeMaskingName { get; set; }
        public string CityName { get; set; }
        public string MakeName { get; set; }

        public IEnumerable<NearByCityDealerCountEntity> DealerCountCityList;

        protected void Page_Load(object sender, EventArgs e)
        {
            BindDealerCountNearByCity();
        }

        /// <summary>
        /// Added By : Sajal Gupta on 19-12-2016;
        /// Desc : Function to bind Dealers Count In Near By Cities 
        /// </summary>
        private void BindDealerCountNearByCity()
        {
            BindDealersCountInNearByCitiesControl objDealerCnt = new BindDealersCountInNearByCitiesControl();
            objDealerCnt.TopCount = TopCount;
            objDealerCnt.CityId = CityId;
            objDealerCnt.MakeId = MakeId;
            DealerCountCityList = objDealerCnt.BindDealersCountInNearByCities();
            FetchedRecordsCount = DealerCountCityList.Count();
        }
    }
}