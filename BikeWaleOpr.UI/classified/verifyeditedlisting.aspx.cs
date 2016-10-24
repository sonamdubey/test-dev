using Bikewale.Entities.Used;
using Bikewale.Notifications;
using BikewaleOpr.BAL.Used;
using BikewaleOpr.Interface.Used;
using BikewaleOpr.Used;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace BikewaleOpr.classified
{
    public class VerifyEditedListing : System.Web.UI.Page
    {
        protected Repeater rptPendingEditedListing;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindEditedListings();
        }

        private void BindEditedListings()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ISellBikes, SellBikes>();
                    container.RegisterType<ISellerRepository, SellerRepository>();
                    ISellBikes objSellBikes = container.Resolve<ISellBikes>();
                    IEnumerable<SellBikeAd> sellListing = objSellBikes.GetClassifiedPendingInquiries();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindEditedListings");
                objErr.SendMail();
            }
        }
    }
}