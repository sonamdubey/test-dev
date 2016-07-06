using BikewaleOpr.DALs;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
        protected uint dealerId = 0;
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
            string arealist = hdnUnmapArea.Value;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                objPriceQuote.MapDealerWithArea(dealerId, arealist);
            }
            hdnUnmapArea.Value = string.Empty;
            BindArea();
        }

        private void UnmapAreas(object sender, EventArgs e)
        {
            string arealist = hdnMapArea.Value;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                objPriceQuote.UnmapDealer(dealerId, arealist);
            }
            hdnMapArea.Value = string.Empty;
            BindArea();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dealerId = Convert.ToUInt32(Request.QueryString["dealerid"]);

            if (!IsPostBack)
            {
                GetCity();
            }
            else
                BindArea();
        }

        private void BindArea()
        {
            try
            {
                cityId = ddlCity.SelectedValue;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                    objMapping = objPriceQuote.GetDealerAreaDetails(Convert.ToUInt32(cityId));
                }
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
                        if (citylist.DealerId != dealerId)
                            objUnMappedCity.Add(citylist);
                    }
                    rptUnmappedCity.DataSource = objUnMappedCity;
                    rptUnmappedCity.DataBind();
                }
            }
            catch (Exception ex)
            {
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
                ManageCities objCity = new ManageCities();
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

        protected string showArea(string areaName, string AreaId, string pinCode, int dealerCount, int dealerRank)
        {
            string retVal = string.Empty;
            if (dealerRank == 1)
            {
                retVal = "<div class='inner-content'>"
                       + "<input type='checkbox' id='chkMappedCity' class='unmapdealers' areaId='" + AreaId + "' dealerCount='" + dealerCount + "' areaName='" + areaName + "' />"
                       + areaName + " - " + pinCode;


                if (dealerCount > 0)
                {
                    retVal += "<a style='float:right;' id='edit_" + AreaId + "' areaId='" + AreaId + "'>View Mapped Dealer( " + dealerCount + " )</a>";
                }

                retVal += "</div>";
            }

            return retVal;
        }
    }
}