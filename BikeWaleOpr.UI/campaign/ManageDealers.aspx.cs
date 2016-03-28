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
    /// <summary>
    /// Created by: Sangram Nandkhile on 25 Mar 2016
    /// Desc:       To manage dealer campaigs with add/update/delete options
    /// </summary>
    public class ManageDealers : System.Web.UI.Page
    {
        #region variable

        protected int dealerId, contractId, campaignId, currentUserId;
        protected string dealerName;
        protected Button btnUpdate;
        protected ManageDealerCampaign dealerCampaign;
        protected TextBox txtdealerRadius, txtDealerEmail, txtMaskingNumber;
        protected string startDate, endDate;
        public Label lblGreenMessage, lblErrorSummary;
        public HtmlGenericControl textArea;
        public bool isCampaignPresent;
        
        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnUpdate.Click += new EventHandler(InserOrUpdateDealerCampaign);
            dealerCampaign = new ManageDealerCampaign();
        }

        private void InserOrUpdateDealerCampaign(object sender, EventArgs e)
        {
            try
            {
                if (isCampaignPresent)
                {
                    dealerCampaign.UpdateBWDealerCampaign(
                        true,
                        campaignId,
                        currentUserId,
                        dealerId,
                        contractId,
                        Convert.ToInt16(txtdealerRadius.Text),
                        txtMaskingNumber.Text,
                        dealerName,
                        txtDealerEmail.Text,
                        false);
                    lblGreenMessage.Text = "Selecte campaign has been Updated !";
                }
                else
                {
                    dealerCampaign.InsertBWDealerCampaign(
                        true,
                        currentUserId,
                        dealerId,
                        contractId,
                        Convert.ToInt16(txtdealerRadius.Text),
                        txtMaskingNumber.Text,
                        dealerName,
                        txtDealerEmail.Text,
                        false);
                    lblGreenMessage.Text = "New campaign has been added !";
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ParseQueryString();
            SetPageVariables();
            if (!IsPostBack)
            {
                if (isCampaignPresent)
                    FetchDealeCampaign();
            }
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
                DataTable dtCampaign = dealerCampaign.FetchBWDealerCampaign(campaignId);
                if(dtCampaign !=null && dtCampaign.Rows.Count > 0)
                {
                    txtdealerRadius.Text = dtCampaign.Rows[0]["DealerLeadServingRadius"].ToString();
                    txtMaskingNumber.Text = dtCampaign.Rows[0]["Number"].ToString();
                    txtDealerEmail.Text = dtCampaign.Rows[0]["DealerEmailId"].ToString();
                }
                else
                {
                    //Response.Redirect("../pagenotfound.aspx");
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
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.SetPageVariables");
                objErr.SendMail();
            }
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
                if (!string.IsNullOrEmpty(Request.QueryString["campaignid"]))
                {
                    campaignId = Convert.ToInt32(Request.QueryString["campaignid"]);
                    isCampaignPresent = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.ParseQueryString");
                objErr.SendMail();
            }
        }
        #endregion
    }
}