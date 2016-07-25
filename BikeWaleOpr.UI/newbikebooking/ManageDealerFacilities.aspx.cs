using BikewaleOpr.DAL;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.NewBikeBooking
{
    public class ManageDealerFacilities : System.Web.UI.Page
    {
        protected Repeater rptFacilities;
        protected Button btnAddFacility, btnUpdateFacility;
        protected TextBox txtFacility;
        protected CheckBox chkIsActiveFacility;
        protected HiddenField hdnFacilityId;

        protected uint dealerId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnAddFacility.Click += new EventHandler(AddFacility);
            btnUpdateFacility.Click += new EventHandler(UpdateFacility);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["dealerId"] != null)
            {
                uint.TryParse(Request.QueryString["dealerId"].ToString(), out dealerId);
            }

            if (!IsPostBack)
            {
                if (dealerId > 0)
                    GetFacilities();
            }
        }

        protected void GetFacilities()
        {
            try
            {
                List<FacilityEntity> objFacilities = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealers, DealersRepository>();
                    IDealers objDealer = container.Resolve<DealersRepository>();
                    objFacilities = objDealer.GetDealerFacilities(dealerId);
                }
                if (objFacilities != null)
                {
                    rptFacilities.DataSource = objFacilities;
                    rptFacilities.DataBind();

                    Trace.Warn("GetFacilities bind data");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetFacilities  ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        protected void AddFacility(object sender, EventArgs e)
        {
            try
            {
                if (dealerId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();
                        objDealer.SaveDealerFacility(dealerId, txtFacility.Text.Trim(), chkIsActiveFacility.Checked);
                    }
                    GetFacilities();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("AddFacility ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        protected void UpdateFacility(object sender, EventArgs e)
        {
            uint facilityID = Convert.ToUInt32(hdnFacilityId.Value);
            try
            {
                if (dealerId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();
                        objDealer.UpdateDealerFacility(facilityID, txtFacility.Text.Trim(), chkIsActiveFacility.Checked);
                    }
                    GetFacilities();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("AddFacility ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            GetFacilities();
        }

    }   // Class
}   // namespace