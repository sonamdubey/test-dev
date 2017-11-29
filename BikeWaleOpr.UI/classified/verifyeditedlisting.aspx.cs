﻿using Bikewale.Entities.Used;
using Bikewale.Notifications;
using BikewaleOpr.BAL.Used;
using BikewaleOpr.Interface.Used;
using BikewaleOpr.Used;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace BikewaleOpr.Classified
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 25 Oct 2016
    /// Summary: Webpage to approve/reject edited listings
    /// </summary>
    public class VerifyEditedListing : System.Web.UI.Page
    {
        protected Repeater rptPendingEditedListing;
        protected IEnumerable<SellBikeAd> sellListing;

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
                    sellListing = objSellBikes.GetClassifiedPendingInquiries();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindEditedListings");
                
            }
        }
    }
}