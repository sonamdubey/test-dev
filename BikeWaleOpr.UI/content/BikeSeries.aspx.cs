using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    public class BikeSeries : System.Web.UI.Page
    {
        protected Button btnSave;
        protected DropDownList cmbMakes;
        protected Repeater rptSeries;
        protected TextBox txtMaskingName, txtSeriesName;
        protected Label lblStatus;
        protected HtmlGenericControl spnKeyErr;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();    
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(SaveBikeSeries);
            cmbMakes.SelectedIndexChanged += new EventHandler(CmdMakes_IndexChanged);
        }

        private void CmdMakes_IndexChanged(object sender, EventArgs e)
        {
            GetAllSeries();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
       
            if (!IsPostBack)
            {
                GetMakes();
            }
            spnKeyErr.InnerText = "";
        }
        
        /// <summary>
        /// Written By : Ashwini Todkar on 24 Feb 2014
        /// Summary    : function to add new make series and also update series details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveBikeSeries(object sender, EventArgs e)
        {
            try
            {
                ManageBikeSeries ms = new ManageBikeSeries();
                if (ms.SaveSeries(txtSeriesName.Text, txtMaskingName.Text, cmbMakes.SelectedValue))
                {

                    GetAllSeries();
                    txtMaskingName.Text = string.Empty;
                    txtSeriesName.Text = string.Empty;
                    spnKeyErr.InnerText = "";
                }
                else
                {
                    spnKeyErr.InnerText = "Series name or series masking name already exists. Can not insert duplicate.";
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("Exception in SaveBikeSeries : ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetAllSeries()
        {
            try
            {
                ManageBikeSeries ms = new ManageBikeSeries();                
                
                DataSet ds = ms.GetSeries(cmbMakes.SelectedValue);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    rptSeries.DataSource = ds;
                    rptSeries.DataBind();
                    lblStatus.Text = "";
                }
                else
                {
                    rptSeries.DataSource = null;
                    rptSeries.DataBind();
                    lblStatus.Text = "Oops no series available for " + cmbMakes.SelectedItem.Text + " !";
                }
            }
            catch(Exception ex)
            {
                Trace.Warn("Exception in GetAllSeries : ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Feb 2014
        /// Summary    : function to get all bike makes
        /// </summary>
        private void GetMakes()
        {
            DataTable dt;
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();
                dt = mmv.GetMakes("All");

                cmbMakes.DataSource = dt;
                cmbMakes.DataValueField = "value";
                cmbMakes.DataTextField = "text";
                cmbMakes.DataBind();

                cmbMakes.Items.Insert(0, new ListItem("--Select Make--", "0"));
            }
            catch (Exception ex)
            {
                Trace.Warn("Exception in GetMakes : ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
        } 
    }
}