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
        protected Button btnSaveEMI, btnUpdateEMI;
        //protected TextBox txtTenure, txtROI, txtLTV, txtloanProvider;
        protected TextBox txtMinPayment, txtMaxPayment, txtMinTenure, txtMaxTenure, txtMinROI, txtMaxROI, txtMinLtv, txtMaxLtv, textLoanProvider, txtFees;
        EmiLoanAmount loanAmount;
        protected int _dealerId = 0;
        protected string cwHostUrl = string.Empty;

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnSaveEMI.Click += new EventHandler(SaveLoanProperties);
            btnUpdateEMI.Click += new EventHandler(UpdateLoanProperties);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            loanAmount = new EmiLoanAmount();
            btnUpdateEMI.Visible = false;

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
                if(loanAmount != null)
                {

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
        /// Created by  : Sangram Nandkhile
        /// Created on  : 14-March-2016
        /// Desc        : To Save Load Properties    
        /// </summary>
        protected async void SaveLoanProperties(object sender, EventArgs e)
        {
             try
            {
                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = string.Format("api/Dealers/SaveDealerEMI/?dealerId={0}&tenure={1}&rateOfInterest={2}&ltv={3}&loanProvider={4}&userID={5}&minDownPayment={6}&maxDownPayment={7}&minTenure={8}&maxTenure={9}&minRateOfInterest={10}&maxRateOfInterest={11}&processingFee={12}&id={13}", _dealerId);
                // Send HTTP GET requests
                bool status = false;
                status = await BWHttpClient.GetApiResponse<bool>(cwHostUrl, _requestType, _apiUrl, status);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by  : Sangram Nandkhile
        /// Created on  : 14-March-2016
        /// Desc        : To update Loan Properties  
        /// </summary>
        protected async void UpdateLoanProperties(object sender, EventArgs e)
        {

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