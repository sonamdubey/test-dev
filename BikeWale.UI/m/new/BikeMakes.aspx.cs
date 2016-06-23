using Bikewale.BindViewModels.Controls;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Memcache;
using Bikewale.Mobile.controls;
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
        protected NewNewsWidget ctrlNews;
        protected NewExpertReviewsWidget ctrlExpertReviews;
        protected NewVideosWidget ctrlVideos;
        protected MMostPopularBikes ctrlMostPopularBikes;
        protected Repeater rptMostPopularBikes, rptDiscontinued;
        protected DealersCard ctrlDealerCard;
        protected bool isDescription = false;

        protected Literal ltrDefaultCityName;
        protected int fetchedRecordsCount = 0;
        protected string makeId = String.Empty;
        protected BikeMakeEntityBase _make = null;
        protected BikeDescriptionEntity _bikeDesc = null;
        protected int uCount = 0;
        protected short reviewTabsCnt = 0;

        //Variable to Assing ACTIVE .css class
        protected bool isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;

        private string makeMaskingName;
        protected Int64 _minModelPrice;
        protected Int64 _maxModelPrice;
        protected uint cityId = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea().CityId;
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

                //To get Upcoming Bike List Details 
                ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcomingBikes.pageSize = 6;
                ctrlUpcomingBikes.MakeId = Convert.ToInt32(makeId);
                ctrlUpcomingBikes.MakeName = _make.MakeName;

                ////news,videos,revews
                ctrlNews.TotalRecords = 2;
                ctrlNews.MakeId = Convert.ToInt32(makeId);
                ctrlNews.WidgetTitle = _make.MakeName;

                ctrlExpertReviews.TotalRecords = 3;
                ctrlExpertReviews.MakeId = Convert.ToInt32(makeId);
                ctrlExpertReviews.MakeMaskingName = makeMaskingName;

                ctrlVideos.TotalRecords = 1;
                ctrlVideos.MakeMaskingName = makeMaskingName;
                ctrlVideos.MakeId = Convert.ToInt32(makeId);

                ctrlDealerCard.CityId = cityId;
                ctrlDealerCard.MakeId = Convert.ToUInt32(makeId);
                ctrlDealerCard.makeMaskingName = makeMaskingName;
                ctrlDealerCard.makeName = _make.MakeName;
                ctrlDealerCard.TopCount = 6;
                ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Mobile_MakePage_GetOffersFromDealer;
                ctrlDealerCard.LeadSourceId = 30;
                BindDiscountinuedBikes();
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
            //fetchedRecordsCount = objMake.FetchedRecordsCount;
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
                    price += " <span class='font16'> Onwards</span>";
                }
            }
            return price;
        }

    }
}