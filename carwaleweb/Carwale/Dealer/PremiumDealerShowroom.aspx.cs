using System;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.Utility;
using System.Configuration;
using System.ComponentModel;
using Carwale.DAL.Classified.UsedDealers;
using Carwale.Entity.Classified.UsedDealers;

namespace Carwale.UI.Dealer
{
    public class PremiumDealerShowroom : System.Web.UI.Page
    {
        public string dealerId =string.Empty, dealerName =string.Empty, dealerCity =string.Empty;
        protected Repeater rptStock, rptMakes;
        protected string dealerEmail = string.Empty, dealerMobile = string.Empty, dealerMaskNumber = string.Empty , dealerAddress1 = string.Empty, dealerAddress2 = string.Empty,
                         dealerState = string.Empty, dealerArea = string.Empty, dealerPincode = string.Empty, lattitude = string.Empty, longitude = string.Empty,
                         dealerWebsiteUrl = string.Empty;
        protected int StockCount = 0;

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["dealerId"] != "" && Request.QueryString["dealerId"] != null)
            {
               dealerId = Request.QueryString["dealerId"];
            }
           
            if(!IsPostBack)
            {
                if(!string.IsNullOrEmpty(dealerId))
                { 
                    GetStockDetails(dealerId);
                    GetDealerDetails(dealerId);
                }
            }
        } //PageLoad

        //Getting stock details from db
       void GetStockDetails(string dealerId)
        {
            try
            {
                int intDealerId;
                Int32.TryParse(dealerId, out intDealerId);
                PremiumDealerRepository dealerCity = new PremiumDealerRepository();
                var retList = dealerCity.GetDealerForCity(intDealerId);

                if (retList.Count > 0)
                {
                    rptStock.DataSource = retList;
                    rptStock.DataBind();
                }
                else
                {
                    rptStock.Visible = false;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : GetStockDetails");
                objErr.SendMail();
            } 
        }

        /// <summary>
        /// Getting Dealer deatails for Address content on the page
        /// </summary>
        /// <param name="dealerId">The Dealer Id</param>
        void GetDealerDetails(string dealerId)
        {
            //Fetching the dealer details from the dealer details object
            UsedCarDealerDetails dealer = new UsedCarDealerDetailsRepository().GetUsedDealerDetails(dealerId);

            if (dealer != null)
            {
                dealerEmail = dealer.Email;
                dealerName = dealer.Organization;
                dealerCity = dealer.City;              
                dealerMobile = dealer.Mobile;
                dealerMaskNumber = dealer.MaskNumber;
                dealerAddress1 = dealer.Address1;
                dealerAddress2 = dealer.Address2;
                dealerArea = dealer.Area;
                dealerState = dealer.State;
                dealerPincode = dealer.PinCode;
                dealerWebsiteUrl = dealer.Website;

                //Lattitude and longitude Used for google map
                lattitude = dealer.Latitude;
                longitude = dealer.Longitude;
            }
        }
    }
}