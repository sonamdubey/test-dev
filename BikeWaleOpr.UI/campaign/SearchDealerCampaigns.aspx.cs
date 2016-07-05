
using BikewaleOpr.DAL;
using BikewaleOpr.Interface;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
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
        protected string cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];

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
                //sets the base URI for HTTP requests
                // get pager instance
                // Send HTTP GET requests 

                DataTable dt = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    dt = objCity.GetDealerCities();
                }
                //dt = await BWHttpClient.GetApiResponse<DataTable>(cwHostUrl, _requestType, _apiUrl, dt);

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