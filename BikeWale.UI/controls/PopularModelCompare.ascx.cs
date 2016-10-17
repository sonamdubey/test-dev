using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created by:-Subodh Jain on 12 sep 2016
    /// Description :- Compare popular similar bikes on model page
    /// </summary>
    public class PopularModelCompare : UserControl
    {
        public Repeater rptPopularCompareBikes;
        public string versionId { get; set; }
        public string ModelName;
        private uint _topCount = 0;
        public uint fetchedCount { get; set; }
        public uint? cityid { get; set; }
        public uint TopCount
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
            BindPopularCompareBikes();
        }
        /// <summary>
        /// Created by : Subodh Jain 12 sep 2016
        /// Desc       : To bind popular compare  bikes for model page
        /// </summary>
        private void BindPopularCompareBikes()
        {
            BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();
            objAlt.cityid = cityid.HasValue ? cityid.Value : 1;
            fetchedCount = objAlt.BindPopularCompareBikes(rptPopularCompareBikes, versionId, TopCount);
        }

        public override void Dispose()
        {
            rptPopularCompareBikes.DataSource = null;
            rptPopularCompareBikes.Dispose();
            base.Dispose();
        }
    }
}
