using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Utility;
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
    /// Description : To show dealer details based on dealer id an campaign id
    /// </summary>
    public class DealerDetails : System.Web.UI.Page
    {
        protected Repeater rptModels;
        protected uint dealerId, campaignId;
        protected int dealerBikesCount = 0;
        protected DealerDetailEntity dealerDetails;
        protected bool isDealerDetail;
        private string dealerQuery = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (ProcessQueryString() && dealerId > 0 && campaignId > 0)
            {
              GetDealerDetails();
            }
            
        }

        #region Get Dealer Details
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 25th March 2016 
        /// Description : To get dealer details and bikes available at dealership
        /// </summary>
        private void GetDealerDetails()
        {
            DealerBikesEntity _dealer = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IDealer, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    _dealer = objCache.GetDealerDetailsAndBikes(dealerId);

                    if (_dealer != null && _dealer.DealerDetails != null)
                    {
                        dealerDetails = _dealer.DealerDetails;
                        isDealerDetail = true;
                        if (_dealer.Models != null && _dealer.Models.Count() > 0)
                        {
                            rptModels.DataSource = _dealer.Models;
                            rptModels.DataBind();
                            dealerBikesCount = _dealer.Models.Count();
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
        /// </summary>
        private bool ProcessQueryString()
        {
            var currentReq = HttpContext.Current.Request;
            try
            {

                if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                {
                    string _dealerQuery = currentReq.QueryString["query"];
                    if (!String.IsNullOrEmpty(_dealerQuery))
                    {
                        dealerQuery = EncodingDecodingHelper.DecodeFrom64(_dealerQuery);
                        uint.TryParse(HttpUtility.ParseQueryString(dealerQuery).Get("dealerId"), out dealerId);
                        uint.TryParse(HttpUtility.ParseQueryString(dealerQuery).Get("campId"), out campaignId);
                        return true;
                    } 
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