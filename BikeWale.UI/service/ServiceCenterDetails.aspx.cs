using Bikewale.BindViewModels.Webforms.Service;
using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Notifications;
using System;
using System.Web;
using System.Web.UI;

namespace Bikewale.Service
{
    /// <summary>
    /// Created By : Aditi Srivastava on 26 Sep 2016
    /// Class to show the bike dealers details
    /// </summary>
    public class ServiceCenterDetails : Page
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, urlCityMaskingName = string.Empty,
        address = string.Empty, maskingNumber = string.Empty, eMail = string.Empty, workingHours = string.Empty, modelImage = string.Empty, dealerName = string.Empty, dealerMaskingName = string.Empty,
        clientIP = string.Empty;
        protected uint makeId, cityId, serviceCenterId;
        protected ushort totalDealers;
        protected ServiceCenterCompleteData objServiceCenterData = null;
        //protected Repeater rptMakes, rptCities, rptDealers;
        //protected bool areDealersPremium = false;
        //protected DealerBikesEntity dealerDetails = null;
        //protected DealerCard ctrlDealerCard;
        //protected LeadCaptureControl ctrlLeadCapture;
        //protected DealerDetailEntity dealerObj;
        public ServiceDetailsPage serviceVM;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();
            if (ProcessQueryString())
            {
                if (serviceCenterId > 0 && serviceVM.BindServiceCenterData(serviceCenterId))
                {
                    BindControls();
                }
                else // If servie Object is null, redirect to page not found
                {
                    RedirectToPageNotFound();
                }
                //if (dealerId > 0)
                //{
                //    GetDealerDetails(dealerId);
                //    ctrlDealerCard.MakeId = Convert.ToUInt32(makeId);
                //    ctrlDealerCard.makeName = makeName;
                //    ctrlDealerCard.makeMaskingName = makeMaskingName;
                //    ctrlDealerCard.CityId = cityId;
                //    ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Desktop_dealer_details_Get_offers;
                //    ctrlDealerCard.LeadSourceId = 38;
                //    ctrlDealerCard.TopCount = Convert.ToUInt16(cityId > 0 ? 3 : 6);
                //    ctrlDealerCard.pageName = "DealerDetail_Page_Desktop";
                //    ctrlDealerCard.DealerId = (uint)dealerId;
                //    ctrlLeadCapture.CityId = cityId;
                //    ctrlLeadCapture.AreaId = 0;
                //}
                //else
                //{
                //    Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //    this.Page.Visible = false;
                //}
            }
        }

        private void BindControls()
        {

        }

        #region Private Method to process querystring

        /// <summary>
        /// Created By :   Sangram Nandkhile on 16 Nov 2016
        /// Description :  Handle Make masking name rename 301 redirection
        /// </summary>
        private bool ProcessQueryString()
        {
            var currentReq = HttpContext.Current.Request;
            bool isValidQueryString = false;
            MakeMaskingResponse objResponse = null;
            try
            {
                if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                {
                    makeMaskingName = currentReq.QueryString["make"];
                    serviceCenterId = Convert.ToUInt32(currentReq.QueryString["id"]);
                    if (serviceCenterId > 0 && !string.IsNullOrEmpty(makeMaskingName))
                    {
                        serviceVM = new ServiceDetailsPage();
                        objResponse = serviceVM.GetMakeResponse(makeMaskingName);
                    }
                }
                else
                {
                    RedirectToPageNotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ProcessQueryString()");
                objErr.SendMail();
            }
            finally
            {
                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200)
                    {
                        makeId = objResponse.MakeId;
                        isValidQueryString = true;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                        isValidQueryString = false;
                    }
                    else
                    {
                        RedirectToPageNotFound();
                        isValidQueryString = false;
                    }
                }
                else
                {
                    RedirectToPageNotFound();
                    isValidQueryString = false;
                }
            }
            return isValidQueryString;
        }
        #endregion

        public void RedirectToPageNotFound()
        {
            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            this.Page.Visible = false;
        }
    }   // End of class
}   // End of namespace