using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikewaleOpr.newbikebooking
{
    public class ManageDealerCampaigns : System.Web.UI.Page
    {
        protected DropDownList ddlMake;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);   
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
               BindMakes();
            }
            
        }

        /// <summary>
        /// Created By : Sushil Kumar on 29th January 2016
        /// Summmary : To bind Make DropDown
        /// </summary>
        private void BindMakes()
        {
            DataTable dt = null;
            string requestType = "PRICEQUOTE";
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();

                dt = mmv.GetMakes(requestType);

                ddlMake.DataSource = dt;
                ddlMake.DataValueField = "Value";
                ddlMake.DataTextField = "TEXT";
                ddlMake.DataBind();
                ddlMake.Items.Insert(0, new ListItem("--Select Make--", "0"));
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.NewBikeBooking.DealerDetailedPriceQuote");
                objErr.SendMail();
            }
        } 
    }
}