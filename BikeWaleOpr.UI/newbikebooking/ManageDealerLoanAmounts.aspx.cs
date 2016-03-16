using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Entities.BikeBooking;

namespace BikeWaleOpr.NewBikeBooking
{
    public class ManageDealerLoanAmounts : System.Web.UI.Page
    {
        protected Button btnSaveEMI, btnReset, btnDelete;
        protected TextBox txtMinPayment, txtMaxPayment, txtMinTenure, txtMaxTenure, txtMinROI, txtMaxROI, txtMinLtv, txtMaxLtv, textLoanProvider, txtFees;
        EmiLoanAmount loanAmount;
        protected int dealerId = 0;
        protected uint loanId = 0;
        protected string cwHostUrl = string.Empty;
        protected Label errorSummary, finishMessage;
        protected HiddenField hdnLoanAmountId;

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnSaveEMI.Click += new EventHandler(SaveLoanProperties);
            btnReset.Click += new EventHandler(ResetFields);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            loanAmount = new EmiLoanAmount();
            if (Request.QueryString["dealerId"] != null)
            {
                int.TryParse(Request.QueryString["dealerId"].ToString(), out dealerId);
            }

            if (!IsPostBack)
            {
                if (dealerId > 0)
                {
                    GetLoanProperties();
                }
            }
        }

        /// <summary>
        /// Created by  : Sangram Nandkhile
        /// Created on  : 14-March-2016
        /// Desc        : Resets all the input
        /// </summary>
        protected void ResetFields(object sender, EventArgs e)
        {
            ClearForm(Page.Form.Controls,true);
            btnDelete.Visible = false;
        }

        #endregion

        #region functions
        /// <summary>
        /// Created by  : Sangram Nandkhile
        /// Created on  : 14-March-2016
        /// Desc        :  To get Loan amount entered by Dealer     
        /// </summary>
        private async void GetLoanProperties()
        {
            loanAmount = new EmiLoanAmount();
            try
            {
                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = string.Format("api/Dealers/GetDealerLoanAmounts/?dealerId={0}",dealerId);
                // Send HTTP GET requests
                loanAmount = await BWHttpClient.GetApiResponse<EmiLoanAmount>(cwHostUrl, _requestType, _apiUrl, loanAmount);
                // populate already saved value
                if(loanAmount != null && loanAmount.Id > 0)
                {
                    txtMinPayment.Text = Convert.ToString(loanAmount.MinDownPayment);
                    txtMaxPayment.Text = Convert.ToString(loanAmount.MaxDownPayment);

                    txtMinTenure.Text = Convert.ToString(loanAmount.MinTenure);
                    txtMaxTenure.Text = Convert.ToString(loanAmount.MaxTenure);

                    txtMinROI.Text = Convert.ToString(loanAmount.MinRateOfInterest);
                    txtMaxROI.Text = Convert.ToString(loanAmount.MaxRateOfInterest);

                    txtMinLtv.Text = Convert.ToString(loanAmount.MinLoanToValue);
                    txtMaxLtv.Text = Convert.ToString(loanAmount.MaxLoanToValue);

                    textLoanProvider.Text = loanAmount.LoanProvider;
                    txtFees.Text = Convert.ToString(loanAmount.ProcessingFee);
                    hdnLoanAmountId.Value = loanAmount.Id.ToString();
                    btnSaveEMI.Text = "Update EMI";
                    btnDelete.Visible = true;
                    loanId = loanAmount.Id;
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
        /// Created by  : Sangram Nandkhile on 14-March-2016
        /// Description : To Save Load Properties    
        /// </summary>
        protected void SaveLoanProperties(object sender, EventArgs e)
        {
            finishMessage.Text = string.Empty;
            errorSummary.Text = string.Empty;
             try
            {
                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = string.Format("api/Dealers/SaveDealerEMI/?dealerId={0}&loanProvider={1}&userID={2}&minDownPayment={3}&maxDownPayment={4}&minTenure={5}&maxTenure={6}&minRateOfInterest={7}&maxRateOfInterest={8}&minLtv={9}&maxLtv={10}&processingFee={11}&id={12}",
                    dealerId,
                   textLoanProvider.Text,
                   CurrentUser.Id,
                   txtMinPayment.Text,
                   txtMaxPayment.Text,
                   txtMinTenure.Text,
                   txtMaxTenure.Text,
                   txtMinROI.Text,
                   txtMaxROI.Text,
                   txtMinLtv.Text,
                   txtMaxLtv.Text,
                   txtFees.Text,
                   hdnLoanAmountId.Value == "0"? null: hdnLoanAmountId.Value);
                // Send HTTP GET requests
                bool status = false;
                status = BWHttpClient.PostSync<bool>(cwHostUrl, _requestType, _apiUrl, status);
                if (status)
                    finishMessage.Text = "Data has been saved !";
                btnDelete.Visible = true;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by  : Sangram Nandkhile on 14-March-2016
        /// Description : Resets all the Textboxes
        /// </summary>
        public void ClearForm(ControlCollection controls, bool? clearLabels)
        {
            bool toClearLabel = clearLabels == null ? false : true;
            foreach (Control c in controls)
            {
                if (c.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                {
                    System.Web.UI.WebControls.TextBox t = (System.Web.UI.WebControls.TextBox)c;
                    t.Text = String.Empty;
                }
                else if (toClearLabel && c.GetType() == typeof(System.Web.UI.WebControls.Label))
                {
                    System.Web.UI.WebControls.Label l = (System.Web.UI.WebControls.Label)c;
                    l.Text = String.Empty;
                }
                if (c.Controls.Count > 0) ClearForm(c.Controls, clearLabels);
            }
        }
        #endregion
    }
}