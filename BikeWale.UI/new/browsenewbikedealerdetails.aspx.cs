using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;
using Bikewale.Memcache;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.New
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 10/8/2012
    ///     Class to show the bike dealers details
    /// </summary>
    public class BrowseNewBikeDealerDetails : Page
    {
        protected HtmlGenericControl spnError;
        protected Label lblResults1;
        protected HtmlTable tblAdvanced;
        protected Panel pnlAd, pnlResult;
        protected DataList dlDealers;
        protected Bikewale.Controls.NewBikeLaunches ctrl_NewBikeLaunches;

        protected string strCity = "", makeName = "", makeId = "", cityId = "", MakeMaskingName = string.Empty;
        public int dealerCount = 0, city = 0;

        public string BackUrl
        {
            get
            {
                if (ViewState["BackUrl"] != null)
                    return ViewState["BackUrl"].ToString();
                else
                    return "";
            }
            set { ViewState["BackUrl"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (!String.IsNullOrEmpty(Request.QueryString["cityId"]) || !int.TryParse(Request.QueryString["cityId"], out city))
            {
                cityId = Request.QueryString["cityId"];
            }

            if (Request.QueryString["make"] != null && Request.QueryString["make"] != "")
            {
                makeId = MakeMapping.GetMakeId(Request.QueryString["make"].ToLower());
            }

            if (!IsPostBack)
            {
                if (makeId != "")
                {
                    BackUrl = "/New/ListNewBikeDealersByCity-" + makeId + ".html";
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            Trace.Warn(makeId);
            if(!string.IsNullOrEmpty(makeId))
                BindDataGrid();

            ctrl_NewBikeLaunches.PQSourceId = (int)PQSourceEnum.Desktop_LocateDealer_NewLaunches;
        }

        private void BindDataGrid()
        {
            string sql;

            CommonOpn objCom = new CommonOpn();

            sql = " SELECT DNC.Name AS DealerName, DNC.Address, DNC.PinCode, DNC.ContactNo,"
                + " DNC.FaxNo, DNC.EMailId, DNC.WebSite, DNC.WorkingHours,"
                + " CMA.Name AS BikeMake, CMA.MaskingName AS MakeMaskingName, C.Name AS City, S.Name AS State"

                + " FROM Dealer_NewBike AS DNC, "
                + " BWCities AS C, BWStates AS S, BikeMakes AS CMA With(NoLock) "

                + " WHERE DNC.MakeId = CMA.ID AND DNC.CityId = C.ID AND C.StateId = S.Id"
                + " AND DNC.CityId = @CityId AND DNC.MakeId = @MakeId "
                + " AND DNC.IsActive = 1 AND C.IsDeleted = 0 AND S.IsDeleted = 0 AND CMA.IsDeleted = 0"
                + " ORDER BY DealerName";

            Trace.Warn(sql);

            SqlParameter[] param = { 
				new SqlParameter("@MakeId", makeId),
				new SqlParameter("@CityId", cityId)						
			};

            Database db = new Database();
            SqlDataReader dr = null;

            try
            {
                objCom.BindListReader(sql, dlDealers, param);

                if (dlDealers.Items.Count == 0)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                else
                {
                    dealerCount = dlDealers.Items.Count;

                    dr = db.SelectQry(sql, param);
                    if (dr.Read())
                    {
                        makeName = dr["BikeMake"].ToString();
                        MakeMaskingName = dr["MakeMaskingName"].ToString();
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                db.CloseConnection();
            }
        }

        // function to return formatted address 
        public string GetAddress(string address, string area, string city, string state, string pinCode)
        {
            string add = "";

            strCity = city;

            if (address != "")
                add = "<strong>Address :</strong> " + address;

            if (area != "")
            {
                if (add == "")
                    add = area;
                else
                    add += ",<br>" + area;
            }

            if (city != "")
            {
                if (add == "")
                    add = city;
                else
                    add += ",<br>" + city;
            }

            if (state != "")
            {
                if (add == "")
                    add = state;
                else
                    add += ", " + state;
            }

            if (pinCode != "" && pinCode != "0")
            {
                if (add == "")
                    add = pinCode;
                else
                    add += " - " + pinCode;
            }
            if (add != "")
            {
                add = "<p>" + add + "</p>";
            }
            return add;
        }

        //function to return formatted contact numbers.
        public string GetContactNumbers(string tel, string mobile, string fax)
        {
            string number = "";

            if (tel != "")
                number = "<strong>Phone : </strong>" + tel;

            if (mobile != "")
            {
                if (number == "")
                    number = "" + mobile;
                else
                    number += ", " + mobile;
            }


            if (fax != "")
            {
                if (number == "")
                    number = "<br><strong>Fax : </strong>" + fax;
                else
                    number += "<br><strong>Fax : </strong>" + fax;
            }
            if (number != "")
            {
                number = "<p>" + number + "</p>";
            }
            return number;
        }

        //function to formatted return url
        public string GetWebsite(string url)
        {
            string website = "";

            if (url != "")
            {
                website = "<p><strong>Website</strong> : <a href='http://" + url.Replace("http://", "") + "' target='new'>" + url.Replace("http://", "") + "</a></p>";
            }

            return website;
        }

        //function to get formatted email address
        public string GetEmail(string email1, string email2)
        {
            string email = "";

            if (email1 != "" && email2 != "")
                email = "<strong>Email :</strong> " + email1 + ", " + email2;
            else if (email1 != "")
                email = "<strong>Email :</strong> " + email1;
            else if (email1 != "")
                email = "<strong>Email :</strong> " + email2;
            if (email != "")
            {
                email = "<p>" + email + "</p>";
            }
            return email;
        }

    }   // End of class
}   // End of namespace