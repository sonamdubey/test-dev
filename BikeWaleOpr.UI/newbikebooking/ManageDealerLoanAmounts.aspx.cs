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
        protected Button btnSaveEMI, btnReset;
        protected TextBox txtMinPayment, txtMaxPayment, txtMinTenure, txtMaxTenure, txtMinROI, txtMaxROI, txtMinLtv, txtMaxLtv, textLoanProvider, txtFees;
        EmiLoanAmount loanAmount;
        protected int _dealerId = 0;
        protected string cwHostUrl = string.Empty;
        protected Label errorSummary, finishMessage;
        protected HiddenField hdnLoanAmountId;

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnSaveEMI.Click += new EventHandler(SaveLoanProperties);
            //btnUpdateEMI.Click += new EventHandler(UpdateLoanProperties);
            btnReset.Click += new EventHandler(ResetFields);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            loanAmount = new EmiLoanAmount();
            if (Request.QueryString["dealerId"] != null)
            {
                int.TryParse(Request.QueryString["dealerId"].ToString(), out _dealerId);
            }

            if (!IsPostBack)
            {
                if (_dealerId > 0)
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
                string _apiUrl = string.Format("api/Dealers/GetDealerLoanAmounts/?dealerId={0}",_dealerId);
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
                    _dealerId,
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
        /// <summary>
        /// Commented code on 11-mar02016 by Sangram Nandkhile
        /// </summary>
        #region commented code on 

        //protected async void GetDealerLoanAmounts()
        //{
        //    EMI emi = null;
            
        //    string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
        //    string _requestType = "application/json";
            
        //    string _apiUrl = "/api/Dealers/GetDealerLoanAmounts/?dealerId=" + dealerId;
        //    // Send HTTP GET requests 
            
        //    emi = await BWHttpClient.GetApiResponse<EMI>(_abHostUrl, _requestType, _apiUrl, emi);

        //    if(emi != null)
        //    {
        //        //loanToValue = emi.LoanToValue.ToString();
        //        //rateOfInterest = emi.RateOfInterest.ToString();
        //        //tenure = emi.Tenure.ToString();

        //        txtLTV.Text = emi.LoanToValue.ToString();
        //        txtROI.Text = emi.RateOfInterest.ToString();;
        //        txtTenure.Text = emi.Tenure.ToString();
        //        txtloanProvider.Text = emi.LoanProvider.ToString();

        //        btnUpdateEMI.Visible = true;
        //        btnSaveEMI.Visible = false;
        //    }
        //}

        //protected void SaveDealerLoanAmounts(object sender, EventArgs e)
        //{
        //    if (dealerId > 0)
        //    {
        //        string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
        //        string _requestType = "application/json";

        //        Trace.Warn("1",dealerId.ToString());
        //        Trace.Warn("2",txtTenure.Text.Trim());
        //        Trace.Warn("3", txtROI.Text.Trim());
        //        Trace.Warn("4", txtLTV.Text.Trim());
        //        Trace.Warn("5", txtloanProvider.Text.Trim());
        //        string _apiUrl = "/api/Dealers/SaveDealerLoanAmounts/?dealerId=" + dealerId + "&tenure=" + txtTenure.Text.Trim() + "&rateOfInterest=" + txtROI.Text.Trim() + "&ltv=" + txtLTV.Text.Trim()+"&loanProvider="+ Server.UrlEncode(txtloanProvider.Text.Trim());
                
        //        // Send HTTP POST requests 

        //        BWHttpClient.PostSync<string>(_abHostUrl, _requestType, _apiUrl, "");

        //        GetDealerLoanAmounts();
        //    }
        //}

        //protected void UpdateDealerLaonAmounts(object sender, EventArgs e)
        //{
        //    if (dealerId > 0)
        //    {
        //        string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
        //        string _requestType = "application/json";

        //        string _apiUrl = "/api/Dealers/UpdateDealerLoanAmounts/?dealerId=" + dealerId + "&tenure=" + txtTenure.Text.Trim() + "&rateOfInterest=" + txtROI.Text.Trim() + "&ltv=" + txtLTV.Text.Trim() + "&loanProvider=" + Server.UrlEncode(txtloanProvider.Text.Trim()) ;
        //        // Send HTTP POST requests 

        //        Trace.Warn("_apiUrl : ", _apiUrl);

        //        BWHttpClient.PostSync<string>(_abHostUrl, _requestType, _apiUrl, "");

        //        GetDealerLoanAmounts();
        //    }
        //}

        #endregion
    }
}