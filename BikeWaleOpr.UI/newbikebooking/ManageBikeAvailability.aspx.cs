using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BikeWaleOpr.Common;
using System.Configuration;
using BikeWaleOpr.Entities;
using System.Threading.Tasks;

namespace BikeWaleOpr.BikeBooking
{
    public class ManageBikeAvailability : System.Web.UI.Page
    {
        protected DropDownList ddlMake, ddlModel, ddlVersions;
        protected TextBox txtdayslimit;
        protected string cwHostUrl = string.Empty;
        protected Button btnsaveData;
        protected HiddenField hdn_ddlMake, hdn_ddlModel, hdn_ddlVersions;
        protected Repeater rptavilableData;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            btnsaveData.Click += new EventHandler(SaveAvailability);
        }

        private void SaveAvailability(object sender, EventArgs e)
        {
            SaveBikeAvailability();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Console.WriteLine("kdjfhkfh" + hdn_ddlMake.Value);
                FillMakes();
                GetBikeAvailability();

            }
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 11th Nov, 2014.
        /// Description : Method To Drop Down Bike Makes.
        /// </summary>
        private void FillMakes()
        {
            try
            {
                DataTable dt;
                MakeModelVersion mmv = new MakeModelVersion();
                dt = mmv.GetMakes("NEW");

                if (dt.Rows.Count > 0)
                {
                    ddlMake.DataSource = dt;
                    ddlMake.DataTextField = "Text";
                    ddlMake.DataValueField = "Value";
                    ddlMake.DataBind();

                    ListItem item = new ListItem("--Select Make--", "0");
                    ddlMake.Items.Insert(0, item);
                    ListItem model = new ListItem("--Select Model--", "0");
                    ddlModel.Items.Insert(0, model);
                    ListItem version = new ListItem("--Select Version--", "0");
                    ddlVersions.Items.Insert(0, version);
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 11th Nov, 2014.
        /// Description : Method To Get Added Bikes Availability By Specific Dealer.
        /// </summary>
        private async void GetBikeAvailability()
        {
            try
            {
                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                // get pager instance

                string _apiUrl = "/api/Dealers/GetBikeAvailability/?dealerId=" + Request.QueryString["dealerId"];
                // Send HTTP GET requests

                Trace.Warn("GetBikeAvailability : API url :" + _apiUrl);

                List<OfferEntity> objAvailibility = null;
                objAvailibility = await BWHttpClient.GetApiResponse<List<OfferEntity>>(cwHostUrl, _requestType, _apiUrl, objAvailibility);

                Trace.Warn("objAvailibility list : ", objAvailibility.Count.ToString());

                if (objAvailibility != null)
                {
                    rptavilableData.DataSource = objAvailibility;
                    rptavilableData.DataBind();
                }

            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 11th Nov, 2014.
        /// Description : Method To Add New Bike Availability By Dealer.
        /// </summary>
        private void SaveBikeAvailability()
        {
            try
            {
                cwHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = "/api/Dealers/SaveBikeAvailability/?dealerId=" + Request.QueryString["dealerId"] + "&bikemodelId=" + hdn_ddlModel.Value + "&bikeversionId=" + hdn_ddlVersions.Value + "&numOfDays=" + txtdayslimit.Text;
         
                Trace.Warn("url : " + cwHostUrl + _apiUrl);
                // Send HTTP GET requests
                bool isSuccess = false;
                bool isDone = BWHttpClient.PostSync<bool>(cwHostUrl, _requestType, _apiUrl, isSuccess);

                GetBikeAvailability();
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