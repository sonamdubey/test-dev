using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Controls;
using Bikewale.controls;
using Bikewale.Common;
using Bikewale.Entities.PriceQuote;

namespace Bikewale
{
    public class Default : System.Web.UI.Page
    {
        protected News_new ctrlNews;
        protected ExpertReviews ctrlExpertReviews;
        protected VideosControl ctrlVideos;
        protected ComparisonMin ctrlCompareBikes;
        protected PopularUsedBikes ctrlPopularUsedBikes;
        protected OnRoadPricequote ctrlOnRoadPriceQuote;
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
            DeviceDetection dd = new DeviceDetection();
            dd.DetectDevice();
            
            ctrlNews.TotalRecords = 3;
            ctrlExpertReviews.TotalRecords = 3;
            ctrlVideos.TotalRecords = 3;
            ctrlCompareBikes.TotalRecords = 4;
            ctrlPopularUsedBikes.TotalRecords = 6;
            ctrlOnRoadPriceQuote.PQSourceId = (int)PQSourceEnum.Desktop_HP_PQ_Widget;
        }
    }
}