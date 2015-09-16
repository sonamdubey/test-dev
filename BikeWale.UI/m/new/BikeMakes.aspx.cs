using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DTO.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Memcache;
using Bikewale.Mobile.controls;
using Bikewale.Mobile.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile
{
    public class BikeMakes : System.Web.UI.Page
    {
        protected MUpcomingBikes ctrlUpcomingBikes;
        protected NewsWidget ctrlNews;
        protected ExpertReviewsWidget ctrlExpertReviews;
        protected VideosWidget ctrlVideos;
        protected MMostPopularBikes ctrlMostPopularBikes;
        protected Repeater rptMostPopularBikes;

        protected bool isDescription = false;
        protected Literal ltrDefaultCityName;
        protected int fetchedRecordsCount = 0;
        protected string makeId = String.Empty;
        protected MakeBase _make = null;
        protected BikeDescription _bikeDesc = null;
        protected int uCount = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Function to process and validate Query String  
            if (ProcessQueryString())
            {
                // ltrDefaultCityName.Text = Bikewale.Common.Configuration.GetDefaultCityName;
                _make = new MakeBase();
                _bikeDesc = new BikeDescription();

                //to get complete make page
                GetMakePage();
                if (!Page.IsPostBack)
                {
                    //To get Upcoming Bike List Details 
                    ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                    ctrlUpcomingBikes.pageSize = 6;
                    ctrlUpcomingBikes.MakeId = Convert.ToInt32(makeId);

                    ////news,videos,revews
                    ctrlNews.TotalRecords = 3;
                    ctrlNews.MakeId = Convert.ToInt32(makeId);
                    ctrlExpertReviews.TotalRecords = 3;
                    ctrlExpertReviews.MakeId = Convert.ToInt32(makeId);
                    ctrlVideos.TotalRecords = 3;
                    ctrlVideos.MakeId = Convert.ToInt32(makeId);

                }
            }
        }

        bool ProcessQueryString()
        {
            bool isSucess = true;

            if (!String.IsNullOrEmpty(Request.QueryString["make"]))
            {
                makeId = MakeMapping.GetMakeId(Request.QueryString["make"]);
                //verify the id as passed in the url
                if (CommonOpn.CheckId(makeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSucess = false;
                }
            }
            else
            {
                //invalid make id, hence redirect to the new default page
                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSucess = false;
            }

            return isSucess;
        }

        private void GetMakePage()
        {
            BindMakePage.totalCount = 6;
            BindMakePage.makeId = Convert.ToInt32(makeId);
            BindMakePage.BindMostPopularBikes(rptMostPopularBikes);
            fetchedRecordsCount = BindMakePage.FetchedRecordsCount;
            _make = BindMakePage.Make;
            _bikeDesc = BindMakePage.BikeDesc;

            if (_bikeDesc != null)
            {
                isDescription = true;
            }
        }

        protected string ShowEstimatedPrice(object estimatedPrice)
        {
            string price = String.Empty;
            if (estimatedPrice != null)
            {
                price = Bikewale.Utility.Format.FormatPrice(estimatedPrice.ToString());
                if (price == "N/A")
                {
                    price = "Price unavailable";
                }
                else
                {
                    price += " <span class='font16'> Onwards</span>";
                }
            }
            return price;
        }

    }
}