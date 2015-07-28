using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using FreeTextBoxControls;
using BikeWaleOpr.Common;

namespace BikeWaleOpr.content
{
    /// <summary>
    /// Created : Sadhana Upadhyay on 27th Feb 2014
    /// Summary : Class to add series synopsis
    /// </summary>

    public class SeriesSynopsis : System.Web.UI.Page
    {
        protected string seriesId = String.Empty, series = string.Empty, makeName = string.Empty;
        protected Button btnSave, btnUpdate;
        protected FreeTextBox seriesDesc;
        protected Label lblMessage;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_OnClick);
            this.btnUpdate.Click += new EventHandler(UpdateSeriesSynopsis);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            if (!String.IsNullOrEmpty(Request.QueryString["series"]))
            {
                seriesId = Request.QueryString["series"].ToString();
            }

            if (!IsPostBack)
            {
                GetSeriesSynopsis();
            }
        }

        //Summary : Method to get series name and synopsis 
        private void GetSeriesSynopsis()
        {
            string synopsis = string.Empty;

            ManageBikeSeries ms = new ManageBikeSeries();
            ms.GetSeriesSynopsis(seriesId, ref series, ref synopsis, ref makeName);

            seriesDesc.Text = synopsis;

            if (String.IsNullOrEmpty(synopsis))
            {
                btnSave.Visible = true;
            }
            else
            {
                btnUpdate.Visible = true;
            }
        }

        /// <summary>
        /// Written By : Sadhana Upadhyay on 27th Feb 2014
        /// Summary    : Update bike series description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateSeriesSynopsis(object sender, EventArgs e)
        {
            ManageBikeSeries ms = new ManageBikeSeries();
            ms.ManageSeriesSynopsis(seriesId, seriesDesc.Text);

            lblMessage.Text = "Data updated sucessfully.";
            lblMessage.Visible = true;

            GetSeriesSynopsis();
        }   //End Of UpdateSeriesSynopsis

        /// <summary>
        /// Written By : Sadhana Upadhyay on 27th Feb 2014
        /// Summary    :  method to save bike series description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            ManageBikeSeries ms = new ManageBikeSeries();
            ms.ManageSeriesSynopsis(seriesId, seriesDesc.Text);

            lblMessage.Text = "Data saved sucessfully.";
            lblMessage.Visible = true;

            GetSeriesSynopsis();
        }//End of class btnSave_OnClick

    }//End of class
}// End of namespace