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
        protected TextBox txtTenure, txtROI, txtLTV, txtloanProvider;

        protected int dealerId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnSaveEMI.Click += new EventHandler(SaveDealerLoanAmounts);
            btnUpdateEMI.Click += new EventHandler(UpdateDealerLaonAmounts);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnUpdateEMI.Visible = false;

            if (Request.QueryString["dealerId"] != null)
            {
                int.TryParse(Request.QueryString["dealerId"].ToString(), out dealerId);
            }

            if (!IsPostBack)
            {
                if (dealerId > 0)
                {
                    GetDealerLoanAmounts();
                }
            }
        }

        protected async void GetDealerLoanAmounts()
        {
            EMI emi = null;
            
            string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            string _requestType = "application/json";
            
            string _apiUrl = "/api/Dealers/GetDealerLoanAmounts/?dealerId=" + dealerId;
            // Send HTTP GET requests 
            
            emi = await BWHttpClient.GetApiResponse<EMI>(_abHostUrl, _requestType, _apiUrl, emi);

            if(emi != null)
            {
                //loanToValue = emi.LoanToValue.ToString();
                //rateOfInterest = emi.RateOfInterest.ToString();
                //tenure = emi.Tenure.ToString();

                txtLTV.Text = emi.LoanToValue.ToString();
                txtROI.Text = emi.RateOfInterest.ToString();;
                txtTenure.Text = emi.Tenure.ToString();
                txtloanProvider.Text = emi.LoanProvider.ToString();

                btnUpdateEMI.Visible = true;
                btnSaveEMI.Visible = false;
            }
        }

        protected void SaveDealerLoanAmounts(object sender, EventArgs e)
        {
            if (dealerId > 0)
            {
                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                Trace.Warn("1",dealerId.ToString());
                Trace.Warn("2",txtTenure.Text.Trim());
                Trace.Warn("3", txtROI.Text.Trim());
                Trace.Warn("4", txtLTV.Text.Trim());
                Trace.Warn("5", txtloanProvider.Text.Trim());
                string _apiUrl = "/api/Dealers/SaveDealerLoanAmounts/?dealerId=" + dealerId + "&tenure=" + txtTenure.Text.Trim() + "&rateOfInterest=" + txtROI.Text.Trim() + "&ltv=" + txtLTV.Text.Trim()+"&loanProvider="+ Server.UrlEncode(txtloanProvider.Text.Trim());
                
                // Send HTTP POST requests 

                BWHttpClient.PostSync<string>(_abHostUrl, _requestType, _apiUrl, "");

                GetDealerLoanAmounts();
            }
        }

        protected void UpdateDealerLaonAmounts(object sender, EventArgs e)
        {
            if (dealerId > 0)
            {
                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = "/api/Dealers/UpdateDealerLoanAmounts/?dealerId=" + dealerId + "&tenure=" + txtTenure.Text.Trim() + "&rateOfInterest=" + txtROI.Text.Trim() + "&ltv=" + txtLTV.Text.Trim() + "&loanProvider=" + Server.UrlEncode(txtloanProvider.Text.Trim()) ;
                // Send HTTP POST requests 

                Trace.Warn("_apiUrl : ", _apiUrl);

                BWHttpClient.PostSync<string>(_abHostUrl, _requestType, _apiUrl, "");

                GetDealerLoanAmounts();
            }
        }
    }
}