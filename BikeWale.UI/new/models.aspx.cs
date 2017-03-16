using Bikewale.BindViewModels.Controls;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Modified By : Lucky Rathore on 05 May 2016
    /// Description : Removed postback and inherit PageBase.
    /// </summary>
    public class Model : PageBase
    {
        protected UpcomingBikes_new ctrlUpcomingBikes;
        protected usedBikeModel ctrlusedBikeModel;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected News_Widget ctrlNews;
        //protected ExpertReviews ctrlExpertReviews;
        protected NewExpertReviews ctrlExpertReviews;
        protected NewVideosControl ctrlVideos;
        protected Repeater rptMostPopularBikes, rptDiscontinued, rptTop;
        protected DealerCard ctrlDealerCard;
        protected bool isDescription = false;
        protected Literal ltrDefaultCityName;
        protected int fetchedRecordsCount = 0;
        protected uint makeId;
        protected BikeMakeEntityBase _make = null;
        protected BikeDescriptionEntity _bikeDesc = null;
        protected Int64 _minModelPrice = 0;
        protected Int64 _maxModelPrice = 0;
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE .css class
        protected bool isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;

        private string makeMaskingName;
        private GlobalCityAreaEntity currentCityArea;
        protected uint cityId = 0;
        protected string cityName = string.Empty, cityMaskingName = string.Empty;
        private CityEntityBase cityDetails = null;
        protected LeadCaptureControl ctrlLeadCapture;
        protected String clientIP = CommonOpn.GetClientIP();

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified By : Lucky Rathore on 01 March 2016.
        /// Description : set make masking name for video controller
        /// Modified By : Lucky Rathore on 05 May 2016
        /// Description : Removed postback.
        /// Modified By : Vivek Gupta on 22 june 2016
        /// Desc: ctrlRecentUsedBikes (values assigned)
        /// Modified By :-Subodh Jain on 16 Dec 2016
        /// Summary :- Added heading to dealer widget
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :  Added dealer card and service center card
        /// Modified by :   Sumit Kate on 02 Jan 2017
        /// Description :   Set Make name and masking name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.

            Trace.Warn("page load starts");
            Form.Action = Request.RawUrl;
            //Function to process and validate Query String  
            if (ProcessQueryString())
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                cityId = currentCityArea.CityId;
                cityName = currentCityArea.City;
                //to get complete make page
                GetMakePage();
                // Modified By :Ashish Kamble on 5 Feb 2016
                string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                if (String.IsNullOrEmpty(originalUrl))
                    originalUrl = Request.ServerVariables["URL"];
                DeviceDetection dd = new DeviceDetection(originalUrl);
                dd.DetectDevice();
                //To get Upcoming Bike List Details 

                BindUserControls();
                BindDiscountinuedBikes();

            }

        }
        /// <summary>
        /// Summary: To bind user controls
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Added used bike widget
        /// </summary>
        private void BindUserControls()
        {
            int _makeId = Convert.ToInt16(makeId);
            if (cityId > 0)
            {
                cityDetails = new CityHelper().GetCityById(cityId);
                if (cityDetails != null)
                    cityMaskingName = cityDetails.CityMaskingName;
            }
            ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            ctrlUpcomingBikes.pageSize = 6;
            ctrlUpcomingBikes.MakeId = _makeId;
            ////news,videos,revews
            ctrlNews.TotalRecords = 3;
            ctrlNews.MakeId = _makeId;
            ctrlNews.WidgetTitle = _make.MakeName;
            ctrlNews.MakeMaskingName = _make.MaskingName;
            ctrlNews.MakeName = _make.MakeName;

            ctrlExpertReviews.TotalRecords = 2;
            ctrlExpertReviews.MakeId = _makeId;
            ctrlExpertReviews.MakeName = _make.MakeName;

            ctrlVideos.TotalRecords = 2;
            ctrlVideos.MakeId = _makeId;
            ctrlVideos.MakeMaskingName = makeMaskingName;
            ctrlVideos.WidgetTitle = _make.MakeName;
            ctrlVideos.MakeName = _make.MakeName;

            ctrlExpertReviews.MakeMaskingName = makeMaskingName;

            ctrlDealerCard.MakeId = makeId;
            ctrlDealerCard.makeName = _make.MakeName;
            ctrlDealerCard.makeMaskingName = _make.MaskingName;
            ctrlDealerCard.CityId = cityId;
            ctrlDealerCard.TopCount = Convert.ToUInt16(cityId > 0 ? 3 : 6);
            ctrlDealerCard.pageName = "Make_Page";
            ctrlDealerCard.widgetHeading = string.Format("{0} showrooms in {1}", _make.MakeName, cityName);

            ctrlLeadCapture.CityId = cityId;
            ctrlLeadCapture.AreaId = 0;

            ctrlServiceCenterCard.MakeId = Convert.ToUInt32(makeId); ;
            ctrlServiceCenterCard.CityId = cityId;
            ctrlServiceCenterCard.makeName = _make.MakeName;
            ctrlServiceCenterCard.cityName = cityName;
            ctrlServiceCenterCard.makeMaskingName = _make.MaskingName;
            ctrlServiceCenterCard.cityMaskingName = cityMaskingName;
            ctrlServiceCenterCard.TopCount = 3;
            ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", _make.MakeName, cityName);

            if (ctrlusedBikeModel != null)
            {

                ctrlusedBikeModel.MakeId = makeId;
                if (cityId > 0)
                    ctrlusedBikeModel.CityId = cityId;
                ctrlusedBikeModel.WidgetTitle = string.Format("Second-hand Honda Bikes in {0}", cityId > 0 ? cityName : "India");
                ctrlusedBikeModel.header = string.Format("Used {0} bikes in {1}", _make.MakeName, cityId > 0 ? cityName : "India");
                ctrlusedBikeModel.WidgetHref = string.Format("/used/{0}-bikes-in-{1}/", _make.MaskingName, cityId > 0 ? cityMaskingName : "india");
                ctrlusedBikeModel.TopCount = 9;
            }

        }


        /// <summary>
        /// Created by: Sangram Nandkhile on 17 Jun 2016
        /// Summary: Bind discontinued bikes on make page
        /// </summary>
        private void BindDiscountinuedBikes()
        {
            IEnumerable<BikeVersionEntity> bikes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    bikes = objCache.GetDiscontinuedBikeModelsByMake(Convert.ToUInt16(makeId));
                    if (bikes != null && bikes.Count() > 0)
                    {
                        foreach (var bike in bikes)
                        {
                            bike.Href = string.Format("/{0}-bikes/{1}/", _make.MaskingName, bike.ModelMasking);
                            bike.BikeName = string.Format("{0} {1}", _make.MakeName, bike.ModelName);
                        }

                        rptDiscontinued.DataSource = bikes;
                        rptDiscontinued.DataBind();
                        fetchedRecordsCount = bikes.Count();

                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindDiscountinuedBike");
                objErr.SendMail();
            }

        }

        bool ProcessQueryString()
        {
            bool isSucess = true;

            if (!String.IsNullOrEmpty(Request.QueryString["make"]))
            {
                makeMaskingName = Request.QueryString["make"];

                if (!String.IsNullOrEmpty(makeMaskingName))
                {
                    MakeMaskingResponse objResponse = null;

                    try
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                                  .RegisterType<ICacheManager, MemcacheManager>()
                                  .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                                 ;
                            var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();

                            objResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                        }
                    }
                    catch (Exception ex)
                    {
                        isSucess = false;
                        Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");

                        Response.Redirect("/new-bikes-in-india/", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    finally
                    {
                        if (objResponse != null)
                        {
                            if (objResponse.StatusCode == 200)
                            {
                                makeId = objResponse.MakeId;
                            }
                            else if (objResponse.StatusCode == 301)
                            {
                                CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                            }
                            else
                            {
                                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                                this.Page.Visible = false;
                                isSucess = false;
                            }
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                            isSucess = false;
                        }
                    }
                }
                else
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
                Response.Redirect("/new-bikes-in-india/", false);
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
            objMake.TopCount = 6;
            objMake.makeId = Convert.ToInt32(makeId);
            objMake.BindMostPopularBikes(rptMostPopularBikes, rptTop);
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
                return String.Format("<span class='bwsprite inr-lg'></span> <span class='font18'>{0}</span><span class='font14'> onwards</span>", Bikewale.Utility.Format.FormatPrice(Convert.ToString(estimatedPrice)));
            }
            else
            {
                return "<span class='font16'>Price Unavailable</span>";
            }
        }

    }
}