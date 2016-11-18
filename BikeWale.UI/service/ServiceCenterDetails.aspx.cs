using Bikewale.BindViewModels.Webforms.Service;
using Bikewale.Controls;
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
    /// Created By : Sangram Nandkhile on 17 Nov 2016
    /// Class to show the service center details
    /// </summary>
    public class ServiceCenterDetails : Page
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, urlCityMaskingName = string.Empty,
        address = string.Empty, maskingNumber = string.Empty, eMail = string.Empty, workingHours = string.Empty, modelImage = string.Empty, dealerName = string.Empty, dealerMaskingName = string.Empty, clientIP = string.Empty;
        protected uint makeId, cityId, serviceCenterId;
        protected ushort totalDealers;
        protected ServiceCenterCompleteData objServiceCenterData = null;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected ServiceSchedule ctrlServiceSchedule;
        protected DealerCard ctrlDealerCard;

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
            }
        }
        /// <summary>
        /// Created By : Sangram Nandkhile on 17 Nov 2016
        /// Desc: Bind Controls for page
        /// </summary>
        private void BindControls()
        {
            ctrlServiceSchedule.MakeId = serviceVM.MakeId;
            ctrlServiceSchedule.MakeName = serviceVM.MakeName;

            ctrlServiceCenterCard.MakeId = serviceVM.MakeId;
            ctrlServiceCenterCard.CityId = serviceVM.CityId;
            ctrlServiceCenterCard.makeName = serviceVM.MakeName;
            ctrlServiceCenterCard.cityName = serviceVM.CityName;
            ctrlServiceCenterCard.makeMaskingName = makeMaskingName;
            ctrlServiceCenterCard.cityMaskingName = serviceVM.CityMaskingName;
            ctrlServiceCenterCard.TopCount = 3;
            ctrlServiceCenterCard.ServiceCenterId = serviceCenterId;

            ctrlDealerCard.MakeId = makeId;
            ctrlDealerCard.makeMaskingName = makeMaskingName;
            ctrlDealerCard.CityId = serviceVM.CityId;
            ctrlDealerCard.cityName = serviceVM.CityName;
            ctrlDealerCard.TopCount = 3;
            ctrlDealerCard.LeadSourceId = 17;
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
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenterDetails.ProcessQueryString()");
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