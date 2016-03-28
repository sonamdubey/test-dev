using BikewaleOpr.Common;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
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
        public int campaignId, dealerId, currentUserId, cityId, stateId, makeId;
        public int? modelId;
        public DropDownList ddlMake, ddlModel, ddlState, ddlCity;
        public Button btnSaveRule, btnReset, btnDeleteRules, btnDelete;
        public Repeater rptRules;
        public ManageDealerCampaignRule campaign = null;
        public HiddenField hdnSelectedModel, hdnSelectedCity, hdnCheckedRules;
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

        private void DeleteRules(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdnCheckedRules.Value))
                {
                    campaign.DeleteDealerCampaignRules(currentUserId, hdnCheckedRules.Value);
                    BindRules();
                    lblErrorSummary.Text = "Selected rules have been deleted !";
                }
            } 
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.DeleteRules");
                objErr.SendMail();
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

        protected void SaveRules(object sender, EventArgs e)
        {
            try
            {
                campaign.InsertBWDealerCampaignRules(currentUserId, campaignId, cityId, dealerId, makeId, stateId, modelId== -1? null: modelId);
                lblGreenMessage.Text = "Rule(s) have been added !";
                BindRules();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.SaveRules");
                objErr.SendMail();
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
            if (!string.IsNullOrEmpty(hdnSelectedCity.Value))
                cityId = Convert.ToInt32(hdnSelectedCity.Value);
            if (!string.IsNullOrEmpty(hdnSelectedModel.Value))
                modelId = Convert.ToInt32(hdnSelectedModel.Value);
            if (!string.IsNullOrEmpty(ddlMake.SelectedValue))
                makeId = Convert.ToInt32(ddlMake.SelectedValue);
            if (!string.IsNullOrEmpty(ddlState.SelectedValue))
                stateId = Convert.ToInt32(ddlState.SelectedValue);
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
                if (dt!=null && dt.Rows.Count > 0)
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.FillMakes");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 19 March 2016.
        /// Description :  Populates the States dropdown
        /// </summary>
        private void FillStates()
        {
            try
            {
                ManageStates objStates = new ManageStates();
                DataSet ds = objStates.GetAllStatesDetails();
                if (ds != null && ds.Tables.Count > 0)
                {
                    ddlState.DataSource = ds.Tables[0];
                    ddlState.DataTextField = "Name";
                    ddlState.DataValueField = "ID";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("--Select State--", "-1"));
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.FillStates");
                objErr.SendMail();
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
            FillStates();
            if(!IsPostBack)
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
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ParseQueryString");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.BindRules");
                objErr.SendMail();
            }
        }
        #endregion
    }
}