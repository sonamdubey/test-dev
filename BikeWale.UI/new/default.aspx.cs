using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.controls;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Common;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.New
{
    public class Default : System.Web.UI.Page
    {
        protected News_new ctrlNews;
        protected UpcomingBikes_new ctrlUpcomingBikes;
        protected NewLaunchedBikes_new ctrlNewLaunchedBikes;
        protected MostPopularBikes_new ctrlMostPopularBikes;
        protected ExpertReviews ctrlExpertReviews;
        protected VideosControl ctrlVideos;
        protected ComparisonMin ctrlCompareBikes;
        protected NewBikesOnRoadPrice NBOnRoadPrice;
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE .css class
        protected bool isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;

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
            ctrlMostPopularBikes.totalCount = 6;
            ctrlMostPopularBikes.PQSourceId = (int)PQSourceEnum.Desktop_New_MostPopular;

            //To get Upcoming Bike List Details 
            ctrlNewLaunchedBikes.pageSize = 6;
            ctrlNewLaunchedBikes.PQSourceId = (int)PQSourceEnum.Desktop_New_NewLaunches;

            //To get Upcoming Bike List Details 
            ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            ctrlUpcomingBikes.pageSize = 6;

            ctrlNews.TotalRecords = 3;
            ctrlExpertReviews.TotalRecords = 3;
            ctrlVideos.TotalRecords = 3;
            ctrlCompareBikes.TotalRecords = 4;

            NBOnRoadPrice.PQSourceId = (int)PQSourceEnum.Desktop_New_PQ_Widget;
        }
    }
}