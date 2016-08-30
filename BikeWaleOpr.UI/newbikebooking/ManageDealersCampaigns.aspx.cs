using AjaxPro;
using BikewaleOpr.Common;
using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
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

        public void GetDealerAsManuFacturer()
        {
         
            IEnumerable<ManufacturerEntity> manufacturers = null;
            ManageManufacturerCampaign _objMfgRepo = new ManageManufacturerCampaign();
            try
            {
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
           
        }
    }
}