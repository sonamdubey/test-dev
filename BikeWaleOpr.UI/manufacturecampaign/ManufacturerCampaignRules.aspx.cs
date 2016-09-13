using BikewaleOpr.Common;
using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace BikewaleOpr.manufacturecampaign
{
    /// <summary>
    /// Written By: Aditi Srivastava on 29 Aug 2016
    /// Summary: Create page for manufacturer campaign rules using the campaign id
    /// </summary>
    public class ManufacturerCampaignRules : System.Web.UI.Page
    {
        #region variable
        public string manufactureName;
        public int campaignId, currentUserId, makeId;
        List<string> distinctModels = null;
        public DropDownList ddlMake, ddlCity;
        public ListBox ddlModel;
        public Button btnSaveRule, btnDeleteRules;
        public Repeater rptRules;
        public List<MfgCityEntity> AllCities = null;
        public MfgNewRulesEntity MfgAddRules = null;
        public ManufacturerCampaign MfgCampaign = new ManufacturerCampaign();
        public ManageDealerCampaignRule campaign = null;
        public HiddenField hdnSelectedModel, hdnSelectedCities, hdnCheckedRules;
        public Label lblGreenMessage, lblErrorSummary;
        public CheckBox chkAllIndia;
        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnSaveRule.Click += new EventHandler(SaveMfgCampaignRules);
            btnDeleteRules.Click += new EventHandler(DeleteMfgCampaignRules);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUserId = Convert.ToInt32(CurrentUser.Id);
            ParseQueryString();
            SetPageVariables();
            if (!IsPostBack)
            {
                chkAllIndia.Checked = true;
                campaign = new ManageDealerCampaignRule();
            }
            FillDropDowns();
            lblGreenMessage.Text = string.Empty;
            lblErrorSummary.Text = string.Empty;
        }

        protected void SaveMfgCampaignRules(object sender, EventArgs e)
        {
            MfgAddRules = new MfgNewRulesEntity();
            int _rowsInserted;
            try
            {
                MfgAddRules.IsAllIndia = chkAllIndia.Checked;
                MfgAddRules.UserId = currentUserId;
                MfgAddRules.CampaignId = campaignId;
                MfgAddRules.ModelIds = hdnSelectedModel.Value;
                if (MfgAddRules.IsAllIndia)
                    MfgAddRules.CityIds = "0";
                else
                    MfgAddRules.CityIds = hdnSelectedCities.Value;
                _rowsInserted = MfgCampaign.SaveManufacturerCampaignRules(MfgAddRules);
                if (_rowsInserted > 0)
                {
                    lblGreenMessage.Text = "Rule(s) have been added !";
                }
                else if (_rowsInserted < 0)
                {
                    lblErrorSummary.Text = "Some error occurred";
                }
                else
                {
                    lblErrorSummary.Text = "Rule(s) already exist";
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
        private void SetSelectedMakeModels(IEnumerable<MfgCampaignRulesEntity> Rules)
        {

            try
            {
                if (Rules != null)
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

            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.ManufacturerCampaign.SetSelectedMakeModels");

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
                    ddlMake.Items.Insert(0, new ListItem("-- Select Make --", "0"));
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
                dt = mmv.GetModels(makeId, "New");
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
                IEnumerable<MfgCityEntity> cities = new List<MfgCityEntity>();
                cities = MfgCampaign.GetManufacturerCities();
                if (cities != null && cities.Count() > 0)
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
        private void DeleteMfgCampaignRules(object sender, EventArgs e)
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
                if (campaignId == 0 || string.IsNullOrEmpty(Request.QueryString["campaignid"]))
                {
                    Response.Redirect("/manufacturecampaign/SearchManufacturerCampaign.aspx");
                }
                if (!string.IsNullOrEmpty(Request.QueryString["manufactureName"]))
                {
                    manufactureName = Request.QueryString["manufactureName"].ToString();
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
                IEnumerable<MfgCampaignRulesEntity> campaignRules = MfgCampaign.FetchManufacturerCampaignRules(campaignId);
                if (campaignRules != null && campaignRules.Count() > 0)
                {
                    rptRules.DataSource = campaignRules;
                    rptRules.DataBind();
                    SetSelectedMakeModels(campaignRules);
                }
                else
                {
                    FillCities();
                    FillMakes();
                    ddlModel.Items.Clear();
                    ddlModel.Items.Insert(0, "--Select Models--");


                }

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