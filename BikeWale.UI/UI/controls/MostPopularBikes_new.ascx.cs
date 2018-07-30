using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Modified by Subodh Jain on 3 oct 2016
    /// Added popular bike widget
    /// </summary>
    public class MostPopularBikes_new : System.Web.UI.UserControl
    {
        public Repeater rptMostPopularBikes, rptPopoularBikeMake;
        public int? totalCount { get; set; }
        public int? makeId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string PageId { get; set; }
        public int PQSourceId { get; set; }
        public int? cityId { get; set; }
        public string cityname = string.Empty;
        public string cityMaskingName = string.Empty;
        public string makeName = string.Empty;
        public bool mostPopular = false, mostPopularByMake = false;
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
            objPop.cityId = this.cityId;
            if (makeId.HasValue && makeId > 0)
            {
                objPop.BindMostPopularBikesMakeCity(rptPopoularBikeMake);
                mostPopularByMake = true;
            }

            else
            {
                objPop.BindMostPopularBikes(rptMostPopularBikes);
                mostPopular = true;
            }
            this.FetchedRecordsCount = objPop.FetchedRecordsCount;
        }

        protected string ShowEstimatedPrice(object estimatedPrice)
        {
            if (estimatedPrice != null && Convert.ToInt32(estimatedPrice) > 0)
            {
                return String.Format("<span class='bwsprite inr-lg'></span> <span class='font18'>{0}</span><span class='font14'> onwards</span>", Bikewale.Utility.Format.FormatPrice(Convert.ToString(estimatedPrice)));
            }
            else
            {
                return "<span class='font14'>Price Unavailable</span>";
            }
        }

        public override void Dispose()
        {
            rptMostPopularBikes.DataSource = null;
            rptMostPopularBikes.Dispose();

            base.Dispose();
        }
    }
}