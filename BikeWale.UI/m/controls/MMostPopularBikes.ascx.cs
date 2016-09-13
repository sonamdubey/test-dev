using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    public class MMostPopularBikes : System.Web.UI.UserControl
    {
        public Repeater rptMostPopularBikes;

        public int? totalCount { get; set; }
        public int? makeId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string PageId { get; set; }
        public int PQSourceId { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MostPopularBikes();
        }

        private void MostPopularBikes()
        {
            BindMostPopularBikesControl objPop = new BindMostPopularBikesControl();
            objPop.totalCount = this.totalCount;
            objPop.makeId = this.makeId;
            objPop.BindMostPopularBikes(rptMostPopularBikes);
            this.FetchedRecordsCount = objPop.FetchedRecordsCount;
        }

        protected string ShowEstimatedPrice(object estimatedPrice)
        {
            if (estimatedPrice != null && Convert.ToInt32(estimatedPrice) > 0)
            {
                return String.Format("<span class='bwmsprite inr-sm-icon'></span> <span class='text-bold font18'>{0}</span><span class='font16'> onwards</span>", Bikewale.Utility.Format.FormatPrice(Convert.ToString(estimatedPrice)));
            }
            else
            {
                return "<span class='font18'>Price Unavailable</span>";
            }
        }

    }
}