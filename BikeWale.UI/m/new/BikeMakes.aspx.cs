﻿
using Bikewale.BindViewModels.Controls;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile
{
    public class BikeMakes : PageBase
    {
        protected NewMUpcomingBikes ctrlUpcomingBikes;
        protected UsedBikeModel ctrlusedBikeModel;
        protected NewNewsWidget ctrlNews;
        protected NewExpertReviewsWidget ctrlExpertReviews;
        protected NewVideosWidget ctrlVideos;
        protected MMostPopularBikes ctrlMostPopularBikes;
        protected Repeater rptMostPopularBikes, rptDiscontinued, rptTop;
        protected DealersCard ctrlDealerCard;
        protected LeadCaptureControl ctrlLeadCapture;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected bool isDescription = false;
        private CityEntityBase cityDetails = null;
        protected Literal ltrDefaultCityName;
        protected int fetchedRecordsCount = 0;
        protected uint makeId;
        protected BikeMakeEntityBase _make = null;
        protected BikeDescriptionEntity _bikeDesc = null;
        protected int uCount = 0;
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE .css class
        protected bool isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;

        private string makeMaskingName, cityMaskingName;
        protected Int64 _minModelPrice;
        protected Int64 _maxModelPrice;
        protected uint cityId = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().CityId;
        protected string cityName = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().City;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        /// Modified By :-Subodh Jain on 16 Dec 2016
        /// Summary :- Added heading to dealer widget
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :  Added dealer card and service center card
        protected void Page_Load(object sender, EventArgs e)
        {
            //Function to process and validate Query String  
            if (ProcessQueryString())
            {
                //to get complete make page
                GetMakePage();
                BindUserControls();
                BindDiscountinuedBikes();
            }
        }
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Added used Bike widget
        private void BindUserControls()
        {
            int _makeId = Convert.ToInt16(makeId);
            if (cityId > 0)
            {
                cityDetails = new CityHelper().GetCityById(cityId);
                if (cityDetails != null)
                    cityMaskingName = cityDetails.CityMaskingName;
            }
            //To get Upcoming Bike List Details 
            ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
            ctrlUpcomingBikes.pageSize = 6;
            ctrlUpcomingBikes.MakeId = Convert.ToInt32(makeId);
            ctrlUpcomingBikes.MakeName = _make.MakeName;
            ctrlUpcomingBikes.MakeMaskingName = _make.MaskingName;

            ////news,videos,revews
            ctrlNews.TotalRecords = 3;
            ctrlNews.MakeId = Convert.ToInt32(makeId);
            ctrlNews.WidgetTitle = _make.MakeName;
            ctrlNews.MakeMaskingName = _make.MaskingName;
            ctrlNews.MakeName = _make.MakeName;

            ctrlExpertReviews.TotalRecords = 2;
            ctrlExpertReviews.MakeId = Convert.ToInt32(makeId);
            ctrlExpertReviews.MakeMaskingName = makeMaskingName;
            ctrlExpertReviews.MakeName = _make.MakeName;

            ctrlVideos.TotalRecords = 1;
            ctrlVideos.MakeMaskingName = makeMaskingName;
            ctrlVideos.MakeId = Convert.ToInt32(makeId);
            ctrlVideos.MakeName = _make.MakeName;

            ctrlDealerCard.CityId = cityId;
            ctrlDealerCard.MakeId = Convert.ToUInt32(makeId);
            ctrlDealerCard.makeMaskingName = makeMaskingName;
            ctrlDealerCard.makeName = _make.MakeName;
            ctrlDealerCard.cityName = cityName;
            ctrlDealerCard.PageName = "Make_Page";
            ctrlDealerCard.TopCount = 6;
            ctrlDealerCard.widgetHeading = string.Format("{0} showrooms in {1}", _make.MakeName, cityName);


            ctrlServiceCenterCard.MakeId = Convert.ToUInt32(makeId); ;
            ctrlServiceCenterCard.makeMaskingName = makeMaskingName;
            ctrlServiceCenterCard.makeName = _make.MakeName;
            ctrlServiceCenterCard.CityId = cityId;
            ctrlServiceCenterCard.cityName = cityName;
            ctrlServiceCenterCard.TopCount = 9;
            ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", _make.MakeName, cityName);
            ctrlLeadCapture.CityId = cityId;
            if (ctrlusedBikeModel != null)
            {

                ctrlusedBikeModel.MakeId = makeId;
                if (cityId > 0)
                    ctrlusedBikeModel.CityId = cityId;
                ctrlusedBikeModel.WidgetTitle = string.Format("Second-hand Honda Bikes in {0}", cityId > 0 ? cityName : "India");
                ctrlusedBikeModel.header = string.Format("Used {0} bikes in {1}", _make.MakeName, cityId > 0 ? cityName : "India");
                ctrlusedBikeModel.WidgetHref = string.Format("/m/used/{0}-bikes-in-{1}/", _make.MaskingName, cityId > 0 ? cityMaskingName : "india");
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
                            bike.Href = string.Format("/m/{0}-bikes/{1}/", _make.MaskingName, bike.ModelMasking);
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
                        Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                        objErr.SendMail();
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
                            }
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
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
            objMake.TopCount = 3;
            objMake.makeId = (int)makeId;
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
                    price += " <span class='font14'> onwards</span>";
                }
            }
            return price;
        }
    }
}