using BikeWaleOpr.Common;
using BikeWaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.BikeBooking
{
    /// <summary>
    /// Author       : Suresh Prajapati.
    /// Created Date : 03rd Dec, 2014
    /// Description  : To Add Disclaimer for Dealers.
    /// </summary>
    public class ManageDealerDisclaimer : System.Web.UI.Page
    {
        protected Repeater rptAddedDisclaimer;
        protected DropDownList ddlMake, ddlModel, ddlVersions;
        protected string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
        protected TextBox txtAddDisclaimer;
        protected Button btnAddDisclaimer;
        protected HiddenField hdn_ddlMake, hdn_ddlModel, hdn_ddlVersions, hdn_Disclaimer;
        protected int dealerId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnAddDisclaimer.Click += new EventHandler(SaveDealerDisclaimer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillMakes();
            txtAddDisclaimer.Focus();

            if (Request.QueryString["dealerId"] != null)
            {
                int.TryParse(Request.QueryString["dealerId"].ToString(), out dealerId);
            }

            if (!IsPostBack)
            {
                if (dealerId > 0)
                {
                    GetDealerDiscliamer();
                }
            }
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 05 Dec, 2014.
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
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Get Disclaimer By Specified Dealer ID.
        /// </summary>

        private async void GetDealerDiscliamer()
        {
            string _requestType = "application/json";

            string _apiUrl = "/api/Dealers/GetDealerDisclaimer/?dealerId=" + dealerId;

            Trace.Warn("GetDealerDisclaimer _apiUrl : ", _apiUrl);
            List<DealerDisclaimerEntity> objDisclaimer = null;
            objDisclaimer = await BWHttpClient.GetApiResponse<List<DealerDisclaimerEntity>>(_abHostUrl, _requestType, _apiUrl, objDisclaimer);

            if (objDisclaimer != null)
            {
                rptAddedDisclaimer.DataSource = objDisclaimer;
                rptAddedDisclaimer.DataBind();
            }

        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Add New Disclaimer for Dealers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveDealerDisclaimer(object sender, EventArgs e)
        {
            if (dealerId > 0)
            {
                string _requestType = "application/json";
                string _apiUrl = "/api/Dealers/SaveDealerDisclaimer/?dealerId=" + dealerId + "&makeId=" + hdn_ddlMake.Value + "&modelId=" + hdn_ddlModel.Value + "&versionId=" + hdn_ddlVersions.Value + "&disclaimer=" + Server.UrlEncode(txtAddDisclaimer.Text.Trim());

                Trace.Warn("_apiUrl : ", _apiUrl);
                BWHttpClient.PostSync<string>(_abHostUrl, _requestType, _apiUrl, "");
                GetDealerDiscliamer();
            }
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Update Disclaimer of a Dealer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //protected void UpdateDealerDisclaimer(object sender, EventArgs e)
        //{
        //    if (dealerId > 0)
        //    {
        //        string _requestType = "application/json";

        //        string _apiUrl = "/api/Dealers/UpdateDealerDisclaimer/?dealerId=" + dealerId +"&versionId="+hdn_ddlVersions.Value+"&disclaimer=" + Server.UrlEncode(txtAddDisclaimer.Text.Trim());
        //        // Send HTTP POST requests 

        //        Trace.Warn("_apiUrl : ", _apiUrl);

        //        BWHttpClient.PostSync<string>(_abHostUrl, _requestType, _apiUrl, "");

        //        GetDealerDiscliamer();
        //    }
        //}
    }
}