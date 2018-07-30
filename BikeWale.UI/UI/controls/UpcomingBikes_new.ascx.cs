using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public partial class UpcomingBikes_new : System.Web.UI.UserControl
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
                return String.Format("<span class='bwsprite inr-xl'></span> <span class='font22'>{0}</span><span class='font16'> onwards</span>", Bikewale.Utility.Format.FormatPrice(Convert.ToString(estimatedPrice)));
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
                return String.Format("<div class='font12 text-light-grey margin-bottom10'>Expected Price</div> <p class='font16 border-solid-top margin-top10 padding-top10'>{0}<span class='font14 text-light-grey'> (Expected launch)</span></p>", Convert.ToString(launchDate));
            }
            else
            {
                return "<p class='font16 border-solid-top margin-top20 padding-top10'><span>Launch date unavailable</span></p>";
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