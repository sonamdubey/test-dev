using BikeWaleOpr.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 26 dec 2014
    /// </summary>
    public class States : Page
    {
        protected Repeater rptStates;
        protected TextBox txtState, txtStdCode, txtMaskingName;
        protected Button btnSave, btnUpdate;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(SaveState);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = string.Empty;
            if (!IsPostBack)
            {
                GetAllStates();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 2nd jan 2914
        /// summary    : method retrives state name,masking name and state code and state id
        /// </summary>
        void GetAllStates()
        {
            try
            {
                ManageStates objMS = new ManageStates();
                DataSet ds = objMS.GetAllStatesDetails();

                rptStates.DataSource = ds;
                rptStates.DataBind();
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
        }//End of GetAllStates method

        /// <summary>
        /// written by : Ashwini Todkar on 2nd jan 2014
        /// summary    : insert state details in BWStates table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveState(object sender, EventArgs e)
        {
            try
            {
                ManageStates objStates = new ManageStates();

                objStates.ManageStateDetails("-1", txtState.Text, txtMaskingName.Text, txtStdCode.Text.ToUpper());

                GetAllStates();
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("ManageStates  ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ManageStates ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }//End of SaveState method
    }//End of States class
}