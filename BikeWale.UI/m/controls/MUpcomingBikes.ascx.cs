using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.controls
{
    public partial class MUpcomingBikes : System.Web.UI.UserControl
    {

        public Repeater rptUpcomingBikes;

        public int sortBy { get; set; }
        public int pageSize { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UpcomingBikes();
        }

        private void UpcomingBikes()
        {
            BindUpcomingBikesControl objUpcoming = new BindUpcomingBikesControl();
            objUpcoming.sortBy = this.sortBy;
            objUpcoming.MakeId = this.MakeId;
            objUpcoming.ModelId = this.ModelId;
            objUpcoming.pageSize = this.pageSize;

            objUpcoming.BindUpcomingBikes(rptUpcomingBikes);
            this.FetchedRecordsCount = objUpcoming.FetchedRecordsCount;
        }

        protected string ShowEstimatedPrice(object estimatedPrice)
        {
            if (estimatedPrice != null && Convert.ToInt32(estimatedPrice) > 0)
            {
                return String.Format("<span class='fa fa-rupee'></span> <span class='font24'>{0}</span><span class='font16'> onwards</span>", Bikewale.Utility.Format.FormatPrice(Convert.ToString(estimatedPrice)));
            }
            else
            {
                return "<span class='font22'>Price Unavailable</span>";
            }
        }

        protected string ShowLaunchDate(object launchDate)
        {
            if (launchDate != null && !String.IsNullOrEmpty(Convert.ToString(launchDate)))
            {
                return String.Format("<div class='padding-top5 clear border-top1'><span class='font16 text-grey'>{0} <span class='font14 text-light-grey'> (Expected launch)</span></span></div>", Convert.ToString(launchDate));
            }
            else
            {
                return "<div class='padding-top5 clear border-top1 margin-top30'><span class='font16 text-grey'>Launch date unavailable</span></div>";
            }
        }

        public override void Dispose()
        {
            rptUpcomingBikes.DataSource = null;
            rptUpcomingBikes.Dispose();

            base.Dispose();
        }

    }
}