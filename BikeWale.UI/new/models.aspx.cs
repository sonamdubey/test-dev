using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.controls;
using Bikewale.Controls;
using Bikewale.DTO.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Memcache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
	public class Model : System.Web.UI.Page
	{
        protected UpcomingBikes_new ctrlUpcomingBikes;
        protected News_new ctrlNews;
        protected ExpertReviews ctrlExpertReviews;
        protected VideosControl ctrlVideos;
        protected MostPopularBikes_new ctrlMostPopularBikes;
        protected Repeater rptMostPopularBikes;

        protected bool isDescription = false;  
        protected Literal ltrDefaultCityName;
        protected int fetchedRecordsCount=0;
        protected string makeId = String.Empty;
        protected BikeMakeEntityBase _make = null;
        protected BikeDescriptionEntity _bikeDesc = null;
        protected Int64 _minModelPrice;
        protected Int64 _maxModelPrice;
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE .css class
        protected bool isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;

        private string makeMaskingName;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

		protected void Page_Load(object sender, EventArgs e)
		{            
            //Function to process and validate Query String  
            if (ProcessQueryString())
            {
                //to get complete make page
                GetMakePage(); 
                if (!Page.IsPostBack)
                {
                    // Modified By :Ashish Kamble on 5 Feb 2016
                    string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                    if (String.IsNullOrEmpty(originalUrl))
                        originalUrl = Request.ServerVariables["URL"];

                    DeviceDetection dd = new DeviceDetection(originalUrl);
                    dd.DetectDevice();

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

                    ctrlExpertReviews.MakeMaskingName = makeMaskingName;

                }
            } 
            
		}

        bool ProcessQueryString()
        {
            bool isSucess = true;

            if (!String.IsNullOrEmpty(Request.QueryString["make"]))
            {
                makeMaskingName = Request.QueryString["make"];
                makeId = MakeMapping.GetMakeId(makeMaskingName);
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
            BindMakePage objMake = new BindMakePage();
            objMake.totalCount = 6;
            objMake.makeId = Convert.ToInt32(makeId);
            objMake.BindMostPopularBikes(rptMostPopularBikes);
            fetchedRecordsCount = objMake.FetchedRecordsCount;
            _make = objMake.Make;
            _bikeDesc = objMake.BikeDesc;

            //To find min and max modelPrice
            _minModelPrice = objMake.MinPrice;
            _maxModelPrice = objMake.MaxPrice;

            if (_bikeDesc != null && _bikeDesc.FullDescription != null && _bikeDesc.SmallDescription != null && _bikeDesc.FullDescription.Trim().Length > 0 && _bikeDesc.SmallDescription.Trim().Length > 0)
            {
                isDescription = true;
            }
        }

        protected string ShowEstimatedPrice(object estimatedPrice)
        {
            if (estimatedPrice != null && Convert.ToInt32(estimatedPrice) > 0)
            {
                return String.Format("<span class='fa fa-rupee'></span> <span class='font22'>{0}</span><span class='font16'> onwards</span>", Bikewale.Utility.Format.FormatPrice(Convert.ToString(estimatedPrice)));
            }
            else
            {
                return "<span class='font22'>Price Unavailable</span>";
            }
        }
       
	}
}