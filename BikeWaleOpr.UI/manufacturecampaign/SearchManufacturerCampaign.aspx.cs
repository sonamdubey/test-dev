﻿
using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface.ManufacturerCampaign;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.manufacturecampaign
{
    public class SearchManufacturerCampaign : System.Web.UI.Page
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

                    if (manufacturers != null && manufacturers.Any())
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
                ErrorClass.LogError(ex, "SearchManufacturerCampaign.BindManufacturerList");
            }
        }   // end of GetManufacturerList

    } //class
}   // namespace