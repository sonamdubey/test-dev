using BikewaleOpr.Common;
using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using BikeWaleOpr.VO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace BikewaleOpr.newbikebooking
{
    /// <summary>
    /// Written By: Aditi Srivastava on 29 Aug 2016
    /// Summary: Create page for manufacturer campaign rules using the campaign id
    /// </summary>
    public class ManufacturerCampaignRules : System.Web.UI.Page
    {
        #region variable
        public int campaignId, currentUserId, makeId;
        List<string> distinctModels = null;
        public DropDownList ddlMake,ddlCity;
        public ListBox ddlModel;
        public Button btnSaveRule, btnDeleteRules;
        public Repeater rptRules;
        public List<MfgCityEntity> AllCities = null;
        public MfgNewRulesEntity MfgAddRules = null;
        public ManufacturerCampaign MfgCampaign = new ManufacturerCampaign();
        public ManageDealerCampaignRule campaign = null;
        public HiddenField hdnSelectedModel, hdnSelectedCities, hdnCheckedRules;
        public Label lblGreenMessage, lblErrorSummary;
        public CheckBox selAllIndia;
        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            currentUserId = Convert.ToInt32(CurrentUser.Id);
            this.Load += new EventHandler(Page_Load);
            btnSaveRule.Click += new EventHandler(SaveRules);
            btnDeleteRules.Click += new EventHandler(DeleteRules);
           
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            if (!IsPostBack)
            {
                selAllIndia.Checked = true;
                campaign = new ManageDealerCampaignRule();
            }
            FillDropDowns();            
            lblGreenMessage.Text = string.Empty;
            lblErrorSummary.Text = string.Empty;
        }

        protected void SaveRules(object sender, EventArgs e)
        {
            MfgAddRules = new MfgNewRulesEntity();
            try
            {
                MfgAddRules.IsAllIndia = selAllIndia.Checked;
                MfgAddRules.UserId = currentUserId;
                MfgAddRules.CampaignId = campaignId;
                MfgAddRules.ModelIds = hdnSelectedModel.Value;
                if (MfgAddRules.IsAllIndia)
                    MfgAddRules.CityIds = "0";
                else
                    MfgAddRules.CityIds = hdnSelectedCities.Value;
                if (MfgCampaign.SaveManufacturerCampaignRules(MfgAddRules))
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.ManufacturerCampaign.SaveRules");
                objErr.SendMail();
            }
        }

        #endregion

        #region functions

       /// <summary>
       /// Created By: Aditi Srivastava on 26 Aug 2016
       /// Description: Set make id
       /// </summary>
        private void SetPageVariables()
        {
               if (!string.IsNullOrEmpty(ddlMake.SelectedValue))
                makeId = Convert.ToInt32(ddlMake.SelectedValue);
           }
        /// <summary>
        /// Created By: Aditi Srivastava on 30 Aug 2016
        /// Description: Set preselected make and models according to campaign id
        /// </summary>
        /// <param name="Rules"></param>
        private void SetSelectedMakeModels(List<MfgCampaignRulesEntity> Rules)
        {
            if (Rules != null)
            {
                try
                {

                    ddlMake.Items.FindByText(Rules.FirstOrDefault() != null ? Rules.FirstOrDefault().MakeName : string.Empty).Selected = true;
                    distinctModels = Rules.GroupBy(s => s.ModelName).Select(s => s.First().ModelName).ToList();
                    FillModels(ddlMake.SelectedItem.Value);
                    if (distinctModels != null)
                    foreach (ListItem item in ddlModel.Items)
                    {
                        if (distinctModels.Contains(item.Text))
                            item.Selected = true;
                    }  
                }
                catch (Exception ex)
                {
                    Trace.Warn(ex.Message);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.ManufacturerCampaign.SetSelectedMakeModels");
                    
                }
                }
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 26 Aug 2016
        /// Description: Populate make dropdown
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
                }
                
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.ManufacturerCampaign.FillMakes");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By: Aditi Srivastava on 30 Aug 2016
        /// Description: Get models according to preselected make
        /// </summary>
        /// <param name="makeId"></param>
        private void FillModels(string makeId)
        {
            try
            {
                DataTable dt;
                MakeModelVersion mmv = new MakeModelVersion();
                dt = mmv.GetModels(makeId,"New");
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlModel.DataSource = dt;
                    ddlModel.DataTextField = "Text"; 
                    ddlModel.DataValueField = "Value"; 
                    ddlModel.DataBind();
                    }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.ManufacturerCampaign.FillMakes");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By: Aditi Srivastava on 26 Aug 2016
        /// Description: Fill cities dropdown
        /// </summary>
        private void FillCities()
        {
            try
            {
                List<MfgCityEntity> cities = new List<MfgCityEntity>();
                cities = MfgCampaign.GetManufacturerCities();
                if (cities != null && cities.Count > 0)
                {
                    ddlCity.DataSource = cities;
                    ddlCity.DataTextField = "CityName";
                    ddlCity.DataValueField = "CityId";
                    ddlCity.DataBind();
                    
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.ManufacturerCampaign.FillCities");
                objErr.SendMail();
            }
        }

        private void FillDropDowns()
        {
            ParseQueryString();
            FillMakes();
            FillCities();
            if (!IsPostBack)
            {
                BindRules();
            }
        }

        /// <summary>
        /// Created By: Aditi Srivastava on 26 Aug 2016
        /// Description: Delete selected rules
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteRules(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdnCheckedRules.Value))
                {
                    MfgCampaign.DeleteManufacturerCampaignRules(currentUserId, hdnCheckedRules.Value);
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
       
        private void ParseQueryString()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["campaignid"]))
                {
                    campaignId = Convert.ToInt32(Request.QueryString["campaignId"]);
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
                List<MfgCampaignRulesEntity> campaignRules = MfgCampaign.FetchManufacturerCampaignRules(campaignId);
                if (campaignRules != null && campaignRules.Count > 0)
                {
                    rptRules.DataSource = campaignRules;
                }
                rptRules.DataBind();
                SetSelectedMakeModels(campaignRules);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.ManufacturerCampaign.BindRules");
                objErr.SendMail();
            }
        }
        #endregion
    }
}