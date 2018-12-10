/*******************************************************************************************************
IN THIS CLASS WE GET THE ID OF THE SELL INQUIRY TABLE, BY THE NAME OF THE QUERYSTRING 'car'. THIS WILL
BE USED TO FETCH ALL THE DETAILS OF THE CAR.
*******************************************************************************************************/
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
using Carwale.Entity.Classified.UsedDealers;
using Carwale.DAL.Classified.UsedDealers;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.Dealer
{
    public class UsedCarDealerShowroom : System.Web.UI.Page
    {

        protected HtmlGenericControl about, liAboutus, liFacilities, divvideo, divabt;
        //protected HtmlAnchor menu2,menu3;
        protected HtmlImage imgmob;//, imgcontact;
        protected Label lblFicilities;
        protected TextBox txtYourMobile, txtCaptcha;
        protected Literal ltAddress1, ltAboutus, ltAddress2, ltCity, ltState, ltPin, ltArea;//ltLandLine
        protected HtmlInputHidden citylatlng, citieslatlng;
        protected Repeater rptStock, rptMakes;
        protected HtmlSelect drpHrs, drpMins, drpMakescount;
        protected HtmlGenericControl iframevideo;
        //protected HtmlIframe iframevideo;

        public string dealerId = "", dealerName = "", dealerCity = "";
        public int sno = 1, _stockCount = 0;
        protected bool videoExists = false;
        protected string dealerWebsite = string.Empty;

        protected Carousel_Home_940x320 corouselHome;

        protected string DealerLogoUrl = string.Empty, DealerHostUrl = string.Empty, FinancerLogo = string.Empty, FinancerHostUrl = string.Empty;

        public string DealerId
        {
            get
            {
                if (ViewState["DealerId"] != null && ViewState["DealerId"].ToString() != "")
                    return ViewState["DealerId"].ToString();
                else
                    return "-1";
            }
            set { ViewState["DealerId"] = value; }
        }

        public int StockCount
        {
            get
            {
                return _stockCount;
            }
            set
            {
                _stockCount = value;
            }
        }

        public string DealerEmail
        {
            get
            {
                if (ViewState["DealerEmail"] != null && ViewState["DealerEmail"].ToString() != "")
                    return ViewState["DealerEmail"].ToString();
                else
                    return "NA";
            }
            set { ViewState["DealerEmail"] = value; }
        }

        public string DealerName
        {
            get
            {
                if (ViewState["DealerName"] != null && ViewState["DealerName"].ToString() != "")
                    return ViewState["DealerName"].ToString();
                else
                    return "";
            }
            set { ViewState["DealerName"] = value; }
        }      

        public string DealerMobile
        {
            get
            {
                if (ViewState["DealerMobile"] != null && ViewState["DealerMobile"].ToString() != "")
                    return ViewState["DealerMobile"].ToString();
                else
                    return "";
            }
            set { ViewState["DealerMobile"] = value; }
        }

        public string DealerAddress
        {
            get
            {
                if (ViewState["DealerAddress"] != null && ViewState["DealerAddress"].ToString() != "")
                    return ViewState["DealerAddress"].ToString();
                else
                    return "";
            }
            set { ViewState["DealerAddress"] = value; }
        }

        public string DealerCity
        {
            get
            {
                if (ViewState["DealerCity"] != null && ViewState["DealerCity"].ToString() != "")
                    return ViewState["DealerCity"].ToString();
                else
                    return "";
            }
            set { ViewState["DealerCity"] = value; }
        }

        public int carscntofdealer
        {
            get
            {
                if (ViewState["carscntofdealer"] != null && ViewState["carscntofdealer"].ToString() != "")
                    return Convert.ToInt32(ViewState["carscntofdealer"]);
                else
                    return 0;
            }
            set { ViewState["carscntofdealer"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }


        void Page_Load(object Sender, EventArgs e)
        {
            if (Request.QueryString["dealerId"] != "" && Request.QueryString["dealerId"] != null)
            {
                DealerId = Request.QueryString["dealerId"];
            }


            if (!IsPostBack)
            {
                ShowDealerDetails();
                GetHrsTable();
                GetMinsTable();
                GetStockCountByMake();
                StockCountofDealer();
                GetAboutDealer(); // Abt Dealer
                corouselHome.GetImages(DealerId);

            }
        } // Page_Load

        public void GetHrsTable()
        {
            DataTable dtHrs = new DataTable();
            DataRow dr;
            dtHrs.Columns.Add("Hrs", typeof(string));
            //
            dr = dtHrs.NewRow();
            dr["Hrs"] = "Hr";
            dtHrs.Rows.Add(dr);
            //
            for (int i = 0; i <= 23; i++)
            {
                dr = dtHrs.NewRow();
                dr["Hrs"] = i.ToString("D2");
                dtHrs.Rows.Add(dr);
            }
            dtHrs.AcceptChanges();
            //return dtHrs;
            drpHrs.DataSource = dtHrs;
            drpHrs.DataTextField = "Hrs";
            drpHrs.DataValueField = "Hrs";
            drpHrs.DataBind();
        }

        // Function to generate a Min DataTable
        public void GetMinsTable()
        {
            DataTable dtMin = new DataTable();
            DataRow dr;
            dtMin.Columns.Add("Min", typeof(string));
            //
            dr = dtMin.NewRow();
            dr["Min"] = "Mn";
            dtMin.Rows.Add(dr);
            //
            for (int i = 0; i <= 59; i++)
            {
                dr = dtMin.NewRow();
                dr["Min"] = i.ToString("D2");
                dtMin.Rows.Add(dr);
            }
            dtMin.AcceptChanges();
            //return dtMin;
            drpMins.DataSource = dtMin;
            drpMins.DataTextField = "Min";
            drpMins.DataValueField = "Min";
            drpMins.DataBind();
        }

        //this fnction checks whether the details of the Dealer is to be shown or not.
        //and the display the result
        void ShowDealerDetails()
        {
            UsedCarDealerDetails dealer = new UsedCarDealerDetailsRepository().GetUsedDealerDetails(DealerId);
            
            DealerEmail = dealer.Email;
            DealerName = dealer.Organization;
            DealerCity = dealer.City;            

            DealerMobile = dealer.Mobile;
           
            DealerAddress = CommonDealers.GetFormattedAddress(
                                    dealer.Address1, 				// address1 dr["Address1"].ToString()
                                    dealer.Address2, 				// address2 dr["Address2"].ToString()
                                    dealer.State,					// state dr["State"].ToString()
                                    dealer.City,					// city
                                    dealer.Area,					// area
                                    dealer.PinCode					// pin dr["Pincode"].ToString()
                                );

            //ltAddress.Text = DealerAddress;

            ltAddress1.Text = dealer.Address1;
            ltAddress2.Text = dealer.Address2;
            ltArea.Text = dealer.Area;
            ltCity.Text = dealer.City;
            ltState.Text = dealer.State;
            ltPin.Text = dealer.PinCode;
            dealerWebsite = dealer.Website;

            citylatlng.Value += dealer.Latitude + "|" + dealer.Longitude;
            citieslatlng.Value += dealer.Latitude + "|" + dealer.Longitude + "|" + DealerName + "|" + dealer.Area + "," + dealer.City + "|" + dealer.MaskNumber + "|" + "$";           
          
        }

        //Modified By : Sadhana Upadhyay on 13 Apr 2015 Added nolock in sql query
        private void StockCountofDealer()
        {
            string sql = "";

            sql = " select COUNT(ProfileId) as StockCount" //Make
                + " FROM livelistings LL "
                + " WHERE LL.SellerType = 1 AND "
                + " LL.InquiryId IN (SELECT ID FROM sellinquiries WHERE DealerId = @DealerId and StatusId=1)";      

            try
            {
                using (DbCommand cmd= DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@DealerId", DbType.Decimal, Decimal.Parse(DealerId)));

                    var ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ClassifiedMySqlReadConnection);
                    //Get Sum of Stock
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        StockCount += Convert.ToInt32(row["StockCount"]);
                    } 
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        // This function will take all the address portions as
        // parameter and will return a complete formatted address.
        private void GetStockCountByMake()
        {

            int intDealerId;
            Int32.TryParse(DealerId, out intDealerId);
            
            try
            {
                var sellInquiry = new DealerSellInquiryRepository().GetSellInquiriesCarMakeForDealer(intDealerId);
                rptMakes.DataSource = sellInquiry;
                rptMakes.DataBind();                

            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        // function to show about dealer, "Avail Exciting Offers", "Avail Facilities " and so on
        private void GetAboutDealer()
        {
            string sql = @" SELECT CategoryId, RebateDescription FROM offersrebates WHERE DealerId = @Id";            
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@Id", DbType.Decimal, Decimal.Parse(DealerId)));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.ClassifiedMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            if (dr["CategoryId"].ToString() == "1")
                            {
                                ltAboutus.Visible = true;
                                ltAboutus.Text = dr["RebateDescription"].ToString();
                            }
                            else if (dr["CategoryId"].ToString() == "2")
                                lblFicilities.Text = dr["RebateDescription"].ToString();
                            else if (dr["CategoryId"].ToString() == "4")
                            {
                                divvideo.Visible = true;
                                iframevideo.Attributes["src"] = "//www.youtube.com/embed/" + dr["RebateDescription"].ToString();
                                videoExists = true;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(ltAboutus.Text) && !videoExists)
                    {
                        if (string.IsNullOrEmpty(lblFicilities.Text))
                        {
                            about.Visible = false;
                            liAboutus.Visible = false;
                            liFacilities.Visible = false;
                        }
                    }

                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }//function

        /// <summary>
        /// New function introduce by Surendra on 7 Dec ,2011
        /// </summary>
        /// <param name="image"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public string GetCarImages(string hostURL, string dirPat, string imgName)
        {
            string str = "";

            if (imgName != "")
                str = "<img class='imgborder' src='http://" + hostURL + dirPat + imgName + "' border='0'></img>";
            else
                str = "<img src='" + ImagingFunctions.GetRootImagePath() + "/images/dealer/nocarimg.gif' border='0'></img>";

            return str;
        }

    } // class
} // namespace