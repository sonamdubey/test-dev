using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.m.controls
{
    /// <summary>
    /// Created by : Sajal Gupta on 12/09/2016
    /// Desc       : View Model to bind and pass repeater data to control
    /// </summary>
    public class PopularModelComparison : System.Web.UI.UserControl
    {

        public Repeater rptPopularBikesComparison;
        public uint fetchedCount { get; set; }
        public IEnumerable<SimilarCompareBikeEntity> objSimilarBikes = null;
        public uint versionId { get; set; }
        public string versionName { get; set; }
        public int? cityid { get; set; }
        public uint TopCount { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindPopularCompareBikes();
        }

        /// <summary>
        /// Created by : Sajal Gupta on 12/09/2016
        /// Desc       : To bind similar bikes
        /// </summary>
        private void BindPopularCompareBikes()
        {
            BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();
            objAlt.cityid = cityid.HasValue && cityid > 0 ? cityid.Value : Convert.ToInt16(Bikewale.Utility.BWConfiguration.Instance.DefaultCity);
            fetchedCount = objAlt.BindPopularCompareBikes(rptPopularBikesComparison, versionId.ToString(), TopCount);
        }

        public override void Dispose()
        {
            rptPopularBikesComparison.DataSource = null;
            rptPopularBikesComparison.Dispose();
            base.Dispose();
        }


    }
}