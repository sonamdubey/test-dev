using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 12 May 2016
    /// Desc       : Created control to show similar Bike links below compare bikes
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Removed repeater logic and dind data using list object
    /// Modified By : Sushil Kumar on 2nd Feb 2017
    /// Description : Removed old logic related to repeaters
    /// </summary>

    public class SimilarCompareBikes : System.Web.UI.UserControl
    {
        public string versionsList { get; set; }
        private ushort _topCount = 0;
        public uint fetchedCount { get; set; }
        public Int64 SponsoredVersionId { get; set; }
        public String FeaturedBikeLink { get; set; }
        public int CityId { get; set; }
        public IEnumerable<SimilarCompareBikeEntity> objSimilarBikes = null;

        public ushort TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindSimilarCompareBikes();
        }
        /// <summary>
        /// Created by : Sangram Nandkhile on 12 May 2016
        /// Desc       : To bind similar bikes
        /// Modified By : Sushil Kumar on 2nd Dec 2016
        /// Description : MOved value into object of similar bikes
        /// Modified By : Sushil Kumar on 2nd Feb 2017
        /// Description : Removed old logic related to repeaters
        /// </summary>
        private void BindSimilarCompareBikes()
        {
            try
            {
                BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();
                objAlt.cityid = CityId;
                objSimilarBikes = objAlt.BindPopularCompareBikes(versionsList, TopCount);
                fetchedCount = objAlt.FetchedRecordsCount;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Mobile.Controls.SimilarCompareBikes.BindSimilarCompareBikes" + (!string.IsNullOrEmpty(versionsList) ? versionsList : ""));
            }
        }
    }
}