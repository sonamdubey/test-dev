using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BikeWaleOpr.Common;
using System.Data;

namespace BikeWaleOpr.Content
{
    public class UpdateSeriesDetails : System.Web.UI.Page
    {
        protected Button btnUpdate;
        protected TextBox txtSeriesName, txtMaskingName;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            //btnUpdate.Click += new EventHandler(UpdateSeriesDetail);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string seriesId = string.Empty;
            if (!IsPostBack)
            {
                ViewState["PreviousPage"] = Request.UrlReferrer;

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    seriesId = Request.QueryString["id"].ToString();
                    Trace.Warn("seriesId  : " + seriesId);
                    GetSriesDetails(seriesId);
                }
            }

        }
        protected void UpdateSeriesDetail(object sender, EventArgs e)
        {
            try
            {
                ManageBikeSeries ms = new ManageBikeSeries();
                if (! ms.UpdateSeries(txtSeriesName.Text.ToString(), txtMaskingName.Text.ToString(), Request.QueryString["id"].ToString()))
                {
                    string script = "alert('Series name or series masking already exists. Can not insert duplicate name');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
                }
            }
            catch(Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdateSeriesDetail  ex : " + ex.Message + ex.Source);
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

                    queryString += "?id=" + Request.QueryString["id"];

                    Response.Redirect(queryString);
                }

            }
 
        }
        void GetSriesDetails(string seriesId)
        {
            DataSet ds = null;
            try
            {
                ManageBikeSeries ms = new ManageBikeSeries();

                ds = ms.GetSeriesDetails(seriesId);

                txtMaskingName.Text = ds.Tables[0].Rows[0]["SeriesMaskingName"].ToString();
                txtSeriesName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetSriesDetails  ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}