
using BikeWaleOpr.Common;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace BikewaleOpr.campaign
{
    public class SearchDealerCampaigns : System.Web.UI.Page
    {
        protected DropDownList drpCity, drpDealer;
        private string _requestType = "application/json";
        protected string BwOprHostUrl = ConfigurationManager.AppSettings["BwOprHostUrlForJs"];
        protected string BwHostUrl = ConfigurationManager.AppSettings["BwHostUrlForJs"];

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillCity();
        }

        private async void FillCity()
        {
            try
            {
                string _apiUrl = "/api/Dealers/GetDealerCities/";
                DataTable dt = null;
                dt = await BWHttpClient.GetApiResponse<DataTable>(BwHostUrl, _requestType, _apiUrl, dt);
                if (dt != null)
                {
                    drpCity.DataSource = dt;
                    drpCity.DataTextField = "Text";
                    drpCity.DataValueField = "Value";
                    drpCity.DataBind();
                    drpCity.Items.Insert(0, new ListItem("--Select City--", "0"));
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}