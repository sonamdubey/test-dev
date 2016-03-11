using BikewaleOpr.common;
using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikewaleOpr.newbikebooking
{
    /// <summary>
    /// Created by : Sangram
    /// Created on : 10-March-2016
    /// Summary    :  Web page to allow dealers to add Benefits/ USP
    /// </summary>
    public class ManageDealerBenefits : System.Web.UI.Page
    {
        #region global variables

        protected string _dealerId = string.Empty, _cityId = string.Empty, cwHostUrl = string.Empty, _currentUserID = string.Empty;
        ManageDealerBenefit manageDealer;
        protected Button btnAdd, btnReset, btnEditSubmit, btnEditReset, Button1;
        protected Repeater rptBenefits;
        protected DropDownList ddlBenefitCat, ddlEditBenefit;
        protected TextBox benefitText, txtEditBenefit;
        protected HiddenField hdnBenefitId;
        
        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            btnAdd.Click += new EventHandler(SaveOffers);
            btnEditSubmit.Click += new EventHandler(SaveEditedOffers);
            btnReset.Click += new EventHandler(ResetForm);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            manageDealer = new ManageDealerBenefit();
            _currentUserID = CurrentUser.Id;
            _dealerId = Request.QueryString["dealerId"];
            _cityId = Request.QueryString["cityId"];
            // First page load
            if (!IsPostBack)
            {
                BindData();
            }
            GetDealerBenefits();
        }

        private void SaveEditedOffers(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
        }
        private void SaveOffers(object sender, EventArgs e)
        {
            SaveDealerBenefit();
        }

        private void ResetForm(object sender, EventArgs e)
        {

            ddlBenefitCat.SelectedIndex = 0;
            benefitText.Text = string.Empty;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Sangram
        /// Created on : 10-March-2016
        /// Summary    : Add new dealer benefit
        /// </summary>
        private async void SaveDealerBenefit()
        {
            try
            {

                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = string.Format("/api/Dealers/SaveDealerBenefit/?dealerId={0}&cityId={1}&catId={2}&benefitText={3}&userId={4}&benefitId={5}", _dealerId, _cityId, ddlBenefitCat.SelectedValue, HttpUtility.UrlEncode(benefitText.Text), CurrentUser.Id, hdnBenefitId.Value);
                // Send HTTP GET requests
                bool status = false;
                status = await BWHttpClient.PostAsync<bool>(cwHostUrl, _requestType, _apiUrl, status);

                GetDealerBenefits();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by : Sangram
        /// Created on : 10-March-2016
        /// Summary    : Get all dealer Benefits
        /// </summary>
        private async void GetDealerBenefits()
        {
            try
            {
                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = "/api/Dealers/GetDealerBenefits/?dealerId=" + _dealerId;
                // Send HTTP GET requests
                List<DealerBenefitEntity> objOfferList = null;
                objOfferList = await BWHttpClient.GetApiResponse<List<DealerBenefitEntity>>(cwHostUrl, _requestType, _apiUrl, objOfferList);
                if (objOfferList != null && objOfferList.Count > 0)
                {
                    rptBenefits.DataSource = objOfferList;
                    rptBenefits.DataBind();
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by : Sangram
        /// Created on : 10-March-2016
        /// Summary    : Bind all the Dropdowns and Repeaters
        /// </summary>
        private void BindData()
        {
            // Bind Benefits for current dealer
            List<DealerBenefitEntity> benefitList = manageDealer.GetDealerBenefits(_dealerId);
            if (benefitList != null && benefitList.Count > 0)
            {
                rptBenefits.DataSource = benefitList;
                rptBenefits.DataBind();
            }

            // Bind Benefits categories
            Dictionary<int, string> benefitCategories = manageDealer.GetDealerCategories(_dealerId);
            if (benefitCategories != null)
            {
                ddlBenefitCat.DataSource = benefitCategories;
                ddlBenefitCat.DataTextField = "Value";
                ddlBenefitCat.DataValueField = "Key";
                ddlBenefitCat.DataBind();
                ddlEditBenefit.DataSource = benefitCategories;
                ddlEditBenefit.DataTextField = "Value";
                ddlEditBenefit.DataValueField = "Key";
                ddlEditBenefit.DataBind();
            }
        }

        #endregion
    }
}