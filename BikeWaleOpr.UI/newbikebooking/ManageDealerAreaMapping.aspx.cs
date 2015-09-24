using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;
using System.Data;
using System.Configuration;
using BikeWaleOpr.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace BikeWaleOpr.NewBikeBooking
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 6 Nov 2014
    /// Summary : to map dealer with area
    /// </summary>
    public class ManageDealerAreaMapping : System.Web.UI.Page
    {
        protected DropDownList ddlCity;
        protected Repeater rptMappedArea, rptUnmappedCity;
        protected Button unmapDealer, MapDealer;
        protected HtmlInputHidden hdnMapArea, hdnUnmapArea;
        protected string cityId = string.Empty;
        protected uint dealerId=0;
        protected List<DealerAreaDetails> objMapping = null;
        protected string abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            //ddlCity.SelectedIndexChanged += new EventHandler(BindArea);
            unmapDealer.Click += new EventHandler(UnmapAreas);
            MapDealer.Click += new EventHandler(MapAreas);
        }

        private void MapAreas(object sender, EventArgs e)
        {
            bool isSuccess = false;
            string arealist = hdnUnmapArea.Value;
            Trace.Warn(arealist);
            string abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            string requestType = "application/json";
            string api = "/api/dealerpricequote/MapDealerWithArea/?dealerid=" + dealerId + "&areaidlist=" + arealist;

            Trace.Warn(abHostUrl + api);
            bool isUpdated = BWHttpClient.PostSync<bool>(abHostUrl, requestType, api, isSuccess);
            hdnUnmapArea.Value = "";
            BindArea();
        }

        private void UnmapAreas(object sender, EventArgs e)
        {
            bool isSuccess = false;
            string arealist = hdnMapArea.Value;
            Trace.Warn(arealist);
            //string abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            string requestType = "application/json";
            string api = "/api/dealerpricequote/UnmapDealerWithArea/?dealerid=" + dealerId + "&areaidlist=" + arealist;

            Trace.Warn(abHostUrl + api);
            bool isUpdated = BWHttpClient.PostSync<bool>(abHostUrl, requestType, api, isSuccess);
            hdnMapArea.Value = "";
            BindArea();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dealerId = Convert.ToUInt32(Request.QueryString["dealerid"]);

            if(!IsPostBack)
            {
            Trace.Warn("DDDDDDDDDs");
            Trace.Warn(ddlCity.SelectedValue);
            GetCity();
            }
            else
                BindArea();
        }

        private  void BindArea()
        {
            try
            {
                cityId = ddlCity.SelectedValue;
                Trace.Warn(cityId);
                //string abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string requestType = "application/json";
                string api = "/api/DealerPriceQuote/GetDealerAreaDetail/?cityid=" + cityId;

                objMapping = BWHttpClient.GetApiResponseSync<List<DealerAreaDetails>>(abHostUrl, requestType, api, objMapping);
                dealerId = Convert.ToUInt32(Request.QueryString["dealerid"]);
                
                if (objMapping != null)
                {
                    var objmappedArea = new List<DealerAreaDetails>();
                    foreach (var citylist in objMapping)
                    {
                        if (citylist.DealerId == dealerId)
                            objmappedArea.Add(citylist);
                    }


                    rptMappedArea.DataSource = objmappedArea;
                    rptMappedArea.DataBind();

                    var objUnMappedCity = new List<DealerAreaDetails>();
                    foreach (var citylist in objMapping)
                    {
                        if (citylist.DealerId != dealerId )
                            objUnMappedCity.Add(citylist);
                    }
                    Trace.Warn("+++", objUnMappedCity.Count.ToString());

                    rptUnmappedCity.DataSource = objUnMappedCity;
                    rptUnmappedCity.DataBind();
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("BindArea Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 6 Nov 2014
        /// Summary : To bind state with dropdown
        /// </summary>
        protected void GetCity()
        {
            try
            {
                ManageCities objCity=new ManageCities();
                DataTable dt = objCity.GetCities(0, "7").Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlCity.DataSource = dt;
                    ddlCity.DataTextField = "Text";
                    ddlCity.DataValueField = "Value";
                    ddlCity.DataBind();

                    ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("objMS.FillStates  ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }//End of GetStates method

        protected string showArea(string areaName,string AreaId, string pinCode, int dealerCount, int dealerRank)
        {
            string retVal = string.Empty;
            if (dealerRank == 1)
            {
                retVal = "<div class='inner-content'>"
                       + "<input type='checkbox' id='chkMappedCity' class='unmapdealers' areaId='" + AreaId + "' dealerCount='" + dealerCount + "' areaName='"+areaName+"' />"
                       + areaName + " - " + pinCode;
                     

                if (dealerCount > 0)
                {
                    retVal += "<a style='float:right;' id='edit_" + AreaId + "' areaId='" + AreaId + "'>View Mapped Dealer( "+dealerCount+" )</a>";
                }

                retVal += "</div>";
            }
        
            return retVal;
        }
    }
}