using AjaxPro;
using Bikewale.Notifications;
using BikewaleOpr.Common;
using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface.ManufacturerCampaign;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.ManufactureCampaign
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
            if (!IsPostBack)
                BindManufacturerList();
        }

        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : Function to bind the manufactureres list to the dropdown
        /// </summary>
        public void BindManufacturerList()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IManufacturerCampaignRepository, ManufacturerCampaign>();
                    IManufacturerCampaignRepository _objMfgRepo = container.Resolve<ManufacturerCampaign>();

                    IEnumerable<ManufacturerEntity> manufacturers = _objMfgRepo.GetManufacturersList();

                    if (manufacturers != null && manufacturers.Count() > 0)
                    {
                        ddlManufacturers.DataSource = manufacturers;

                        ddlManufacturers.DataTextField = "Organization";
                        ddlManufacturers.DataValueField = "Id";

                        ddlManufacturers.DataBind();

                        ddlManufacturers.Items.Insert(0, new ListItem("-- Select Manufacturer --", "0"));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // end of GetManufacturerList

    } //class
}   // namespace