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
    /// <summary>
    /// Author       : Suresh Prajapati.
    /// Created Date : 03rd Dec, 2014
    /// Description  : To Add Disclaimer for Dealers.
    /// </summary>
    public class ManageDealerDisclaimer : System.Web.UI.Page
    {
        protected Repeater rptAddedDisclaimer;
        protected DropDownList ddlMake, ddlModel, ddlVersions;
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

        private void GetDealerDiscliamer()
        {
            List<DealerDisclaimerEntity> objDisclaimer = null;

            if (dealerId > 0)
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDealer = container.Resolve<DealersRepository>();
                    objDisclaimer = objDealer.GetDealerDisclaimer(Convert.ToUInt32(dealerId));
                }
            }

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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDealer = container.Resolve<DealersRepository>();

                    objDealer.SaveDealerDisclaimer(Convert.ToUInt32(dealerId)
                                                  , Convert.ToUInt32(hdn_ddlMake.Value)
                                                  , Convert.ToUInt32(hdn_ddlModel.Value)
                                                  , Convert.ToUInt32(hdn_ddlVersions.Value)
                                                  , Convert.ToString(Server.UrlEncode(txtAddDisclaimer.Text.Trim()))
                                                 );
                }

                GetDealerDiscliamer();
            }
        }
    }
}