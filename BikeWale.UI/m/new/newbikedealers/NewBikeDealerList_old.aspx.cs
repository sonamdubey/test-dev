using Bikewale.BAL.Dealer;
using Bikewale.Common;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.New
{
    /// <summary>
    /// Created By : Ashwini Todkar on 5 June 2014
    /// </summary>
    public class NewBikeDealerList_Old : System.Web.UI.Page
    {
        protected Repeater rptDealers;
        protected string make = String.Empty, city = String.Empty, canonicalUrl = String.Empty;
        protected int dealerCount = 0;
        uint makeId = 0, cityId = 0;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                if (ProcessQS())
                {
                    MakeModelVersion objMMV = new MakeModelVersion();

                    objMMV.GetMakeDetails(makeId.ToString());
                    make = objMMV.BikeName;
                    GetDealerDetailsList(makeId, cityId);
                }
                else
                {
                    UrlRewrite.Return404();

                }
            }
        }

        private bool ProcessQS()
        {
            bool isSuccess = true;

            if (!String.IsNullOrEmpty(Request.QueryString["make"]) && !String.IsNullOrEmpty(Request.QueryString["city"]))
            {
                if (!UInt32.TryParse(Request.QueryString["city"], out cityId))
                {
                    isSuccess = false;
                }

                if (!String.IsNullOrEmpty(MakeMapping.GetMakeId(Request.QueryString["make"].ToLower())))
                    makeId = Convert.ToUInt32(MakeMapping.GetMakeId(Request.QueryString["make"].ToLower()));
                else
                {
                    isSuccess = false;
                }
            }
            else
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        private void GetDealerDetailsList(uint makeId, uint cityId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealer, Dealer>();

                    IDealer objDealer = container.Resolve<IDealer>();

                    List<NewBikeDealerEntity> objDealerList = objDealer.GetDealersList(makeId, cityId);

                    dealerCount = objDealerList.Count;

                    if (objDealerList.Count > 0)
                    {
                        rptDealers.DataSource = objDealerList;
                        rptDealers.DataBind();
                    }
                    else
                    {
                        Response.Redirect("/m/new-bikes-in-india/", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn("Exception in GetDealerDetailsList() " + err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        protected string GetDealerCity(string cityName)
        {
            city = cityName;
            canonicalUrl = "https://www.bikewale.com/m/new/" + Request.QueryString["make"] + "-" + "dealers/" + Request.QueryString["city"] + "-" + city.ToLower() + ".html";
            return "";
        }

        // function to return formatted address 
        protected string GetFormattedAddress(string address, string city, string state, string pinCode)
        {
            string add = "";

            if (address != "")
                add = "Address : " + address;

            if (city != "")
            {
                if (add == "")
                    add = city;
                else
                    add += "<br>" + city;
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
    }
}