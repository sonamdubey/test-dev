using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    public class MUpcomingBikes : System.Web.UI.UserControl
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
                return String.Format("<span class='bwmsprite inr-xsm-icon'></span> <span class='text-bold font16'>{0}</span><span class='font14 text-bold'> onwards</span>", Bikewale.Utility.Format.FormatPrice(Convert.ToString(estimatedPrice)));
            }
            else
            {
                return "<span class='font14'>Price Unavailable</span>";
            }
        }

        protected string ShowLaunchDate(object launchDate)
        {
            if (launchDate != null && !String.IsNullOrEmpty(Convert.ToString(launchDate)))
            {
                return String.Format("<p class='font11 text-light-grey'>Expected launch</p><p class='font16 text-bold text-default'>{0}</p>", Convert.ToString(launchDate));
            }
            else
            {
                return "<p class='font14 text-default'>Launch date unavailable</p>";
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