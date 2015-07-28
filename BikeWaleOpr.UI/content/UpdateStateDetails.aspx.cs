using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BikeWaleOpr.Common;
using BikeWaleOpr.VO;

namespace BikeWaleOpr.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 2nd Jan 2014
    /// </summary>
    public class UpdateStateDetails : Page
    {
        protected TextBox txtState, txtStdCode, txtMaskingName;
        protected Button btnUpdate;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnUpdate.Click += new EventHandler(UpdateStateDetail);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            string StateId = string.Empty;
            if (!IsPostBack)
            {
                ViewState["PreviousPage"] = Request.UrlReferrer;

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    StateId = Request.QueryString["id"].ToString();
                    GetStateDetails(StateId);
                }
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 2nd jan 2014
        /// summary    : retrieves state name,masking name,std code
        /// </summary>
        /// <param name="stateId"></param>
        protected void GetStateDetails(string stateId)
        {
            State objState = null;

            try
            {                
                ManageStates objMS = new ManageStates();

                objState = objMS.GetStateDetails(stateId);

                txtState.Text = objState.StateName;
                txtMaskingName.Text = objState.MaskingName;
                txtStdCode.Text = objState.StdCode;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("objState.GetStateDetails sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("objState.GetStateDetails ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }//End of GetStateDetails method

        /// <summary>
        /// Written by : Ashwini Todkar on 2nd jan 2014
        /// summary    : update state name,masking name,std code on update button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateStateDetail(object sender, EventArgs e)
        {
            try
            {
                ManageStates objMS = new ManageStates();
                objMS.ManageStateDetails(Request.QueryString["id"].ToString(), txtState.Text, txtMaskingName.Text, txtStdCode.Text.ToUpper());                          
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("objState.ManageStateDetails  : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("objState.ManageStateDetails  : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (ViewState["PreviousPage"] != null)
                {
                    Response.Redirect(ViewState["PreviousPage"].ToString());
                }
            }
        }//End of updateState method
    }//End of UpdateStateDetails class
}