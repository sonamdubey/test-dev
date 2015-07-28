using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BikeWaleOpr.Entities;

namespace BikeWaleOpr.NewBikeBooking
{
    public class ManageDealerFacilities : System.Web.UI.Page
    {
        protected Repeater rptFacilities;
        protected Button btnAddFacility, btnUpdateFacility;
        protected TextBox txtFacility;
        protected CheckBox chkIsActiveFacility;
        protected HiddenField hdnFacilityId;

        protected int dealerId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnAddFacility.Click += new EventHandler(AddFacility);
            btnUpdateFacility.Click += new EventHandler(UpdateFacility);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["dealerId"] != null)
            {   
                int.TryParse(Request.QueryString["dealerId"].ToString(), out dealerId);
            }

            if(!IsPostBack)
            {
                if(dealerId > 0)
                    GetFacilities();                
            }
        }

        protected async void GetFacilities()
        {
            string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            string _requestType = "application/json";

            List<FacilityEntity> objFacilities = null;
            
            string _apiUrl = "/api/Dealers/GetDealerFacilities/?dealerId=" + dealerId;
            // Send HTTP GET requests 

            objFacilities = await BWHttpClient.GetApiResponse<List<FacilityEntity>>(_abHostUrl, _requestType, _apiUrl, objFacilities);

            if (objFacilities != null)
            {
                    rptFacilities.DataSource = objFacilities;
                    rptFacilities.DataBind();

                Trace.Warn("GetFacilities bind data");
            }
        }

        protected void AddFacility(object sender, EventArgs e)
        {
            if(dealerId > 0)
            {
                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = "/api/Dealers/SaveDealerFacilities/?dealerId=" + dealerId + "&facility=" + txtFacility.Text.Trim() + "&isActive=" + chkIsActiveFacility.Checked.ToString();
                // Send HTTP POST requests 
            
                BWHttpClient.PostSync<string>(_abHostUrl, _requestType, _apiUrl, "");

                GetFacilities();
            }
        }

        protected void UpdateFacility(object sender, EventArgs e)
        { 
            if(dealerId > 0)
            {
                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = "/api/Dealers/UpdateDealerFacilities/?facilityId=" + hdnFacilityId.Value + "&facility=" + txtFacility.Text.Trim() + "&isActive=" + chkIsActiveFacility.Checked.ToString();
                // Send HTTP POST requests 

                Trace.Warn("_apiUrl : " + _apiUrl);
            
                BWHttpClient.PostSync<string>(_abHostUrl, _requestType, _apiUrl, "");

                GetFacilities();
            }
        }

    }   // Class
}   // namespace