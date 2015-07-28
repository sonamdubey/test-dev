using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BikeWaleOpr.Common;
using BikeWaleOpr.VO;
using System.Collections.Specialized;

namespace BikeWaleOpr.Content
{
    /// <summary>
    /// created By : Ashwini todkar on 2nd Jan 2013
    /// </summary>
    public class UpdateCityDetails : Page
    {
        protected DropDownList ddlStates;
        protected TextBox txtCity, txtStdCode, txtMaskingName, txtLatitude, txtLongitude, txtPin;
        protected Button btnUpdate;        

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            string cityId = string.Empty; 
            if (!IsPostBack)
            {
                ViewState["PreviousPage"] = Request.UrlReferrer;
               
                GetStates();
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    cityId = Request.QueryString["id"].ToString();
                    Trace.Warn("city Id : " + cityId);
                    GetCityDetails(cityId);
                }
            }
        }

        /// <summary>
        /// Written By : Ashwini todkar on 2nd jan 2014
        /// summary    : method updates city details masking name, name, lattitude,longitude,pin on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {               
                ManageCities objMC = new ManageCities();
                City objCity = new City();

                if (!string.IsNullOrEmpty(Request.QueryString["id"].ToString()))
                {
                    objCity.CityId = Request.QueryString["id"].ToString();
                    objCity.CityName = txtCity.Text.Trim();
                    objCity.MaskingName = txtMaskingName.Text.Trim();
                    objCity.Lattitude = txtLatitude.Text.Trim();
                    objCity.Longitude = txtLongitude.Text.Trim();
                    objCity.StdCode = txtStdCode.Text.Trim();
                    objCity.DefaultPinCode = txtPin.Text.Trim();
                    objCity.StateId = ddlStates.SelectedItem.Value;
                    objMC.ManageCityDetails(objCity);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("objState.ManageCityDetails  : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (ViewState["PreviousPage"] != null)
                {
                    Uri prevPageUri = (Uri)ViewState["PreviousPage"];
                    string queryString = prevPageUri.AbsoluteUri;

                    if (queryString.IndexOf("?") > 0)
                    {
                        queryString = queryString.Split('?')[0];                        
                    }

                    queryString += "?stateid=" + ddlStates.SelectedValue;

                    Response.Redirect(queryString);
                }
            }
        }//End of btnUpdate_Click

        /// <summary>
        /// Written By : Ashwini Todkar on 2nd jan 2014
        /// summary    : method retrievs city name,stdcode,pincode,lattitude,masking name,longitude ,state id
        /// </summary>
        /// <param name="cityId"></param>
        protected void GetCityDetails(string cityId)
        {
            City objCity = new City();
            ManageCities objMC = new ManageCities();
            objCity = objMC.GetCityDetails(cityId);

            txtCity.Text = objCity.CityName;
            txtMaskingName.Text = objCity.MaskingName;
            txtLatitude.Text = objCity.Lattitude;
            txtLongitude.Text = objCity.Longitude;
            txtPin.Text = objCity.DefaultPinCode;
            txtStdCode.Text = objCity.StdCode;
            //Trace.Warn("txtStdCode.Text " + txtStdCode.Text);
            ddlStates.SelectedValue = objCity.StateId;
        }//End of GetCityDetails

        /// <summary>
        /// Written By : Aswhini Todkar on 2nd jan 2014
        /// summary    : retrieves state id as value and state name as text
        /// </summary>
        protected void GetStates()
        {
            ManageStates objMS = new ManageStates();
            DataTable dt = objMS.FillStates();

            try
            {
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
    }//End of  UpdateCityDetails class
}