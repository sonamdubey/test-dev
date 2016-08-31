using AjaxPro;
using BikewaleOpr.Common;
using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface.ManufacturerCampaign;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.NewBikeBooking
{
    public class ManageDealersCampaigns : System.Web.UI.Page
    {
       protected DropDownList ddlManufacturers;

       override protected void OnInit(EventArgs e)
       {
           this.Load += new System.EventHandler(this.Page_Load);
       }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetDealerAsManuFacturer();

        }

        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description :To fetch all the manufacturer in dropdown
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public void GetDealerAsManuFacturer()
        {
         
            IEnumerable<ManufacturerEntity> manufacturers = null;
            ManufacturerCampaign _objMfgRepo = new ManufacturerCampaign();
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IManufacturerCampaign, ManufacturerCampaign>();


                    _objMfgRepo = container.Resolve<ManufacturerCampaign>();


                    manufacturers = _objMfgRepo.GetDealerAsManuFacturer();
                    if (manufacturers != null && manufacturers.Count() > 0)
                    {
                        ddlManufacturers.DataSource = manufacturers;
                        ddlManufacturers.DataTextField = "Name";
                        ddlManufacturers.DataValueField = "Id";
                        ddlManufacturers.DataBind();
                        ddlManufacturers.Items.Insert(0, new ListItem("--Select Manufacturer--", "0"));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
           
        }
    }
}