using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikewaleOpr.Campaign
{
    public class ManageDealer : System.Web.UI.Page
    {
        protected bool isEdit = true;
        protected Button btnUpdate;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["campaignid"] == null)
            {
                isEdit = false;
            }

        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnUpdate.Click += new EventHandler(InserOrUpdateDealerCampaign);
           // dealerCampaign = new ManageDealerCampaign();

        }



        private void InserOrUpdateDealerCampaign(object sender, EventArgs e)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IManufacturerCampaignRepository, ManufacturerCampaign>();
                    IManufacturerCampaignRepository objMfgCampaign = container.Resolve<IManufacturerCampaignRepository>();
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "InserOrUpdateDealerCampaign");
                objErr.SendMail();
            }
        }

        
    }
}