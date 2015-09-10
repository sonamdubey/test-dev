using Bikewale.Entities.BikeData;
using Bikewale.Mobile.controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Mobile.Controls;

namespace Bikewale.Mobile.New
{
	public class Default : System.Web.UI.Page
	{
        protected MUpcomingBikes mctrlUpcomingBikes;
        protected MNewLaunchedBikes mctrlNewLaunchedBikes;
        protected MMostPopularBikes mctrlMostPopularBikes;
        protected NewsWidget ctrlNews;
        protected ExpertReviewsWidget ctrlExpertReviews;
        protected VideosWidget ctrlVideos;
        protected CompareBikesMin ctrlCompareBikes;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            //to get Most Popular Bikes
           mctrlMostPopularBikes.totalCount = 6;

            //To get Upcoming Bike List Details 
           mctrlNewLaunchedBikes.pageSize = 6;
           mctrlNewLaunchedBikes.curPageNo = null;

            //To get Upcoming Bike List Details 
            mctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            mctrlUpcomingBikes.pageSize = 6;

            ctrlNews.TotalRecords = 3;
            ctrlExpertReviews.TotalRecords = 3;
            ctrlVideos.TotalRecords = 3;
            ctrlCompareBikes.TotalRecords = 1;
        }
	}
}