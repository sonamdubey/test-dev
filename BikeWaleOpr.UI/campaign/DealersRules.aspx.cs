using BikewaleOpr.Cache.Campaigns;
using BikewaleOpr.Common;
using BikeWaleOpr.Common;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace BikewaleOpr.Campaign
{
    /// <summary>
    /// Created By : Sangram Nandkhile on 19 March 2016.
    /// Description : To add/update/delete dealer campaign rules
    /// </summary>
    public class DealersRules : System.Web.UI.Page
    {
        #region variable
        public int campaignId, dealerId, currentUserId, makeId;
        public string modelId,dealerName;
        public DropDownList ddlMake, ddlModel; //, ddlState, ddlCity;
        public Button btnSaveRule, btnReset, btnDeleteRules, btnDelete;
        public Repeater rptRules;
        public ManageDealerCampaignRule campaign = null;
        public HiddenField hdnSelectedModel, hdnCheckedRules;
        public Label lblGreenMessage, lblErrorSummary;
        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            currentUserId = Convert.ToInt32(CurrentUser.Id);
            this.Load += new EventHandler(Page_Load);
            btnSaveRule.Click += new EventHandler(SaveRules);
            btnDelete.Click += new EventHandler(DeleteRules);
        }

        /// <summary>
        /// Modified by : Sanskar Gupta on 13 Feb 2018
        /// Description : Added function call to clear dealer bikes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteRules(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdnCheckedRules.Value))
                {
                    campaign.DeleteDealerCampaignRules(currentUserId, hdnCheckedRules.Value);
                    BindRules();
                    Dealers.ClearDealerBikes(dealerId);
                    lblErrorSummary.Text = "Selected rules have been deleted !";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.DeleteRules");
                
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            campaign = new ManageDealerCampaignRule();
            FillDropDowns();
            lblGreenMessage.Text = string.Empty;
            lblErrorSummary.Text = string.Empty;
        }

        /// <summary>
        /// Modified by : Sanskar Gupta on 13 Feb 2018
        /// Description : Added function call to clear dealer bikes
        /// </summary>
        protected void SaveRules(object sender, EventArgs e)
        {
            try
            {
                if (campaign.InsertBWDealerCampaignRules(currentUserId, campaignId, dealerId, makeId, modelId))
                {
                    Dealers.ClearDealerBikes(dealerId);
                    lblGreenMessage.Text = "Rule(s) have been added !";
                }
                else
                {
                    lblErrorSummary.Text = "Some error has occurred !";
                }

                BindRules();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.SaveRules");
                
            }
        }

        #endregion

        #region functions

        /// <summary>
        /// Created By : Sangram Nandkhile on 19 March 2016.
        /// Description :  Sets basic variable for get, set, update functions
        /// </summary>
        private void SetPageVariables()
        {            
            if (!string.IsNullOrEmpty(hdnSelectedModel.Value))
                modelId = hdnSelectedModel.Value;
            if (!string.IsNullOrEmpty(ddlMake.SelectedValue))
                makeId = Convert.ToInt32(ddlMake.SelectedValue);            
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 19 March 2016.
        /// Description :  Populates the makes dropdown
        /// </summary>
        private void FillMakes()
        {
            try
            {
                DataTable dt;
                MakeModelVersion mmv = new MakeModelVersion();
                dt = mmv.GetMakes("New");
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlMake.DataSource = dt;
                    ddlMake.DataTextField = "Text";
                    ddlMake.DataValueField = "Value";
                    ddlMake.DataBind();
                    ListItem item = new ListItem("--Select Make--", "0");
                    ddlMake.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.FillMakes");
                
            }
        }
         
        /// <summary>
        /// Created By : Sangram Nandkhile on 19 March 2016.
        /// Description :  Populates all the dropdown lists
        /// </summary>
        private void FillDropDowns()
        {
            ParseQueryString();
            FillMakes();

            if (!IsPostBack)
            {
                BindRules();
            }
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 19 March 2016.
        /// Description :  Parses query string to fetch campaign id
        /// </summary>
        private void ParseQueryString()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["campaignid"]))
                {
                    campaignId = Convert.ToInt32(Request.QueryString["campaignId"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["dealerid"]))
                {
                    dealerId = Convert.ToInt32(Request.QueryString["dealerid"]);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["dealerName"]))
                {
                    dealerName = Convert.ToString(Request.QueryString["dealerName"]);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ParseQueryString");
                
            }
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 19 March 2016.
        /// Description : shows current rules for camp id
        /// </summary>
        private void BindRules()
        {
            try
            {
                rptRules.DataSource = null;
                DataTable dbRules = campaign.FetchBWDealerCampaignRules(campaignId, dealerId);
                if (dbRules != null && dbRules.Rows.Count > 0)
                {
                    rptRules.DataSource = dbRules;
                }
                rptRules.DataBind();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.BindRules");
                
            }
        }
        #endregion
    }
}