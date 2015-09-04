using Bikewale.Entities.BikeData;
using Bikewale.Mobile.controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.New
{
	public class Default : System.Web.UI.Page
	{
       // protected News_new ctrlNews;
        protected MUpcomingBikes mctrlUpcomingBikes;
        protected MNewLaunchedBikes mctrlNewLaunchedBikes;
        protected MMostPopularBikes mctrlMostPopularBikes;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //To get News List
           // mctrlNews.TotalRecords = 3;

            //to get Most Popular Bikes
           mctrlMostPopularBikes.totalCount = 6;

            //To get Upcoming Bike List Details 
           mctrlNewLaunchedBikes.pageSize = 6;
           mctrlNewLaunchedBikes.curPageNo = null;

            //To get Upcoming Bike List Details 
            mctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            mctrlUpcomingBikes.pageSize = 6;
        }
	}
}