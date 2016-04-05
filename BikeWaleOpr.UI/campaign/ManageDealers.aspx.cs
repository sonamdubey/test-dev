using BikewaleOpr.common;
using BikewaleOpr.Common;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        protected string dealerName, oldMaskingNumber, dealerMobile, reqFormMaskingNumber;
        protected Button btnUpdate;
        protected ManageDealerCampaign dealerCampaign;
        protected TextBox txtdealerRadius, txtDealerEmail, txtMaskingNumber;
        protected string startDate, endDate;
        public Label lblGreenMessage, lblErrorSummary;
        public HtmlGenericControl textArea;
        public bool isCampaignPresent;
        public DropDownList ddlMaskingNumber;
        public HiddenField hdnOldMaskingNumber;
        
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
            KnowlarityAPI callApp = new KnowlarityAPI();
            bool isMaskingChanged = hdnOldMaskingNumber.Value == reqFormMaskingNumber ? false : true;
            bool IsProd  = Convert.ToBoolean(ConfigurationManager.AppSettings["isProduction"]);
            try
            {
                // Update campaign
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
                    lblGreenMessage.Text = "Selected campaign has been Updated !";
                    if (IsProd && isMaskingChanged)
                    {
                        // Release previous number and add new number
                        callApp.clearMaskingNumber(hdnOldMaskingNumber.Value);
                        callApp.pushDataToKnowlarity(false, "-1", dealerMobile, string.Empty, reqFormMaskingNumber);
                    }
                }
                else // Insert new campaign
                {
                   campaignId = dealerCampaign.InsertBWDealerCampaign(
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
                    isCampaignPresent = true;
                    if (IsProd)
                    {
                        // Add new number to knowlarity
                        callApp.pushDataToKnowlarity(false, "-1", dealerMobile, string.Empty, reqFormMaskingNumber);
                    }
                }
                ClearForm(Page.Form.Controls, true);
            }
            catch
            {
                throw;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ParseQueryString();
            if (Request.Form["txtMaskingNumber"] != null)
            {
                reqFormMaskingNumber = Convert.ToString(Request.Form["txtMaskingNumber"]);
            }
            SetPageVariables();
            if (isCampaignPresent)
                FetchDealeCampaign();
            if (!IsPostBack)
            {
                LoadMaskingNumbers();
            }

        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 05-Apr-2016
        /// Description : To Load masking numbers dropdown
        /// </summary>
        private void LoadMaskingNumbers()
        {
            try
            {
                DataTable dtb = dealerCampaign.BindMaskingNumbers(dealerId);
                List<ListItem> maskingList = new List<ListItem>();
                if (dtb != null)
                {
                    foreach ( DataRow dr in dtb.Rows )
                    {
                        ListItem lst = new ListItem(Convert.ToString(dr[1]), Convert.ToString(dr[0]));
                        if(dr[2].ToString() == "1")
                        {
                            lst.Attributes.Add("disabled", "disabled");
                        }
                        maskingList.Add(lst);
                    }
                    ddlMaskingNumber.Items.AddRange(maskingList.ToArray());
                    ddlMaskingNumber.DataBind();
                    ListItem item = new ListItem("--Select Number--", "0");
                    ddlMaskingNumber.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.LoadMaskingNumbers");
                objErr.SendMail();
            }
        }

        #endregion

        #region functions

        /// <summary>
        /// Created By : Sangram Nandkhile on 21st March 2016.
        /// Description : To fetch current campaign details
        /// Updated by: Sangram
        /// Summary:    Added dealerMobile,hdnOldMaskingNumber,oldMaskingNumber
        /// </summary>
        private void FetchDealeCampaign()
        {
            try
            {
                DataTable dtCampaign = dealerCampaign.FetchBWDealerCampaign(campaignId);
                if(dtCampaign !=null && dtCampaign.Rows.Count > 0)
                {
                    txtdealerRadius.Text = dtCampaign.Rows[0]["DealerLeadServingRadius"].ToString();
                    if (!String.IsNullOrEmpty(Convert.ToString(dtCampaign.Rows[0]["Number"])))
                    {
                        txtMaskingNumber.Text = Convert.ToString(dtCampaign.Rows[0]["Number"]);
                        oldMaskingNumber = txtMaskingNumber.Text;
                        hdnOldMaskingNumber.Value = txtMaskingNumber.Text;
                    }
                    oldMaskingNumber = txtMaskingNumber.Text;
                    txtDealerEmail.Text = dtCampaign.Rows[0]["DealerEmailId"].ToString();
                    dealerMobile = dtCampaign.Rows[0]["dealerMobile"].ToString();
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
                if (c.Controls.Count > 0) ClearForm(c.Controls, clearLabels);
            }
        }
        #endregion
    }
}