using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 12 May 2016
    /// Desc       : Created control to show similar Bike links below compare bikes 
    /// </summary>
    public class SimilarCompareBikes : System.Web.UI.UserControl
    {
        public Repeater rptSimilarBikes;
        public string versionsList { get; set; }
        private uint _topCount = 0;
        public uint fetchedCount { get; set; }
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
            BindSimilarCompareBikes();
        }
        /// <summary>
        /// Created by : Sangram Nandkhile on 12 May 2016
        /// Desc       : To bind similar bikes
        /// </summary>
        private void BindSimilarCompareBikes()
        {
            BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();
            fetchedCount = objAlt.BindAlternativeBikes(rptSimilarBikes, versionsList, TopCount);
        }

        public override void Dispose()
        {
            rptSimilarBikes.DataSource = null;
            rptSimilarBikes.Dispose();
            base.Dispose();
        }
    }
}