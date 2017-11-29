using BikeWaleOpr.Common;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace BikewaleOpr.NewBikeBooking
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 28 Oct 2015
    /// </summary>
    public class DealerDetailedPriceQuote : System.Web.UI.Page
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
        /// Created By : Sadhana Upadhyay on 28 Oct 2015
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
                ErrorClass.LogError(ex, "BikewaleOpr.NewBikeBooking.DealerDetailedPriceQuote");
                
            }
        }   //End of BindMakes Method
    }   //End of class
}   //End of namespace