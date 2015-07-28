using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.VO;

namespace BikeWaleOpr.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 27th dec 2013
    /// </summary>
    public class Cities : System.Web.UI.Page
    {
        protected Button btnSave;
        protected DropDownList ddlStates;
        protected TextBox txtCity, txtStdCode, txtMaskingName, txtLatitude, txtLongitude, txtPin;
        protected Repeater rptCities;
        protected HtmlGenericControl spnState;
        protected HiddenField hdn_stateId;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            ddlStates.SelectedIndexChanged += new EventHandler(DdlStates_SelectedIndexChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetStates();

                string stateId = Request.QueryString["stateid"];

                if (!String.IsNullOrEmpty(stateId))
                {
                    ddlStates.SelectedValue = stateId;
                    GetAllCitiesDetails(stateId);
                }
            }
        }

        protected void DdlStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            string stateId = ddlStates.SelectedItem.Value;

            if (!string.IsNullOrEmpty(stateId) && stateId != "0")
                GetAllCitiesDetails(stateId);
            else
            {
                rptCities.DataSource = null;
                rptCities.DataBind();
            }
         }

        /// <summary>
        /// Written By : Ashwini Todkar on 2nd jan 2014
        /// summary    : method adds city in database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {             
                ManageCities objMC = new ManageCities();
                City objCity = new City();

                objCity.CityId = "-1";
                objCity.CityName = txtCity.Text.Trim();
                objCity.MaskingName = txtMaskingName.Text.Trim();
                objCity.Lattitude = txtLatitude.Text.Trim();
                objCity.Longitude = txtLongitude.Text.Trim();
                objCity.StdCode = txtStdCode.Text.Trim();
                objCity.DefaultPinCode = txtPin.Text.Trim();
                objCity.StateId = ddlStates.SelectedItem.Value;
                objMC.ManageCityDetails(objCity);
                GetAllCitiesDetails(objCity.StateId);
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("ManageCities  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ManageCities ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// wriiten By : Ashwini Todkar on 2nd jan 2013
        /// summary    : retrieves all cities and its detail of particular state
        /// </summary>
        /// <param name="stateId"></param>
        protected void GetAllCitiesDetails(string stateId)
        {           
            DataSet ds = null;

            try
            {
                Database db = new Database();
                ManageCities objMC = new ManageCities();

                ds = objMC.GetAllCitiesDetails(stateId);

                rptCities.DataSource = ds;
                rptCities.DataBind();    
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }//End of GetAllCitiesDetails

        /// <summary>
        /// Written By : Ashwini Todkar on 2nd jan 2014
        /// summary    : method retrieves all states id as value and name as text
        /// </summary>
        protected void GetStates()
        {    
            try
            {
                ManageStates objMS = new ManageStates();
                DataTable dt = objMS.FillStates();
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlStates.DataSource = dt;
                    ddlStates.DataTextField = "Text";
                    ddlStates.DataValueField = "Value";
                    ddlStates.DataBind();

                    ddlStates.Items.Insert(0, new ListItem("--Select State--", "0"));
                    ddlStates.Items[0].Attributes.Add("disabled", "disabled");
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("objMS.FillStates  ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }//End of GetStates method
    }//End of Cities class
}