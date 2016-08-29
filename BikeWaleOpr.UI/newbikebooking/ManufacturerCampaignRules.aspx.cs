using BikewaleOpr.Common;
using BikeWaleOpr.Common;
using BikeWaleOpr.VO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace BikewaleOpr.newbikebooking
{
    public class ManufacturerCampaignRules : System.Web.UI.Page
    {
        #region variable
        public int campaignId, dealerId, currentUserId, cityId, stateId, makeId;
        public string modelId;
        public DropDownList ddlMake, ddlModel, ddlState, ddlCity;
        public Button btnSaveRule, btnReset, btnDeleteRules, btnDelete;
        public Repeater rptRules;
        public ManageDealerCampaignRule campaign = null;
        public HiddenField hdnSelectedModel, hdnSelectedCity, hdnCheckedRules;
        public Label lblGreenMessage, lblErrorSummary;
        public CheckBox selAllIndia;
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
            selAllIndia.Checked = true;
            campaign = new ManageDealerCampaignRule();
            FillDropDowns();
            lblGreenMessage.Text = string.Empty;
            lblErrorSummary.Text = string.Empty;
        }
        protected void SaveRules(object sender, EventArgs e)
        {
            try
            {
                if (campaign.InsertBWDealerCampaignRules(currentUserId, campaignId, cityId, dealerId, makeId, stateId, modelId))
                {
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.SaveRules");
                objErr.SendMail();
            }
        }

        #endregion

        #region functions

       
        private void SetPageVariables()
        {
            //if (!string.IsNullOrEmpty(hdnSelectedCity.Value))
            //    cityId = Convert.ToInt32(hdnSelectedCity.Value);
            if (!string.IsNullOrEmpty(hdnSelectedModel.Value))
                modelId = hdnSelectedModel.Value;
            if (!string.IsNullOrEmpty(ddlMake.SelectedValue))
                makeId = Convert.ToInt32(ddlMake.SelectedValue);
           
        }

      
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.FillMakes");
                objErr.SendMail();
            }
        }

      
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

        private void FillCities()
        {
            try
            {
                ManageCities objStates = new ManageCities();
                DataSet ds = objStates.GetMfgCampaignCities();
                if (ds != null && ds.Tables.Count > 0)
                {
                    ddlCity.DataSource = ds.Tables[0];
                    ddlCity.DataTextField = "Name";
                    ddlCity.DataValueField = "ID";
                    ddlCity.DataBind();
                    
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.FillCities");
                objErr.SendMail();
            }
        }

        private void FillDropDowns()
        {
            ParseQueryString();
            FillMakes();
         //   FillStates();
            FillCities();
            if (!IsPostBack)
            {
                BindRules();
            }
        }

       
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