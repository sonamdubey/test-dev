using BikewaleOpr.DAL;
using BikewaleOpr.Entities;
using BikewaleOpr.Interfaces;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.BikeBooking
{
    public class ManageBookingAmount : System.Web.UI.Page
    {
        protected Repeater rptAddedBkgAmount;
        protected DropDownList ddlMake, ddlModel, ddlVersions;
        protected string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
        protected Label lblMessage;
        protected TextBox txtAddBkgAmount;
        protected Button btnAddAmount;
        protected HiddenField hdn_ddlMake, hdn_ddlModel, hdn_ddlVersions, hdn_BkgAmount;
        protected int dealerId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnAddAmount.Click += new EventHandler(SaveBookingAmount);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["dealerId"] != null)
            {
                int.TryParse(Request.QueryString["dealerId"].ToString(), out dealerId);
            }

            if (!IsPostBack)
            {
                FillMakes();
                if (dealerId > 0)
                {
                    GetDealerBookingAmount();
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

        private async void GetDealerBookingAmount()
        {
            string _requestType = "application/json";

            string _apiUrl = "/api/Dealers/GetBikeBookingAmount/?dealerId=" + dealerId;

            Trace.Warn("GetBikeBookingAmount _apiUrl : ", _abHostUrl + _apiUrl);
            List<BookingAmountEntity> objBkgAmount = null;
            objBkgAmount = await BWHttpClient.GetApiResponse<List<BookingAmountEntity>>(_abHostUrl, _requestType, _apiUrl, objBkgAmount);

            if (objBkgAmount != null && objBkgAmount.Count > 0)
            {
                rptAddedBkgAmount.DataSource = objBkgAmount;
                rptAddedBkgAmount.DataBind();
            }

        }

        protected void SaveBookingAmount(object sender, EventArgs e)
        {
            uint makeId = Convert.ToUInt32(hdn_ddlMake.Value);
            int modelId = Convert.ToInt32(hdn_ddlModel.Value);
            int versionId = Convert.ToInt32(hdn_ddlVersions.Value);
            uint amount = 0;
            bool testPass = UInt32.TryParse(Server.UrlEncode(txtAddBkgAmount.Text.Trim()), out amount);

            if (dealerId > 0 && makeId > 0 && modelId > 0 && amount >= 0 && testPass)
            {
                BookingAmountEntity objBkgAmt = new BookingAmountEntity()
                {
                    objDealer = new NewBikeDealers() { DealerId = Convert.ToUInt32(dealerId) },
                    objModel = new BikeModelEntityBase() { ModelId = modelId },
                    objVersion = new BikeVersionEntityBase() { VersionId = versionId },
                    objBookingAmountEntityBase = new BookingAmountEntityBase() { Amount = amount }
                };

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objPQ = container.Resolve<DealersRepository>();
                    objPQ.SaveBookingAmount(objBkgAmt);
                }

                GetDealerBookingAmount();
            }
            txtAddBkgAmount.Text = "";
        }
    }
}