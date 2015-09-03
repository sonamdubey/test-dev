using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.controls;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;

namespace Bikewale.New
{
    public class Default : System.Web.UI.Page
    {
        protected News_new ctrlNews;
        protected UpcomingBikes_new ctrlUpcomingBikes;
        protected NewLaunchedBikes_new ctrlNewLaunchedBikes;
        protected MostPopularBikes_new ctrlMostPopularBikes;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //To get News List
            //ctrlNews.TotalRecords = 3;

            //to get Most Popular Bikes
            ctrlMostPopularBikes.totalCount = 6;

            //To get Upcoming Bike List Details 
            ctrlNewLaunchedBikes.pageSize = 6;
            //ctrlNewLaunchedBikes.curPageNo = null;

            //To get Upcoming Bike List Details 
            ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            ctrlUpcomingBikes.pageSize = 6;
        }
    }
}