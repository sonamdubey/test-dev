using BikewaleOpr.DAL;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.BikeBooking
{
    public class ManageBikeAvailability : System.Web.UI.Page
    {
        protected DropDownList ddlMake, ddlModel, ddlVersions;
        protected TextBox txtdayslimit;
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
        private void GetBikeAvailability()
        {
            try
            {
                uint dealerId = Convert.ToUInt32(Request.QueryString["dealerId"]);
                List<OfferEntity> objAvailibility = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();

                    objAvailibility = objCity.GetBikeAvailability(dealerId);
                }

                if (objAvailibility != null)
                {
                    rptavilableData.DataSource = objAvailibility;
                    rptavilableData.DataBind();
                }
            }
            catch (Exception err)
            {
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
                uint dealerId = Convert.ToUInt32(Request.QueryString["dealerId"]);
                bool isSuccess = false;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objCity = container.Resolve<DealersRepository>();
                    isSuccess = objCity.SaveBikeAvailability(dealerId, Convert.ToUInt32(hdn_ddlModel.Value), Convert.ToUInt32(hdn_ddlVersions.Value), Convert.ToUInt16(txtdayslimit.Text));
                }
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