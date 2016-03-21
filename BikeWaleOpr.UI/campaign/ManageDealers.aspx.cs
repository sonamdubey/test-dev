using BikewaleOpr.Common;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikewaleOpr.Campaign
{
    public class ManageDealers : System.Web.UI.Page
    {
        #region variable

        protected int dealerId, contractId, campaignId, currentUserId;
        protected string dealerName;
        protected Button btnUpdate;
        protected ManageDealerCampaign dealerCampaign;
        protected TextBox txtdealerRadius, txtDealerEmail, txtMaskingNumber, txtStartDate, txtEndDate;
        protected string startDate, endDate;
        public Label lblGreenMessage, lblErrorSummary;
        public HtmlGenericControl textArea;
        public HiddenField hdnStartDate, hdnEndDate;
        public DateTime dtStart, dtEnd;
        bool isCampaignActive;
        
        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnUpdate.Click += new EventHandler(UpdateDealerCampaign);
        }

        private void UpdateDealerCampaign(object sender, EventArgs e)
        {
            dealerCampaign = new ManageDealerCampaign();

            if (DateTime.Now > dtStart && DateTime.Now < dtEnd)
                isCampaignActive = true;

            dealerCampaign.InsertBWDealerCampaign(
                isCampaignActive,
                currentUserId,
                dealerId,
                contractId,
                Convert.ToInt16(txtdealerRadius.Text),
                dtStart,
                dtEnd,
                txtMaskingNumber.Text,
                dealerName,
                txtDealerEmail.Text,
                false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ParseQueryString();
            SetPageVariables();
            FetchDealeCampaign();
        }

        #endregion

        #region functions

        /// <summary>
        /// Created By : Sangram Nandkhile on 21st March 2016.
        /// Description : To fetch current campaign details
        /// </summary>
        private void FetchDealeCampaign()
        {
            try
            {
                dealerCampaign = new ManageDealerCampaign();
                DataTable dtCampaign = dealerCampaign.FetchBWDealerCampaign(contractId);
                if(dtCampaign !=null && dtCampaign.Rows.Count > 0)
                {
                    txtdealerRadius.Text = dtCampaign.Rows[0]["DealerLeadServingRadius"].ToString();
                    txtMaskingNumber.Text = dtCampaign.Rows[0]["Number"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.FetchDealeCampaign");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By :   Sangram Nandkhile on 21st March 2016.
        /// Description :  Sets basic variable for get, set, update functions
        /// </summary>
        private void SetPageVariables()
        {
            try
            {
                currentUserId = Convert.ToInt32(CurrentUser.Id);
                if (!string.IsNullOrEmpty(hdnStartDate.Value))
                {
                    dtStart = DateTime.ParseExact(hdnStartDate.Value, "dd-MM-yyyy", CultureInfo.CurrentCulture);
                }
                if (!string.IsNullOrEmpty(hdnEndDate.Value))
                {
                    dtEnd = DateTime.ParseExact(hdnEndDate.Value, "dd-MM-yyyy", CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.SetPageVariables");
                objErr.SendMail();
            }
            //startDate = Request.Form[txtStartDate.UniqueID];
            //endDate = Request.Form[txtEndDate.UniqueID];
            //var dateStr = @"2011-03-21 13:26";
            //var dateTime = DateTime.ParseExact(dateStr, "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 21st March 2016.
        /// Description : Parses query string to fetch campaign id, dealerid and dealerName
        /// </summary>
        private void ParseQueryString()
        {
            try
            {
                if (Request.QueryString["contractid"] == null || Request.QueryString["dealerid"] == null || Request.QueryString["dealername"] == null)
                {
                    //page not found
                    Response.Redirect("../pagenotfound.aspx");
                }
                if (!string.IsNullOrEmpty(Request.QueryString["contractid"]))
                {
                    contractId = Convert.ToInt32(Request.QueryString["contractid"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["dealerid"]))
                {
                    dealerId = Convert.ToInt32(Request.QueryString["dealerid"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["dealername"]))
                {
                    dealerName = Request.QueryString["dealername"];
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.ParseQueryString");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 21st March 2016.
        /// Description : 
        /// </summary>
        private void Test()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.");
                objErr.SendMail();
            }
        }
 
        #endregion
    }
}