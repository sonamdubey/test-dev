using BikewaleOpr.common;
using BikeWaleOpr.Common;
using FreeTextBoxControls;
using System;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.content
{
    /// <summary>
    /// Created : Ashwini Todkar on 20 Feb 2014
    /// Summary : Class to add make synopsis
    /// </summary>

    public class MakeSynopsis : System.Web.UI.Page
    {
        protected string makeId = String.Empty, make = string.Empty;
        protected Button btnSave, btnUpdate;
        protected FreeTextBox makeDescription;
        protected Label lblMessage;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_OnClick);
            this.btnUpdate.Click += new EventHandler(UpdateMakeSynopsis);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            
            if (!String.IsNullOrEmpty(Request.QueryString["make"]))
            {
                makeId = Request.QueryString["make"].ToString();                
            }

            if (!IsPostBack)
            {                
                GetMakeSynopsis();
            }
        }

        //Summary : Method to get make name and synopsis 
        private void GetMakeSynopsis()
        {
            string synopsis = string.Empty;

            MakeModelVersion mmv = new MakeModelVersion();
            mmv.GetMakeSynopsis(makeId, ref make, ref synopsis);
            Trace.Warn(synopsis);
            makeDescription.Text = synopsis;

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
        /// Written By : Ashwini Todkar 0n 20 Feb 2014
        /// Summary    : Update bike make description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateMakeSynopsis(object sender, EventArgs e)
        {
            MakeModelVersion mmv = new MakeModelVersion();
            mmv.ManageMakeSynopsis(makeId,makeDescription.Text);

            lblMessage.Text = "Data updated sucessfully.";
            lblMessage.Visible = true;

            GetMakeSynopsis();

            //Refresh memcache object for bikemake description change
            MemCachedUtility.Remove(string.Format("BW_MakeDescription_{0}", makeId));
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 20 Feb 2014
        /// Summary    :  method to save bike make description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            MakeModelVersion mmv = new MakeModelVersion();
            Trace.Warn("entered syn");
            mmv.ManageMakeSynopsis(makeId, makeDescription.Text);

            lblMessage.Text = "Data saved sucessfully.";
            lblMessage.Visible = true;

            GetMakeSynopsis();

            //Refresh memcache object for bikemake description change
            MemCachedUtility.Remove(string.Format("BW_MakeDescription_{0}", makeId));
        }
        
    }//End of class MakeSynopsis
}