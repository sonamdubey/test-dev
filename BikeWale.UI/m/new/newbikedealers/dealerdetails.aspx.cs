using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Mobile.Controls;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace Bikewale.Mobile
{
    /// <summary>
    /// Modified By : Sushil Kumar
    /// Modified On : 25 March 2016
    /// Description : To show dealer details based on dealer id an campaign id.
    /// Modified By : Lucky Rathore on 30 March 2016
    /// Description : dealerLat, dealerLong, dealerName, dealerArea, dealerCity added and _dealerQuery removed.
    /// </summary>
    public class DealerDetails : System.Web.UI.Page
    {
        protected Repeater rptModels, rptModelList;
        protected uint dealerId, campaignId = 0, cityId;
        protected int dealerBikesCount = 0;
        protected DealerDetailEntity dealerDetails;
        protected MostPopularBikesBase modelDetails;
        protected bool isDealerDetail;
        private string cityName = string.Empty;
        protected string makeName = string.Empty, dealerName = string.Empty, dealerArea = string.Empty, dealerCity = string.Empty;
        protected double dealerLat, dealerLong;
        protected DealersCard ctrlDealerCard;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (ProcessQueryString() && dealerId > 0)
            {
                GetDealerDetails();
            }

        }





        #region Get Dealer Details
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 25th March 2016 
        /// Description : To get dealer details and bikes available at dealership
        /// Modified By : Lucky Rathore on 30 March 2016
        /// Description : dealerLat, dealerLong, dealerName, dealerArea, dealerCity Intialize, renamed dealer from _dealer.
        /// Modified By : Sajal Gupta on 26-09-2016
        /// Description : Changed method to get details only on basis of dealerId and added details of dealer to the controller.
        /// </summary>
        private void GetDealerDetails()
        {
            DealerBikesEntity dealer = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IDealer, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    dealer = objCache.GetDealerDetailsAndBikes(dealerId);

                    if (dealer != null && dealer.DealerDetails != null)
                    {
                        dealerDetails = dealer.DealerDetails;
                        modelDetails = dealer.Models.FirstOrDefault();

                        isDealerDetail = true;

                        dealerName = dealerDetails.Name;
                        dealerArea = dealerDetails.Area.AreaName;
                        dealerCity = dealerDetails.City;

                        dealerLat = dealerDetails.Area.Latitude;
                        dealerLong = dealerDetails.Area.Longitude;

                        ctrlDealerCard.CityId = (uint)dealerDetails.CityId;
                        ctrlDealerCard.MakeId = (uint)modelDetails.MakeId;
                        ctrlDealerCard.makeMaskingName = modelDetails.MakeMaskingName;
                        ctrlDealerCard.makeName = modelDetails.MakeName;
                        ctrlDealerCard.cityName = dealerCity;
                        ctrlDealerCard.PageName = "Dealer_Details";
                        ctrlDealerCard.TopCount = 6;
                        ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Mobile_MakePage_GetOffersFromDealer;
                        ctrlDealerCard.LeadSourceId = 30;

                        campaignId = dealerDetails.CampaignId;
                        ctrlDealerCard.DealerId = (int)dealerId;

                        if (dealer.Models != null && dealer.Models.Count() > 0)
                        {
                            makeName = dealer.Models.FirstOrDefault().objMake.MakeName;
                            rptModels.DataSource = dealer.Models;
                            rptModels.DataBind();
                            dealerBikesCount = dealer.Models.Count();
                            rptModelList.DataSource = dealer.Models;
                            rptModelList.DataBind();
                        }
                    }
                    else
                    {
                        Response.Redirect("/pagenotfound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "GetDealerDetails");
                objErr.SendMail();
            }

        }
        #endregion

        #region Private Method to process querystring
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 16th March 2016 
        /// Description : Private Method to parse encoded query string and get values for dealerId and campaignId
        /// Modified By : Lucky Rathore on 30 March 2016
        /// Description : Renamed dealerQuery from _dealerQuery.
        /// </summary>
        private bool ProcessQueryString()
        {
            var currentReq = HttpContext.Current.Request;
            try
            {

                if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                {
                    uint.TryParse(currentReq.QueryString["dealerId"], out dealerId);
                    return true;
                }
                else
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            catch (Exception ex)
            {

                Trace.Warn("ProcessQueryString Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, currentReq.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return false;
        }
        #endregion
    }
}