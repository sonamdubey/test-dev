using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 12 May 2016
    /// Desc       : Created control to show similar Bike links below compare bikes
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Removed repeater logic and bind data using list object
    /// </summary>
    public class SimilarCompareBikes : System.Web.UI.UserControl
    {
        public string versionsList { get; set; }
        protected ICollection<SimilarCompareBikeEntity> objSimilarBikes = null;
        private ushort _topCount = 0;
        public uint fetchedCount { get; set; }
        public int? cityid { get; set; }
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
        /// </summary>
        private void BindSimilarCompareBikes()
        {
            BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();
            objAlt.cityid = cityid.HasValue ? cityid.Value : Convert.ToInt16(Bikewale.Utility.BWConfiguration.Instance.DefaultCity);
            objSimilarBikes = objAlt.BindPopularCompareBikes(versionsList, TopCount);
            fetchedCount = objAlt.FetchedRecordsCount;
        }

    }
}