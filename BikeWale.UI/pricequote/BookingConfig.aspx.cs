using Bikewale.Common;
using Bikewale.DTO.BookingSummary;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.Mobile.PriceQuote;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Pricequote
{
    /// <summary>
    /// Author  : Sushil Kumar
    /// Created On : 3rd December 2015
    /// Description : Booking configurator codebehind
    /// </summary>
    public class BookingConfig : System.Web.UI.Page
    {
        protected uint dealerId = 0, versionId = 0, cityId = 0, pqId = 0, areaId = 0,varientPrice = 0;
        protected string clientIP = String.Empty, pageUrl = String.Empty, bikeName = String.Empty;
        protected BookingSummaryBase objBookingConfig = null;
        protected Repeater rptVarients = null, rptVersionColors = null;
        protected BikeDealerPriceDetailDTO selectedVarient = null;
        protected HiddenField selectedVersionId = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            if (!IsPostBack)
            {
                ProcessCookie();
                GetVersionNQuotationDetails();
            }
            else
            {
                if (ViewState["BookingConfig"] != null)
                {
                    string json = (string)ViewState["BookingConfig"];
                    objBookingConfig = JsonConvert.DeserializeObject<BookingSummaryBase>(json);
                }

                if (objBookingConfig.Varients != null && objBookingConfig.Varients.Count > 0)
                {
                    rptVarients.DataSource = objBookingConfig.Varients;
                    rptVarients.DataBind();
                }

            }


        }

        private void GetVersionNQuotationDetails()
        {
            bool _isContentFound = true;
            try
            {
                //sets the base URI for HTTP requests
                string _abHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = String.Format("api/BookingSummary?pqId={0}&versionId={1}&dealerId={2}&cityId={3}", pqId, versionId, dealerId, cityId);
                // Send HTTP GET requests 

                objBookingConfig = BWHttpClient.GetApiResponseSync<BookingSummaryBase>(_abHostUrl, _requestType, _apiUrl, objBookingConfig);

                if (objBookingConfig != null && objBookingConfig.DealerQuotation != null && objBookingConfig.Customer != null)
                {
                    //if (objBookingConfig.DealerQuotation.objBookingAmt == null || (objBookingConfig.DealerQuotation.objBookingAmt != null && objBookingConfig.DealerQuotation.objBookingAmt.Amount == 0))
                    //{
                    //    HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/detaileddealerquotation.aspx", false);
                    //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //    this.Page.Visible = false;
                    //    return;
                    //}

                    bikeName = String.Format("{0} {1}", objBookingConfig.DealerQuotation.objQuotation.objMake.MakeName, objBookingConfig.DealerQuotation.objQuotation.objModel.ModelName);

                    if (objBookingConfig.Varients != null)
                    {
                        BindVarientDetails();
                    }

                    //save booking details object in viewState
                    string jsonModel = JsonConvert.SerializeObject(objBookingConfig);
                    ViewState["BookingConfig"] = jsonModel;

                }
                else
                {
                    _isContentFound = false;
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private void BindVarientDetails()
        {

            if (String.IsNullOrEmpty(selectedVersionId.Value))
            {
                versionId = Convert.ToUInt32(selectedVersionId.Value);
            }                

            if (versionId > 0 && objBookingConfig != null && objBookingConfig.Varients != null && objBookingConfig.Varients.Count > 0)
            {

                rptVarients.DataSource = objBookingConfig.Varients;
                rptVarients.DataBind();

                foreach (var varient in objBookingConfig.Varients)
                {
                    if (varient.MinSpec.VersionId == versionId)
                    {
                        selectedVarient = varient;
                        varientPrice = Convert.ToUInt32(selectedVarient.MinSpec.Price);

                        rptVersionColors.DataSource = varient.BikeModelColors;
                        rptVersionColors.DataBind();
                        break;
                    }
                }
            }
        }

        //protected void getCustomerDetails()
        //{
        //    using (IUnityContainer container = new UnityContainer())
        //    {
        //        container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
        //        IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

        //        objCustomer = objDealer.GetCustomerDetails(Convert.ToUInt32(PriceQuoteCookie.PQId));
        //        if (objCustomer.objColor != null)
        //            color = objCustomer.objColor.ColorName;
        //    }
        //}




        #region Private Method to process cookie
        /// <summary>
        /// Checks for the valid PQ Cookie
        /// </summary>
        private void ProcessCookie()
        {
            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                if (UInt32.TryParse(PriceQuoteCookie.PQId, out pqId) && UInt32.TryParse(PriceQuoteCookie.DealerId, out dealerId) && UInt32.TryParse(PriceQuoteCookie.VersionId, out versionId))
                {
                    cityId = Convert.ToUInt32(PriceQuoteCookie.CityId);
                    areaId = Convert.ToUInt32(PriceQuoteCookie.AreaId);

                    if (dealerId > 0)
                    {
                        clientIP = Bikewale.Common.CommonOpn.GetClientIP();
                        pageUrl = Request.ServerVariables["URL"];
                    }
                    else
                    {
                        Response.Redirect("/", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect("/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }
        #endregion
    }
}